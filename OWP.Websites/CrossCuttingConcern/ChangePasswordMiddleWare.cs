using MediatR;
using Org.BouncyCastle.Asn1.Ocsp;
using OWP.ChatServer.Handler;

namespace OWP.Websites.CrossCuttingConcern;

using System.Text;
using System.Text.Json;
using Fanex.NAP.SignIn.Models;
using Newtonsoft.Json;

public class ChangePasswordMiddleWare(RequestDelegate next,
    [FromServices] INapSession session,
    [FromServices] IServiceProvider serviceProvider)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var updatePasswordInfo=await GetNewPassword(context);

        if(updatePasswordInfo is not null 
           && context.Request.Method == HttpMethods.Post
           && context.Request.Path.Value == "/Auth/ChangePassword")
        {
            var errorMessage = await ReadResponseData(context);
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                await HandleChangePassword(updatePasswordInfo);
            }
            return;
        }

        await next(context);
    }


    public async Task<string> ReadResponseData(HttpContext context)
    {
        var originalBody = context.Response.Body;
        var responseBody=string.Empty;
        try
        {
            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            await next(context);

            memStream.Position = 0;
            responseBody = await new StreamReader(memStream).ReadToEndAsync();

            memStream.Position = 0;
            await memStream.CopyToAsync(originalBody);
        }
        catch(Exception ex)
        {

            // TODO
        }
        finally
        {
            context.Response.Body = originalBody;
        }
        using var document = JsonDocument.Parse(responseBody);
        var root = document.RootElement;

        var errorMessage = root.GetProperty("errorMsg").GetString();
        return errorMessage;
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
    public async Task HandleChangePassword(UpdatePasswordModel model)
    {
        model.Username = session.UserName;
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new UpdatePasswordRequest(session.UserId, session.UserName, model.RawUserPwd));

    }
}