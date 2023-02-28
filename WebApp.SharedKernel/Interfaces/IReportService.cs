
namespace WebApp.SharedKernel.Interfaces
{
    public interface IReportService
    {
        Task<byte[]> GenerateReportAsync(string reportName, object dataSource, string reportType = "pdf", string dataSetName = "DataSet1", Dictionary<string, string> parameters = null);
    }
}
