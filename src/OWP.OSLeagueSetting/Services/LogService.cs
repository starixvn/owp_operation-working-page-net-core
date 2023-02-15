namespace OWP.OSLeagueSetting.Services;

using AutoMapper;
using Extensions;
using Models;
using Utils;

public interface LogService
{
    Task<IList<LogDisplayModel>> Build(int leagueSettingId);
}

public class LogServiceImp : LogService
{
    private readonly Repository repository;
    private readonly IMapper mapper;

    public LogServiceImp(Repository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IList<LogDisplayModel>> Build(int leagueSettingId)
    {
        var (logLeagueInfo, logSetting) = await repository.GetLogSetting(leagueSettingId);

        var logInfoSort = logLeagueInfo.OrderByDescending(x => x.ModifiedDate);

        return BuildLogs(logInfoSort, logSetting);
    }

    public IList<LogDisplayModel> BuildLogs(IEnumerable<UserLogModel> logInfos, IEnumerable<UserLogModel> logSettings)
    {
        var logDisplayModel = new List<LogDisplayModel>();
        foreach (var log in logInfos)
        {
            var logSetting = logSettings.FirstOrDefault(x => x.ModifiedDate == log.ModifiedDate);

            var infoBeforeChange = ConvertFromXML<LeagueInfoModelDeserialize>(log?.DataOld);
            var infoAfterChange = ConvertFromXML<LeagueInfoModelDeserialize>(log?.Data);
            var settingBeforeChange = ConvertFromXML<SettingModelDeserialize>(logSetting?.DataOld);
            var settingAfterChange = ConvertFromXML<SettingModelDeserialize>(logSetting?.Data);

            logDisplayModel.Add(new LogDisplayModel
            {
                OldValue = CombineLogData(infoBeforeChange, settingBeforeChange),
                NewValue = CombineLogData(infoAfterChange, settingAfterChange),
                Action = logSetting?.Action ?? log.Action,
                ModifyDate = log.ModifiedDate.ToGmt8().ToString("MM/dd/yyyy hh:mm tt"),
                ModifyBy = log.ModifiedBy
            });
        }

        return logDisplayModel;
    }

    private static T ConvertFromXML<T>(string xml)
        => xml is null ? default : BuildLogXmlHelper.Deserialize<T>(xml).FirstOrDefault();

    private LogDataLeagueSetting CombineLogData(LeagueInfoModelDeserialize info, SettingModelDeserialize setting)
    {
        var data = mapper.Map<LogDataLeagueSetting>(setting) ?? new LogDataLeagueSetting();
        data.LeagueName = info?.LeagueName;
        data.Region = info?.Region;
        data.Category = info?.Category;
        data.IsActive = info?.IsActive;

        return data;
    }
}