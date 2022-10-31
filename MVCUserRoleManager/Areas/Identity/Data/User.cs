using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MVCUserRoleManager.Areas.Identity.Data
{
    public class User : IdentityUser
    {
        public User() : base()
        {
        }

        public User(string userName, string firstName, string lastName) : base(userName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        [StringLength(35)]
        public string? FirstName { get; set; }

        [StringLength(35)]
        public string? LastName { get; set; }

        [NotMapped]
        public IEnumerable<string> RolesForUser { get; set; }
    }
}