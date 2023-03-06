using WebApp.DataTransferObjects.Dtos;
using WebApp.DataTransferObjects.Filters;
using WebApp.DataTransferObjects.Filters.Auth;
using WebApp.DataTransferObjects.Helpers;
using WebApp.DataTransferObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
