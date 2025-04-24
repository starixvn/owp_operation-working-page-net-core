using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace OWP.Api.Chat.ChatEndpoint;

public class OwpChatEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/owp/chat");

        group.MapPost("create-account", async (AccountCreatorRequest request, [FromServices] ISender sender) =>
        {
            var result = await sender.Send(request);
            return Results.Ok(result);
        }).AllowAnonymous()
        .WithName("OwpChatAccountCreator")
        .WithSummary("Create account owp chat account")
        .WithDescription("Create account owp chat account");

        group.MapPost("update-password", async (UpdatePasswordRequest request, [FromServices] ISender sender) =>
        {
            var result = await sender.Send(request);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("OwpChatUpdatePassword")
        .WithSummary("Update password for owp chat account")
        .WithDescription("Update password for an existing owp chat account");
    }
}
