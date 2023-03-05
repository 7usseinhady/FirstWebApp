using WebApp.SharedKernel.Consts;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataTransferObjects.Helpers;

namespace WebApp.API.Bases
{
    public class APIControllerBase : ControllerBase
    {
        protected HolderOfDTO _holderOfDTO;

        protected APIControllerBase(HolderOfDTO holderOfDTO)
        {
            _holderOfDTO = holderOfDTO;
        }
        protected IActionResult NotValidModelState()
        {
            _holderOfDTO.Add(Res.state, false);
            _holderOfDTO.Add(Res.modelState, ModelState);
            return BadRequest(_holderOfDTO);
        }

        protected IActionResult State(HolderOfDTO holderOfDTO)
        {
            if (holderOfDTO is not null)
            {
                if (!(bool)holderOfDTO[Res.state])
                    return BadRequest(holderOfDTO);

                return Ok(holderOfDTO);
            }
            return BadRequest();
        }
    }
}
