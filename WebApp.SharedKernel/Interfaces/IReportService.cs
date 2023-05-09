
namespace WebApp.SharedKernel.Interfaces
{
    public interface IReportService
    {
        byte[] GenerateReport(string reportName, object dataSource, string reportType = "pdf", string dataSetName = "DataSet1", Dictionary<string, string> parameters = null!);
    }
}
