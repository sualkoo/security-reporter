using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Models.ProjectReport;

namespace webapi.ProjectSearch.Services;

public class ProjectDataValidator : IProjectDataValidator
{
    private readonly ILogger logger;

    public ProjectDataValidator()
    {
        var loggerFactory = LoggerProvider.GetLoggerFactory();
        logger = loggerFactory.CreateLogger<ProjectDataValidator>();
    }

    public bool Validate(ProjectReportData projectReport)
    {
        var result = true;

            var validationResults = new EntityValidationResult[]
            {
                DataAnnotation.ValidateEntity<ProjectReportData>(projectReport),
                DataAnnotation.ValidateEntity<DocumentInformation>(projectReport.DocumentInfo),
                DataAnnotation.ValidateList<ReportVersionEntry>(projectReport.DocumentInfo.ReportDocumentHistory),
                DataAnnotation.ValidateEntity<ProjectInformation>(projectReport.ProjectInfo),
                DataAnnotation.ValidateTimeFrames(projectReport.ProjectInfo),
                DataAnnotation.ValidateEntity(projectReport.ProjectInfo.ApplicationManager),
                DataAnnotation.ValidateEntity(projectReport.ProjectInfo.BusinessOwner),
                DataAnnotation.ValidateEntity(projectReport.ProjectInfo.BusinessRepresentative),
                DataAnnotation.ValidateList<ProjectInformationParticipant>(projectReport.ProjectInfo.TechnicalContacts),
                DataAnnotation.ValidateEntity(projectReport.ProjectInfo.PentestLead),
                DataAnnotation.ValidateEntity(projectReport.ProjectInfo.PentestCoordinator),
                DataAnnotation.ValidateList<ProjectInformationParticipant>(projectReport.ProjectInfo.PentestTeam),
                DataAnnotation.ValidateList<Finding>(projectReport.Findings),
                DataAnnotation.ValidateEntity<ScopeAndProcedures>(projectReport.ScopeAndProcedures),
                //DataAnnotation.ValidateList<ScopeProcedure>(projectReport.ScopeAndProcedures.InScope),
                //DataAnnotation.ValidateList<ScopeProcedure>(projectReport.ScopeAndProcedures.OutOfScope),
                DataAnnotation.ValidateEntity<TestingMethodology>(projectReport.TestingMethodology),
                //DataAnnotation.ValidateList<Tool>(projectReport.TestingMethodology.ToolsUsed),
            };

        var errors = new List<string>();

        foreach (var validationResult in validationResults)
            if (validationResult.HasError)
            {
                result = false;
                foreach (var error in validationResult.ValidationErrors)
                {
                    logger.LogError(error.ErrorMessage);
                    errors.Add(error.ErrorMessage);
                }
            }

        if (result)
            logger.LogInformation("Validation of Project Report successed!");
        else
            throw new CustomException(StatusCodes.Status400BadRequest, "Validation fail", errors);

        return result;
    }
}