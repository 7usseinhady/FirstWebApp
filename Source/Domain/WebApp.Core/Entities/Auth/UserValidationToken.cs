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
        [Required, MaxLength(6)] 
        public string ValidationCode { get; set; } = default!;

        [Required, MaxLength(400)] 
        public string ValidationToken { get; set; } = default!;
        
        public DateTime CreatedOn { get; set; }

        [MaxLength(200)]
        public string? IpAdress { get; set; }

        [MaxLength(200)]
        public string? MacAdress { get; set; }

        [MaxLength(400)]
        public string? Agent { get; set; }

        public DateTime ExpiresOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool isUsed { get; set; }

        public bool isExpired => DateTime.UtcNow >= ExpiresOn;
        public bool isRevoked => RevokedOn is not null;
        public bool isActive => !isExpired && !isRevoked && !isUsed;

    }
}
