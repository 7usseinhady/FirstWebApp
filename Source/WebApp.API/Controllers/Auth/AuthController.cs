using WebApp.API.Bases;
using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Interfaces.Custom.Services.Auth;
using WebApp.SharedKernel.Dtos.Auth.Request;
using WebApp.SharedKernel.Dtos.Auth.Response;
using WebApp.SharedKernel.Dtos;
using WebApp.API.Consts;

namespace WebApp.API.Controllers
{
    [Route(Routes.apiBase)]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(HolderOfDto holderOfDto, IAuthService authService) : base(holderOfDto)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegisterRequestDto userRegisterRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _authService.RegisterUserAsync(userRegisterRequestDto, HttpContext));
        }

        //[Authorize(Roles = $"{Role.Admin}")]
        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdminAsync([FromBody] AdminRegisterRequestDto adminRegisterRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _authService.RegisterAdminAsync(adminRegisterRequestDto, HttpContext));
        }

        [HttpPost("ResendEmailConfirmationCode")]
        public async Task<IActionResult> ResendEmailConfirmationCodeAsync([FromBody] PersonalKeyRequestDto personalKeyRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _authService.ResendEmailConfirmationCodeAsync(personalKeyRequestDto, HttpContext));
        }

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync([FromBody] EmailPhoneConfirmationRequestDto emailConfirmationRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            _holderOfDto = await _authService.EmailConfirmationAsync(emailConfirmationRequestDto, HttpContext);
            return State(_holderOfDto);

        }

        [HttpPost("ResendPhoneConfirmationCode")]
        public async Task<IActionResult> ResendPhoneConfirmationCodeAsync([FromBody] PersonalKeyRequestDto personalKeyRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _authService.ResendPhoneConfirmationCodeAsync(personalKeyRequestDto, HttpContext));
        }

        [HttpPost("PhoneConfirmation")]
        public async Task<IActionResult> PhoneConfirmationAsync([FromBody] EmailPhoneConfirmationRequestDto emailConfirmationRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            _holderOfDto = await _authService.PhoneConfirmationAsync(emailConfirmationRequestDto, HttpContext);
            return State(_holderOfDto);


        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginRequestDto userLoginRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            _holderOfDto = await _authService.LoginAsync(userLoginRequestDto, HttpContext);
            return State(_holderOfDto);
        }
        
        [AllowAnonymous]
        [HttpPost("AutoLogin")]
        public async Task<IActionResult> AutoLoginAsync([FromBody] TokenRequestDto tokenRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            var refreshToken = tokenRequestDto.Token ??  Request.Cookies[Res.refreshToken];
            _holderOfDto = await _authService.AutoLoginAsync(refreshToken!, HttpContext);
            return State(_holderOfDto);
        }

        [AllowAnonymous]
        [HttpPost("RefreshTokens")]
        public async Task<IActionResult> RefreshTokensAsync([FromBody] TokenRequestDto tokenRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            var refreshToken = tokenRequestDto.Token ?? Request.Cookies[Res.refreshToken];
            refreshToken = refreshToken ?? "";
            _holderOfDto = await _authService.RefreshTokensAsync(refreshToken, HttpContext);
            return State(_holderOfDto);
        }

        //[Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequestDto changePasswordRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            changePasswordRequestDto.RefreshToken = changePasswordRequestDto.RefreshToken ?? Request.Cookies[Res.refreshToken];

            _holderOfDto = await _authService.ChangePasswordAsync(changePasswordRequestDto, HttpContext);

            return State(_holderOfDto);
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] PersonalKeyRequestDto personalKeyRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            _holderOfDto = await _authService.ForgotPasswordAsync(personalKeyRequestDto, HttpContext);

            return State(_holderOfDto);
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            _holderOfDto = await _authService.ResetPasswordAsync(resetPasswordRequestDto);

            return State(_holderOfDto);
        }

        //[Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] TokenRequestDto tokenRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            var refreshToken = tokenRequestDto.Token ?? Request.Cookies[Res.refreshToken];
            refreshToken = refreshToken ?? "";
            RequestUtils.DeleteCookie(HttpContext, Res.refreshToken);
            return State(await _authService.LogoutAsync(refreshToken, HttpContext));
        }

        [Authorize(Roles = $"{MainRoles.Admin}")]
        [HttpPost("addUserRole")]
        public async Task<IActionResult> AddUserRoleAsync([FromBody] UserRoleRequestDto userRoleRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _authService.AddUserToRoleAsync(userRoleRequestDto));
        }

        //[Authorize]
        [HttpPost("GetRoleUsers")]
        public async Task<IActionResult> GetRoleUsersAsync([FromBody] string roleName)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _authService.GetRoleUsersAsync(roleName));
        }

    }
}
