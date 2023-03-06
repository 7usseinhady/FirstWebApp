using WebApp.DataTransferObjects.Dtos.Auth.Request;
using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObjects.Dtos.Auth.Response
{
    public class RoleResponseDto : RoleRequestDto
    {
        public int? UserCount { get; set; }

    }
}
