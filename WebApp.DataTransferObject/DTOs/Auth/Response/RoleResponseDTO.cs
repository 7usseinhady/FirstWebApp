using WebApp.DataTransferObjects.DTOs.Auth.Request;
using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObjects.DTOs.Auth.Response
{
    public class RoleResponseDTO : RoleRequestDTO
    {
        public int? UserCount { get; set; }

    }
}
