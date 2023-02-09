namespace OWP.ProductAssignmentReference;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowOrigin")]
[Authorize]
public class ProductAssignmentController : ControllerBase
{
    private readonly ProductAssignmentService service;

    public ProductAssignmentController(ProductAssignmentService service)
    {
        this.service = service;
    }

    [HttpGet("ref-resource/{name}")]
    public async Task<IEnumerable<object>> GetRefResourceByName(string name) => await service.GetResourceByName(name);

    [HttpGet("last-update")]
    public async Task<object> GetLastUpdate()
        => await service.GetLastUpdate();

    [HttpGet]
    public async Task<IEnumerable<UserInfo>> Get([FromQuery] QueryUserInfo queryInfo)
        => await service.QueryUser(queryInfo);

    [HttpPost]
    public async Task Post(UserInfoEditor info)
        => await service.UpdateUserInfo(info);
}