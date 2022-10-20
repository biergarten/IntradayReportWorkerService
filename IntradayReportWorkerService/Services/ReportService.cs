using Services;

namespace IntradayReportWorkerService.Services
{
    public class ReportService
    {
        private readonly IPowerService _powerService;
        private readonly IDataAggregationService _aggregationService;
        private readonly IExportService _exportService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReportService> _logger;
        public ReportService(IPowerService powerService, 
            IDataAggregationService aggregationService,
            IExportService exportService,
            IConfiguration configuration, 
            ILogger<ReportService> logger)
        {
            _powerService = powerService;
            _aggregationService = aggregationService;
            _exportService = exportService;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task GetIntradayReport(DateTime time)
        {
            try
            {
                _logger.LogInformation("Getting the report for {time}", time.ToString("yyyyMMdd_HHmm"));
                
                var trades = await _powerService.GetTradesAsync(time);
                var dictionaryResult = _aggregationService.GetAggregatedVolumes(trades);

                _exportService.OutputData(dictionaryResult, time);

                _logger.LogInformation("Done getting the report for {time}", time.ToString("yyyyMMdd_HHmm"));
            }
            catch (PowerServiceException ex)
            {
                _logger.LogError(ex, "Error when getting trades: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

      
    }


}
