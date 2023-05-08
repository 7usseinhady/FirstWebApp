using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObject.Filters.Auth
{
    public class UserFilter : PagerFilter
    {
        public string? Id { get; set; }
        public string? RoleName { get; set; }

        [Display(Name = "FullName")]
        public string? FullName { get; set; }

        [Display(Name = "UserName")]
        public string? Username { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "SecondPhoneNumber")]
        public string? SecondPhoneNumber { get; set; }

        [Display(Name = "Gender")]
        public int? IsFemale { get; set; }

        [Display(Name = "Status")]
        public int? IsInactive { get; set; }

        public string? CommonKeyWord { get; set; }

        [Display(Name = "DateFrom")]
        public DateTime? CreationDateFrom { get; set; }

        [Display(Name = "DateTo")]
        public DateTime? CreationDateTo { get; set; }


    }
}
