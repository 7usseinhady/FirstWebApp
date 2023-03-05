using WebApp.DataTransferObjects.DTOs.Auth.Request;
using WebApp.DataTransferObjects.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObjects.DTOs.Auth.Response
{
    public class UserResponseDTO : UserRequestDTO, IFilePathDTO
    {
        [Display(Name = "UserName"), Required(ErrorMessage = "Required")]
        public string UserName { get; set; }

        [Display(Name = "FullName")]
        public string? FullName { get; set; }

        public string? Path { get; set; }
        public string? DisplayPath { get; set; }


    }
}