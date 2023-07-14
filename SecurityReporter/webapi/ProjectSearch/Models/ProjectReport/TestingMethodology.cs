using webapi.ProjectSearch.Models;

namespace webapi.Models.ProjectReport
{
    public class TestingMethodology : IEntity
    {
        public List<Tool>? ToolsUsed { get; set; }
        public List<string>? AttackVectors { get; set; }
    }
}
