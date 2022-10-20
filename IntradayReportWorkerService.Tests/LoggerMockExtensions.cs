using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace IntradayReportWorkerService.Tests
{
    public static class LoggerMockExtensions
    {
        public static Mock<ILogger<T>> VerifyLoggerWasCalled<T>(this Mock<ILogger<T>> logger, LogLevel logLevel, string messageStartsWith)
        {
            Func<object, Type, bool> state = (v, t) => v.ToString().StartsWith(messageStartsWith);
            logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == logLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v, t)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true))
                , Times.Once);
            return logger;
        }
    }
}
