using AutoMapper;
using WebApp.Core.Bases;
using WebApp.Core.Interfaces;
using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApp.DataTransferObject.Filters.Auth;
using WebApp.Core.Interfaces.Custom.Services.Auth;
using WebApp.Core.Extensions;
using WebApp.DataTransferObject.Dtos.Auth.Response;
using WebApp.Core.Entities.Auth;
using WebApp.DataTransferObject.Dtos;
using WebApp.DataTransferObject.Helpers;

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
                var query = _unitOfWork.roles.BuildRoleQuery(roleFilter);
                int totalCount = await query.CountAsync();

                var page = new PagerResponseDto(totalCount, roleFilter.PageSize, roleFilter.CurrentPage, roleFilter.MaxPaginationWidth);
                _holderOfDto.Add(Res.page, page);
                lIndicator.Add(true);

                // pagination
                query = query.AddPage(page.Skip, page.Take);
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
                _holderOfDto.Add(Res.oRole, _mapper.Map<RoleResponseDto>(await _unitOfWork.roles.BuildRoleQuery(roleFilter).SingleOrDefaultAsync()));
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
