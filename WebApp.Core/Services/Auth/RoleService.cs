using AutoMapper;
using WebApp.Core.Bases;
using WebApp.Core.Entities;
using WebApp.Core.Interfaces;
using WebApp.Core.Interfaces.Custom.Services;
using WebApp.DataTransferObjects.DTOs;
using WebApp.DataTransferObjects.Filters;
using WebApp.DataTransferObjects.Interfaces;
using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApp.DataTransferObjects.Helpers;
using WebApp.DataTransferObjects.Filters.Auth;
using Microsoft.AspNetCore.Identity;
using WebApp.Core.Interfaces.Custom.Services.Auth;
using WebApp.Core.Extensions;
using WebApp.DataTransferObjects.DTOs.Auth.Response;
using WebApp.Core.Entities.Auth;

namespace WebApp.Core.Services
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, HolderOfDTO holderOfDTO, ICulture culture) : base(unitOfWork, mapper, holderOfDTO, culture)
        {
        }

        public async Task<HolderOfDTO> GetAllAsync(RoleFilter roleFilter)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                var query = await _unitOfWork.roles.BuildRoleQueryAsync(roleFilter);
                int totalCount = await query.CountAsync();

                var page = new Pager();
                page.Set(roleFilter.PageSize, roleFilter.CurrentPage, totalCount);
                _holderOfDTO.Add(Res.page, page);
                lIndicator.Add(true);

                // pagination
                query = query.AddPage(page.Skip, page.PageSize);
                _holderOfDTO.Add(Res.lRoles, _mapper.Map<List<RoleResponseDTO>>(await query.ToListAsync()));
                lIndicator.Add(true);
            }
            catch (Exception ex)
            {
                _holderOfDTO.Add(Res.message, ex.Message);
                
                lIndicator.Add(false);
            }
            _holderOfDTO.Add(Res.state, lIndicator.All(x => x));
            return _holderOfDTO;
        }
        public async Task<HolderOfDTO> GetByIdAsync(string id)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                var roleFilter = new RoleFilter() { Id = id };
                _holderOfDTO.Add(Res.oRole, _mapper.Map<RoleResponseDTO>(await (await _unitOfWork.roles.BuildRoleQueryAsync(roleFilter)).SingleOrDefaultAsync()));
                lIndicator.Add(true);
            }
            catch (Exception ex)
            {
                _holderOfDTO.Add(Res.message, ex.Message);
                
                lIndicator.Add(false);
            }
            _holderOfDTO.Add(Res.state, lIndicator.All(x => x));
            
            return _holderOfDTO;
        }
        public async Task<HolderOfDTO> SaveAsync(string roleName)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                var identityRole = new Role() { Id = Guid.NewGuid().ToString(), Name = roleName, NormalizedName = roleName.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() };
                await _unitOfWork.roles.AddAsync(identityRole);
                lIndicator.Add(_unitOfWork.Complete() > 0);
            }
            catch (Exception ex)
            {
                _holderOfDTO.Add(Res.message, ex.Message);
                
                lIndicator.Add(false);
            }
            _holderOfDTO.Add(Res.state, lIndicator.All(x => x));
            
            return _holderOfDTO;
        }
        public HolderOfDTO Delete(string id)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                _unitOfWork.roles.Delete(id);
                lIndicator.Add(_unitOfWork.Complete() > 0);
            }
            catch (Exception ex)
            {
                _holderOfDTO.Add(Res.message, ex.Message);
                
                lIndicator.Add(false);
            }
            _holderOfDTO.Add(Res.state, lIndicator.All(x => x));
            
            return _holderOfDTO;
        }
    }
}
