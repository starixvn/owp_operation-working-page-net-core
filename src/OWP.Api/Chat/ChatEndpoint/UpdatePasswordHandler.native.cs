using OWP.CQRS;
using System.Text;
using Newtonsoft.Json;
using OWP.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Fanex.Data.Repository;
using Olympus.OWP.Log.Criteria;
using OWP.Extensions;

namespace OWP.Api.Chat.ChatEndpoint;

public record UpdatePasswordRequest(string Username, string NewPassword) : Command<ResultT<object>>;

public class UpdatePasswordHandler(
    IHttpClientFactory httpClientFactory,
    IDynamicRepository dynamicRepository) : CommandHandler<UpdatePasswordRequest, ResultT<object>>
{
    public async Task<ResultT<object>> Handle(
        UpdatePasswordRequest request, CancellationToken cancellationToken)
    {
        try
        {
            using var client = httpClientFactory.CreateClient("OWPChat");
           
            var userId = await GetUserIdOfChatServerByUserName(client, request.Username, cancellationToken);
            var updateData = new
            {
                userId,
                data = new
                {
                    password = request.NewPassword
                }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(updateData),
                Encoding.UTF8,
                "application/json");
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "Update_OWPChat_UserInfo", JsonConvert.SerializeObject(updateData));

            var response = await client.PostAsync("users.update", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                await LogDetailedPayload(DateTime.Now.ToGmt8(), "Update_OWPChat_UserInfo_Fail", JsonConvert.SerializeObject(response));

                return ResultT<object>.Failure(new List<Error>()
                    {
                        Error.Failure(ErrorCode.Error,"Failed to update password",null)
                    });

                
            }
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "Update_OWPChat_UserInfo_Success", JsonConvert.SerializeObject(response));

            return ResultT<object>.Success(new { message = "Password updated successfully" });
        }
        catch (Exception ex)
        {
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "UpdateOWPChatPassword_Exception", JsonConvert.SerializeObject(ex));
            return ResultT<object>.Failure(new List<Error>()
            {
                Error.Failure(ErrorCode.Error, "Update account password chat fail", null)
            });
        }
    }

    private async Task<string> GetUserIdOfChatServerByUserName(HttpClient httpClient, string userName, CancellationToken cancellationToken)
    {
        try
        {
            var response = await httpClient.GetAsync($"users.info?username={userName}", cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                await LogDetailedPayload(DateTime.Now.ToGmt8(), "Get_OWPChat_UserId_Fail", JsonConvert.SerializeObject(responseBody));
                return string.Empty;
            }

            dynamic result = JsonConvert.DeserializeObject(responseBody);
            var success = (bool)result.success;
            if (!success)
            {
                await LogDetailedPayload(DateTime.Now.ToGmt8(), "Get_OWPChat_UserId_Fail", JsonConvert.SerializeObject(responseBody));
                return string.Empty;
            }
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "Get_OWPChat_UserId_OK", JsonConvert.SerializeObject(new{ result.user}));
            return result.user._id;
        }
        catch (Exception e)
        {
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "Get_OWPChat_UserId_Exception", JsonConvert.SerializeObject(e));
            return string.Empty;
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