﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Core.Interfaces;
using WebApp.SharedKernel.Interfaces;

namespace WebApp.Core.Entities.Auth
{
    public partial class UserRole : IdentityUserRole<string>, IUserInsert, IUserUpdate
    {
        
        public string? UserInsertId { get; set; }

        [ForeignKey(nameof(UserInsertId))]
        public virtual User? UserInsert { get; set; }
        public DateTime? UserInsertDate { get; set; }

        public string? UserUpdateId { get; set; }

        [ForeignKey(nameof(UserUpdateId))]
        public virtual User? UserUpdate { get; set; }
        public DateTime? UserUpdateDate { get; set; }
    }
}
