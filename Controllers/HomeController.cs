using System.Security.Claims;
using AdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AdminPanel.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CmsLinksOptions _cmsLinks;

    public HomeController(ILogger<HomeController> logger, IOptions<CmsLinksOptions> cmsLinks)
    {
        _logger = logger;
        _cmsLinks = cmsLinks.Value;
    }

    public IActionResult Index()
    {
        var userRole = User.FindFirstValue(ClaimTypes.Role) ?? "Viewer";
        var allowedCms = User.Claims
            .Where(c => c.Type == "AllowedCms")
            .Select(c => c.Value)
            .ToList();

        var availableLinks = userRole == "Admin" || userRole == "GlobalAdmin"
            ? _cmsLinks.Links
            : _cmsLinks.Links.Where(l => allowedCms.Contains(l.Name)).ToList();

        var model = new AdminDashboardViewModel
        {
            UserName = User.Identity?.Name ?? "Unknown",
            UserEmail = User.FindFirstValue(ClaimTypes.Email) ?? "N/A",
            CmsLinks = availableLinks,
            Role = userRole
        };

        return View(model);
    }

    [Authorize(Policy = "EditorOrAbove")]
    public IActionResult SsoRedirect(string url)
    {
        if (string.IsNullOrEmpty(url))
            return BadRequest("URL is required");

        return Redirect(url);
    }
}
