using AuthenticationDemo.Models;
using AuthenticationDemo.Models.Entities;
using AuthenticationDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Text;

namespace AuthenticationDemo.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserRepository _userRepo;
    private readonly Random _random = new Random();

    public HomeController(IUserRepository userRepo, ILogger<HomeController> logger)
    {
        _logger = logger;
        _userRepo = userRepo;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult GetUserName()
    {
        if (User.Identity!.IsAuthenticated)
        {
            string username = User.Identity.Name ?? "";
            return Content(username);
        }
        return Content("No user");
    }

    public async Task<IActionResult> GetUserId()
    {
        if (User.Identity!.IsAuthenticated)
        {
            string username = User.Identity.Name ?? "";
            var user = await _userRepo.ReadByUsernameAsync(username);
            if (user != null)
            {
                return Content(user.Id);
            }
        }
        return Content("No user");
    }

    public async Task<IActionResult> CreateTestUser()
    {
        var n = _random.Next(100);
        var check = await _userRepo.ReadByUsernameAsync($"test{n}@test.com");
        if (check == null)
        {
            var user = new ApplicationUser
            {
                Email = $"test{n}@test.com",
                UserName = $"test{n}@test.com",
                FirstName = $"User{n}",
                LastName = $"Userlastname{n}"
            };
            await _userRepo.CreateAsync(user, "Pass123!");
            return Content($"Created test user 'test{n}@test.com' with password 'Pass123!'");
        }
        return Content("The user was already created.");
    }

    public async Task<IActionResult> TestAssignUserToRole()
    {
        await _userRepo.AssignUserToRoleAsync("fake@email.com", "TestRole");
        return Content("Assigned 'fake@email.com' to role 'TestRole'");
    }

    public async Task<IActionResult> ShowRoles(string userName)
    {
        ApplicationUser? user = await _userRepo.ReadByUsernameAsync(userName);
        StringBuilder builder = new();
        foreach (var roleName in user!.Roles)
        {
            builder.Append(roleName + " ");
        }
        return Content($"UserName: {user.UserName} Roles: {builder}");
    }

    public async Task<IActionResult> HasRole(string userName, string roleName)
    {
        ApplicationUser? user = await _userRepo.ReadByUsernameAsync(userName);
        if (user!.HasRole(roleName))
        {
            return Content($"{userName} has role {roleName}");
        }
        return Content($"{userName} does not have role {roleName}");
    }

    [Authorize(Roles = "TestRole")]
    public IActionResult TestRoleCheck()
    {
        return Content("Restricted to role TestRole");
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}