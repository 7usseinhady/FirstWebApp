using WebApp.DataTransferObjects.Helpers;

namespace WebApp.DataTransferObjects.Filters.Auth
{
    public class UserRoleFilter : Pager
    {
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
    }
}
