namespace OWP.AutoAssignment;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.InternalApi;
using Newtonsoft.Json;
using OWP.Models;
using Services.PublicApi;
using Services.SaveResponse;

[Route("api/public/auto-assignment")]
[ApiController]
[AllowAnonymous]
[EnableCors("AllowOrigin")]
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