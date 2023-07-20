namespace webapi.Models.ProjectReport
{
    public class WorstCaseScenarioReport
    {
        public string findingDescription;
        public List<List<bool>> worstCaseReport;
        public WorstCaseScenarioReport() {
            this.worstCaseReport = new List<List<bool>>();
        }
        
    }
}
