using WebApp.SharedKernel.Consts;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataTransferObjects.Helpers;

namespace WebApp.API.Bases
{
    public class APIControllerBase : ControllerBase
    {
        protected HolderOfDto _holderOfDto;

        protected APIControllerBase(HolderOfDto holderOfDto)
        {
            _holderOfDto = holderOfDto;
        }
        protected IActionResult NotValidModelState()
        {
            _holderOfDto.Add(Res.state, false);
            _holderOfDto.Add(Res.modelState, ModelState);
            return BadRequest(_holderOfDto);
        }

        protected IActionResult State(HolderOfDto holderOfDto)
        {
            if (holderOfDto is not null)
            {
                if (!(bool)holderOfDto[Res.state])
                    return BadRequest(holderOfDto);

                return Ok(holderOfDto);
            }
            return BadRequest();
        }
    }
}
