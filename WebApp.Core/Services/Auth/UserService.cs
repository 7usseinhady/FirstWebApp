using AutoMapper;
using WebApp.Core.Bases;
using WebApp.Core.Interfaces;
using WebApp.DataTransferObjects.Interfaces;
using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApp.DataTransferObjects.Helpers;
using WebApp.DataTransferObjects.Filters.Auth;
using Microsoft.AspNetCore.Identity;
using WebApp.Core.Entities.Auth;
using WebApp.SharedKernel.Dtos;
using WebApp.Core.Interfaces.Custom.Services.Auth;
using WebApp.DataTransferObjects.Dtos.Auth.Request;
using WebApp.Core.Extensions;
using WebApp.DataTransferObjects.Dtos.Auth.Response;
using WebApp.DataTransferObject.Dtos;

namespace WebApp.Core.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IFileUtils _fileUtils;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, HolderOfDto holderOfDto, ICulture culture, UserManager<User> userManager, IFileUtils fileUtils) : base(unitOfWork, mapper, holderOfDto, culture)
        {
            _userManager = userManager;
            _fileUtils = fileUtils;
        }

        public async Task<HolderOfDto> GetAllAsync(UserFilter userFilter)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                var query = await _unitOfWork.users.BuildUserQueryAsync(userFilter);
                int totalCount = await query.CountAsync();

                var page = new Pager();
                page.Set(userFilter.PageSize, userFilter.CurrentPage, totalCount);
                _holderOfDto.Add(Res.page, page);
                lIndicator.Add(true);

                // pagination
                query = query.AddPage(page.Skip, page.PageSize);
                _holderOfDto.Add(Res.lUsers, _mapper.Map<List<UserResponseDto>>(await query.ToListAsync()));
                lIndicator.Add(true);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);
                lIndicator.Add(false);
            }
            _holderOfDto.Add(Res.state, lIndicator.All(x => x));
            return _holderOfDto;
        }

        public async Task<HolderOfDto> GetByIdAsync(string userId)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                var userFilter = new UserFilter() { Id = userId };
                var query = await _unitOfWork.users.BuildUserQueryAsync(userFilter);
                var userDto = _mapper.Map<UserResponseDto>(query.SingleOrDefault());
                _holderOfDto.Add(Res.oUser, userDto);
                lIndicator.Add(true);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);
                lIndicator.Add(false);
            }
            _holderOfDto.Add(Res.state, lIndicator.All(x => x));

            return _holderOfDto;
        }
        public async Task<HolderOfDto> GetByRefreshTokenAsync(string token)
        {
            try
            {
                var holder = await GetUserIdAsync(_userManager, token);
                if (!holder.ContainsKey(Res.state) || !(bool)holder[Res.state])
                    return holder;

                var userId = (string)holder[Res.uid];

                return await GetByIdAsync(userId);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);
                _holderOfDto.Add(Res.state, false);
                return _holderOfDto;
            }
        }

        public async Task<HolderOfDto> UpdateAsync(UserRequestDto userRequestDto)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                var user = await _userManager.FindByIdAsync(userRequestDto.Id);
                if (user is not null)
                {
                    if (!string.IsNullOrEmpty(userRequestDto.PhoneNumber) && user.PhoneNumber != userRequestDto.PhoneNumber)
                    {
                        var userByPhone = await _userManager.Users.Where(x => x.PhoneNumber == userRequestDto.PhoneNumber).ToListAsync();
                        if (userByPhone.Count > 0)
                        {
                            _holderOfDto.Add(Res.state, false);
                            _holderOfDto.Add(Res.message, "phone number is already exist");
                            return _holderOfDto;
                        }
                        user.Code = userRequestDto.Code;
                        user.LocalPhoneNumber = userRequestDto.LocalPhoneNumber;
                        user.PhoneNumber = userRequestDto.PhoneNumber;
                        user.PhoneNumberConfirmed = false;
                    }

                    if (!string.IsNullOrEmpty(userRequestDto.Email) && user.Email != userRequestDto.Email)
                    {
                        var userByEmail = await _userManager.Users.Where(x => x.Email == userRequestDto.Email).ToListAsync();
                        if (userByEmail.Count > 0)
                        {
                            _holderOfDto.Add(Res.state, false);
                            _holderOfDto.Add(Res.message, "Email is already exist");
                            return _holderOfDto;
                        }
                        user.Email = userRequestDto.Email;
                        user.EmailConfirmed = false;
                    }

                    user.SecondLocalPhoneNumber = userRequestDto.SecondLocalPhoneNumber;
                    user.SecondPhoneNumber = userRequestDto.SecondPhoneNumber;
                    user.FirstName = userRequestDto.FirstName;
                    user.LastName = userRequestDto.LastName;
                    user.IsFemale = userRequestDto.IsFemale;
                    user.IsInactive = userRequestDto.IsInactive;
                    user.UserUpdateId = userRequestDto.Id;
                    user.UserUpdateDate = DateTime.UtcNow;

                    var result = await _userManager.UpdateAsync(user);
                    lIndicator.Add(result.Succeeded);
                }
                else
                    lIndicator.Add(false);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);

                lIndicator.Add(false);
            }

            _holderOfDto.Add(Res.id, userRequestDto.Id);
            _holderOfDto.Add(Res.state, lIndicator.All(x => x));

            return _holderOfDto;
        }

        public async Task<HolderOfDto> UpdateByRefreshTokenAsync(UserRequestDto userRequestDto)
        {
            try
            {
                var holder = await GetUserIdAsync(_userManager, userRequestDto.Id);
                if (!holder.ContainsKey(Res.state) || !(bool)holder[Res.state])
                    return holder;

                var userId = (string)holder[Res.uid];
                userRequestDto.Id = userId;
                return await UpdateAsync(userRequestDto);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);
                _holderOfDto.Add(Res.state, false);
                return _holderOfDto;
            }
        }

        public async Task<HolderOfDto> UpdateUserDeviceIdAsync(UserDeviceIdRequestDto userDeviceIdRequestDto)
        {
            var lIndicator = new List<bool>();
            try
            {
                var oUser = await _userManager.FindByIdAsync(userDeviceIdRequestDto.UserId);
                if (oUser is null)
                {
                    _holderOfDto.Add(Res.state, false);
                    _holderOfDto.Add(Res.message, "Not Found User");
                    return _holderOfDto;
                }

                oUser.DeviceTokenId = userDeviceIdRequestDto.DeviceId;

                var result = await _userManager.UpdateAsync(oUser);
                lIndicator.Add(result.Succeeded);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);
                lIndicator.Add(false);
            }

            _holderOfDto.Add(Res.state, lIndicator.All(x => x));
            return _holderOfDto;
        }

        public async Task<HolderOfDto> UpdateUserLangAsync(UserLangRequestDto userLangRequestDto)
        {
            var lIndicator = new List<bool>();
            try
            {
                var oUser = await _userManager.FindByIdAsync(userLangRequestDto.UserId);
                if (oUser is null)
                {
                    _holderOfDto.Add(Res.state, false);
                    _holderOfDto.Add(Res.message, "Not Found User");
                    return _holderOfDto;
                }

                oUser.LastLang = userLangRequestDto.LastLang;

                var result = await _userManager.UpdateAsync(oUser);
                lIndicator.Add(result.Succeeded);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);
                lIndicator.Add(false);
            }

            _holderOfDto.Add(Res.state, lIndicator.All(x => x));
            return _holderOfDto;
        }

        public async Task<HolderOfDto> DeactiveUserAsync(string userId)
        {
            var lIndicator = new List<bool>();
            try
            {
                var oUser = await _userManager.FindByIdAsync(userId);
                if (oUser is null)
                {
                    _holderOfDto.Add(Res.state, false);
                    _holderOfDto.Add(Res.message, "Not Found User");
                    return _holderOfDto;
                }

                oUser.IsInactive = true;

                var result = await _userManager.UpdateAsync(oUser);
                lIndicator.Add(result.Succeeded);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);
                lIndicator.Add(false);
            }

            _holderOfDto.Add(Res.state, lIndicator.All(x => x));
            return _holderOfDto;
        }
        public async Task<HolderOfDto> ProfilePictureAsync(FileDto fileDto)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                if (fileDto is not null && !string.IsNullOrEmpty(fileDto.Id))
                {
                    fileDto.FilePath = @"\Images\Users\";
                    if (fileDto.File is not null && fileDto.File.Length > 0)
                    {
                        var holder = _fileUtils.UploadImage(fileDto);
                        bool isUploaded = (bool)holder[Res.state];

                        if (isUploaded)
                        {
                            var user = await _userManager.FindByIdAsync(fileDto.Id);
                            if (user is not null)
                            {
                                user.Path = holder[Res.filePath] as string;
                                user.UserUpdateId = fileDto.Id;
                                user.UserUpdateDate = DateTime.UtcNow;
                                var result = await _userManager.UpdateAsync(user);
                                lIndicator.Add(result.Succeeded);
                            }
                            else
                                lIndicator.Add(false);
                        }
                        lIndicator.Add(isUploaded);
                    }
                    else
                        lIndicator.Add(false);
                }
                else
                    lIndicator.Add(false);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);

                lIndicator.Add(false);
            }
            _holderOfDto.Add(Res.state, lIndicator.All(x => x));
            return _holderOfDto;

        }
        public async Task<HolderOfDto> ProfilePictureRefreshTokenAsync(FileDto fileDto)
        {
            try
            {
                var holder = await GetUserIdAsync(_userManager, fileDto.Id);
                if (!holder.ContainsKey(Res.state) || !(bool)holder[Res.state])
                    return holder;

                var userId = (string)holder[Res.uid];
                fileDto.Id = userId;
                return await ProfilePictureAsync(fileDto);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);
                _holderOfDto.Add(Res.state, false);
                return _holderOfDto;
            }
        }
        public async Task<HolderOfDto> DeleteProfilePictureAsync(string id)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is null)
                {
                    _holderOfDto.Add(Res.message, "Invalid User");
                    _holderOfDto.Add(Res.state, false);
                    return _holderOfDto;
                }
                bool isDelete = _fileUtils.DeleteFile(user.Path);
                if (isDelete)
                {
                    user.Path = null;
                    var result = await _userManager.UpdateAsync(user);
                    lIndicator.Add(result.Succeeded);
                }
                lIndicator.Add(isDelete);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);

                lIndicator.Add(false);
            }
            _holderOfDto.Add(Res.state, lIndicator.All(x => x));
            return _holderOfDto;
        }
        public async Task<HolderOfDto> DeleteProfilePictureRefreshTokenAsync(string token)
        {
            try
            {
                var holder = await GetUserIdAsync(_userManager, token);
                if (!holder.ContainsKey(Res.state) || !(bool)holder[Res.state])
                    return holder;

                var userId = (string)holder[Res.uid];
                return await DeleteProfilePictureAsync(userId);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);
                _holderOfDto.Add(Res.state, false);
                return _holderOfDto;
            }
        }
        public HolderOfDto Delete(string id)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                _unitOfWork.users.DeleteById(id);
                lIndicator.Add(_unitOfWork.Complete() > 0);
            }
            catch (Exception ex)
            {
                _holderOfDto.Add(Res.message, ex.Message);

                lIndicator.Add(false);
            }
            _holderOfDto.Add(Res.state, lIndicator.All(x => x));

            return _holderOfDto;
        }
    }
}
