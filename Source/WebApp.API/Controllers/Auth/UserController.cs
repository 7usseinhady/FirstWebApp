﻿using WebApp.API.Bases;
using WebApp.SharedKernel.Dtos;
using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Interfaces.Custom.Services.Auth;
using WebApp.SharedKernel.Dtos.Auth.Request;
using WebApp.SharedKernel.Dtos.Auth.Request.Filters;
using WebApp.API.Consts;

namespace WebApp.API.Controllers
{
    [Route(Routes.apiBase)]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        private readonly IUserService _userService;
        public UserController(HolderOfDto holderOfDto, IUserService userService) : base(holderOfDto)
        {
            _userService = userService;
        }


        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllAsync(UserFilterRequestDto userFilter)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _userService.GetAllAsync(userFilter));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _userService.GetByIdAsync(id));
        }

        [HttpGet("GetByRF{id}")]
        public async Task<IActionResult> GetByRFAsync(string id)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _userService.GetByRefreshTokenAsync(id));
        }

        [HttpPut]
        public async Task<IActionResult> Put(UserEditRequestDto userRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _userService.UpdateAsync(userRequestDto));

        }

        [HttpPut("UpdateUserDeviceId")]
        public async Task<IActionResult> UpdateUserDeviceIdAsync(UserDeviceIdRequestDto userDeviceIdRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _userService.UpdateUserDeviceIdAsync(userDeviceIdRequestDto));

        }

        [HttpPut("UpdateUserLang")]
        public async Task<IActionResult> UpdateUserLangAsync(UserLangRequestDto userLangRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _userService.UpdateUserLangAsync(userLangRequestDto));

        }

        [HttpPut("DeactiveUser")]
        public async Task<IActionResult> DeactiveUser(string id)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _userService.DeactiveUserAsync(id));

        }

        [HttpPut("UpdateByRF")]
        public async Task<IActionResult> UpdateByRF(UserEditRequestDto userRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _userService.UpdateByRefreshTokenAsync(userRequestDto));

        }

        [HttpPost("UploadUserImage")]
        public async Task<IActionResult> UploadUserImageAsync([FromForm] FileDto fileDto)
        {
            if (!ModelState.IsValid)
            {
                return NotValidModelState();
            }
            return State(await _userService.ProfilePictureAsync(fileDto));
        }

        [HttpPost("UploadProfileByRF")]
        public async Task<IActionResult> UploadProfileByRFAsync([FromForm] FileDto fileDto)
        {
            if (!ModelState.IsValid)
            {
                return NotValidModelState();
            }
            return State(await _userService.ProfilePictureRefreshTokenAsync(fileDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return NotValidModelState();
            }
            return State(await _userService.DeleteProfilePictureAsync(id));
        }

        [HttpDelete("RemoveProfileByRF{id}")]
        public async Task<IActionResult> RemoveProfileByRF(string id)
        {
            if (!ModelState.IsValid)
            {
                return NotValidModelState();
            }
            return State(await _userService.DeleteProfilePictureRefreshTokenAsync(id));
        }

    }
}
