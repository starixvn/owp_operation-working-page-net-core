using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OWP.Extensions;
using OWP.OSLeagueSetting.Models;
using OWP.Utils;

namespace OWP.OSLeagueSetting.Services;

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
        var userLogs = (await repository.GetLogSetting(leagueSettingId))
            .OrderByDescending(x => x.ModifiedDate)
            .ToList();

        return BuildLogs(userLogs, mapper);
    }

    public static IList<LogDisplayModel> BuildLogs(IList<UserLogModel> userLog, IMapper mapper)
    {
        var logDisplayModel = new List<LogDisplayModel>();
        foreach (var log in userLog)
        {
            var productBeforeChange = !string.IsNullOrEmpty(log.DataOld)
                ? BuildLogXmlHelper.Deserialize<LogModelDeserialize>(log.DataOld).FirstOrDefault()
                : new LogModelDeserialize();
            var productAfterChange = !string.IsNullOrEmpty(log.Data)
                ? BuildLogXmlHelper.Deserialize<LogModelDeserialize>(log.Data).FirstOrDefault()
                : new LogModelDeserialize();

            logDisplayModel.Add(new LogDisplayModel
            {
                OldValue = mapper.Map<OldDataLeagueSetting>(productBeforeChange),
                NewValue = mapper.Map<NewDataLeagueSetting>(productAfterChange),
                Action = log.Action,
                ModifyDate = log.ModifiedDate.ToGmt8().ToString("MM/dd/yyyy hh:mm tt"),
                ModifyBy = log.ModifiedBy
            });
        }

        return logDisplayModel;
    }
}