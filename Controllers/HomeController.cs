using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SweetNela.Models;

namespace SweetNela.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [TempData]
    public string Message { get; set; } = string.Empty;

    public IActionResult Index()
    {
        var cookieOption = new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };
        Response.Cookies.Append("MiCookie", "ValorDelLaCookie", cookieOption);
        return View();
    }

    public async Task<IActionResult> Privacy()
    {
        if (User.Identity.IsAuthenticated)
        {
            var userId = _userManager.GetUserId(User);

            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var phoneNumber = user.PhoneNumber ?? "No phone number provided";
                var email = user.Email;
                var userName = user.UserName;

                Message = $"Customer {userName} (Email: {email}, Phone: {phoneNumber}) added.";
            }
            else
            {
                Message = "User data could not be retrieved.";
            }
        }
        else
        {
            Message = "No customer is logged in.";
        }

        return View();
    }
}
