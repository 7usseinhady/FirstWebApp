using AutoMapper;
using WebApp.Core.Bases;
using WebApp.Core.Entities;
using WebApp.Core.Interfaces;
using WebApp.Core.Interfaces.Custom.Services;
using WebApp.DataTransferObjects.Dtos;
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
using WebApp.DataTransferObjects.Dtos.Auth.Response;
using WebApp.Core.Entities.Auth;
using WebApp.DataTransferObject.Dtos;

namespace WebApp.Core.Services
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, HolderOfDto holderOfDto, ICulture culture) : base(unitOfWork, mapper, holderOfDto, culture)
        {
        }

        public async Task<HolderOfDto> GetAllAsync(RoleFilter roleFilter)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                var query = await _unitOfWork.roles.BuildRoleQueryAsync(roleFilter);
                int totalCount = await query.CountAsync();

                var page = new Pager();
                page.Set(roleFilter.PageSize, roleFilter.CurrentPage, totalCount);
                _holderOfDto.Add(Res.page, page);
                lIndicator.Add(true);

                // pagination
                query = query.AddPage(page.Skip, page.PageSize);
                _holderOfDto.Add(Res.lRoles, _mapper.Map<List<RoleResponseDto>>(await query.ToListAsync()));
                lIndicator.Add(true);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);
                
                lIndicator.Add(false);
            }
            _holderOfDto.Add(Res.state, lIndicator.All(x => x));
            return _holderOfDto;
        }
        public async Task<HolderOfDto> GetByIdAsync(string id)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                var roleFilter = new RoleFilter() { Id = id };
                _holderOfDto.Add(Res.oRole, _mapper.Map<RoleResponseDto>(await (await _unitOfWork.roles.BuildRoleQueryAsync(roleFilter)).SingleOrDefaultAsync()));
                lIndicator.Add(true);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);
                
                lIndicator.Add(false);
            }
            _holderOfDto.Add(Res.state, lIndicator.All(x => x));
            
            return _holderOfDto;
        }
        public async Task<HolderOfDto> SaveAsync(string roleName)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                var role = new Role() { Id = Guid.NewGuid().ToString(), Name = roleName, NormalizedName = roleName.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() };
                await _unitOfWork.roles.AddAsync(role);
                lIndicator.Add(_unitOfWork.Complete() > 0);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);
                
                lIndicator.Add(false);
            }
            _holderOfDto.Add(Res.state, lIndicator.All(x => x));
            
            return _holderOfDto;
        }
        public HolderOfDto Delete(string id)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                _unitOfWork.roles.DeleteById(id);
                lIndicator.Add(_unitOfWork.Complete() > 0);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);
                
                lIndicator.Add(false);
            }
            _holderOfDto.Add(Res.state, lIndicator.All(x => x));
            
            return _holderOfDto;
        }
    }
}
