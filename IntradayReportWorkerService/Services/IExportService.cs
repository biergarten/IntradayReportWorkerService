namespace IntradayReportWorkerService.Services
{
    public interface IExportService
    {
        void OutputData(Dictionary<string, double> dictionaryResult, DateTime time);
    }
}