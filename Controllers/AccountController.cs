using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult SignIn()
    {
        var redirectUrl = Url.Action("Index", "Home");
        return Challenge(
            new AuthenticationProperties { RedirectUri = redirectUrl },
            OpenIdConnectDefaults.AuthenticationScheme);
    }

    [HttpGet]
    public new IActionResult SignOut()
    {
        var callbackUrl = Url.Action("SignedOut", "Account", values: null, protocol: Request.Scheme);
        return SignOut(
            new AuthenticationProperties { RedirectUri = callbackUrl },
            OpenIdConnectDefaults.AuthenticationScheme);
    }

    public IActionResult SignedOut()
    {
        return RedirectToAction("Index", "Home");
    }
}
