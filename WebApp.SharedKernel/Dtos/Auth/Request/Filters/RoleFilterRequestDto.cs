using WebApp.SharedKernel.Dtos.Request.Filters;

namespace WebApp.SharedKernel.Dtos.Auth.Request.Filters
{
    public class RoleFilterRequestDto : PagerFilterRequestDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}
