using System.Threading.Tasks;
using OWP.MessageManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OWP.MessageManagement;

[Route("api/message_management")]
public class MessageManagementApiController : ControllerBase
{
    private readonly Repository repository;

    public MessageManagementApiController(Repository repository)
    {
        this.repository = repository;
    }

    [Route("search")]
    [HttpPost]
    public async Task<IActionResult> GetByUserIdWithPaging([FromBody] MessageBoxQueryParam param)
    {
        var result = await repository.GetByUserIdWithPaging(param);
       return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("update_read_status")]
    [HttpPut]
    public async Task<object> UpdateReadStatus([FromBody] MessageBoxUpdateParam model)
    {
        await repository.UpdateReadStatus(model);

        return new { msg = "OK" };
    }
}
