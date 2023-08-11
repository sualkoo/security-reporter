namespace webapi.Dashboard.Services
{
    public interface IDashboardService
    {
        public Task<List<Tuple<string, int, int>>> GetCriticalityData();

        public Task<List<Tuple<string, int, int>>> GetVulnerabilityData();

        public Task<List<Tuple<int, int>>> GetCWEData();
    }
}
