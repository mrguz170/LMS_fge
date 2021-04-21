using Microsoft.AspNetCore.Identity;
using System;
using LMS_fge.Models.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS_fge.Models.User
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public StatusCore Status { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        [Required]
        public string PaternalSurname { get; set; }

        [StringLength(50)]
        [Required]
        public string MaternalSurname { get; set; }

        [StringLength(10)]
        [Required]
        public string UserKey { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public string NameComplete => $"{Name} {PaternalSurname} {MaternalSurname}";
    }

    public class ApplicationRole : IdentityRole<Guid>
    {
        public StatusCore Status { get; set; }

        [StringLength(180)]
        [Required]
        public string Description { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationUserClaim : IdentityUserClaim<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }

    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }

    public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationUserToken : IdentityUserToken<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
