using WebApp.DataTransferObjects.Filters.Auth;
using WebApp.SharedKernel.DTOs;
using WebApp.DataTransferObjects.DTOs.Auth.Request;
using WebApp.DataTransferObjects.Helpers;

namespace WebApp.Core.Interfaces.Custom.Services.Auth
{
    public interface IUserService
    {
        Task<HolderOfDTO> GetAllAsync(UserFilter userFilter);
        Task<HolderOfDTO> GetByIdAsync(string id);
        Task<HolderOfDTO> GetByRefreshTokenAsync(string token);
        Task<HolderOfDTO> UpdateAsync(UserRequestDTO userRequestDTO);
        Task<HolderOfDTO> UpdateByRefreshTokenAsync(UserRequestDTO userRequestDTO);
        Task<HolderOfDTO> UpdateUserDeviceIdAsync(UserDeviceIdRequestDTO userDeviceIdRequestDTO);
        Task<HolderOfDTO> UpdateUserLangAsync(UserLangRequestDTO userLangRequestDTO);
        Task<HolderOfDTO> DeactiveUserAsync(string userId);

        Task<HolderOfDTO> ProfilePictureAsync(FileDTO fileDTO);
        Task<HolderOfDTO> ProfilePictureRefreshTokenAsync(FileDTO fileDTO);

        Task<HolderOfDTO> DeleteProfilePictureAsync(string id);
        Task<HolderOfDTO> DeleteProfilePictureRefreshTokenAsync(string token);


    }
}
