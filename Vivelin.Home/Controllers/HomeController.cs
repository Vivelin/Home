using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult Index()
        {
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
