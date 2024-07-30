using Fanex.NAP.Client.Attributes;
using OWP.Constants;
namespace OWP.EventPicking.Mvc;

using Business = Fanex.NAP.Client.Helpers.Business;

[Route("TaskScheduling/EventPicking")]
public class EventPickingController : Controller
{

    [NapAuth(FunctionKey = FunctionKeys.EventPicking, Policy = Business.Constant.Policy.Read)]
    public ActionResult Index()
    {
        return View("Index", DateTime.Now);
    }

}
