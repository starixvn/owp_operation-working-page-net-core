using System.Text;
using Fanex.Data.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Olympus.OWP.Log.Criteria;
using OWP.CQRS;
using OWP.Extensions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OWP.ChatServer.Handler;

public record AccountRocketChatCreatorRequest(string Username) : Command<Unit>;


public class AccountCreatorHandler(
    IHttpClientFactory httpClientFactory,
    IDynamicRepository dynamicRepository,
    [FromServices] IConfiguration configuration) : CommandHandler<AccountRocketChatCreatorRequest, Unit>
{
    public async Task<Unit> Handle(
        AccountRocketChatCreatorRequest request, CancellationToken cancellationToken)
    {
        var env = string.Empty;

        try
        {
            env = configuration.GetSection("Fanex.NAP.Client:Environment").Value;
           
            using var client = httpClientFactory.CreateClient("OWPChat");

            var userData = new
            {
                username = request.Username,
                password = "*#*#1234aa*#*#",
                email = $"{request.Username}@example.com",
                name = request.Username,
                roles = new[] { "user" }
            };

            var response = await client.PostAsync("users.create",
                new StringContent(JsonSerializer.Serialize(userData),
                    Encoding.UTF8,
                    "application/json"),
                cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            var action = !response.IsSuccessStatusCode
                ? "CreateOWPChatAccount_Fail"
                : "CreateOWPChatAccount_Success";

            await LogDetailedPayload(DateTime.Now.ToGmt8(), action, JsonConvert.SerializeObject(new
            {
                responseBody,
                env
            }));
        }
        catch (Exception exception)
        {
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "CreateOWPChatAccount_Exception",
                JsonConvert.SerializeObject(new {exception, env}));
        }
        return Unit.Value;
    }

    private async Task LogDetailedPayload(DateTime CreatedDate, string Action, string Payload)
    {
        await dynamicRepository.ExecuteAsync(new OwpLogCriteria()
        {
            CreatedDate = CreatedDate,
            Action = Action,
            Payload = Payload
        });
    }
}
