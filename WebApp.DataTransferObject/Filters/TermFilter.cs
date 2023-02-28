using WebApp.DataTransferObjects.Helpers;

namespace WebApp.DataTransferObjects.Filters
{
    public class TermFilter: Pager
    {
        public int? Id { get; set; }
        public string? TermTitle { get; set; }


    }
}
