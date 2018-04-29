using System;
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
        public IActionResult Index()
        {
            // var authentication = await HttpContext.AuthenticateAsync(); if
            // (authentication.Succeeded && authentication.Properties != null) {
            // var userId = User.FindFirst(x => x.Type ==
            // ClaimTypes.NameIdentifier)?.Value; var accessToken =
            // authentication.Properties.GetTokenValue("access_token"); }

            var viewModel = new HomeViewModel
            {
                Quote = Context.Quotes.Sample()
            };
            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}