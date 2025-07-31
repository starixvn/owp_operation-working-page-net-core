using Carter;
using Fanex.NAP.Client;
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

        app.MapGet("api/owp/chat/check-account-exists", async ([FromServices] ISender sender)=>
            {
                return await sender.Send(new AccountRocketChatValidatorRequest());
            }).RequireAuthorization()
            .WithName("RC check account already exists")
            .WithSummary("RC check account already exists")
            .WithDescription("RC check account already exists");
        app.MapPost("api/owp/chat/create-account-rc", async (
                [FromServices] ISender sender, 
                [FromServices] INapSession napSession)=>
            {
                return await sender.Send(new AccountRocketChatCreatorRequest(napSession.UserId,napSession.UserName));
            }).RequireAuthorization()
            .WithName("RC account creator")
            .WithSummary("RC account creator")
            .WithDescription("RC account creator");
    }
}
