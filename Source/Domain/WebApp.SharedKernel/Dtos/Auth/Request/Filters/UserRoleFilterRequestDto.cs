using WebApp.SharedKernel.Bases;

namespace WebApp.SharedKernel.Dtos.Auth.Request.Filters
{
    public class UserRoleFilterRequestDto : BaseFilterRequestDto
    {
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
    }
}
