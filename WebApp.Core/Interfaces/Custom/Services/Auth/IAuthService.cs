using Microsoft.AspNetCore.Http;
using WebApp.DataTransferObjects.DTOs.Auth.Request;
using WebApp.DataTransferObjects.Helpers;

namespace WebApp.Core.Interfaces.Custom.Services.Auth
{
    public interface IAuthService
    {
        Task<HolderOfDTO> RegisterAdminAsync(AdminRegisterRequestDTO adminRegisterRequestDTO, HttpContext? httpContext);
        Task<HolderOfDTO> RegisterUserAsync(UserRegisterRequestDTO userRegisterRequestDTO, HttpContext? httpContext);

        Task<HolderOfDTO> EmailConfirmationAsync(EmailPhoneConfirmationRequestDTO emailConfirmationRequestDTO, HttpContext? httpContext);
        Task<HolderOfDTO> ResendEmailConfirmationCodeAsync(PersonalKeyRequestDTO personalKeyRequestDTO, HttpContext? httpContext);
        Task<HolderOfDTO> PhoneConfirmationAsync(EmailPhoneConfirmationRequestDTO phoneConfirmationRequestDTO, HttpContext? httpContext);
        Task<HolderOfDTO> ResendPhoneConfirmationCodeAsync(PersonalKeyRequestDTO personalKeyRequestDTO, HttpContext? httpContext);

        Task<HolderOfDTO> AutoLoginAsync(string refreshToken, HttpContext? httpContext);
        Task<HolderOfDTO> LoginAsync(UserLoginRequestDTO userLoginRequestDTO, HttpContext? httpContext);
        Task<HolderOfDTO> RefreshTokensAsync(string refreshToken, HttpContext? httpContext);
        Task<HolderOfDTO> LogoutAsync(string refreshToken, HttpContext? httpContext);

        Task<HolderOfDTO> ChangePasswordAsync(ChangePasswordRequestDTO changePasswordRequestDTO, HttpContext? httpContext);
        Task<HolderOfDTO> ForgotPasswordAsync(PersonalKeyRequestDTO personalKeyRequestDTO, HttpContext? httpContext);
        Task<HolderOfDTO> ResetPasswordAsync(ResetPasswordRequestDTO resetPasswordRequestDTO);

        Task<HolderOfDTO> AddUserToRoleAsync(UserRoleRequestDTO userRoleDTO);
        Task<HolderOfDTO> GetRoleUsersAsync(string roleName);
    }
}
