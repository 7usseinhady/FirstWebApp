using Microsoft.AspNetCore.Http;
using WebApp.SharedKernel.Dtos;
using WebApp.SharedKernel.Dtos.Auth.Request;

namespace WebApp.Core.Interfaces.Custom.Services.Auth
{
    public interface IAuthService
    {
        Task<HolderOfDto> RegisterAdminAsync(AdminRegisterRequestDto adminRegisterRequestDto, HttpContext? httpContext);
        Task<HolderOfDto> RegisterUserAsync(UserRegisterRequestDto userRegisterRequestDto, HttpContext? httpContext);

        Task<HolderOfDto> EmailConfirmationAsync(EmailPhoneConfirmationRequestDto emailConfirmationRequestDto, HttpContext? httpContext);
        Task<HolderOfDto> ResendEmailConfirmationCodeAsync(PersonalKeyRequestDto personalKeyRequestDto, HttpContext? httpContext);
        Task<HolderOfDto> PhoneConfirmationAsync(EmailPhoneConfirmationRequestDto phoneConfirmationRequestDto, HttpContext? httpContext);
        Task<HolderOfDto> ResendPhoneConfirmationCodeAsync(PersonalKeyRequestDto personalKeyRequestDto, HttpContext? httpContext);

        Task<HolderOfDto> AutoLoginAsync(string refreshToken, HttpContext? httpContext);
        Task<HolderOfDto> LoginAsync(UserLoginRequestDto userLoginRequestDto, HttpContext? httpContext);
        Task<HolderOfDto> RefreshTokensAsync(string refreshToken, HttpContext? httpContext);
        Task<HolderOfDto> LogoutAsync(string refreshToken, HttpContext? httpContext);

        Task<HolderOfDto> ChangePasswordAsync(ChangePasswordRequestDto changePasswordRequestDto, HttpContext? httpContext);
        Task<HolderOfDto> ForgotPasswordAsync(PersonalKeyRequestDto personalKeyRequestDto, HttpContext? httpContext);
        Task<HolderOfDto> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto);

        Task<HolderOfDto> AddUserToRoleAsync(UserRoleRequestDto userRoleRequestDto);
        Task<HolderOfDto> GetRoleUsersAsync(string roleName);
    }
}
