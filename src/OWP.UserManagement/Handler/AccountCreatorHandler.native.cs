using OWP.CQRS;
using Newtonsoft.Json;
using Fanex.Data.Repository;
using Fanex.Logging;
using Microsoft.Extensions.Configuration;
using Olympus.OWP.Log.Criteria;
using OWP.Extensions;
using OWP.Models;
using OWP.UserManagement.Criteria;

namespace OWP.UserManagement.Handler;

public record AccountOwpCreatorRequest(int UserId) : Command<ResultT<string>>;


public class AccountCreatorHandler(
    IDynamicRepository dynamicRepository,
    IConfiguration configuration) : CommandHandler<AccountOwpCreatorRequest, ResultT<string>>
{
    private const string ACCOUNT_CREATOR_SUCCESS_MESSAGE = "OK";

    public async Task<ResultT<string>> Handle(
        AccountOwpCreatorRequest request, CancellationToken cancellationToken)
    {
        var message = await CreateOWPAccount(request.UserId);
        if (message == ACCOUNT_CREATOR_SUCCESS_MESSAGE)
        {
            return ResultT<string>.Success(message);
        }

        return ResultT<string>.Failure(new List<Error>()
        {
            Error.Failure(ErrorCode.Error,message,null)
        });
    }

  private async Task<string> CreateOWPAccount(int userId)
    {
        var env = configuration.GetSection("Fanex.NAP.Client:Environment").Value;
        var command = new InsertUserToOWPCommand(userId);
        await dynamicRepository.ExecuteAsync(command);
        if (command.ErrCode > 0)
        {
            var message = $"SyncUserToOWP error: UserId: {userId}; Message: {command.ErrMsg}";
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "CreateOWPAccount_Fail",
                JsonConvert.SerializeObject(new
                {
                    message,
                    env
                }));
            await Logger.Log.ErrorAsync(message);
            return message;
        }
        else
        {
            await LogDetailedPayload(DateTime.Now.ToGmt8(), "CreateOWPAccount_Success",
                JsonConvert.SerializeObject(new {userId, env}));
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
