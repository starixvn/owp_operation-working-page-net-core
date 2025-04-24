using OWP.CQRS;
using System.Text;
using Newtonsoft.Json;
using OWP.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Fanex.Data.Repository;
using Fanex.NAP.Client;
using Olympus.OWP.Log.Criteria;
using OWP.Extensions;

namespace OWP.Api.Chat.ChatEndpoint;

public record AccountCreatorRequest(string Username, string Password) : Command<ResultT<object>>;


public class AccountCreatorHandler(
    IHttpClientFactory httpClientFactory,
    IDynamicRepository dynamicRepository,
    INapSession napSession) : CommandHandler<AccountCreatorRequest, ResultT<object>>
{
    public async Task<ResultT<object>> Handle(
        AccountCreatorRequest request, CancellationToken cancellationToken)
    {
        try
        {
            using var client = httpClientFactory.CreateClient("OWPChat");

            var userData = new
            {
                username = request.Username,
                password = request.Password,
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

                return ResultT<object>.Failure(new List<Error>()
                {
                    Error.Failure(ErrorCode.Error, "Create account owp chat fail", null)
                });
            }

            dynamic result = JsonConvert.DeserializeObject(responseBody);
            var success = (bool)result.success;

            var action = success
                ? "CreateOWPChatAccount_Success"
                : "CreateOWPChatAccount_Fail";
            await LogDetailedPayload(DateTime.Now.ToGmt8(), action, JsonConvert.SerializeObject(result));

            return success
                ?
                ResultT<object>.Failure(new List<Error>()
                {
                    Error.Failure(ErrorCode.Error,"Create account owp chat fail",null)
                })
                : ResultT<object>.Success(new { message = "Create account owp chat successfully!" });

        }
        catch (Exception e)
        {
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "CreateOWPChatAccount_Exception", JsonConvert.SerializeObject(e));
            return ResultT<object>.Failure(new List<Error>()
            {
                Error.Failure(ErrorCode.Error, "Create account owp chat fail", null)
            });
        }
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
