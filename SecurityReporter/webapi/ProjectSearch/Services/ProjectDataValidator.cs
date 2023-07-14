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
                DataAnnotation.ValidateEntity<ProjectInformation>(projectReport.ProjectInfo),
                DataAnnotation.ValidateList<Finding>(projectReport.Findings),
                DataAnnotation.ValidateEntity<ScopeAndProcedures>(projectReport.ScopeAndProcedures),
                DataAnnotation.ValidateEntity<TestingMethodology>(projectReport.TestingMethodology),
            };
            //bool validationResultsFindings = new EntityValidationResult[] {
            //    DataAnnotation.ValidateEntity<Finding>(projectReport.Findings),
            //};

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
            Console.Read();
            return result;
        }
    }
}
