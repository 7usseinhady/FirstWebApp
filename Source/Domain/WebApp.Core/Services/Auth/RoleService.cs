﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp.Core.Bases;
using WebApp.Core.Extensions;
using WebApp.Core.Interfaces;
using WebApp.Core.Interfaces.Custom.Services.Auth;
using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Dtos;
using WebApp.SharedKernel.Dtos.Auth.Request;
using WebApp.SharedKernel.Dtos.Auth.Request.Filters;
using WebApp.SharedKernel.Dtos.Auth.Response;
using WebApp.SharedKernel.Dtos.Response;
using WebApp.SharedKernel.Helpers;

namespace WebApp.Core.Services;

public class RoleService : BaseService, IRoleService
{
    public RoleService(IUnitOfWork unitOfWork, IMapper mapper, HolderOfDto holderOfDto, Indicator indicator) : base(unitOfWork, mapper, holderOfDto, indicator)
    {
    }

    public async Task<HolderOfDto> GetAllAsync(RoleFilterRequestDto roleFilter)
    {
        List<bool> lIndicator = new List<bool>();
        try
        {
            var query = await _unitOfWork.Roles.FilterQueryAsync(await _unitOfWork.Roles.BuildBaseQueryAsync(), roleFilter);
            int totalCount = await query.CountAsync();

            var paginator = new PaginatorResponseDto(totalCount, roleFilter.PageSize, roleFilter.CurrentPage, roleFilter.MaxPaginationWidth);
            _holderOfDto.Add(Res.paginator, paginator);
            lIndicator.Add(true);

            // pagination
            query = query.AddPage(paginator.Skip, paginator.Take);
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
            var roleFilter = new RoleFilterRequestDto() { Id = id };
            _holderOfDto.Add(Res.oRole, _mapper.Map<RoleResponseDto>(await (await _unitOfWork.Roles.FilterQueryAsync(await _unitOfWork.Roles.BuildBaseQueryAsync(), roleFilter)).SingleOrDefaultAsync()));
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
    public async Task<HolderOfDto> SaveAsync(RoleRequestDto roleRequestDto)
    {
        List<bool> lIndicator = new List<bool>();
        try
        {
            var role = _mapper.Map<Entities.Auth.Role>(roleRequestDto);
            role.Id = Guid.NewGuid().ToString();
            role.ConcurrencyStamp = Guid.NewGuid().ToString();

            await _unitOfWork.Roles.AddAsync(role);
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
    public async Task<HolderOfDto> UpdateAsync(RoleRequestDto roleRequestDto)
    {
        List<bool> lIndicator = new List<bool>();
        try
        {
            Entities.Auth.Role role = (await _unitOfWork.Roles.BuildBaseQueryAsync()).fi
            var role = _mapper.Map<Entities.Auth.Role>(roleRequestDto);
            role.Id = Guid.NewGuid().ToString();
            role.ConcurrencyStamp = Guid.NewGuid().ToString();

            await _unitOfWork.Roles.AddAsync(role);
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
            _unitOfWork.Roles.DeleteById(id);
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
