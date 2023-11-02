using AuthenticationDemo.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationDemo.Services;

public class Initializer
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public Initializer(
       ApplicationDbContext db,
       UserManager<ApplicationUser> userManager,
       RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedUsersAsync()
    {
        _db.Database.EnsureCreated();

        if (!_db.Roles.Any(r => r.Name == "Admin"))
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        }

        if (!_db.Roles.Any(r => r.Name == "Teacher"))
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = "Teacher" });
        }

        if (!_db.Roles.Any(r => r.Name == "Student"))
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = "Student" });
        }

        if (!_db.Users.Any(u => u.UserName == "admin@test.com"))
        {
            var user = new ApplicationUser
            {
                Email = "admin@test.com",
                UserName = "admin@test.com",
            };
            await _userManager.CreateAsync(user, "Pass123!");
            await _userManager.AddToRoleAsync(user, "Admin");
        }
    }
}

