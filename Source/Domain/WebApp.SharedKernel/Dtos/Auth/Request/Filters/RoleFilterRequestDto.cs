using WebApp.SharedKernel.Bases;

namespace WebApp.SharedKernel.Dtos.Auth.Request.Filters
{
    public class RoleFilterRequestDto : BaseFilterRequestDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}
