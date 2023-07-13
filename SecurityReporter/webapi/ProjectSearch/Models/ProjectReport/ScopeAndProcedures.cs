namespace webapi.Models.ProjectReport
{
    public class ScopeAndProcedures
    {
        public List<ScopeProcedure>? InScope { get; set; }
        public List<ScopeProcedure>? OutOfScope { get; set; }
        public List<string>? WorstCaseScenarios { get; set; }
        public List<string>? Description { get; set; }
        //Boolean pole na Findings case scenarios
    }
}
