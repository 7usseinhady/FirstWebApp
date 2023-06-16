using WebApp.SharedKernel.Dtos;
using WebApp.SharedKernel.Dtos.Auth.Request;
using WebApp.SharedKernel.Dtos.Auth.Request.Filters;

namespace WebApp.Core.Interfaces.Custom.Services.Auth
{
    public interface IUserService
    {
        Task<HolderOfDto> GetAllAsync(UserFilterRequestDto userFilter);
        Task<HolderOfDto> GetByIdAsync(string userId);
        Task<HolderOfDto> GetByRefreshTokenAsync(string token);
        Task<HolderOfDto> UpdateAsync(UserEditRequestDto userRequestDto);
        Task<HolderOfDto> UpdateByRefreshTokenAsync(UserEditRequestDto userRequestDto);
        Task<HolderOfDto> UpdateUserDeviceIdAsync(UserDeviceIdRequestDto userDeviceIdRequestDto);
        Task<HolderOfDto> UpdateUserLangAsync(UserLangRequestDto userLangRequestDto);
        Task<HolderOfDto> DeactiveUserAsync(string userId);

        Task<HolderOfDto> ProfilePictureAsync(FileDto fileDto);
        Task<HolderOfDto> ProfilePictureRefreshTokenAsync(FileDto fileDto);

        Task<HolderOfDto> DeleteProfilePictureAsync(string id);
        Task<HolderOfDto> DeleteProfilePictureRefreshTokenAsync(string token);


    }
}
