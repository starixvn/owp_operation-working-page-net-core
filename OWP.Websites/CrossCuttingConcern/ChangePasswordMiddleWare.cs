namespace OWP.Websites.CrossCuttingConcern;

using System.Text;
using Fanex.NAP.SignIn.Models;
using Newtonsoft.Json;

public class ChangePasswordMiddleWare(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var model = await HandleChangePassword(context);

        await next(context);

        if(model != null)
        {
            Console.WriteLine(JsonConvert.SerializeObject(model));
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
        return model;
    }
}