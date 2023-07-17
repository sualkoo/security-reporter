using System.ComponentModel.DataAnnotations;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Models;

namespace webapi.ProjectSearch.Services
{
    public class ProjectDataValidator : IProjectDataValidator
    {
        public bool Validate(ProjectReportData projectReport)
        {
            bool result = true;

            var validationResults = new EntityValidationResult[]
            {
                DataAnnotation.ValidateEntity<ProjectReportData>(projectReport),
                DataAnnotation.ValidateEntity<DocumentInformation>(projectReport.DocumentInfo),
                DataAnnotation.ValidateList<ReportVersionEntry>(projectReport.DocumentInfo.ReportDocumentHistory),
                //DataAnnotation.ValidateEntity<ProjectInformation>(projectReport.ProjectInfo),
                //DataAnnotation.ValidateEntity(projectReport.ProjectInfo.ApplicationManager),
                //DataAnnotation.ValidateEntity(projectReport.ProjectInfo.BusinessOwner),
                //DataAnnotation.ValidateEntity(projectReport.ProjectInfo.BusinessRepresentative),
                //DataAnnotation.ValidateList<ProjectInformationParticipant>(projectReport.ProjectInfo.TechnicalContacts),
                //DataAnnotation.ValidateEntity(projectReport.ProjectInfo.PentestLead),
                //DataAnnotation.ValidateEntity(projectReport.ProjectInfo.PentestCoordinator),
                //DataAnnotation.ValidateList<ProjectInformationParticipant>(projectReport.ProjectInfo.PentestTeam),
                //DataAnnotation.ValidateList<Finding>(projectReport.Findings),
                //DataAnnotation.ValidateEntity<ScopeAndProcedures>(projectReport.ScopeAndProcedures),
                //DataAnnotation.ValidateList<ScopeProcedure>(projectReport.ScopeAndProcedures.InScope),
                //DataAnnotation.ValidateList<ScopeProcedure>(projectReport.ScopeAndProcedures.OutOfScope),
                //DataAnnotation.ValidateEntity<TestingMethodology>(projectReport.TestingMethodology),
                //DataAnnotation.ValidateList<Tool>(projectReport.TestingMethodology.ToolsUsed),
            };
            

            foreach ( var validationResult in validationResults )
            {
                if (validationResult.HasError)
                {
                    result = false;
                    foreach (var error in validationResult.ValidationErrors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }
            if (result )
            {
                Console.WriteLine("Validation of Project Report successed!");
            }
            return result;
        }
    }
}
