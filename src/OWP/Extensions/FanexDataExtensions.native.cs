using OWP.Models;

namespace OWP.Extensions;

using Criteria;
using Fanex.Data.Repository;
using Newtonsoft.Json;

public static class FanexDataExtensions
{
    public static async Task<OWPCommandResult> ExecuteCommandAsync(this IDynamicRepository repository, CommandCriteria criteria)
    {
        await repository.ExecuteAsync(criteria).ConfigureAwait(false);
        if (criteria.ErrCode != 0)
        {
            throw new Exception(criteria.ErrMsg);
        }

        return new OWPCommandResult
        {
            Code = 0,
            Message = criteria.ErrMsg
        };
    }


    public static void ThrowExceptionIfInvalid(this ICriteria criteria)
    {
        if (criteria.IsValid())
        {
            return;
        }

        var message = $"Invalid criteria: {JsonConvert.SerializeObject(criteria)}";

        throw new ArgumentException(message);
    }
}