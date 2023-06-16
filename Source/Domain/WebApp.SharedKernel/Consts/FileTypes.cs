
using System.Collections.Immutable;

namespace WebApp.SharedKernel.Consts
{
    public static class FileTypes
    {
        public const string Image = "Image";
        public static readonly ImmutableList<string> ImageExtensions = ImmutableList.Create(".jpg", ".jpeg", ".png");

        public const string Document = "Document";
        public static readonly ImmutableList<string> DocumentExtensions = ImmutableList.Create(".pdf");

        public const string ReportDocument = "ReportDocument";
        public static readonly ImmutableList<string> ReportDocumentExtensions = ImmutableList.Create(".pdf", ".xlsx", ".xls", ".docx", ".doc");

        public const string Audio = "Audio";
        public const string Video = "Video";

        public static string GetReportExtension(string reportType)
        {
            var renderType = ReportDocumentExtensions.Single(x => x == ".pdf");
            if (!string.IsNullOrEmpty(reportType))
            {
                switch (reportType.ToLower())
                {
                    default:
                        renderType = ReportDocumentExtensions.Single(x => x == ".pdf");
                        break;
                    case "word":
                        renderType = ReportDocumentExtensions.Single(x => x == ".doc");
                        break;
                    case "excel":
                        renderType = ReportDocumentExtensions.Single(x => x == ".xls");
                        break;
                }
            }
            return renderType;
        }

    }
}
