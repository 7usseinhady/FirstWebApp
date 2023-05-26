using WebApp.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Core.Interfaces;

namespace WebApp.Core.Entities.Auth
{
    public partial class User : IdentityUser, IUserInsertion, IUserModification ,IInactive
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

        public string? InsertedById { get; set; }

        [ForeignKey(nameof(InsertedById))]
        public virtual User? InsertedBy { get; set; }
        public DateTime? InsertedOn { get; set; }

        public string? ModifiedById { get; set; }

        [ForeignKey(nameof(ModifiedById))]
        public virtual User? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public ICollection<UserRefreshToken> RefreshTokens { get; set; }
        public ICollection<UserValidationToken> ValidationTokens { get; set; }

        
        
    }
}
