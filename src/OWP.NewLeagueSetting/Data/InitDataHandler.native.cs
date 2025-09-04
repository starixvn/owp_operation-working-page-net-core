

namespace OWP.NewLeagueSetting.Data;

public record InitDataRequest(Stream Stream) : IRequest<IList<LeagueSettingInfo>>;
public class InitDataHandler(Repository repository) : IRequestHandler<InitDataRequest, IList<LeagueSettingInfo>>
{
    public async Task<IList<LeagueSettingInfo>> Handle(InitDataRequest request, CancellationToken cancellationToken)
    {
        var dataExcel = ReadExcel.LoadData(request.Stream);

        ReadExcel.CorrectSpecialSpreadValue(dataExcel);
        var settingInfo = StandardizedLeagueInfo.Execute(dataExcel);

        await InitData(settingInfo, cancellationToken);
        return settingInfo;
    }

    public async Task InitData(IList<LeagueSettingInfo> settingSource, CancellationToken cancellationToken, int skip = 0, int take = 500)
    {
        if (skip * take > settingSource.Count)
        {
            return;
        }
        await repository.InitData(settingSource.Skip(skip * take).Take(take).ToList(), cancellationToken);
        await InitData(settingSource, cancellationToken, skip + 1);
    }
}