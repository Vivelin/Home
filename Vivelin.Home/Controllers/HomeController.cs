using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Vivelin.Home.Data;
using Vivelin.Home.ViewModels;

namespace Vivelin.Home.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(HomeContext context, IConfigurationRoot configuration)
        {
            Context = context;
            Configuration = configuration;
        }

        public HomeContext Context { get; }

        public IConfigurationRoot Configuration { get; }

        [HttpGet("~/")]
        public async Task<IActionResult> Dashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.UserId = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                ViewBag.AccessToken = await HttpContext.GetTokenAsync("access_token");
                ViewBag.ClientId = Configuration["TwitchClientId"];
            }

            var viewModel = new HomeViewModel
            {
                Quote = Context.Quotes.Sample()
            };
            return View(viewModel);
        }

        [HttpGet("~/status")]
        public IActionResult Status()
        {
            return ViewComponent("Status");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}