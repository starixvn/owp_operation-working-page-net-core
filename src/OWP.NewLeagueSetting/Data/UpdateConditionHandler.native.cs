

using OWP.NewLeagueSetting.Helper;

namespace OWP.NewLeagueSetting.Data;

public record UpdateConditionRequest(Stream Stream) : IRequest<IList<UpdateCondition>>;
public class UpdateConditionHandler : IRequestHandler<UpdateConditionRequest, IList<UpdateCondition>>
{
    private readonly Repository repository;

    public UpdateConditionHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<UpdateCondition>> Handle(UpdateConditionRequest request, CancellationToken cancellationToken)
    {
        var dataExcel = ReadExcel.LoadData(request.Stream);

        var settingInfo = ConditionHelper.BuildCondition(dataExcel);

        await InitData(settingInfo, cancellationToken);
        return settingInfo;
    }

    public async Task InitData(IList<UpdateCondition> settingSource, CancellationToken cancellationToken, int skip = 0, int take = 500)
    {
        if (skip * take > settingSource.Count)
        {
            return;
        }
        await repository.UpdateCondition(settingSource.Skip(skip * take).Take(take).ToList(), cancellationToken);
        await InitData(settingSource, cancellationToken, skip + 1);
    }
}