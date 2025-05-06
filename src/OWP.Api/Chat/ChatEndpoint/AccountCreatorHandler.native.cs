using OWP.CQRS;
using System.Text;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Fanex.Data.Repository;
using Fanex.Logging;
using Fanex.NAP.Client;
using Olympus.OWP.Log.Criteria;
using OWP.Extensions;
using OWP.Api.Chat.Criteria;

namespace OWP.Api.Chat.ChatEndpoint;

public record AccountCreatorRequest(string Username,int UserId) : Command<string>;


public class AccountCreatorHandler(
    IHttpClientFactory httpClientFactory,
    IDynamicRepository dynamicRepository,
    INapSession napSession) : CommandHandler<AccountCreatorRequest, string>
{
    private const string ACCOUNT_CREATOR_SUCCESS_MESSAGE="OK";

    public async Task<string> Handle(
        AccountCreatorRequest request, CancellationToken cancellationToken)
    {
        var message=await CreateOWPAccount(request.UserId);
        if (message == ACCOUNT_CREATOR_SUCCESS_MESSAGE)
        {
            await CreateChatAccount(request, cancellationToken);
        }

        return message;
    }

    private async Task CreateChatAccount(AccountCreatorRequest request, CancellationToken cancellationToken)
    {
        try
        {
            using var client = httpClientFactory.CreateClient("OWPChat");

            var userData = new
            {
                username = request.Username,
                password = "Password",
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


            if (!response.IsSuccessStatusCode)
            {
                await LogDetailedPayload(DateTime.Now.ToGmt8(), "CreateOWPChatAccount_Fail", JsonConvert.SerializeObject(responseBody));
            }

            dynamic result = JsonConvert.DeserializeObject(responseBody);
            var success = (bool)result.success;

            var action = success
                ? "CreateOWPChatAccount_Success"
                : "CreateOWPChatAccount_Fail";
            await LogDetailedPayload(DateTime.Now.ToGmt8(), action, JsonConvert.SerializeObject(result));

        }
        catch (Exception e)
        {
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "CreateOWPChatAccount_Exception",
                JsonConvert.SerializeObject(e));
        }
    }
    private async Task<string> CreateOWPAccount(int userId)
    {
        var command = new InsertUserToOWPCommand(userId);
        await dynamicRepository.ExecuteAsync(command);
        if (command.ErrCode > 0)
        {
            var message = $"SyncUserToOWP error: UserId: {userId}; Message: {command.ErrMsg}";
            await Logger.Log.ErrorAsync(message);
            return message;
        }

        return ACCOUNT_CREATOR_SUCCESS_MESSAGE;

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
