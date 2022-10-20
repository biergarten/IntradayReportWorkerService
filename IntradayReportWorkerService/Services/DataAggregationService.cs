using IntradayReportWorkerService.Extensions;
using Services;

namespace IntradayReportWorkerService.Services
{
    public class DataAggregationService: IDataAggregationService
    {
        public Dictionary<string, double> GetAggregatedVolumes(IEnumerable<PowerTrade> trades)
        {
            var dictionaryResult = new Dictionary<string, double>();
            foreach (var trade in trades)
            {
                foreach (var powerPeriod in trade.Periods)
                {
                    if (!dictionaryResult.ContainsKey(powerPeriod.ToLocalTimeString()))
                        dictionaryResult.Add(powerPeriod.ToLocalTimeString(), powerPeriod.Volume);
                    else
                        dictionaryResult[powerPeriod.ToLocalTimeString()] += powerPeriod.Volume;
                }
            }

            return dictionaryResult;
        }
    }
}