using WebApp.DataTransferObjects.Filters.Auth;
using WebApp.SharedKernel.Dtos;
using WebApp.DataTransferObjects.Dtos.Auth.Request;
using WebApp.DataTransferObjects.Helpers;

namespace WebApp.Core.Interfaces.Custom.Services.Auth
{
    public interface IUserService
    {
        Task<HolderOfDto> GetAllAsync(UserFilter userFilter);
        Task<HolderOfDto> GetByIdAsync(string id);
        Task<HolderOfDto> GetByRefreshTokenAsync(string token);
        Task<HolderOfDto> UpdateAsync(UserRequestDto userRequestDto);
        Task<HolderOfDto> UpdateByRefreshTokenAsync(UserRequestDto userRequestDto);
        Task<HolderOfDto> UpdateUserDeviceIdAsync(UserDeviceIdRequestDto userDeviceIdRequestDto);
        Task<HolderOfDto> UpdateUserLangAsync(UserLangRequestDto userLangRequestDto);
        Task<HolderOfDto> DeactiveUserAsync(string userId);

        Task<HolderOfDto> ProfilePictureAsync(FileDto fileDto);
        Task<HolderOfDto> ProfilePictureRefreshTokenAsync(FileDto fileDto);

        Task<HolderOfDto> DeleteProfilePictureAsync(string id);
        Task<HolderOfDto> DeleteProfilePictureRefreshTokenAsync(string token);


    }
}
