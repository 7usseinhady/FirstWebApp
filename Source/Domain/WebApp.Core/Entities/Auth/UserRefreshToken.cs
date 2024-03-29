﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Core.Bases;
using WebApp.Core.Consts;

namespace WebApp.Core.Entities.Auth
{
    [Table("UserRefreshTokens", Schema = SqlSchemas.auth)]
    [Owned]
    public class UserRefreshToken : BaseEntity<long>
    {
        [Required, MaxLength(400)] 
        public string RefreshToken { get; set; } = default!;
        
        public DateTime CreatedOn { get; set; }

        [MaxLength(200)]
        public string? IpAdress { get; set; }

        [MaxLength(200)]
        public string? MacAdress { get; set; }

        [MaxLength(400)]
        public string? Agent { get; set; }

        public DateTime ExpiresOn { get; set; }
        public DateTime? RevokedOn { get; set; }

        public bool isExpired => DateTime.UtcNow >= ExpiresOn;
        public bool isRevoked => RevokedOn is not null;
        public bool isActive => !isExpired && !isRevoked;

    }
}
