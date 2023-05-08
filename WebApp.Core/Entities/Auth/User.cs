using WebApp.DataTransferObject.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Core.Interfaces;

namespace WebApp.Core.Entities.Auth
{
    public partial class User : IdentityUser, IUserInsert, IUserUpdate ,IInactive
    {
        public User()
        {
            RefreshTokens = new HashSet<UserRefreshToken>();
            ValidationTokens = new HashSet<UserValidationToken>();
        }

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = default!;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = default!;

        [NotMapped]
        public string? FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [MaxLength(4)]
        public string? Code { get; set; }

        [MaxLength(10)]
        public string? LocalPhoneNumber { get; set; }

        [MaxLength(13)]
        public override string? PhoneNumber { get; set; }


        [MaxLength(10)]
        public string? SecondLocalPhoneNumber { get; set; }

        [MaxLength(13)]
        public string? SecondPhoneNumber { get; set; }
        public bool? IsFemale { get; set; }
        public string? Path { get; set; }
        public bool IsBasedEmail { get; set; }

        [MaxLength(300)]
        public string? DeviceTokenId { get; set; }

        [MaxLength(10)]
        public string? LastLang { get; set; }
        public bool IsInactive { get; set; }

        public string? UserInsertId { get; set; }

        [ForeignKey(nameof(UserInsertId))]
        public virtual User? UserInsert { get; set; }
        public DateTime? UserInsertDate { get; set; }

        public string? UserUpdateId { get; set; }

        [ForeignKey(nameof(UserUpdateId))]
        public virtual User? UserUpdate { get; set; }
        public DateTime? UserUpdateDate { get; set; }

        public ICollection<UserRefreshToken> RefreshTokens { get; set; }
        public ICollection<UserValidationToken> ValidationTokens { get; set; }

        
        
    }
}
