using WebApp.DataTransferObject.Dtos.Auth.Request;
using WebApp.DataTransferObject.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObject.Dtos.Auth.Response
{
    public class UserResponseDto : UserRequestDto, IFilePathDto
    {
        [Display(Name = "UserName"), Required(ErrorMessage = "Required")]
        public string UserName { get; set; } = default!;

        [Display(Name = "FullName")]
        public string? FullName { get; set; }

        public string? Path { get; set; }
        public string? DisplayPath { get; set; }


    }
}