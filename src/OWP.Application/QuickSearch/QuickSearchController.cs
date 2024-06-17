

namespace OWP.Application.QuickSearch;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


[Route("api/application")]
public class QuickSearchController : ControllerBase
{
    private readonly Repository repository;

    public QuickSearchController(Repository repository)
    {
        this.repository = repository;
    }

    [Route("quicksearch")]
    [HttpPost]
    public async Task<IEnumerable<MatchInfoModel>> Query(QueryModel model)
    {
        return await repository.Query(model);
    }
}