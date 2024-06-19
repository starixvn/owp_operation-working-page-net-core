using Fanex.NAP.Client.Attributes;
using OWP.Constants;
using Microsoft.Extensions.Configuration;
namespace OWP.EventPicking.Mvc;

using Business = Fanex.NAP.Client.Helpers.Business;

[Route("TaskScheduling/EventPicking")]
public class EventPickingController(IConfiguration configuration) : Controller
{
    private readonly IConfiguration configuration = configuration;
    
    [NapAuth(FunctionKey = FunctionKeys.EventPicking, Policy = Business.Constant.Policy.Read)]
    public ActionResult Index()
    {
        // TODO
        //var functionName = configuration.GetSection["EventPicking"];
        //await accessCounter.UpdateCounter(functionName);

        return View("Index", DateTime.Now);
    }

}
