using MediatR;

namespace OWP.Websites.CrossCuttingConcern;

using System.Text;
using Fanex.NAP.SignIn.Models;
using Newtonsoft.Json;

public class ChangePasswordMiddleWare(RequestDelegate next,
    [FromServices] INapSession session,
    [FromServices] ISender sender)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var model = await HandleChangePassword(context);

        await next(context);

        if (model is not null)
        {

            //group.MapPost("update-password", async (UpdatePasswordRequest request, [FromServices] ISender sender) =>
            //{
            //    var result = await sender.Send(request);
            //    return Results.Ok(result);
            //})
            //.RequireAuthorization()
            //.WithName("OwpChatUpdatePassword")
            //.WithSummary("Update password for owp chat account")
            //.WithDescription("Update password for an existing owp chat account");
        }
    }

    public async Task<UpdatePasswordModel> HandleChangePassword(HttpContext context)
    {
        var request = context.Request;
        if (request.Method != HttpMethods.Post || request.Path.Value != "/Auth/ChangePassword")
        {
            return null;
        }

        request.EnableBuffering();
        using var reader = new StreamReader(
            context.Request.Body,
            Encoding.UTF8,
            leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        var model = JsonConvert.DeserializeObject<UpdatePasswordModel>(body);
        model.Username = session.UserName;
        return model;
    }
}