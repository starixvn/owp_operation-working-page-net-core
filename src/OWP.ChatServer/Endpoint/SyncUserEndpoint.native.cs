using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OWP.ChatServer.Handler;

namespace OWP.ChatServer.Endpoint;
public class SyncUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/owp/chat/update-user-name", async (UpdateUserNameInfo[] info, [FromServices] ISender sender)
                => await sender.Send(new UpdateUserNameRequest(info))).RequireAuthorization()
            .WithName("RCUpdateUserName")
            .WithSummary("RC update user name")
            .WithDescription("RC update user name");
    }
}
