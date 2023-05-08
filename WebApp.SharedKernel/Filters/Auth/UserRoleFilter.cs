namespace WebApp.SharedKernel.Filters.Auth
{
    public class UserRoleFilter : PagerFilter
    {
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
    }
}
