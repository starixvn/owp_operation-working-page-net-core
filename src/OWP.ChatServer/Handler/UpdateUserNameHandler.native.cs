using System.Text;
using Fanex.Data.Repository;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Olympus.OWP.Log.Criteria;
using OWP.CQRS;
using OWP.Extensions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OWP.ChatServer.Handler;

public record UpdateUserNameInfo(string OldUsername, string NewUserName);

public record UpdateUserNameRequest(UpdateUserNameInfo[] UpdateUserNameInfo) : Command<Unit>;


public class UpdateUserNameHandler(
    IDynamicRepository repository, 
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory) : CommandHandler<UpdateUserNameRequest, Unit>
{
    public async Task<Unit> Handle(UpdateUserNameRequest request, CancellationToken cancellationToken)
    {
        var env = string.Empty;
        var response = new HttpResponseMessage();
        foreach (var updateUserNameInfo in request.UpdateUserNameInfo)
        {
            try
            {
                env = configuration.GetSection("Fanex.NAP.Client:Environment").Value;
                using var client = httpClientFactory.CreateClient("OWPChat");

                var userId = await GetUserIdOfChatServerByUserName(client, updateUserNameInfo.OldUsername, cancellationToken);

                var updateData = new
                {
                    userId,
                    data = new
                    {
                        username = updateUserNameInfo.NewUserName
                    }
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(updateData),
                    Encoding.UTF8,
                    "application/json");
                await LogDetailedPayload(DateTime.Now.ToGmt8(), "[Update_Username]Update_OWPChat_UserInfo",
                    userId);

                response = await client.PostAsync("users.update", content, cancellationToken);
            }
            catch (Exception ex)
            {
                await LogDetailedPayload(DateTime.Now.ToGmt8(), "[Update_Username]UpdateOWPChatPassword_Exception",
                    JsonSerializer.Serialize(new
                    {
                        ex,
                        env
                    }));
            }
            finally
            {
                var action = !response.IsSuccessStatusCode
                    ? "[Update_Username]Update_OWPChat_UserInfo_Fail"
                    : "[Update_Username]Update_OWPChat_UserInfo_Success";
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                await LogDetailedPayload(DateTime.Now.ToGmt8(), action, JsonSerializer.Serialize(new
                {
                    responseBody,
                    env
                }));
            }
        }
        return Unit.Value;
    }

    private async Task<string> GetUserIdOfChatServerByUserName(HttpClient httpClient, string userName, CancellationToken cancellationToken)
    {
        try
        {
            var response = await httpClient.GetAsync($"users.info?username={userName}", cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                await LogDetailedPayload(DateTime.Now.ToGmt8(), "[Update_Username]Get_OWPChat_UserId_Fail", JsonConvert.SerializeObject(response));
                return string.Empty;
            }

            dynamic result = JsonConvert.DeserializeObject(responseBody);
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "[Update_Username]Get_OWPChat_UserId_OK", JsonConvert.SerializeObject(new { result.user }));
            return result.user._id;
        }
        catch (Exception e)
        {
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "[Update_Username]Get_OWPChat_UserId_Exception", JsonConvert.SerializeObject(e));
            return string.Empty;
        }
    }

    private async Task LogDetailedPayload(DateTime CreatedDate, string Action, string Payload)
    {
        await repository.ExecuteAsync(new OwpLogCriteria()
        {
            CreatedDate = CreatedDate,
            Action = Action,
            Payload = Payload
        });
    }
}
