using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Fanex.Data.Repository;
using Fanex.NAP.Client;
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

public record AccountRocketChatValidatorRequest : Command<bool>;

public class AccountValidatorHandler(
    IHttpClientFactory httpClientFactory,
    IDynamicRepository dynamicRepository,
    [FromServices] IConfiguration configuration,
    [FromServices] INapSession napSession
    ) : CommandHandler<AccountRocketChatValidatorRequest, bool>
{
    public async Task<bool> Handle(
        AccountRocketChatValidatorRequest request, CancellationToken cancellationToken)
    {
        var env = string.Empty;

        try
        {
            env = configuration.GetSection("Fanex.NAP.Client:Environment").Value;

            var client = httpClientFactory.CreateClient("OWPChat");

            return await ExistsUserByUsername(client, napSession.UserName);
           
        }
        catch (Exception exception)
        {
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "ExistsUserByUsername - Fail",
                JsonConvert.SerializeObject(new { exception }));
        }

        return true;
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

    public async Task<bool> ExistsUserByUsername(HttpClient client, string username)
    {
        var json = string.Empty;
        try
        {
            var url = $"/api/v1/users.info?username={username}";
            var response = await client.GetAsync(url);
            json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                dynamic result = JsonConvert.DeserializeObject(json);
                if (!string.IsNullOrWhiteSpace(result.user._id.ToString()))
                {
                    await LogDetailedPayload(DateTime.Now.ToGmt8(), "ExistsUserByUsername", $"User: {username} already exists");
                    return true;
                }
            }
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "ExistsUserByUsername", $"User: {username} not exists");

            return false;
        }
        catch (Exception exception)
        {
            throw new Exception($"Error fetching user info: {json}");
        }
    }
}
