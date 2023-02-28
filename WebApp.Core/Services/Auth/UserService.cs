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
using WebApp.SharedKernel.DTOs;
using WebApp.Core.Interfaces.Custom.Services.Auth;
using WebApp.DataTransferObjects.DTOs.Auth.Request;
using WebApp.Core.Extensions;
using WebApp.DataTransferObjects.DTOs.Auth.Response;

namespace WebApp.Core.Services
{
    public class UserService : BaseService, IUserService
    {
        private UserManager<User> _userManager;
        private readonly IFileUtils _fileUtils;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, HolderOfDTO holderOfDTO, ICulture culture, UserManager<User> userManager, IFileUtils fileUtils) : base(unitOfWork, mapper, holderOfDTO, culture)
        {
            _userManager = userManager;
            _fileUtils = fileUtils;
        }

        public async Task<HolderOfDTO> GetAllAsync(UserFilter userFilter)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                var query = await _unitOfWork.users.BuildUserQueryAsync(userFilter);
                int totalCount = await query.CountAsync();

                var page = new Pager();
                page.Set(userFilter.PageSize, userFilter.CurrentPage, totalCount);
                _holderOfDTO.Add(Res.page, page);
                lIndicator.Add(true);

                // pagination
                query = query.AddPage(page.Skip, page.PageSize);
                _holderOfDTO.Add(Res.lUsers, _mapper.Map<List<UserResponseDTO>>(await query.ToListAsync()));
                lIndicator.Add(true);
            }
            catch (Exception ex)
            {
                _holderOfDTO.Add(Res.message, ex.Message);
                lIndicator.Add(false);
            }
            _holderOfDTO.Add(Res.state, lIndicator.All(x => x));
            return _holderOfDTO;
        }

        public async Task<HolderOfDTO> GetByIdAsync(string userId)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                var userFilter = new UserFilter() { Id = userId };
                var query = await _unitOfWork.users.BuildUserQueryAsync(userFilter);
                var userDTO = _mapper.Map<UserResponseDTO>(query.SingleOrDefault());
                _holderOfDTO.Add(Res.oUser, userDTO);
                lIndicator.Add(true);
            }
            catch (Exception ex)
            {
                _holderOfDTO.Add(Res.message, ex.Message);
                lIndicator.Add(false);
            }
            _holderOfDTO.Add(Res.state, lIndicator.All(x => x));

            return _holderOfDTO;
        }
        public async Task<HolderOfDTO> GetByRefreshTokenAsync(string token)
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
                _holderOfDTO.Add(Res.message, ex.Message);
                _holderOfDTO.Add(Res.state, false);
                return _holderOfDTO;
            }
        }

        public async Task<HolderOfDTO> UpdateAsync(UserRequestDTO userRequestDTO)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                var user = await _userManager.FindByIdAsync(userRequestDTO.Id);
                if (user is not null)
                {
                    if (!string.IsNullOrEmpty(userRequestDTO.PhoneNumber))
                    {
                        if (user.PhoneNumber != userRequestDTO.PhoneNumber)
                        {
                            var userByPhone = await _userManager.Users.Where(x => x.PhoneNumber == userRequestDTO.PhoneNumber).ToListAsync();
                            if (userByPhone.Count() > 0)
                            {
                                _holderOfDTO.Add(Res.state, false);
                                _holderOfDTO.Add(Res.message, "phone number is already exist");
                                return _holderOfDTO;
                            }
                            user.Code = userRequestDTO.Code;
                            user.LocalPhoneNumber = userRequestDTO.LocalPhoneNumber;
                            user.PhoneNumber = userRequestDTO.PhoneNumber;
                            user.PhoneNumberConfirmed = false;
                        }
                    }

                    if (!string.IsNullOrEmpty(userRequestDTO.Email))
                    {
                        if (user.Email != userRequestDTO.Email)
                        {
                            var userByEmail = await _userManager.Users.Where(x => x.Email == userRequestDTO.Email).ToListAsync();
                            if (userByEmail.Count() > 0)
                            {
                                _holderOfDTO.Add(Res.state, false);
                                _holderOfDTO.Add(Res.message, "Email is already exist");
                                return _holderOfDTO;
                            }
                            user.Email = userRequestDTO.Email;
                            user.EmailConfirmed = false;
                        }
                    }

                    user.SecondLocalPhoneNumber = userRequestDTO.SecondLocalPhoneNumber;
                    user.SecondPhoneNumber = userRequestDTO.SecondPhoneNumber;
                    user.FirstName = userRequestDTO.FirstName;
                    user.LastName = userRequestDTO.LastName;
                    user.IsFemale = userRequestDTO.IsFemale;
                    user.IsInactive = userRequestDTO.IsInactive;
                    user.UserUpdateId = userRequestDTO.Id;
                    user.UserUpdateDate = DateTime.UtcNow;

                    var result = await _userManager.UpdateAsync(user);
                    lIndicator.Add(result.Succeeded);
                }
                else
                    lIndicator.Add(false);
            }
            catch (Exception ex)
            {
                _holderOfDTO.Add(Res.message, ex.Message);

                lIndicator.Add(false);
            }

            _holderOfDTO.Add(Res.id, userRequestDTO.Id);
            _holderOfDTO.Add(Res.state, lIndicator.All(x => x));

            return _holderOfDTO;
        }

        public async Task<HolderOfDTO> UpdateByRefreshTokenAsync(UserRequestDTO userRequestDTO)
        {
            try
            {
                var holder = await GetUserIdAsync(_userManager, userRequestDTO.Id);
                if (!holder.ContainsKey(Res.state) || !(bool)holder[Res.state])
                    return holder;

                var userId = (string)holder[Res.uid];
                userRequestDTO.Id = userId;
                return await UpdateAsync(userRequestDTO);
            }
            catch (Exception ex)
            {
                _holderOfDTO.Add(Res.message, ex.Message);
                _holderOfDTO.Add(Res.state, false);
                return _holderOfDTO;
            }
        }

        public async Task<HolderOfDTO> UpdateUserDeviceIdAsync(UserDeviceIdRequestDTO userDeviceIdRequestDTO)
        {
            var lIndicator = new List<bool>();
            try
            {
                var oUser = await _userManager.FindByIdAsync(userDeviceIdRequestDTO.UserId);
                if (oUser is null)
                {
                    _holderOfDTO.Add(Res.state, false);
                    _holderOfDTO.Add(Res.message, "Not Found User");
                    return _holderOfDTO;
                }

                oUser.DeviceTokenId = userDeviceIdRequestDTO.DeviceId;

                var result = await _userManager.UpdateAsync(oUser);
                lIndicator.Add(result.Succeeded);
            }
            catch (Exception ex)
            {
                _holderOfDTO.Add(Res.message, ex.Message);
                lIndicator.Add(false);
            }

            _holderOfDTO.Add(Res.state, lIndicator.All(x => x));
            return _holderOfDTO;
        }

        public async Task<HolderOfDTO> UpdateUserLangAsync(UserLangRequestDTO userLangRequestDTO)
        {
            var lIndicator = new List<bool>();
            try
            {
                var oUser = await _userManager.FindByIdAsync(userLangRequestDTO.UserId);
                if (oUser is null)
                {
                    _holderOfDTO.Add(Res.state, false);
                    _holderOfDTO.Add(Res.message, "Not Found User");
                    return _holderOfDTO;
                }

                oUser.LastLang = userLangRequestDTO.LastLang;

                var result = await _userManager.UpdateAsync(oUser);
                lIndicator.Add(result.Succeeded);
            }
            catch (Exception ex)
            {
                _holderOfDTO.Add(Res.message, ex.Message);
                lIndicator.Add(false);
            }

            _holderOfDTO.Add(Res.state, lIndicator.All(x => x));
            return _holderOfDTO;
        }

        public async Task<HolderOfDTO> DeactiveUserAsync(string userId)
        {
            var lIndicator = new List<bool>();
            try
            {
                var oUser = await _userManager.FindByIdAsync(userId);
                if (oUser is null)
                {
                    _holderOfDTO.Add(Res.state, false);
                    _holderOfDTO.Add(Res.message, "Not Found User");
                    return _holderOfDTO;
                }

                oUser.IsInactive = true;

                var result = await _userManager.UpdateAsync(oUser);
                lIndicator.Add(result.Succeeded);
            }
            catch (Exception ex)
            {
                _holderOfDTO.Add(Res.message, ex.Message);
                lIndicator.Add(false);
            }

            _holderOfDTO.Add(Res.state, lIndicator.All(x => x));
            return _holderOfDTO;
        }
        public async Task<HolderOfDTO> ProfilePictureAsync(FileDTO fileDTO)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                if (fileDTO is not null && !string.IsNullOrEmpty(fileDTO.Id))
                {
                    fileDTO.FilePath = @"\Images\Users\";
                    if (fileDTO.File is not null && fileDTO.File.Length > 0)
                    {
                        var holder = await _fileUtils.UploadImageAsync(fileDTO);
                        bool isUploaded = (bool)holder[Res.state];

                        if (isUploaded)
                        {
                            var user = await _userManager.FindByIdAsync(fileDTO.Id);
                            if (user is not null)
                            {
                                user.Path = holder[Res.filePath] as string;
                                user.UserUpdateId = fileDTO.Id;
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
                _holderOfDTO.Add(Res.message, ex.Message);

                lIndicator.Add(false);
            }
            _holderOfDTO.Add(Res.state, lIndicator.All(x => x));
            return _holderOfDTO;

        }
        public async Task<HolderOfDTO> ProfilePictureRefreshTokenAsync(FileDTO fileDTO)
        {
            try
            {
                var holder = await GetUserIdAsync(_userManager, fileDTO.Id);
                if (!holder.ContainsKey(Res.state) || !(bool)holder[Res.state])
                    return holder;

                var userId = (string)holder[Res.uid];
                fileDTO.Id = userId;
                return await ProfilePictureAsync(fileDTO);
            }
            catch (Exception ex)
            {
                _holderOfDTO.Add(Res.message, ex.Message);
                _holderOfDTO.Add(Res.state, false);
                return _holderOfDTO;
            }
        }
        public async Task<HolderOfDTO> DeleteProfilePictureAsync(string id)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is null)
                {
                    _holderOfDTO.Add(Res.message, "Invalid User");
                    _holderOfDTO.Add(Res.state, false);
                    return _holderOfDTO;
                }
                bool isDelete = await _fileUtils.DeleteFileAsync(user.Path);
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
                _holderOfDTO.Add(Res.message, ex.Message);

                lIndicator.Add(false);
            }
            _holderOfDTO.Add(Res.state, lIndicator.All(x => x));
            return _holderOfDTO;
        }
        public async Task<HolderOfDTO> DeleteProfilePictureRefreshTokenAsync(string token)
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
                _holderOfDTO.Add(Res.message, ex.Message);
                _holderOfDTO.Add(Res.state, false);
                return _holderOfDTO;
            }
        }
        public HolderOfDTO Delete(string id)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                _unitOfWork.users.Delete(id);
                lIndicator.Add(_unitOfWork.Complete() > 0);
            }
            catch (Exception ex)
            {
                _holderOfDTO.Add(Res.message, ex.Message);

                lIndicator.Add(false);
            }
            _holderOfDTO.Add(Res.state, lIndicator.All(x => x));

            return _holderOfDTO;
        }
    }
}
