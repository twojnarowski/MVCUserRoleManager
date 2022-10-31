using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MVCUserRoleManager.Areas.Identity.Data
{
    public class Role : IdentityRole
    {
        public Role()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }

        public Role(string roleName, string badge) : base(roleName)
        {
            Badge = badge;
        }

        [StringLength(6, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string? Badge { get; set; }
    }
}