using WebApp.DataTransferObjects.Dtos.Auth.Request;
using WebApp.DataTransferObjects.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObjects.Dtos.Auth.Response
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