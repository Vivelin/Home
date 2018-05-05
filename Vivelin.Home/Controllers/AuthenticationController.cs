using System;
using AspNet.Security.OAuth.Twitch;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Vivelin.Home.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpPost("~/login")]
        public IActionResult LogIn(string returnUrl)
        {
            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl, IsPersistent = true }, TwitchAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpPost("~/logout")]
        public IActionResult LogOut(string returnUrl)
        {
            return SignOut(new AuthenticationProperties { RedirectUri = returnUrl }, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}