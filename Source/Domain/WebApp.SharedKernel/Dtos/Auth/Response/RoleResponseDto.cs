using WebApp.SharedKernel.Dtos.Auth.Request;
using System.ComponentModel.DataAnnotations;

namespace WebApp.SharedKernel.Dtos.Auth.Response
{
    public class RoleResponseDto : RoleRequestDto
    {
        public int? UserCount { get; set; }

    }
}
