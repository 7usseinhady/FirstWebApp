using WebApp.DataTransferObject.Dtos.Auth.Request;
using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObject.Dtos.Auth.Response
{
    public class RoleResponseDto : RoleRequestDto
    {
        public int? UserCount { get; set; }

    }
}
