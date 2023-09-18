using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Models.ParameterObject;
using Fanex.NAP.Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ONEbook.UM.AspNetCore.Filters;
using OWP.Models;
using OWP.Services;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;

namespace OWP.Websites.Controllers;

public class HomeController : Controller
{
    private readonly INapSession session;

    private readonly INapSettings napSettings;

    private readonly AccessCounter accessCounter;

    private readonly AccessCounterModel counterModel;


    public HomeController(INapSession session, INapSettings napSettings, AccessCounter accessCounter, IOptions<AccessCounterModel> counterOptions)
    {
        this.session = session;
        this.napSettings = napSettings;
        this.accessCounter = accessCounter;
        this.counterModel = counterOptions.Value;
    }

    [AllowAnonymous]
    [SkipUMFilter]
    public IActionResult Index()
    {
        ViewBag.IsSessionAvailable = HttpContext.Session.IsAvailable;
        ViewBag.UserId = session.UserId;
        ViewBag.Username = session.UserName;
        ViewBag.Ip = GetIp();
        ViewBag.FakeLoginEnabled = false;

        ViewBag.FakeLoginEnabled = napSettings.Environment == "UAT";
        return View();
    }

    [AllowAnonymous]
    [SkipUMFilter]
    public string GetAccessCounterConfig()
    => counterModel.ToString();

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

    [AllowAnonymous]
    public IActionResult ExpiredSession()
    {
        return View();
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> FakeLogin()
    {
        if (napSettings.Environment == "LIVE")
        {
            return Content("This function is unavailable");
        }

        var criteria = new LoginParameter
        {
            UserId = 8,
            SiteId = 99,
            LoginDate = DateTime.Now,
            NetSessionId = HttpContext.Session.Id,
            Ip = "127.0.0.1",
            BrowserName = Request.Headers["User-Agent"].ToString(),
            CountryCode = "VN",
            Domain = "owp-local.com",
            Platform = Environment.OSVersion.Platform.ToString()
        };

        var securityService = new SecurityService();
        var onUserId = await securityService.LoginAsync(criteria);

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
            new(ClaimTypes.Name, "sa"),
            new(ClaimTypes.NameIdentifier, "8")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
    }
}