using WebApp.DataTransferObjects.DTOs;
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
        public Task<HolderOfDTO> GetAllAsync(RoleFilter roleFilter);
        public Task<HolderOfDTO> GetByIdAsync(string id);
        public Task<HolderOfDTO> SaveAsync(string roleName);
        public HolderOfDTO Delete(string id);
    }
}
