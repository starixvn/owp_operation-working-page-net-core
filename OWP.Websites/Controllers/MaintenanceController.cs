
using OWP.Services;

namespace OWP.Websites.Controllers;

public class MaintenanceMiddleware(RequestDelegate next, MaintenanceService maintenanceService)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var isUnderMaintenance = maintenanceService.IsUnderMaintenance();

        if (isUnderMaintenance && !context.Request.Path.StartsWithSegments("/Maintenance/Offline")
                               && !context.Request.Path.StartsWithSegments("/Maintenance/TurnOffMaintenanceMode"))
        {
            context.Response.Redirect("/Maintenance/Offline");
            return;
        }

        await next(context);
    }
}

public class MaintenanceController(MaintenanceService maintenanceService) : Controller
{
    [HttpGet]
    public IActionResult SetMaintenanceMode()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SetMaintenanceMode(bool isUnderMaintenance, string from, string to)
    {
        maintenanceService.SetMaintenanceMode(isUnderMaintenance);
        TempData["From"] = from;
        TempData["To"] = to;
        return RedirectToAction("Offline");
    }

    [HttpGet]
    public IActionResult Offline()
    {
        ViewBag.From = TempData["From"];
        ViewBag.To = TempData["To"];
        return View("~/Views/Maintenance/Offline.cshtml");
    }

    [HttpPost]
    public IActionResult TurnOffMaintenanceMode()
    {
        maintenanceService.SetMaintenanceMode(false);
        return RedirectToAction("Index", "Home");
    }
}

