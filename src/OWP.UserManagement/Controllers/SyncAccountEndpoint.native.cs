using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using OWP.ChatServer.Handler;
using OWP.UserManagement.Handler;
using OWP.UserManagement.Models;

namespace OWP.UserManagement.Controllers;

public class OwpAccountantEndpoint(IConfiguration configuration) : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/owp/chat");

        group.MapPost("create-account", async (SyncUserModel request, [FromServices] ISender sender) =>
        {
            var result = await sender.Send(new AccountOwpCreatorRequest(request));
            if (result.IsSuccess && configuration.GetValue<bool>("ChatServer:EnableFuncCreateAccount"))
            {
                await sender.Send(new AccountRocketChatCreatorRequest(request.UserId,request.Username));
            }

            return result.IsSuccess
                ? "OK"
                : result.Errors.FirstOrDefault().Description;
        }).AllowAnonymous()
        .WithName("OwpAccountCreator")
        .WithSummary("Create account owp chat, account owp")
        .WithDescription("Create account owp chat, account owp");
    }
}
