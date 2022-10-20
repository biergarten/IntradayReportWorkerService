using IntradayReportWorkerService.Services;
using Microsoft.Extensions.Options;

namespace IntradayReportWorkerService
{
    public sealed class WindowsBackgroundService : BackgroundService
    {
        private readonly ReportService _reportService;
        private readonly ILogger<WindowsBackgroundService> _logger;
        private readonly  int _intervalInMinutes;


        public WindowsBackgroundService(
            ReportService reportService,
            ILogger<WindowsBackgroundService> logger,
            IOptions<ServiceOptions> options)
        {
            (_reportService, _logger) = (reportService, logger);
            _intervalInMinutes = options.Value.IntervalInMinutes;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Service started at: {time}", DateTimeOffset.Now);

                while (!stoppingToken.IsCancellationRequested)
                {
                    var time = DateTime.Now;
                    
                    await _reportService.GetIntradayReport(time);

                    await Task.Delay(TimeSpan.FromMinutes(_intervalInMinutes), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                
                Environment.Exit(1);
            }
        }
    }
}