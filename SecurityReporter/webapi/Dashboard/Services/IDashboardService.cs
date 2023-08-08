namespace webapi.Dashboard.Services
{
    public interface IDashboardService
    {
        public Task<List<Tuple<int, int>>> GetCriticalityData();

        public Task<List<Tuple<int, int>>> GetVulnerabilityData();
    }
}
