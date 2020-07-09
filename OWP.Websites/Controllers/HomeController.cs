using Fanex.Caching;
using Fanex.NAP.Client.Helpers.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.Websites.Models;
using System.Diagnostics;

namespace OWP.Websites.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICache cache;

        private readonly INapSession session;

        public HomeController(ICache cache, INapSession session)
        {
            this.cache = cache;
            this.session = session;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Menu()
        {
            var menuItemList = session.MenuItemList;
            return View(menuItemList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
