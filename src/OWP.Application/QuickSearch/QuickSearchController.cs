namespace OWP.Application.QuickSearch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

[Route("api/application")]
public class QuickSearchController : ControllerBase
{
    private readonly QuickSearchRepository repository;

    public QuickSearchController(QuickSearchRepository repository)
    {
        this.repository = repository;
    }

    [Route("quicksearch")]
    [HttpPost]
    public async Task<IActionResult> Query([FromBody] QueryModel model)
    {
         var result= await repository.Query(model);
         return Ok(JsonConvert.SerializeObject(result));
    }
}