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

    private static readonly Dictionary<string, List<string>> GroupCmsAccess = new()
    {
        { "f25f2a08-75df-4a8d-b840-3413487b6113", new List<string> { "WordPress CMS", "Umbraco CMS", "Strapi CMS", "Sitecore CMS" } },
        { "GROUP_ID_EDITORS", new List<string> { "WordPress CMS", "Umbraco CMS" } },
        { "GROUP_ID_VIEWERS", new List<string>() }
    };

    public HomeController(ILogger<HomeController> logger, IOptions<CmsLinksOptions> cmsLinks)
    {
        _logger = logger;
        _cmsLinks = cmsLinks.Value;
    }

    public IActionResult Index()
    {
        var allClaims = User.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();
        
        var userGroups = User.Claims
            .Where(c => c.Type == "groups")
            .Select(c => c.Value)
            .ToList();

        var userRole = "Viewer";
        var allowedCms = new List<string>();

        if (userGroups.Any(g => g == GetGroupId("CMS-Admins")))
        {
            userRole = "Admin";
            allowedCms = _cmsLinks.Links.Select(l => l.Name).ToList();
        }
        else if (userGroups.Any(g => g == GetGroupId("CMS-Editors")))
        {
            userRole = "Editor";
            allowedCms = GroupCmsAccess["CMS-Editors"];
        }

        var availableLinks = _cmsLinks.Links
            .Where(l => allowedCms.Contains(l.Name))
            .ToList();

        var model = new AdminDashboardViewModel
        {
            UserName = User.Identity?.Name ?? "Unknown",
            UserEmail = User.FindFirstValue(ClaimTypes.Email) ?? "N/A",
            CmsLinks = availableLinks,
            Role = userRole,
            DebugClaims = allClaims
        };

        return View(model);
    }

    public IActionResult SsoRedirect(string url)
    {
        if (string.IsNullOrEmpty(url))
            return BadRequest("URL is required");

        return Redirect(url);
    }

    private string GetGroupId(string groupName)
    {
        return groupName switch
        {
            "CMS-Admins" => "f25f2a08-75df-4a8d-b840-3413487b6113",
            "CMS-Editors" => "GROUP_ID_EDITORS",
            "CMS-Viewers" => "GROUP_ID_VIEWERS",
            _ => groupName
        };
    }
}
