using AuthenticationDemo.Models.Entities;

namespace AuthenticationDemo.Services;

public interface IUserRepository
{
    Task<ApplicationUser?> ReadByUsernameAsync(string username);
    Task<ApplicationUser> CreateAsync(ApplicationUser user, string password);
    Task AssignUserToRoleAsync(string userName, string roleName);
}
