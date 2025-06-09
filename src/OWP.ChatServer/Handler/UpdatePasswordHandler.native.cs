using OWP.CQRS;
using System.Text;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Fanex.Data.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Olympus.OWP.Log.Criteria;
using OWP.ChatServer.Criteria;
using OWP.Extensions;
using System.Net;

namespace OWP.ChatServer.Handler;

public record UpdatePasswordRequest(int UserId, string Username, string NewPassword) : Command<Unit>;

public class UpdatePasswordHandler(
    [FromServices]IHttpClientFactory httpClientFactory,
    [FromServices]IDynamicRepository dynamicRepository,
    [FromServices] IConfiguration configuration) : CommandHandler<UpdatePasswordRequest, Unit>
{
    public async Task<Unit> Handle(
        UpdatePasswordRequest request, CancellationToken cancellationToken)
    {
        var env = string.Empty;
        var response= new HttpResponseMessage();
        try
        {
            env = configuration.GetSection("Fanex.NAP.Client:Environment").Value;
            using var client = httpClientFactory.CreateClient("OWPChat");

            var userId = await GetUserIdOfChatServerByUserName(client, request.Username, cancellationToken);
            if (string.IsNullOrWhiteSpace(userId))
            {
                var errorMessage= $"UserId not found for username: {request.Username}. Maybe account admin not be authorize, or user not exists, search OWPLogs with action=Get_OWPChat_UserId_Fail, Get_OWPChat_UserId_Exception to see more information";
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent(errorMessage);
                throw new Exception(errorMessage);
            }
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
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "Update_OWPChat_UserInfo",
                userId);

            response = await client.PostAsync("users.update", content, cancellationToken);
        }
        catch (Exception ex)
        {
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "UpdateOWPChatPassword_Exception",
                JsonConvert.SerializeObject(new
                {
                    ex.Message,
                    env
                }));
        }
        finally
        {
            var action = !response.IsSuccessStatusCode
            ?"Update_OWPChat_UserInfo_Fail"
            : "Update_OWPChat_UserInfo_Success";
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            await LogDetailedPayload(DateTime.Now.ToGmt8(), action, JsonConvert.SerializeObject(new
            {
                responseBody,
                env
            }));
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
                await LogDetailedPayload(DateTime.Now.ToGmt8(), "Get_OWPChat_UserId_Fail", JsonConvert.SerializeObject(response));
                return string.Empty;
            }

            dynamic result = JsonConvert.DeserializeObject(responseBody);
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "Get_OWPChat_UserId_OK", JsonConvert.SerializeObject(new { result.user }));
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