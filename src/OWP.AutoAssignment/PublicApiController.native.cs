using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.AutoAssignment.Models.InternalApi;
using OWP.AutoAssignment.Services.PublicApi;
using OWP.AutoAssignment.Services.SaveResponse;
using OWP.Models;

namespace OWP.AutoAssignment;

[Route("api/public/auto-assignment")]
[ApiController]
[AllowAnonymous]
public class PublicApiController : ControllerBase
{
    private readonly GetRequestService getRequestService;
    private readonly SaveRequestService saveRequestService;

    public PublicApiController(
        SaveRequestService saveRequestService,
        GetRequestService getRequestService)
    {
        this.getRequestService = getRequestService;
        this.saveRequestService = saveRequestService;
    }

    [HttpGet]
    [Route("get-new")]
    public new async Task<IActionResult> Request()
    {
        var sender = await getRequestService.GetRequest();
        return Ok(JsonConvert.SerializeObject(sender));
    }

    [HttpPost]
    [Route("save-request")]
    public async Task<ResultDataObject> SaveRequest(SaveRequestModel model)
        => await saveRequestService.Save(model);
}