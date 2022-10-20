using IntradayReportWorkerService;
using IntradayReportWorkerService.Services;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using Services;

IHost host = Host.CreateDefaultBuilder(args)
     .UseWindowsService(options =>
     {
         options.ServiceName = ".NET Joke Service";
     })
    .ConfigureServices((context, services) =>
    {
        LoggerProviderOptions.RegisterProviderOptions<
            EventLogSettings, EventLogLoggerProvider>(services);

        services.AddSingleton<ReportService>();
        services.AddSingleton<IPowerService, PowerService>();
        services.AddSingleton<IDataAggregationService, DataAggregationService>();
        services.AddSingleton<IExportService, ExportService>();
        services.Configure<ServiceOptions>(context.Configuration.GetSection("WorkerOptions"));
        services.AddHostedService<WindowsBackgroundService>();
    })
    .ConfigureLogging((context, logging) =>
    {
        // See: https://github.com/dotnet/runtime/issues/47303
        logging.AddConfiguration(
            context.Configuration.GetSection("Logging"));
    })
    .Build();

await host.RunAsync();
