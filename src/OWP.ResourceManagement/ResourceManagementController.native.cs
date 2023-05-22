using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.ResourceManagement.Models;
using OWP.Services;

namespace OWP.ResourceManagement;

[Route("api/resource-assignment")]
[ApiController]
[EnableCors("AllowOrigin")]
[Authorize]
public class ResourceManagementController : ControllerBase
{
    private readonly Repository repository;
    private readonly RefResourceService refResourceService;

    public ResourceManagementController(Repository repository, RefResourceService refResourceService)
    {
        this.repository = repository;
        this.refResourceService = refResourceService;
    }

    [Route("")]
    [HttpGet]
    public async Task<IActionResult> Get(string refName)
    {
        var data = await refResourceService.GetRefResourcesByName(refName);

        var result = data.Select(x => new RefResourceModel
        {
            RefName = refName,
            Id = x.ResourceId,
            Value = x.Value
        }).ToList();

        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("")]
    [HttpPost]
    public async Task<IActionResult> Post(RefResourceModel model)
    {
        if (string.IsNullOrWhiteSpace(model.RefName) || string.IsNullOrWhiteSpace(model.Value))
        {
            return Ok();
        }

        var result = await repository.Insert(model);
        return Ok(JsonConvert.SerializeObject(result));

    }

    [Route("")]
    [HttpPut]
    public async Task<IActionResult> Put(RefResourceModel model)
    {
        if (model.Id <= 0 || string.IsNullOrWhiteSpace(model.RefName) ||
            string.IsNullOrWhiteSpace(model.Value))
        {
            return Ok();
        }

        var result = await repository.Update(model);
        return Ok(JsonConvert.SerializeObject(result));

    }
}