using Fanex.Caching;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Models.ParameterObject;
using Fanex.NAP.Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OWP.Websites.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

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

        [AllowAnonymous]
        public IActionResult ExpiredSession()
        {
            return View();
        }

#if DEBUG

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> FakeLogin()
        {
            var criteria = new LoginParameter
            {
                UserId = 8,
                SiteId = 99,
                LoginDate = DateTime.Now,
                NetSessionId = HttpContext?.Session.Id,
                Ip = "127.0.0.1",
                BrowserName = Request?.Headers["User-Agent"].ToString(),
                CountryCode = "VN",
                Domain = "owp-local.com",
                Platform = Environment.OSVersion.Platform.ToString()
            };

            var securityService = new SecurityService();
            var onUserId = securityService.Login(criteria);

            await SaveUserSession(onUserId);

            return Content("Faked Login");
        }

        private async Task SaveUserSession(int onUserId)
        {
            const int fakeUserId = 8;
            session.UserId = fakeUserId;
            session.UserName = "sa";
            session.OnUserId = onUserId;

            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, "sa"),
                            new Claim(ClaimTypes.NameIdentifier, "8")
                        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }

#endif
    }
}