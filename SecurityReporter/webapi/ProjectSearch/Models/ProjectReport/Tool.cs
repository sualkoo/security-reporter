using webapi.ProjectSearch.Models;

namespace webapi.Models.ProjectReport
{
    public class Tool : IEntity
    {
        public string? ToolName { get; set; }
        public string? Version { get; set; }
        public string? TestType { get; set; }
        public string? WorkType { get; set; }
    }
}
