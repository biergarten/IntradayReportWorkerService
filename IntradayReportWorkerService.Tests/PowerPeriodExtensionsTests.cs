using IntradayReportWorkerService.Extensions;
using Services;
using Xunit;

namespace IntradayReportWorkerService.Tests
{
    public class PowerPeriodExtensionsTests
    {
        [Theory]
        [InlineData(1, "23:00")]
        [InlineData(2, "00:00")]
        [InlineData(12, "10:00")]
        [InlineData(24, "22:00")]

        public void ShouldGetTheRightString(int period, string expectedResult)
        {
            PowerPeriod powerPeriod = new PowerPeriod();
            powerPeriod.Period = period;

            var result = powerPeriod.ToLocalTimeString();

            Assert.Equal(expectedResult, result);
        }
    }
}