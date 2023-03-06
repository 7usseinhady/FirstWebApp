using WebApp.API.Bases;
using WebApp.DataTransferObjects.Filters.Auth;
using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Interfaces.Custom.Services.Auth;
using WebApp.DataTransferObjects.Helpers;

namespace WebApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : APIControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(HolderOfDto holderOfDto, IRoleService roleService) : base(holderOfDto)
        {
            _roleService = roleService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromBody] RoleFilter roleFilter)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _roleService.GetAllAsync(roleFilter));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _roleService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] string roleName)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _roleService.SaveAsync(roleName));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(_roleService.Delete(id));
        }
    }
}
