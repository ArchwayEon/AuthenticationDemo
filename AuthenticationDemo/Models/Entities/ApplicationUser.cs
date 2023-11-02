using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationDemo.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    [NotMapped]
    public ICollection<string> Roles { get; set; }
        = new List<string>();

    public bool HasRole(string roleName)
    {
        return Roles.Any(r => r == roleName);
    }

}
