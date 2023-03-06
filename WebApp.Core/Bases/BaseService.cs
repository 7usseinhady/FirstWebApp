using AutoMapper;
using WebApp.Core.Interfaces;
using WebApp.DataTransferObjects.Helpers;
using WebApp.DataTransferObjects.Interfaces;
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
using WebApp.DataTransferObject.Dtos;
using WebApp.DataTransferObject.Dtos.Response;

namespace WebApp.Core.Bases
{
    public class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly HolderOfDto _holderOfDto;
        protected readonly ICulture _culture;
        protected readonly ConnectionStringConfiguration? _connectionStringConfiguration;
        protected BaseService(IUnitOfWork unitOfWork, IMapper mapper, HolderOfDto holderOfDto, ICulture culture)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _holderOfDto = holderOfDto;
            _culture = culture;
        }
        protected BaseService(IUnitOfWork unitOfWork, IMapper mapper, HolderOfDto holderOfDto, ICulture culture, ConnectionStringConfiguration connectionStringConfiguration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _holderOfDto = holderOfDto;
            _culture = culture;
            _connectionStringConfiguration = connectionStringConfiguration;
        }
        


        protected async Task<HolderOfDto> GetUserIdAsync(UserManager<User> userManager, string refreshToken)
        {
            HolderOfDto internalHolder = new HolderOfDto();
            // Check if Refresh Token is Empty
            if (string.IsNullOrEmpty(refreshToken))
            {
                internalHolder.Add(Res.state, false);
                internalHolder.Add(Res.message, "Refresh Token is required!");
                return internalHolder;
            }
            // Get User by any Refresh Token Even if it is Inactive 
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.RefreshToken == refreshToken));

            if (user == null)
            {
                internalHolder.Add(Res.state, false);
                internalHolder.Add(Res.message, "Invalid token");
                return internalHolder;
            }
            if (user.IsInactive)
            {
                internalHolder.Add(Res.state, false);
                internalHolder.Add(Res.message, "User is Inactive!");
                return internalHolder;

            }
            var currentUserRefreshToken = user.RefreshTokens.Single(t => t.RefreshToken == refreshToken);
            if (!currentUserRefreshToken.isActive)
            {
                internalHolder.Add(Res.state, false);
                internalHolder.Add(Res.message, "Inactive token");
                return internalHolder;
            }

            internalHolder.Add(Res.state, true);
            internalHolder.Add(Res.uid, user.Id);
            return internalHolder;
        }

        protected async Task<string?> GetUserDeviceTokenAsync(UserManager<User> userManager, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return null;

                var user = await userManager.FindByIdAsync(userId);

                if (user is null)
                    return null;

                return user.DeviceTokenId;
            }
            catch
            {
                return null;
            }
        }

        protected async Task<string> GetUserLastLangAsync(UserManager<User> userManager, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return "ar";

                var user = await userManager.FindByIdAsync(userId);

                if (user is null)
                    return "ar";

                return user.LastLang ?? "ar";
            }
            catch
            {
                return "ar";
            }
        }
        
        protected async Task<int> GetUserLastLangIndexAsync(UserManager<User> userManager, string userId)
        {
            try
            {
                return Culture.getIndexOfCulture(await GetUserLastLangAsync(userManager, userId));
            }
            catch
            {
                return 0;
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
