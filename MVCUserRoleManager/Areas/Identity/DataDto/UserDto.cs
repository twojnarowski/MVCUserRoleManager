using System.ComponentModel.DataAnnotations;

namespace MVCUserRoleManager.Areas.Identity.DataDto
{
    public class UserDto
    {
        [StringLength(35)]
        public string? FirstName { get; set; }

        [StringLength(35)]
        public string? LastName { get; set; }

        public string Id { get; set; }
    }
}