using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<List<RefResourceModel>> Get(string refName)
    {
        var data = await refResourceService.GetRefResourcesByName(refName);

        return data.Select(x => new RefResourceModel
        {
            RefName = refName,
            Id = x.ResourceId,
            Value = x.Value
        }).ToList();
    }

    [Route("")]
    [HttpPost]
    public async Task<RefResourceModel> Post(RefResourceModel model)
    {
        if (!string.IsNullOrWhiteSpace(model.RefName) && !string.IsNullOrWhiteSpace(model.Value))
        {
            return await repository.Insert(model);
        }

        return await Task.FromResult(model);
    }

    [Route("")]
    [HttpPut]
    public async Task<RefResourceModel> Put(RefResourceModel model)
    {
        if (model.Id > 0 && !string.IsNullOrWhiteSpace(model.RefName) && !string.IsNullOrWhiteSpace(model.Value))
        {
            return await repository.Update(model);
        }

        return await Task.FromResult(model);
    }
}