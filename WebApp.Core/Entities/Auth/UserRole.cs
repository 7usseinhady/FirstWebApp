using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Core.Interfaces;
using WebApp.SharedKernel.Interfaces;

namespace WebApp.Core.Entities.Auth
{
    public partial class UserRole : IdentityUserRole<string>, IUserInsertion, IUserModification
    {
        
        public string? InsertedById { get; set; }

        [ForeignKey(nameof(InsertedById))]
        public virtual User? InsertedBy { get; set; }
        public DateTime? InsertedOn { get; set; }

        public string? ModifiedById { get; set; }

        [ForeignKey(nameof(ModifiedById))]
        public virtual User? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
