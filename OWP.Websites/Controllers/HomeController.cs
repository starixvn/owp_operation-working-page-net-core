using System.Text;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Models.ParameterObject;
using Fanex.NAP.Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Collect()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("GC.GetTotalMemory(false): {0:n0}\n<br />", GC.GetTotalMemory(false));

#pragma warning disable S1215 // "GC.Collect" should not be called
            GC.Collect();
            GC.WaitForPendingFinalizers();
#pragma warning restore S1215 // "GC.Collect" should not be called

            builder.AppendFormat("GC.WaitForPendingFinalizers was called but GetTotalMemory(false) still is: {0:n0}\n<br />", GC.GetTotalMemory(false));

            GC.GetTotalMemory(true);

            using var currentProcess = Process.GetCurrentProcess();
            builder.AppendFormat("GC.GetTotalMemory(true) was called and GetTotalMemory is: {0:n0}\n<br />", GC.GetTotalMemory(true));
            builder.AppendFormat("VirtualMemorySize64 is: {0:n0}\n<br />", currentProcess.VirtualMemorySize64);
            builder.AppendFormat("PrivateMemorySize64 is: {0:n0}\n<br />", currentProcess.PrivateMemorySize64);
            builder.AppendFormat("WorkingSet64 is: {0:n0}\n<br />", currentProcess.WorkingSet64);
            builder.AppendFormat("PagedMemorySize64 is: {0:n0}\n<br />", currentProcess.PagedMemorySize64);
            builder.AppendFormat("NonpagedSystemMemorySize64 is: {0:n0}\n<br />", currentProcess.NonpagedSystemMemorySize64);

            return Content(builder.ToString());
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