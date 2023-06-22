using Fanex.Data.Repository;
using MediatR;
using OWP.Extensions;
using OWP.ProviderArrangement.Criteria;
using OWP.ProviderArrangement.Models;
using OWP.Utils;
// ReSharper disable LoopCanBeConvertedToQuery

namespace OWP.ProviderArrangement.Handlers;

public class GetLogRequest : IRequest<IList<LogDisplayModel>>
{

}

public class GetLogRequestHandler : IRequestHandler<GetLogRequest, IList<LogDisplayModel>>
{

    private readonly IDynamicRepository repository;

    public GetLogRequestHandler(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<LogDisplayModel>> Handle(GetLogRequest request, CancellationToken cancellationToken)
    {
        var criteria = new GetLogProviderCriteria();
        var logModel = await repository.FetchAsync<UserLogModel>(criteria, cancellationToken);

        return BuildLogs(logModel);
    }

    public static IList<LogDisplayModel> BuildLogs(IEnumerable<UserLogModel> logInfos)
    {
        var logResult = new List<LogDisplayModel>();

        foreach (var log in logInfos)
        {
            var infoBeforeChange = ConvertFromXML<ProviderModelDeserialize>(log?.DataOld);
            var infoAfterChange = ConvertFromXML<ProviderModelDeserialize>(log?.Data);
            logResult.Add(new LogDisplayModel
            {
                OldValue = infoBeforeChange?.Providers?.Split(',').ToList(),
                NewValue = infoAfterChange.Providers?.Split(',').ToList(),
                Action = infoAfterChange.ActionName,
                ModifyDate = log?.ModifiedDate.ToGmt8().ToString("MM/dd/yyyy hh:mm tt"),
                ModifyBy = log?.ModifiedBy
            });

        }

        return logResult;
    }
    private static T ConvertFromXML<T>(string xml)
        => xml is null ? default : BuildLogXmlHelper.Deserialize<T>(xml).FirstOrDefault();
}