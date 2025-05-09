using System.Text;
using System.Text.Json;
using Fanex.Data.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Olympus.OWP.Log.Criteria;
using OWP.ChatServer.Criteria;
using OWP.CQRS;
using OWP.Extensions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OWP.ChatServer.Handler;

public record AccountRocketChatCreatorRequest(int UserId, string Username) : Command<Unit>;


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
                password = "password",
                email = $"{request.Username}@example.com",
                name = request.Username,
                roles = new[] { "user" }
            };

            var response = await client.PostAsync("users.create",
                new StringContent(JsonSerializer.Serialize(userData),
                    Encoding.UTF8,
                    "application/json"),
                cancellationToken);

            await HandleResponse(response, request.UserId, cancellationToken);
        }
        catch (Exception exception)
        {
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "CreateOWPChatAccount_Exception",
                JsonConvert.SerializeObject(new { exception, env }));
        }
        finally
        {
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "CreateOWPChatAccount_User",
                JsonConvert.SerializeObject(new { request.Username, env }));
        }
        return Unit.Value;
    }

    private async Task HandleResponse(
        HttpResponseMessage response,
        int UserId, CancellationToken cancellationToken)
    {
        var env = configuration.GetSection("Fanex.NAP.Client:Environment").Value;

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        var action = "CreateOWPChatAccount_Fail";
        if (response.IsSuccessStatusCode)
        {
            using var document = JsonDocument.Parse(responseBody);
            var root = document.RootElement;

            var chatUserId = root.GetProperty("user").GetProperty("_id").GetString();
            await UpdateChatUserId(JsonSerializer.Serialize(new List<object>()
            {
                new
                {
                    UserId,
                    ChatUserId = chatUserId,
                }
            }));
            action= "CreateOWPChatAccount_Success";
        }

        await LogDetailedPayload(DateTime.Now.ToGmt8(), action, JsonConvert.SerializeObject(new
        {
            responseBody,
            env
        }));
    }

    private async Task UpdateChatUserId(string userJson)
    {
        await dynamicRepository.ExecuteAsync(new ChatUserInfoCriteria()
        {
            UserJson = userJson
        });
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
