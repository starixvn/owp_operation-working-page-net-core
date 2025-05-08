using OWP.CQRS;
using Newtonsoft.Json;
using Fanex.Data.Repository;
using Fanex.Logging;
using Microsoft.Extensions.Configuration;
using Olympus.OWP.Log.Criteria;
using OWP.Extensions;
using OWP.Models;
using OWP.UserManagement.Criteria;
using OWP.UserManagement.Models;

namespace OWP.UserManagement.Handler;

public record AccountOwpCreatorRequest(SyncUserModel UserModel) : Command<ResultT<string>>;


public class AccountCreatorHandler(
    IDynamicRepository dynamicRepository,
    IConfiguration configuration) : CommandHandler<AccountOwpCreatorRequest, ResultT<string>>
{
    private const string ACCOUNT_CREATOR_SUCCESS_MESSAGE = "OK";

    public async Task<ResultT<string>> Handle(
        AccountOwpCreatorRequest request, CancellationToken cancellationToken)
    {
        var message = await CreateOWPAccount(request);
        if (message == ACCOUNT_CREATOR_SUCCESS_MESSAGE)
        {
            return ResultT<string>.Success(message);
        }

        return ResultT<string>.Failure(new List<Error>()
        {
            Error.Failure(ErrorCode.Error,message,null)
        });
    }

  private async Task<string> CreateOWPAccount(AccountOwpCreatorRequest request)
  {
      var env = configuration.GetSection("Fanex.NAP.Client:Environment").Value;
      var command = new InsertUserToOWPCommand
      {
          UserId = request.UserModel.UserId,
          Username = request.UserModel.Username
      };
      await dynamicRepository.ExecuteAsync(command);
      if (command.ErrCode > 0)
      {
          var message = $"SyncUserToOWP error: UserId: {request.UserModel.UserId}; Message: {command.ErrMsg}";
          await LogDetailedPayload(DateTime.Now.ToGmt8(), "CreateOWPAccount_Fail",
              JsonConvert.SerializeObject(new
              {
                  request,
                  message,
                  env
              }));
          await Logger.Log.ErrorAsync(message);
          return message;
      }

      await LogDetailedPayload(DateTime.Now.ToGmt8(), "CreateOWPAccount_Success",
          JsonConvert.SerializeObject(new { request, env }));

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
