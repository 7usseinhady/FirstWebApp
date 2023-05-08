using WebApp.DataTransferObject.Dtos;
using WebApp.DataTransferObject.Filters;
using WebApp.DataTransferObject.Filters.Auth;
using WebApp.DataTransferObject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.DataTransferObject.Dtos;

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
