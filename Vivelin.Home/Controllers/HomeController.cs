using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vivelin.Home.Data;

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
            var quote = Context.Quotes.Sample();
            if (quote != null)
            {
                ViewData["QuoteText"] = quote.Text;
                ViewData["QuoteCite"] = quote.Citation;
            }
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
