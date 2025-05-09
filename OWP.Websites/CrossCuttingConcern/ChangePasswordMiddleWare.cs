using MediatR;
using OWP.ChatServer.Handler;

namespace OWP.Websites.CrossCuttingConcern;

using System.Text;
using Fanex.NAP.SignIn.Models;
using Newtonsoft.Json;

public class ChangePasswordMiddleWare(RequestDelegate next,
    [FromServices] INapSession session,
    [FromServices] IServiceProvider serviceProvider)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var updatePasswordInfo=await GetNewPassword(context);
       await next(context);
       if (updatePasswordInfo is not null)
       {
           await HandleChangePassword(updatePasswordInfo);
       }

    }

    private static async Task<UpdatePasswordModel> GetNewPassword(HttpContext context)
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
        return model;
       
    }
    public async Task<UpdatePasswordModel> HandleChangePassword(UpdatePasswordModel model)
    {
        model.Username = session.UserName;
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new UpdatePasswordRequest(session.UserName, model.RawUserPwd));
        return model;
    }
}