using WebApp.API.Bases;
using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Interfaces.Custom.Services.Auth;
using WebApp.DataTransferObjects.DTOs.Auth.Request;
using WebApp.DataTransferObjects.DTOs.Auth.Response;
using WebApp.DataTransferObjects.Helpers;

namespace WebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : APIControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(HolderOfDTO holderOfDTO, IAuthService authService) : base(holderOfDTO)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegisterRequestDTO userRegisterRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _authService.RegisterUserAsync(userRegisterRequestDTO, HttpContext));
        }

        //[Authorize(Roles = $"{Role.Admin}")]
        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdminAsync([FromBody] AdminRegisterRequestDTO adminRegisterRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _authService.RegisterAdminAsync(adminRegisterRequestDTO, HttpContext));
        }

        [HttpPost("ResendEmailConfirmationCode")]
        public async Task<IActionResult> ResendEmailConfirmationCodeAsync([FromBody] PersonalKeyRequestDTO personalKeyRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _authService.ResendEmailConfirmationCodeAsync(personalKeyRequestDTO, HttpContext));
        }

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync([FromBody] EmailPhoneConfirmationRequestDTO emailConfirmationRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            _holderOfDTO = await _authService.EmailConfirmationAsync(emailConfirmationRequestDTO, HttpContext);
            CheckStateAndSetRefreshToken(HttpContext, _holderOfDTO);

            return State(_holderOfDTO);

        }

        [HttpPost("ResendPhoneConfirmationCode")]
        public async Task<IActionResult> ResendPhoneConfirmationCodeAsync([FromBody] PersonalKeyRequestDTO personalKeyRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _authService.ResendPhoneConfirmationCodeAsync(personalKeyRequestDTO, HttpContext));
        }

        [HttpPost("PhoneConfirmation")]
        public async Task<IActionResult> PhoneConfirmationAsync([FromBody] EmailPhoneConfirmationRequestDTO emailConfirmationRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            _holderOfDTO = await _authService.PhoneConfirmationAsync(emailConfirmationRequestDTO, HttpContext);
            CheckStateAndSetRefreshToken(HttpContext, _holderOfDTO);

            return State(_holderOfDTO);


        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginRequestDTO userLoginRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            _holderOfDTO = await _authService.LoginAsync(userLoginRequestDTO, HttpContext);
            CheckStateAndSetRefreshToken(HttpContext, _holderOfDTO);

            return State(_holderOfDTO);
        }
        
        [AllowAnonymous]
        [HttpPost("AutoLogin")]
        public async Task<IActionResult> AutoLoginAsync([FromBody] TokenRequestDTO tokenRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();
            var refreshToken = tokenRequestDTO.Token ??  Request.Cookies[Res.refreshToken];

            _holderOfDTO = await _authService.AutoLoginAsync(refreshToken!, HttpContext);

            CheckStateAndSetRefreshToken(HttpContext, _holderOfDTO);

            return State(_holderOfDTO);
        }

        [AllowAnonymous]
        [HttpPost("RefreshTokens")]
        public async Task<IActionResult> RefreshTokensAsync([FromBody] TokenRequestDTO tokenRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            var refreshToken = tokenRequestDTO.Token ?? Request.Cookies[Res.refreshToken];
            refreshToken = refreshToken ?? "";
            _holderOfDTO = await _authService.RefreshTokensAsync(refreshToken, HttpContext);

            CheckStateAndSetRefreshToken(HttpContext, _holderOfDTO);

            return State(_holderOfDTO);
        }

        //[Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequestDTO changePasswordRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            changePasswordRequestDTO.RefreshToken = changePasswordRequestDTO.RefreshToken ?? Request.Cookies[Res.refreshToken];

            _holderOfDTO = await _authService.ChangePasswordAsync(changePasswordRequestDTO, HttpContext);

            return State(_holderOfDTO);
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] PersonalKeyRequestDTO personalKeyRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            _holderOfDTO = await _authService.ForgotPasswordAsync(personalKeyRequestDTO, HttpContext);

            return State(_holderOfDTO);
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequestDTO resetPasswordRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            _holderOfDTO = await _authService.ResetPasswordAsync(resetPasswordRequestDTO);

            return State(_holderOfDTO);
        }

        //[Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] TokenRequestDTO tokenRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            var refreshToken = tokenRequestDTO.Token ?? Request.Cookies[Res.refreshToken];
            refreshToken = refreshToken ?? "";
            RequestUtils.DeleteCookie(HttpContext, Res.refreshToken);
            return State(await _authService.LogoutAsync(refreshToken, HttpContext));
        }

        [Authorize(Roles = $"{MainRoles.Admin}")]
        [HttpPost("addUserRole")]
        public async Task<IActionResult> AddUserRoleAsync([FromBody] UserRoleRequestDTO userRoleRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _authService.AddUserToRoleAsync(userRoleRequestDTO));
        }

        //[Authorize]
        [HttpPost("GetRoleUsers")]
        public async Task<IActionResult> GetRoleUsersAsync([FromBody] string roleName)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _authService.GetRoleUsersAsync(roleName));
        }

        private void CheckStateAndSetRefreshToken(HttpContext httpContext, HolderOfDTO holder)
        {
            UserAuthResponseDTO? userAuthResponseDTO = null;
            if ((bool)holder[Res.state])
            {
                if (holder.ContainsKey(Res.isConfirmed) && (bool)holder[Res.isConfirmed] && holder.ContainsKey(Res.oUserAuth))
                {
                    userAuthResponseDTO = (UserAuthResponseDTO)holder[Res.oUserAuth];
                }
            }
            if (userAuthResponseDTO is not null && !String.IsNullOrEmpty(userAuthResponseDTO.RefreshToken))
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = userAuthResponseDTO.RefreshTokenExpiration.ToUniversalTime(),
                    HttpOnly = true,
                    Secure = true
                };
                RequestUtils.SetCookie(httpContext, Res.refreshToken, userAuthResponseDTO.RefreshToken, cookieOptions);
            }
            else
                RequestUtils.DeleteCookie(httpContext, Res.refreshToken);
        }


    }
}
