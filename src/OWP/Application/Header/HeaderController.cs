using OWP.Application.Header;

namespace OWP.Application;

using System.Threading.Tasks;
using System.Web.Http;

[RoutePrefix("api/application/header")]
public class HeaderController : ApiController
{
    private readonly Service service;

    public HeaderController(Service service)
    {
        this.service = service;
    }

    [Route("")]
    [HttpGet]
    public async Task<Header> Query()
    {
        return await service.Query();
    }
}