using Microsoft.Extensions.Options;
using System.Text;

namespace IntradayReportWorkerService.Services
{
    public class ExportService : IExportService
    {
        private readonly string _locationPath;

        public ExportService(IOptions<ServiceOptions> options)
        {
            _locationPath = options.Value.ReportsLocation;
        }
        public void OutputData(Dictionary<string, double> dictionaryResult, DateTime time)
        {
            var outputLines = new List<string>();
            outputLines.Add("Local Time,Volume");

            foreach (var result in dictionaryResult)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(result.Key);
                sb.Append(",");
                sb.Append(result.Value);
                outputLines.Add(sb.ToString());
            }
            using (var f = new StreamWriter(Path.Combine(_locationPath, $"PowerPosition_{time.ToString("yyyyMMdd_HHmm")}.csv")))
            {
                foreach (var item in outputLines)
                {
                    f.WriteLine(item);
                }
            }
        }
    }
}