using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services.Extractor;
using webapi.Service;

namespace webapi.ProjectSearch.Services;

public class ProjectReportService : IProjectReportService
{
    private readonly ILogger Logger;
    private IProjectReportService projectReportServiceImplementation;
    private bool generatePdfs;
    
    public ProjectReportService(IProjectDataParser parser, IDbProjectDataParser dbParser,
        IProjectDataValidator validator, ICosmosService cosmosService, IPdfBuilder pdfBuilder,
        IAzureBlobService azureBlobService, IConfiguration configuration)
    {
        Validator = validator;
        Parser = parser;
        DBParser = dbParser;
        CosmosService = cosmosService;
        PdfBuilder = pdfBuilder;
        AzureBlobService = azureBlobService;
        var loggerFactory = LoggerProvider.GetLoggerFactory();
        Logger = loggerFactory.CreateLogger<ProjectDataValidator>();
        
        generatePdfs = false;
        var pdfGenerationAllowed = configuration["GeneratePdfsFromReports"];
        if (bool.TryParse(pdfGenerationAllowed, out bool generatePdfsValue))
        {
            generatePdfs = generatePdfsValue;
        }
    }

    private IProjectDataValidator Validator { get; }
    private IProjectDataParser Parser { get; }
    private IDbProjectDataParser DBParser { get; }
    private ICosmosService CosmosService { get; }
    private IAzureBlobService AzureBlobService { get; }
    private IPdfBuilder PdfBuilder { get; }

    public async Task<ProjectReportData> GetReportByIdAsync(Guid id)
    {
        Logger.LogInformation("Fetching project report by ID");
        return await CosmosService.GetProjectReport(id.ToString());
    }

    public async Task<ProjectReportData> SaveReportFromZipAsync(IFormFile file)
    {
        Logger.LogInformation("Saving new project report");
        ProjectReportData newReportData;

        if (Path.GetExtension(file.FileName)?.ToLower() != ".zip")
            throw new CustomException(StatusCodes.Status406NotAcceptable,
                "Invalid file type. Only .zip files are allowed.");

        newReportData = Parser.Extract(file.OpenReadStream());

        Validator.Validate(newReportData);

        newReportData.Id = Guid.NewGuid();
        var result = await CosmosService.AddProjectReport(newReportData);

        if (!result)
            throw new CustomException(StatusCodes.Status500InternalServerError,
                "Failed to save ProjectReport to database.");

        try
        {
            if (generatePdfs)
            {
                var generatedPdf = await PdfBuilder.GeneratePdfFromZip(file.OpenReadStream(), newReportData.Id);
                await AzureBlobService.SaveReportPdf(generatedPdf.FileContents, newReportData.Id,
                    newReportData.DocumentInfo!.ProjectReportName!);
            }
            await AzureBlobService.SaveImagesFromZip(newReportData.Id, newReportData.Findings);
        }
        catch (Exception)
        {
            Logger.LogInformation("PDF generation failed, deleting project report...");
            await CosmosService.DeleteProjectReports(new List<string> { newReportData.Id.ToString() });
            throw;
        }

        return newReportData;
    }

    public async Task<PagedDbResults<List<FindingResponse>>> GetReportFindingsAsync(
        string? projectName, string? details, string? impact, string? repeatability, string? references,
        string? cWE, int page)
    {
        Logger.LogInformation("Fetching project reports by keywords");
        if (!string.IsNullOrEmpty(projectName) && !string.IsNullOrEmpty(details) &&
            !string.IsNullOrEmpty(impact) && !string.IsNullOrEmpty(repeatability) &&
            !string.IsNullOrEmpty(references) &&
            !string.IsNullOrEmpty(cWE))
            throw new CustomException(StatusCodes.Status400BadRequest, "Missing parameters.");

        return await CosmosService.GetPagedProjectReportFindings(projectName, details, impact, repeatability,
            references, cWE, page);
    }

    public async Task<bool> DeleteReportAsync(List<string> ids)
    {
        Logger.LogInformation("Deleting items from database");

        await CosmosService.DeleteProjectReports(ids);
        foreach (var id in ids) await AzureBlobService.DeleteReportFolder(new Guid(id));

        return true;
    }

    public async Task<bool> DeleteReportAllAsync()
    {
        Logger.LogInformation("Deleting all items from database");
        var deletedProjects = await CosmosService.DeleteAllReportsAsync();
        foreach (var id in deletedProjects) await AzureBlobService.DeleteReportFolder(new Guid(id));

        return true;
    }

    public async Task<FileContentResult> GetReportSourceByIdAsync(Guid id)
    {
        var data = await CosmosService.GetProjectReport(id.ToString());

        await AzureBlobService.LoadImagesFromDb(id, data);
        var zip = DBParser.Extract(data);
        return zip;
    }

    public async Task<FileContentResult> GetPdfByProjectIdAsync(Guid id)
    {
        return await AzureBlobService.GetReportPdf(id);
    }
}