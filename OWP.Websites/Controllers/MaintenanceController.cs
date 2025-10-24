using OWP.Services;

namespace OWP.Websites.Controllers;

public class MaintenanceMiddleware(RequestDelegate next, MaintenanceService maintenanceService)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var isUnderMaintenance = maintenanceService.IsUnderMaintenance();
        if (isUnderMaintenance && !context.Request.Path.StartsWithSegments("/Maintenance/Offline")
                               && !context.Request.Path.StartsWithSegments("/Maintenance/SetMaintenanceMode")
                               && !context.Request.Path.StartsWithSegments("/Maintenance/TurnOffMaintenanceMode"))
        {
            var fromTime = maintenanceService.GetFromTime();
            var toTime = maintenanceService.GetToTime();
            context.Response.Redirect($"/Maintenance/Offline?from={fromTime}&to={toTime}");
            return;
        }

        await next(context);
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="maintenanceService"></param>
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
        maintenanceService.SetMaintenanceMode(isUnderMaintenance, from, to);
        return RedirectToAction("Offline", new { from, to });
    }

    [HttpGet]
    public IActionResult Offline(string from, string to)
    {
        ViewBag.From = from;
        ViewBag.To = to;
        return View("~/Views/Maintenance/Offline.cshtml");
    }

    [HttpPost]
    public IActionResult TurnOffMaintenanceMode()
    {
        maintenanceService.SetMaintenanceMode(false);
        return RedirectToAction("Index", "Home");
    }
}

