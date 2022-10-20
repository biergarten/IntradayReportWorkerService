using Services;

namespace IntradayReportWorkerService.Services
{
    public interface IDataAggregationService
    {
        public Dictionary<string, double> GetAggregatedVolumes(IEnumerable<PowerTrade> trades);
    }
}