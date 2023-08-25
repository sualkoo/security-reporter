using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Service;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace webapi.ProjectManagement.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectController : ControllerBase
{
    public ICosmosService CosmosService { get; }
    public IAzureBlobService AzureBlobService { get; }

    public ProjectController(ICosmosService cosmosService, IAzureBlobService azureBlobService)
    {
        CosmosService = cosmosService;
        AzureBlobService = azureBlobService;
    }

    [HttpPost("add")]
    [Authorize(Policy = "AdminCoordinatorPolicy")]
    public async Task<IActionResult> PostProject(ProjectData project)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("POST");
        Console.ResetColor();
        Console.WriteLine("\t /Project/add");

        bool result = await CosmosService.AddProject(project);

        if (!result)
        {
            Console.WriteLine("Error occured in Project/add post request.");
            return StatusCode(400, "Error: Unable to insert project into DB.");
        }
        Console.WriteLine("Request executed without any errors.");
        return StatusCode(201, project);
    }


    [HttpPost("delete")]
    [Authorize(Policy = "AdminCoordinatorPolicy")]
    public async Task<IActionResult> DeleteProject(List<string> ids)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("POST");
        Console.ResetColor();
        Console.WriteLine("\t /Project/delete");

        var result = await CosmosService.DeleteProjects(ids);

        if (result.Count > 0)
        {
            return StatusCode(404, result);
        }
        return StatusCode(200);
    }

    [HttpGet("checkFileExists/{id}")]
    [Authorize(Policy = "AdminCoordinatorClientPolicy")]
    public async Task<IActionResult> CheckFileExists(string id)
    {
        string scopeFileName = $"scope_{id}.pdf";
        string questionnaireFileName = $"questionare_{id}.pdf";
        string reportFileName = $"report_{id}.pdf";

        bool scopeFileExists = await AzureBlobService.CheckFileExistsAsync(scopeFileName);
        bool questionnaireFileExists = await AzureBlobService.CheckFileExistsAsync(questionnaireFileName);
        bool reportFileExists = await AzureBlobService.CheckFileExistsAsync(reportFileName);

        var fileExistenceData = new
        {
            ScopeFileExists = scopeFileExists,
            QuestionnaireFileExists = questionnaireFileExists,
            ReportFileExists = reportFileExists
        };

        return StatusCode(200, fileExistenceData);
    }


    [HttpGet("uploadStatus/{id}")]
    [Authorize(Policy = "AdminCoordinatorClientPolicy")]
    public async Task<IActionResult> GetUploadStatus(string id)
    {
        try
        {
            bool uploadStatus = await CosmosService.GetUploadStatus(id);

            if (uploadStatus)
            {
                return StatusCode(200, new { IsUploaded = true });
            }
            else
            {
                return StatusCode(404, new { IsUploaded = false });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(400, new { Error = $"Error retrieving upload status: {ex.Message}" });
        }
    }


    [HttpGet("retrieve")]
    [Authorize(Policy = "AdminCoordinatorClientPolicy")]
    public async Task<IActionResult> GetItems([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] FilterData filter, [FromQuery] SortData sort)
    {
        var items = new CountProjects();

        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("GET");
            Console.ResetColor();
            Console.WriteLine("\t /Project/retrieve");

            items = await CosmosService.GetItems(pageSize, pageNumber, filter, sort);

            if (items.Projects.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success: Data retrieved successfully.");
                Console.ResetColor();

                return StatusCode(200, items);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Info: No data available.");
                Console.ResetColor();

                return StatusCode(204, "No data available.");
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {ex.Message}");
            Console.ResetColor();

            return StatusCode(400, $"Error retrieving data: {ex.Message}");
        }
    }

    [HttpPut("update")]
    [Authorize(Policy = "AdminCoordinatorPolicy")]
    public async Task<IActionResult> UpdateProject(ProjectData project)
    {
        Console.WriteLine("Updating project..");

        bool result = await CosmosService.UpdateProject(project);

        if (!result)
        {
            Console.WriteLine("Error occured in Project/update put request.");
            return StatusCode(400, "Error: Unable to update project in DB.");
        }

        Console.WriteLine("Request executed without any errors.");
        return StatusCode(202, project);
    }

    [HttpGet("download")]
    [Authorize(Policy = "AdminCoordinatorPolicy")]
    public async Task<IActionResult> Download(string name)
    {
        Console.WriteLine("Downloading file..");

        bool result = await AzureBlobService.DownloadProject(name);

        if (!result)
        {
            Console.WriteLine("Error occured in Project/download get request.");
            return StatusCode(400, "Error: Unable to download file.");
        }

        Console.WriteLine("Request executed without any errors.");
        return Ok();
    }

    [HttpGet("getProject")]
    [Authorize(Policy = "AdminCoordinatorPolicy")]
    public async Task<IActionResult> GetProjectById(string id)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("GET");
        Console.ResetColor();
        Console.WriteLine($"\t /Project/{id}");

        try
        {
            var project = await CosmosService.GetProjectById(id);

            if (project != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success: Project data retrieved successfully.");
                Console.ResetColor();

                return StatusCode(200, project);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Info: Project with ID '{id}' not found.");
                Console.ResetColor();

                return StatusCode(404, $"Project with ID '{id}' not found.");
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {ex.Message}");
            Console.ResetColor();

            return StatusCode(400, $"Error retrieving project data: {ex.Message}");
        }
    }

    [HttpPost("upload")]
    [Authorize(Policy = "AdminCoordinatorPolicy")]
    public async Task<IActionResult> UploadFile(IFormFile file, string destination, string id)
    {
        try
        {
            if (string.IsNullOrEmpty(destination))
            {
                return StatusCode(400, "Error: Destination parameter is required.");
            }

            if (destination != "scope" && destination != "questionare" && destination != "report")
            {
                return StatusCode(400, "Error: Invalid destination parameter.");
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
            {
                return StatusCode(400, "Error: Uploaded file must be a PDF.");
            }

            await AzureBlobService.UploadProjectFile(file, destination + "_" + id + ".pdf");

            Console.WriteLine($"File uploaded successfully");

            return StatusCode(201, "File uploaded successfully.");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {ex.Message}");
            Console.ResetColor();

            return StatusCode(500, $"Error uploading file: {ex.Message}");
        }
    }

    [HttpDelete("deleteBlobFile")]
    [Authorize(Policy = "AdminCoordinatorPolicy")]
    public async Task<IActionResult> DeleteBlobFile(string destination, string id)
    {
            if (string.IsNullOrEmpty(destination))
            {
                return StatusCode(400, "Error: Destination parameter is required.");
            }

            if (destination != "scope" && destination != "questionare" && destination != "report")
            {
                return StatusCode(400, "Error: Invalid destination parameter.");
            }

            await AzureBlobService.DeleteProjectFile(destination + "_" + id + ".pdf");
            return StatusCode(204, "File deleted successfully.");
    }
}
