using WebApp.SharedKernel.Filters.Auth;
using WebApp.SharedKernel.Dtos;
using WebApp.SharedKernel.Dtos.Auth.Request;
using WebApp.SharedKernel.Dtos;

namespace WebApp.Core.Interfaces.Custom.Services.Auth
{
    public interface IUserService
    {
        Task<HolderOfDto> GetAllAsync(UserFilter userFilter);
        Task<HolderOfDto> GetByIdAsync(string userId);
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
