using IntradayReportWorkerService.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace IntradayReportWorkerService.Tests
{
    public class ReportServiceTests
    {
        private readonly Mock<IPowerService>  _powerServiceMock = new Mock<IPowerService>();
        private readonly Mock<IDataAggregationService> _dataAggregationService = new Mock<IDataAggregationService>();
        private readonly Mock<IExportService>  _exportServiceMock = new Mock<IExportService>();
        private readonly Mock<ILogger<ReportService>> _loggerMock = new Mock<ILogger<ReportService>>();

        [Fact]
        public async Task ShouldLogErrorButNoThrowAnErrorWhenErrorGettingTrades()
        {
            
            _powerServiceMock.Setup(x => x.GetTradesAsync(It.IsAny<DateTime>()))
                .ThrowsAsync(new PowerServiceException("a power service exception"));
            
            var reportService = new ReportService(_powerServiceMock.Object, _dataAggregationService.Object, _exportServiceMock.Object,null, _loggerMock.Object);

            await reportService.GetIntradayReport(DateTime.Now);

            _loggerMock.VerifyLoggerWasCalled(LogLevel.Error, "Error when getting trades: a power service exception");
        }

        [Fact]
        public async Task ShouldLogErrorAndThrowAnErrorWhenUnhandledError()
        {

            _powerServiceMock.Setup(x => x.GetTradesAsync(It.IsAny<DateTime>()))
                .ThrowsAsync(new Exception("an unexpected error"));

            var reportService = new ReportService(_powerServiceMock.Object, _dataAggregationService.Object, _exportServiceMock.Object, null, _loggerMock.Object);

            var ex = await Assert.ThrowsAsync<Exception>(() =>  reportService.GetIntradayReport(DateTime.Now));

            _loggerMock.VerifyLoggerWasCalled(LogLevel.Error, "an unexpected error");

        }

        [Fact]
        public async Task ShouldLogInfoThatIsDone()
        {
            _powerServiceMock.Setup(x => x.GetTradesAsync(It.IsAny<DateTime>()))
               .ReturnsAsync(new List<PowerTrade>());

            var reportService = new ReportService(_powerServiceMock.Object, _dataAggregationService.Object, _exportServiceMock.Object, null, _loggerMock.Object);

             await reportService.GetIntradayReport(DateTime.Now);

            _loggerMock.VerifyLoggerWasCalled(LogLevel.Information, "Done getting the report for");
        }

    }
}
