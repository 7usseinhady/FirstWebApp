using WebApp.API.Bases;
using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Interfaces.Custom.Services.Auth;
using WebApp.SharedKernel.Dtos;
using WebApp.SharedKernel.Dtos.Auth.Request.Filters;
using WebApp.API.Consts;
using WebApp.SharedKernel.Dtos.Auth.Request;

namespace WebApp.API.Controllers
{
    [Route(Routes.apiBase)]
    [ApiController]
    public class RolesController : ApiControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(HolderOfDto holderOfDto, IRoleService roleService) : base(holderOfDto)
        {
            _roleService = roleService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromBody] RoleFilterRequestDto roleFilter)
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
        public async Task<IActionResult> PostAsync([FromBody] RoleRequestDto roleRequestDto)
        {
            if (!ModelState.IsValid)
                return NotValidModelState();

            return State(await _roleService.SaveAsync(roleRequestDto));
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
