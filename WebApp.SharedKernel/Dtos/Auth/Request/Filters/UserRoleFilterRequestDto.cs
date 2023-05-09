using WebApp.SharedKernel.Dtos.Request.Filters;

namespace WebApp.SharedKernel.Dtos.Auth.Request.Filters
{
    public class UserRoleFilterRequestDto : PagerFilterRequestDto
    {
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
    }
}
