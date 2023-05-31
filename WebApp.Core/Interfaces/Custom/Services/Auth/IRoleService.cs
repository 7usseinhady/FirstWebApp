using WebApp.SharedKernel.Dtos;
using WebApp.SharedKernel.Dtos.Auth.Request;
using WebApp.SharedKernel.Dtos.Auth.Request.Filters;

namespace WebApp.Core.Interfaces.Custom.Services.Auth
{
    public interface IRoleService
    {
        public Task<HolderOfDto> GetAllAsync(RoleFilterRequestDto roleFilter);
        public Task<HolderOfDto> GetByIdAsync(string id);
        public Task<HolderOfDto> SaveAsync(RoleRequestDto roleRequestDto);
        public HolderOfDto Delete(string id);
    }
}
