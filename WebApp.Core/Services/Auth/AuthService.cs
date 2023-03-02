using AutoMapper;
using WebApp.Core.Bases;
using WebApp.Core.Entities.Auth;
using WebApp.Core.Interfaces;
using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using WebApp.SharedKernel.Helpers;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using WebApp.Core.Interfaces.Custom.Services.Auth;
using WebApp.DataTransferObjects.DTOs.Auth.Request;
using WebApp.SharedKernel.Helpers.Email;
using WebApp.DataTransferObjects.DTOs.Auth.Response;
using WebApp.DataTransferObjects.Helpers;

namespace WebApp.Core.Services.Auth
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly Helpers.JWT _jwt;
        private readonly IServer _server;
        private readonly ISMSService _smsService;
        private readonly IFileUtils _fileUtils;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, HolderOfDTO holderOfDTO, ICulture culture, RoleManager<Role> roleManager, UserManager<User> userManager, IEmailSender emailSender, IOptions<Helpers.JWT> jwt, IServer server, ISMSService smsService, IFileUtils fileUtils) : base(unitOfWork, mapper, holderOfDTO, culture)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _jwt = jwt.Value;
            _server = server;
            _smsService = smsService;
            _fileUtils = fileUtils;
        }

        #region Registeration and Answer
        public async Task<HolderOfDTO> RegisterAdminAsync(AdminRegisterRequestDTO adminRegisterRequestDTO, HttpContext? httpContext)
        {
            if (adminRegisterRequestDTO.IsBasedEmail)
            {
                // Check mail if Exists or valid
                if (string.IsNullOrEmpty(adminRegisterRequestDTO.Email))
                {
                    _holderOfDTO.Add(Res.state, false);
                    _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Email is required"].Value);
                    return _holderOfDTO;
                }
                else if (!ObjectUtils.IsValidEmail(adminRegisterRequestDTO.Email))
                {
                    _holderOfDTO.Add(Res.state, false);
                    _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Email is not correct"].Value);
                    return _holderOfDTO;
                }
                else if (await _userManager.FindByEmailAsync(adminRegisterRequestDTO.Email) is not null)
                {
                    _holderOfDTO.Add(Res.state, false);
                    _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Email is already registered"].Value);
                    return _holderOfDTO;
                }
            }
            else
            {
                // Check Phone if Exists or valid
                if (string.IsNullOrEmpty(adminRegisterRequestDTO.PhoneNumber))
                {
                    _holderOfDTO.Add(Res.state, false);
                    _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Phone is required"].Value);
                    return _holderOfDTO;
                }
                else if (!ObjectUtils.IsPhoneNumber(adminRegisterRequestDTO.PhoneNumber))
                {
                    _holderOfDTO.Add(Res.state, false);
                    _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Phone is not correct"].Value);
                    return _holderOfDTO;
                }
                else if (_userManager.Users.Where(x => x.PhoneNumber == adminRegisterRequestDTO.PhoneNumber).Count() > 0)
                {
                    _holderOfDTO.Add(Res.state, false);
                    _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Phone is already registered"].Value);
                    return _holderOfDTO;
                }
            }

            // Check username if Exists
            if (!string.IsNullOrEmpty(adminRegisterRequestDTO.Username) && await _userManager.FindByNameAsync(adminRegisterRequestDTO.Username) is not null)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Username is already registered"].Value);
                return _holderOfDTO;
            }

            // Check Role
            if (!await _roleManager.RoleExistsAsync(adminRegisterRequestDTO.RoleName))
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid Role"].Value);
                return _holderOfDTO;
            }

            // Create User With Hashing Password
            var user = _mapper.Map<User>(adminRegisterRequestDTO);
            user.UserInsertDate = DateTime.Now;
            var resultUser = await _userManager.CreateAsync(user, adminRegisterRequestDTO.Password);
            if (!resultUser.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in resultUser.Errors)
                    errors += $"{error.Description},";
                errors = errors.Remove(errors.Length - 1, 1);
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, errors);
                return _holderOfDTO;
            }
            // Add User To Role.
            var resultRole = await _userManager.AddToRoleAsync(user, adminRegisterRequestDTO.RoleName);
            if (!resultRole.Succeeded)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["User was created successfully"].Value);
                return _holderOfDTO;
            }

            if (adminRegisterRequestDTO.RoleName == SharedKernel.Consts.MainRoles.User)
            {
                if (user.IsBasedEmail)
                    return await sendEmailConfirmationCodeAsync(user, httpContext);
                else
                    return await sendPhoneConfirmationCodeAsync(user, httpContext);
            }

            _holderOfDTO.Add(Res.state, true);
            _holderOfDTO.Add(Res.isConfirmed, false);
            _holderOfDTO.Add(Res.basedEmail, user.IsBasedEmail);
            return _holderOfDTO;
        }

        public async Task<HolderOfDTO> RegisterUserAsync(UserRegisterRequestDTO userRegisterRequestDTO, HttpContext? httpContext)
        {
            if (!await _roleManager.RoleExistsAsync(SharedKernel.Consts.MainRoles.User))
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid Role"].Value);
                return _holderOfDTO;
            }
            var user = _mapper.Map<AdminRegisterRequestDTO>(userRegisterRequestDTO);
            user.RoleName = SharedKernel.Consts.MainRoles.User;
            return await RegisterAdminAsync(user, httpContext);
        }

        #endregion

        #region Confirmation Code
        public async Task<HolderOfDTO> EmailConfirmationAsync(EmailPhoneConfirmationRequestDTO emailConfirmationRequestDTO, HttpContext? httpContext)
        {
            var user = await getUserAsync(emailConfirmationRequestDTO.PersonalKey);
            if (user == null)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid Personal Key"].Value);
                return _holderOfDTO;
            }
            if (user.IsInactive)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["User is Inactive"].Value);
                return _holderOfDTO;

            }
            if (string.IsNullOrEmpty(emailConfirmationRequestDTO.TokenCode) || emailConfirmationRequestDTO.TokenCode.Length != 6)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid Code"].Value);
                return _holderOfDTO;
            }

            // Get Current ResetPasswordToken by Code
            var currentResetPasswordToken = user.ValidationTokens.SingleOrDefault(t => t.ValidationCode == emailConfirmationRequestDTO.TokenCode);

            if (currentResetPasswordToken is null || !currentResetPasswordToken.isActive)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid or Inactive Code"].Value);
                return _holderOfDTO;
            }

            var result = await _userManager.ConfirmEmailAsync(user, currentResetPasswordToken.ValidationToken);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error.Description},";
                errors = errors.Remove(errors.Length - 1, 1);
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, errors);
                return _holderOfDTO;
            }
            //
            user.ValidationTokens.ToList().RemoveAll(t => !t.isUsed && (t.isRevoked || t.isExpired));
            currentResetPasswordToken.isUsed = true;
            user.ValidationTokens.Add(currentResetPasswordToken);
            //
            var newUserRefreshToken = GenerateUserRefreshToken(httpContext);
            user.RefreshTokens.Add(newUserRefreshToken);
            // Save Updates
            await _userManager.UpdateAsync(user);

            user.IsBasedEmail = true;
            var holderResult = await BuildUserAuthAsync(user, newUserRefreshToken);
            return holderResult;
        }

        public async Task<HolderOfDTO> ResendEmailConfirmationCodeAsync(PersonalKeyRequestDTO personalKeyRequestDTO, HttpContext? httpContext)
        {
            var user = await getUserAsync(personalKeyRequestDTO.PersonalKey);

            if (user == null)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid personal key"].Value);
                return _holderOfDTO;
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return await sendEmailConfirmationCodeAsync(user, httpContext);

            _holderOfDTO.Add(Res.state, true);

            if (user.IsBasedEmail)
            {
                _holderOfDTO.Add(Res.isConfirmed, user.EmailConfirmed);
                _holderOfDTO.Add(Res.basedEmail, true);
            }
            else
            {
                _holderOfDTO.Add(Res.isConfirmed, user.PhoneNumberConfirmed);
                _holderOfDTO.Add(Res.basedEmail, false);
            }
            return _holderOfDTO;
        }

        public async Task<HolderOfDTO> PhoneConfirmationAsync(EmailPhoneConfirmationRequestDTO phoneConfirmationRequestDTO, HttpContext? httpContext)
        {
            var user = await getUserAsync(phoneConfirmationRequestDTO.PersonalKey);
            if (user == null)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid Personal Key"].Value);
                return _holderOfDTO;
            }
            if (user.IsInactive)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["User is Inactive"].Value);
                return _holderOfDTO;

            }
            if (string.IsNullOrEmpty(phoneConfirmationRequestDTO.TokenCode) || phoneConfirmationRequestDTO.TokenCode.Length != 6)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid Code"].Value);
                return _holderOfDTO;
            }

            // Get Current ResetPasswordToken by Code
            var currentResetPasswordToken = user.ValidationTokens.SingleOrDefault(t => t.ValidationCode == phoneConfirmationRequestDTO.TokenCode);

            if (currentResetPasswordToken is null || !currentResetPasswordToken.isActive || currentResetPasswordToken.ValidationToken != "UserPhoneConfirmation")
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid or Inactive Code"].Value);
                return _holderOfDTO;
            }

            user.PhoneNumberConfirmed = true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error.Description},";
                errors = errors.Remove(errors.Length - 1, 1);
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, errors);
                return _holderOfDTO;
            }
            //
            user.ValidationTokens.ToList().RemoveAll(t => !t.isUsed && (t.isRevoked || t.isExpired));
            currentResetPasswordToken.isUsed = true;
            user.ValidationTokens.Add(currentResetPasswordToken);
            //
            var newUserRefreshToken = GenerateUserRefreshToken(httpContext);
            user.RefreshTokens.Add(newUserRefreshToken);
            // Save Updates
            await _userManager.UpdateAsync(user);

            user.IsBasedEmail = false;
            var holderResult = await BuildUserAuthAsync(user, newUserRefreshToken);
            return holderResult;
        }

        public async Task<HolderOfDTO> ResendPhoneConfirmationCodeAsync(PersonalKeyRequestDTO personalKeyRequestDTO, HttpContext? httpContext)
        {
            var user = await getUserAsync(personalKeyRequestDTO.PersonalKey);

            if (user == null)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid personal key"].Value);
                return _holderOfDTO;
            }

            if (!await _userManager.IsPhoneNumberConfirmedAsync(user))
                return await sendPhoneConfirmationCodeAsync(user, httpContext);

            _holderOfDTO.Add(Res.state, true);

            if (user.IsBasedEmail)
            {
                _holderOfDTO.Add(Res.isConfirmed, user.EmailConfirmed);
                _holderOfDTO.Add(Res.basedEmail, true);
            }
            else
            {
                _holderOfDTO.Add(Res.isConfirmed, user.PhoneNumberConfirmed);
                _holderOfDTO.Add(Res.basedEmail, false);
            }
            return _holderOfDTO;
        }
        #endregion

        #region Login and Logout
        public async Task<HolderOfDTO> LoginAsync(UserLoginRequestDTO userLoginRequestDTO, HttpContext? httpContext)
        {
            var user = await getUserAsync(userLoginRequestDTO.PersonalKey);

            if (user is null || !await _userManager.CheckPasswordAsync(user, userLoginRequestDTO.Password))
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Personal Key or Password is incorrect"].Value);
                return _holderOfDTO;
            }
            if (user.IsInactive)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["User is Inactive"].Value);
                return _holderOfDTO;

            }

            bool loginWith = user.IsBasedEmail;
            if (ObjectUtils.IsValidEmail(userLoginRequestDTO.PersonalKey))
                loginWith = true;
            else if (ObjectUtils.IsPhoneNumber(userLoginRequestDTO.PersonalKey))
                loginWith = false;


            if (loginWith)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                    return await sendEmailConfirmationCodeAsync(user, httpContext);
            }
            else
            {
                if (!await _userManager.IsPhoneNumberConfirmedAsync(user))
                    return await sendPhoneConfirmationCodeAsync(user, httpContext);
            }

            //
            user.ValidationTokens.ToList().RemoveAll(t => !t.isUsed && (t.isRevoked || t.isExpired));
            //
            user.RefreshTokens.ToList().RemoveAll(t => !t.isActive);
            var userRefreshToken = GenerateUserRefreshToken(httpContext);
            user.RefreshTokens.ToList().RemoveAll(t => t.IpAdress == userRefreshToken.IpAdress && t.Agent == userRefreshToken.Agent);
            user.RefreshTokens.Add(userRefreshToken);
            // 
            await _userManager.UpdateAsync(user);

            user.IsBasedEmail = loginWith;
            return await BuildUserAuthAsync(user, userRefreshToken);
        }

        public async Task<HolderOfDTO> AutoLoginAsync(string refreshToken, HttpContext? httpContext)
        {
            return await RefreshTokensAsync(refreshToken, httpContext);
        }

        public async Task<HolderOfDTO> RefreshTokensAsync(string refreshToken, HttpContext? httpContext)
        {
            // Check if Refresh Token is Empty
            if (string.IsNullOrEmpty(refreshToken))
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Refresh Token is required"].Value);
                return _holderOfDTO;
            }
            // Get User by any Refresh Token Even if it is Inactive 
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.RefreshToken == refreshToken));

            if (user == null)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid token"].Value);
                return _holderOfDTO;
            }
            if (user.IsInactive)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["User is Inactive"].Value);
                return _holderOfDTO;

            }

            user.ValidationTokens.ToList().RemoveAll(t => !t.isUsed && (t.isRevoked || t.isExpired));
            //
            var currentUserRefreshToken = user.RefreshTokens.Single(t => t.RefreshToken == refreshToken);
            user.RefreshTokens.ToList().RemoveAll(t => t.IpAdress == currentUserRefreshToken.IpAdress && t.Agent == currentUserRefreshToken.Agent);
            user.RefreshTokens.ToList().RemoveAll(t => !t.isActive);
            if (!currentUserRefreshToken.isActive)
            {
                await _userManager.UpdateAsync(user);
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Inactive token"].Value);
                return _holderOfDTO;
            }

            if (user.IsBasedEmail)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                    return await sendEmailConfirmationCodeAsync(user, httpContext);
            }
            else
            {
                if (!await _userManager.IsPhoneNumberConfirmedAsync(user))
                    return await sendPhoneConfirmationCodeAsync(user, httpContext);
            }

            currentUserRefreshToken = GenerateUserRefreshToken(httpContext);
            user.RefreshTokens.Add(currentUserRefreshToken);
            await _userManager.UpdateAsync(user);

            return await BuildUserAuthAsync(user, currentUserRefreshToken);
        }

        public async Task<HolderOfDTO> LogoutAsync(string refreshToken, HttpContext? httpContext)
        {
            // Check if Refresh Token is Empty
            if (string.IsNullOrEmpty(refreshToken))
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Refresh Token is required"].Value);
                return _holderOfDTO;
            }

            // Get User by any Refresh Token Even if it is Inactive 
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.RefreshToken == refreshToken));

            if (user == null)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid token"].Value);
                return _holderOfDTO;
            }

            // Get Current UserRefreshToken by refreshToken
            var currentUserRefreshToken = user.RefreshTokens.Single(t => t.RefreshToken == refreshToken);

            user.RefreshTokens.Remove(currentUserRefreshToken);
            user.RefreshTokens.ToList().RemoveAll(t => !t.isActive);

            if (!currentUserRefreshToken.isActive)
            {
                await _userManager.UpdateAsync(user);
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid token"].Value);
                return _holderOfDTO;
            }
            await _userManager.UpdateAsync(user);

            _holderOfDTO.Add(Res.state, true);
            return _holderOfDTO;
        }
        #endregion

        #region Forgot and Change Password
        public async Task<HolderOfDTO> ChangePasswordAsync(ChangePasswordRequestDTO changePasswordRequestDTO, HttpContext? httpContext)
        {
            // Check if Refresh Token is Empty
            if (string.IsNullOrEmpty(changePasswordRequestDTO.RefreshToken))
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Refresh Token is required"].Value);
                return _holderOfDTO;
            }

            // Get User by any Refresh Token Even if it is Inactive 
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.RefreshToken == changePasswordRequestDTO.RefreshToken));

            if (user == null)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid token"].Value);
                return _holderOfDTO;
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return await sendEmailConfirmationCodeAsync(user, httpContext);

            if (user.IsInactive)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["User is Inactive"].Value);
                return _holderOfDTO;

            }

            // Get Current UserRefreshToken by refreshToken
            var currentUserRefreshToken = user.RefreshTokens.Single(t => t.RefreshToken == changePasswordRequestDTO.RefreshToken);

            if (!currentUserRefreshToken.isActive)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Inactive token"].Value);
                return _holderOfDTO;
            }

            if (!await _userManager.CheckPasswordAsync(user, changePasswordRequestDTO.OldPassword))
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Old Password is incorrect"].Value);
                return _holderOfDTO;
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordRequestDTO.OldPassword, changePasswordRequestDTO.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error.Description},";
                errors = errors.Remove(errors.Length - 1, 1);
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, errors);
                return _holderOfDTO;
            }
            // log out user
            user.RefreshTokens.Remove(currentUserRefreshToken);

            // save change password history
            var changePasswordToken = GenerateUserValidationToken(httpContext, "UserChangePassword");
            changePasswordToken.isUsed = true;
            user.ValidationTokens.Add(changePasswordToken);
            await _userManager.UpdateAsync(user);

            var messageContent = _culture.SharedLocalizer["WebApp Password Changed"].Value;

            if (!string.IsNullOrEmpty(user.Email) && user.EmailConfirmed)
            {
                try
                {
                    // send change password notification to email
                    var message = new EmailMessage(new List<string>() { user.Email }, _culture.SharedLocalizer["WebApp App, Did you change your Password"].Value, messageContent, null);
                    _emailSender.SendEmailAsync(message);
                }
                catch { }
            }
            //if (!string.IsNullOrEmpty(user.PhoneNumber) && user.PhoneNumberConfirmed)
            //{
            //    try
            //    {
            //        // send change password notification to PhoneNumber
            //        _smsService.Send(user.PhoneNumber, messageContent);
            //    }
            //    catch { }
            //}

            _holderOfDTO.Add(Res.state, true);
            if (user.IsBasedEmail)
            {
                _holderOfDTO.Add(Res.isConfirmed, user.EmailConfirmed);
                _holderOfDTO.Add(Res.basedEmail, true);
            }
            else
            {
                _holderOfDTO.Add(Res.isConfirmed, user.PhoneNumberConfirmed);
                _holderOfDTO.Add(Res.basedEmail, false);
            }
            return _holderOfDTO;
        }

        public async Task<HolderOfDTO> ForgotPasswordAsync(PersonalKeyRequestDTO personalKeyRequestDTO, HttpContext? httpContext)
        {
            var user = await getUserAsync(personalKeyRequestDTO.PersonalKey);
            if (user == null)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid Personal Key"].Value);
                return _holderOfDTO;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var userResetPasswordToken = GenerateUserValidationToken(httpContext, token);
            user.ValidationTokens.Add(userResetPasswordToken);
            user.ValidationTokens.ToList().RemoveAll(t => !t.isUsed && (t.isRevoked || t.isExpired));
            await _userManager.UpdateAsync(user);

            var messageContent = $"{_culture.SharedLocalizer["Your Reset Code is"].Value} : {userResetPasswordToken.ValidationCode}";
            if (!string.IsNullOrEmpty(user.Email))
            {
                try
                {
                    var message = new EmailMessage(new List<string>() { user.Email }, "Acceptance WebApp Request", messageContent, null);
                    _emailSender.SendEmailAsync(message);
                }
                catch
                { }
            }
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                try
                {
                    _smsService.SendAsync(user.PhoneNumber, messageContent);
                }
                catch
                { }
            }
            _holderOfDTO.Add(Res.state, true);
            return _holderOfDTO;
        }

        public async Task<HolderOfDTO> ResetPasswordAsync(ResetPasswordRequestDTO resetPasswordRequestDTO)
        {
            var user = await getUserAsync(resetPasswordRequestDTO.personalKey);
            if (user == null)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid email"].Value);
                return _holderOfDTO;
            }

            if (string.IsNullOrEmpty(resetPasswordRequestDTO.TokenCode) || resetPasswordRequestDTO.TokenCode.Length != 6)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid Code"].Value);
                return _holderOfDTO;
            }

            // Get Current ResetPasswordToken by Code
            var currentResetPasswordToken = user.ValidationTokens.SingleOrDefault(t => t.ValidationCode == resetPasswordRequestDTO.TokenCode);

            if (currentResetPasswordToken is null || !currentResetPasswordToken.isActive)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid or Inactive Code"].Value);
                return _holderOfDTO;
            }

            var result = await _userManager.ResetPasswordAsync(user, currentResetPasswordToken.ValidationToken, resetPasswordRequestDTO.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error.Description},";
                errors = errors.Remove(errors.Length - 1, 1);
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, errors);
                return _holderOfDTO;
            }
            user.ValidationTokens.ToList().RemoveAll(t => !t.isUsed && (t.isRevoked || t.isExpired));
            currentResetPasswordToken.isUsed = true;
            user.ValidationTokens.Add(currentResetPasswordToken);
            await _userManager.UpdateAsync(user);

            _holderOfDTO.Add(Res.state, true);
            return _holderOfDTO;

        }
        #endregion

        #region Addational Methods
        public async Task<HolderOfDTO> AddUserToRoleAsync(UserRoleRequestDTO userRoleRequestDTO)
        {
            var user = await _userManager.FindByIdAsync(userRoleRequestDTO.UserId);
            var roleName = await getRoleNameAsync(userRoleRequestDTO.RoleId);
            if (user is null || roleName is null)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Invalid user ID or Role"].Value);
                return _holderOfDTO;
            }
            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["User already assigned to this role"].Value);
                return _holderOfDTO;
            }
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["Something went wrong when tring to add role to user"].Value);
                return _holderOfDTO;
            }
            _holderOfDTO.Add(Res.state, true);
            return _holderOfDTO;
        }

        public async Task<HolderOfDTO> GetRoleUsersAsync(string roleName)
        {
            var lUsers = await _userManager.GetUsersInRoleAsync(roleName);
            if (lUsers is null || lUsers.Count == 0)
            {
                _holderOfDTO.Add(Res.state, false);
                _holderOfDTO.Add(Res.message, _culture.SharedLocalizer["This role does not have users"].Value);
                return _holderOfDTO;
            }
            _holderOfDTO.Add(Res.state, true);
            _holderOfDTO.Add(Res.lUsers, _mapper.Map<List<UserResponseDTO>>(lUsers));
            return _holderOfDTO;
        }
        #endregion

        #region Helper Methods
        private async Task<User> getUserAsync(string personalKey)
        {
            User user = null;
            if (ObjectUtils.IsSALocalPhoneNumber(personalKey))
            {
                user = await _userManager.Users.Where(x => x.LocalPhoneNumber == personalKey).SingleOrDefaultAsync();
            }
            else if (ObjectUtils.IsValidEmail(personalKey))
            {
                user = await _userManager.FindByEmailAsync(personalKey);
            }
            else
            {
                user = await _userManager.FindByNameAsync(personalKey);
            }
            return user;
        }

        private async Task<HolderOfDTO> sendPhoneConfirmationCodeAsync(User user, HttpContext httpContext)
        {
            user.ValidationTokens.ToList().RemoveAll(t => !t.isUsed && (t.isRevoked || t.isExpired));
            var userPhoneToken = GenerateUserValidationToken(httpContext, "UserPhoneConfirmation");
            userPhoneToken.ExpiresOn = DateTime.UtcNow.AddHours(6);
            user.ValidationTokens.Add(userPhoneToken);
            // Save Updates
            await _userManager.UpdateAsync(user);
            try
            {
                // send code to PhoneNumber
                var messageContent = $"Your Confirmation Code is : {userPhoneToken.ValidationCode}";
                _smsService.SendAsync(user.PhoneNumber, messageContent);
            }
            catch { };
            _holderOfDTO.Add(Res.state, true);
            _holderOfDTO.Add(Res.isConfirmed, user.PhoneNumberConfirmed);
            _holderOfDTO.Add(Res.basedEmail, false);

            return _holderOfDTO;

        }

        private async Task<HolderOfDTO> sendEmailConfirmationCodeAsync(User user, HttpContext httpContext)
        {
            user.ValidationTokens.ToList().RemoveAll(t => !t.isUsed && (t.isRevoked || t.isExpired));
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var userEmailConfirmationToken = GenerateUserValidationToken(httpContext, token);
            userEmailConfirmationToken.ExpiresOn = DateTime.UtcNow.AddHours(6);
            user.ValidationTokens.Add(userEmailConfirmationToken);
            // Save Updates
            await _userManager.UpdateAsync(user);
            try
            {
                // send code to email
                var messageContent = $"Your Email Confirmation Code is : {userEmailConfirmationToken.ValidationCode}";
                var message = new EmailMessage(new List<string>() { user.Email }, "WebApp App, Email Confirmation Code", messageContent, null);
                _emailSender.SendEmailAsync(message);
            }
            catch { };
            _holderOfDTO.Add(Res.state, true);
            _holderOfDTO.Add(Res.isConfirmed, user.EmailConfirmed);
            _holderOfDTO.Add(Res.basedEmail, true);
            return _holderOfDTO;
        }

        private async Task<string> getRoleNameAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is not null)
                return role.Name;
            return null;
        }



        private UserRefreshToken GenerateUserRefreshToken(HttpContext? httpContext)
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);
            var ipAdress = RequestUtils.GetClientIPAddress(httpContext);
            var userAgent = RequestUtils.GetClientAgent(httpContext);
            return new UserRefreshToken
            {
                RefreshToken = Convert.ToBase64String(randomNumber),
                CreatedOn = DateTime.UtcNow,
                IpAdress = ipAdress,
                Agent = userAgent,
                ExpiresOn = DateTime.UtcNow.AddDays(10)
            };
        }

        private UserValidationToken GenerateUserValidationToken(HttpContext? httpContext, string resetPasswordToken)
        {
            Random generator = new Random();
            String code = generator.Next(0, 1000000).ToString("D6");
            var ipAdress = RequestUtils.GetClientIPAddress(httpContext);
            var userAgent = RequestUtils.GetClientAgent(httpContext);
            return new UserValidationToken
            {
                ValidationCode = code,
                ValidationToken = resetPasswordToken,
                CreatedOn = DateTime.UtcNow,
                IpAdress = ipAdress,
                Agent = userAgent,
                ExpiresOn = DateTime.UtcNow.AddHours(2)
            };
        }

        private async Task<HolderOfDTO> BuildUserAuthAsync(User user, UserRefreshToken userRefreshToken)
        {
            var rolesList = (List<string>)await _userManager.GetRolesAsync(user);
            var jwtSecurityToken = await CreateAccessTokenAsync(user);

            var existsUser = new UserAuthResponseDTO
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Username = user.UserName,
                Roles = rolesList,
                RefreshToken = userRefreshToken.RefreshToken,
                RefreshTokenExpiration = userRefreshToken.ExpiresOn,

                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                AccessTokenExpiration = jwtSecurityToken.ValidTo,
                Path = user.Path,
                DisplayPath = BuildProfileImagePath(user.Path)

            };
            _holderOfDTO.Add(Res.state, true);
            if (user.IsBasedEmail)
            {
                _holderOfDTO.Add(Res.isConfirmed, user.EmailConfirmed);
                _holderOfDTO.Add(Res.basedEmail, true);
            }
            else
            {
                _holderOfDTO.Add(Res.isConfirmed, user.PhoneNumberConfirmed);
                _holderOfDTO.Add(Res.basedEmail, false);
            }
            _holderOfDTO.Add(Res.oUserAuth, existsUser);
            return _holderOfDTO;
        }

        private async Task<JwtSecurityToken> CreateAccessTokenAsync(User user)
        {
            // Get User Claims
            var userClaims = await _userManager.GetClaimsAsync(user);

            // Get User Roles and add its to User Claims
            var userRoles = (List<string>)await _userManager.GetRolesAsync(user);
            userClaims.Add(new Claim("roles", "Anonamouse"));
            userClaims.Add(new Claim("roles", "Anonamouse"));
            if (userRoles is not null && userRoles.Count > 0)
            {
                foreach (var userRole in userRoles)
                    userClaims.Add(new Claim("roles", userRole));
            }

            // Add Some User Claims
            var claims = new[]
            {
                new Claim("uid", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? " "),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }
            .Union(userClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_jwt.DurationInHours),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        private string BuildProfileImagePath(string path)
        {
            var adresses = _server.Features.Get<IServerAddressesFeature>().Addresses.ToList<string>();
            if (!string.IsNullOrEmpty(path))
            {
                var uri = path.Replace(@"\", @"/");
                if (adresses is not null && adresses.Count > 0)
                {
                    return $"{adresses[0]}/{uri}";
                }
                return uri;
            }
            else
                return $"{adresses[0]}/Images/NoProfile.png";
        }
        #endregion

    }
}
