using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Vivelin.Home.Data;
using Vivelin.Home.ViewModels;

namespace Vivelin.Home.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(HomeContext context)
        {
            Context = context;
        }

        public HomeContext Context { get; }

        [HttpGet("~/")]
        public async Task<IActionResult> Dashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.UserId = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                ViewBag.AccessToken = await HttpContext.GetTokenAsync("access_token");
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