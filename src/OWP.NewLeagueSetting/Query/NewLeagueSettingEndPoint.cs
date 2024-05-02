using OWP.NewLeagueSetting.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carter.Response;

namespace OWP.NewLeagueSetting.Query;
public class NewLeagueSettingEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/NewLeagueSetting/getAllListSettings", async (string league , ISender sender, HttpResponse res) =>
        {
           var result =  await sender.Send(new QueryLeagueSettingRequest(league));
           return res.Negotiate(result);
        }).RequireAuthorization();
    }
}

