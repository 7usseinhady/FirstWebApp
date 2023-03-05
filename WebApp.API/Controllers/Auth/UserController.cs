using WebApp.API.Bases;
using WebApp.DataTransferObjects.Filters.Auth;
using WebApp.SharedKernel.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Interfaces.Custom.Services.Auth;
using WebApp.DataTransferObjects.DTOs.Auth.Request;
using WebApp.DataTransferObjects.Helpers;

namespace WebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : APIControllerBase
    {
        private readonly IUserService _userService;
        public UserController(HolderOfDTO holderOfDTO, IUserService userService) : base(holderOfDTO)
        {
            _userService = userService;
        }


        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllAsync(UserFilter userFilter)
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
        public async Task<IActionResult> Put(UserRequestDTO userRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _userService.UpdateAsync(userRequestDTO));

        }

        [HttpPut("UpdateUserDeviceId")]
        public async Task<IActionResult> UpdateUserDeviceIdAsync(UserDeviceIdRequestDTO userDeviceIdRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _userService.UpdateUserDeviceIdAsync(userDeviceIdRequestDTO));

        }

        [HttpPut("UpdateUserLang")]
        public async Task<IActionResult> UpdateUserLangAsync(UserLangRequestDTO userLangRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _userService.UpdateUserLangAsync(userLangRequestDTO));

        }

        [HttpPut("DeactiveUser")]
        public async Task<IActionResult> DeactiveUser(string id)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _userService.DeactiveUserAsync(id));

        }

        [HttpPut("UpdateByRF")]
        public async Task<IActionResult> UpdateByRF(UserRequestDTO userRequestDTO)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _userService.UpdateByRefreshTokenAsync(userRequestDTO));

        }

        [HttpPost("UploadUserImage")]
        public async Task<IActionResult> UploadUserImageAsync([FromForm] FileDTO fileDTO)
        {
            if (!ModelState.IsValid)
            {
                return NotValidModelState();
            }
            return State(await _userService.ProfilePictureAsync(fileDTO));
        }

        [HttpPost("UploadProfileByRF")]
        public async Task<IActionResult> UploadProfileByRFAsync([FromForm] FileDTO fileDTO)
        {
            if (!ModelState.IsValid)
            {
                return NotValidModelState();
            }
            return State(await _userService.ProfilePictureRefreshTokenAsync(fileDTO));
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
