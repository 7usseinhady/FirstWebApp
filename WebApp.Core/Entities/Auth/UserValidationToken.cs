using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Core.Bases;
using WebApp.Core.Consts;

namespace WebApp.Core.Entities.Auth
{
    [Table("UserValidationTokens", Schema = SqlSchemas.auth)]
    [Owned]
    public class UserValidationToken : BaseEntity<long>
    {
        [Key]
        public override long Id { get; set; }

        [Required, MaxLength(6)]
        public string ValidationCode { get; set; }

        [Required, MaxLength(400)]
        public string ValidationToken { get; set; }
        
        public DateTime CreatedOn { get; set; }

        [MaxLength(200)]
        public string? IpAdress { get; set; }

        [MaxLength(400)]
        public string? Agent { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool isExpired => DateTime.UtcNow >= ExpiresOn;
        public DateTime? RevokedOn { get; set; }
        public bool isRevoked => RevokedOn is not null;
        public bool isUsed { get; set; }
        public bool isActive => !isExpired && !isRevoked && !isUsed;

    }
}
