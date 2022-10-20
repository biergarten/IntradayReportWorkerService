using Services;


namespace IntradayReportWorkerService.Extensions
{
    public static class PowerPeriodExtensions
    {
        public static string ToLocalTimeString(this PowerPeriod powerPeriod)
        {
            int hourOfTheDay = (powerPeriod.Period + 22) % 24;
            return new TimeOnly(hourOfTheDay, 0).ToString("HH:mm");
        }
    }
}
