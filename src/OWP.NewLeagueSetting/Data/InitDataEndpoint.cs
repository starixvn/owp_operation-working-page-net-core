using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace OWP.NewLeagueSetting.Data.InitData
{
    public class InitDataEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("new-league", async (HttpRequest request, ISender sender) =>
            {
                await using var ms = new MemoryStream();
                await Request.Form.Files[0].CopyToAsync(ms);

            }).Accepts("multipart/form-data")
.Produces(200); ;
        }
    }
}