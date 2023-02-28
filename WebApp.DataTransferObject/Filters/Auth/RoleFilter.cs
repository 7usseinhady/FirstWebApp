using WebApp.DataTransferObjects.Helpers;

namespace WebApp.DataTransferObjects.Filters.Auth
{
    public class RoleFilter : Pager
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}
