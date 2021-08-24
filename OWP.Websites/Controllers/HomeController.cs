using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Threading.Tasks;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Models.ParameterObject;
using Fanex.NAP.Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OWP.Websites.Models;

namespace OWP.Websites.Controllers
{
    public class HomeController : Controller
    {
        private readonly INapSession session;

        public HomeController(INapSession session)
        {
            this.session = session;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewBag.IsSessionAvailable = HttpContext.Session.IsAvailable;
            ViewBag.UserId = session.UserId;
            ViewBag.Username = session.UserName;
            ViewBag.Ip = GetIp();
            ViewBag.FakeLoginEnabled = false;
#if DEBUG
            ViewBag.FakeLoginEnabled = true;
#endif
            return View();
        }

        private static string GetIp()
        {
            var hostname = Dns.GetHostName();
            var log = $"Hostname: {hostname}. ";
            IPHostEntry hostEntry = null;
            try
            {
                hostEntry = Dns.GetHostEntry(hostname);
            }
            catch (Exception e)
            {
                log += $"Error when get host entry: {e.Message}. ";
            }

            var ip = GetMachineIp(hostEntry);
            log += $"IP:{ip}. ";

            return log;
        }

        private static string GetMachineIp(IPHostEntry hostEntry)
        {
            foreach (var ip in hostEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "1.1.1.1";
        }

        [AllowAnonymous]
        public IActionResult Test()
        {
            return Content("Test");
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