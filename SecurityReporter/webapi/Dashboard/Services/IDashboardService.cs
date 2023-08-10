namespace webapi.Dashboard.Services
{
    public interface IDashboardService
    {
        public Task<List<Tuple<string, int>>> GetCriticalityData();

        public Task<List<Tuple<string, int>>> GetVulnerabilityData();
    }
}
