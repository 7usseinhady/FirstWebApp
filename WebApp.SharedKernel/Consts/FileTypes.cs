
namespace WebApp.SharedKernel.Consts
{
    public static class FileTypes
    {
        public const string Image = "Image";
        public static readonly List<string> ImageExtensions = new List<string>() { ".jpg", ".jpeg", ".png" };

        public const string Document = "Document";
        public static readonly List<string> DocumentExtensions = new List<string>() { ".pdf" };

        public const string ReportDocument = "ReportDocument";
        public static readonly List<string> ReportDocumentExtensions = new List<string>() { ".pdf", ".xlsx", ".xls", ".docx", ".doc" };

        public const string Audio = "Audio";
        public const string Video = "Video";

        public static string GetReportExtension(string reportType)
        {
            var renderType = ReportDocumentExtensions.Where(x => x == ".pdf").Single();
            if (!string.IsNullOrEmpty(reportType))
            {
                switch (reportType.ToLower())
                {
                    default:
                    case "pdf":
                        renderType = ReportDocumentExtensions.Where(x => x == ".pdf").Single();
                        break;
                    case "word":
                        renderType = ReportDocumentExtensions.Where(x => x == ".doc").Single();
                        break;
                    case "excel":
                        renderType = ReportDocumentExtensions.Where(x => x == ".xls").Single();
                        break;
                }
            }
            return renderType;
        }

    }
}
