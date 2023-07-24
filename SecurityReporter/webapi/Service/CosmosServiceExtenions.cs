using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.Models;
using webapi.ProjectSearch.Models;

namespace webapi.Service
{
    public static class CosmosServiceExtensions
    {
        public static Task<List<ProjectData>> GetItems(this ICosmosService cosmosService, int pageSize, int pageNumber, string projectName = null, string projectStatus = null,
                                                       string questionnaire = null, string projectScope = null, DateTime? startDate = null,
                                                       DateTime? endDate = null, int? pentestDurationMin = null, int? pentestDurationMax = null,
                                                       string ikoAndTKO = null)
        {
            return cosmosService.GetItems(pageSize, pageNumber, projectName, projectStatus, questionnaire, projectScope, startDate, endDate, pentestDurationMin, pentestDurationMax, ikoAndTKO);
        }
    }
}
