using WebApp.SharedKernel.Dtos;
using WebApp.SharedKernel.Filters;
using WebApp.SharedKernel.Filters.Auth;
using WebApp.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.SharedKernel.Dtos;

namespace WebApp.Core.Interfaces.Custom.Services.Auth
{
    public interface IRoleService
    {
        public Task<HolderOfDto> GetAllAsync(RoleFilter roleFilter);
        public Task<HolderOfDto> GetByIdAsync(string id);
        public Task<HolderOfDto> SaveAsync(string roleName);
        public HolderOfDto Delete(string id);
    }
}
