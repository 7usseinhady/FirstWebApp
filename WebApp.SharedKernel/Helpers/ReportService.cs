using AspNetCore.Reporting;
using WebApp.SharedKernel.Interfaces;
using System.Text;

namespace WebApp.SharedKernel.Helpers
{
    public class ReportService : IReportService
    {
        public async Task<byte[]> GenerateReportAsync(string reportName, object dataSource, string reportType = "pdf", string dataSetName = "DataSet1", Dictionary<string, string> parameters = null)
        {
            try
            {
                string rdlcFilePath = string.Format(@"wwwroot\rdlcs\{0}.rdlc", reportName);

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding.GetEncoding("windows-1252");

                var report = new LocalReport(rdlcFilePath);

                report.AddDataSource(dataSetName, dataSource);
                var result = report.Execute(GetRenderType(reportType), parameters : parameters);
                return result.MainStream;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private RenderType GetRenderType(string reportType)
        {
            var renderType = RenderType.Pdf;
            if (!string.IsNullOrEmpty(reportType))
            {
                switch (reportType.ToLower())
                {
                    default:
                    case "pdf":
                        renderType = RenderType.Pdf;
                        break;
                    case "word":
                        renderType = RenderType.Word;
                        break;
                    case "excel":
                        renderType = RenderType.Excel;
                        break;
                }
            }
            return renderType;
        }

    }
}
