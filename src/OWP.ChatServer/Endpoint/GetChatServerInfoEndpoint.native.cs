using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OWP.ChatServer.Handler;

namespace OWP.ChatServer.Endpoint;
public class GetChatServerInfoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/owp/chat/get-chat-server-info", async ([FromServices] ISender sender)
                => await sender.Send(new GetChatServerRequest())).RequireAuthorization()
            .WithName("RCGetChatServerInfo")
            .WithSummary("RC get chat server info")
            .WithDescription("RC get chat server info");
    }
}
