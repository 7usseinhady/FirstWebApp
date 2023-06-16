using AutoMapper;
using WebApp.Core.Interfaces;
using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Helpers;
using WebApp.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Core.Entities.Auth;
using System.Data;
using Microsoft.Data.SqlClient;
using WebApp.Core.Helpers;
using System.Runtime.ExceptionServices;
using WebApp.SharedKernel.Dtos;
using WebApp.SharedKernel.Dtos.Response;
using WebApp.SharedKernel.Localization;

namespace WebApp.Core.Bases
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly HolderOfDto _holderOfDto;
        protected Indicator _indicator;
        protected readonly ConnectionStringConfiguration? _connectionStringConfiguration;

        protected BaseService(IUnitOfWork unitOfWork, IMapper mapper, HolderOfDto holderOfDto, Indicator indicator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _holderOfDto = holderOfDto;
            _indicator = indicator;
        }
        protected BaseService(IUnitOfWork unitOfWork, IMapper mapper, HolderOfDto holderOfDto, Indicator indicator, ConnectionStringConfiguration connectionStringConfiguration) : this(unitOfWork, mapper, holderOfDto, indicator)
        {
            _connectionStringConfiguration = connectionStringConfiguration;
        }
        


        protected async Task<HolderOfDto> GetUserIdAsync(UserManager<User> userManager, string? refreshToken)
        {
            try
            {
                // Check if Refresh Token is Empty
                if (string.IsNullOrEmpty(refreshToken))
                {
                    _holderOfDto.Add(Res.state, false);
                    _holderOfDto.Add(Res.message, "Refresh Token is required!");
                    return _holderOfDto;
                }
                // Get User by any Refresh Token Even if it is Inactive 
                var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.RefreshToken == refreshToken));

                if (user == null)
                {
                    _holderOfDto.Add(Res.state, false);
                    _holderOfDto.Add(Res.message, "Invalid token");
                    return _holderOfDto;
                }
                if (user.IsInactive)
                {
                    _holderOfDto.Add(Res.state, false);
                    _holderOfDto.Add(Res.message, "User is Inactive!");
                    return _holderOfDto;

                }
                var currentUserRefreshToken = user.RefreshTokens.Single(t => t.RefreshToken == refreshToken);
                if (!currentUserRefreshToken.isActive)
                {
                    _holderOfDto.Add(Res.state, false);
                    _holderOfDto.Add(Res.message, "Inactive token");
                    return _holderOfDto;
                }

                _holderOfDto.Add(Res.state, true);
                _holderOfDto.Add(Res.uid, user.Id);
                return _holderOfDto;
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.state, false);
                _holderOfDto.Add(Res.message, ex.Message);
                return _holderOfDto;
            }
        }

        protected static async Task<string?> GetUserDeviceTokenAsync(UserManager<User> userManager, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return null;

                var user = await userManager.FindByIdAsync(userId);

                if (user is null || user.IsInactive)
                    return null;

                return user.DeviceTokenId;
            }
            catch
            {
                return null;
            }
        }

        protected static async Task<ELanguages> GetUserLastLangAsync(UserManager<User> userManager, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return Culture.Default;

                var user = await userManager.FindByIdAsync(userId);

                if (user is null)
                    return Culture.Default;

                return Culture.GetLanguage(user.LastLang!);
            }
            catch
            {
                return Culture.Default;
            }
        }

        protected StoredProcedureResponseDto FireStoredProcedure(string storedProcedureName, List<SqlParameter>? sqlParameters)
        {
            var storedProcedureReturn = new StoredProcedureResponseDto();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionStringConfiguration?.ConStr))
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (sqlParameters is not null && sqlParameters.Any())
                        sqlDataAdapter.SelectCommand.Parameters.AddRange(sqlParameters.ToArray());
                    sqlDataAdapter.Fill(storedProcedureReturn.dataTable);
                    storedProcedureReturn.lParameters = sqlDataAdapter.GetFillParameters().ToList();
                }
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
            return storedProcedureReturn;
        }

    }
}
