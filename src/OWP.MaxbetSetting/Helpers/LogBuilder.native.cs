namespace OWP.MaxbetSetting.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using OWP.Utils;
    using OWP.Extensions;
    using AutoMapper;

    public static class LogBuilder
    {
        public static IList<LogDisplayModel> Build(IList<UserLogModel> userLog, IMapper mapper)
        {
            var logDisplayModel = new List<LogDisplayModel>();
            foreach (var log in userLog)
            {
                var productBeforeChange = !string.IsNullOrEmpty(log.DataOld) ? BuildLogXmlHelper.Deserialize<LogModelDeserialize>(log.DataOld).FirstOrDefault() : new LogModelDeserialize();
                var productAfterChange = !string.IsNullOrEmpty(log.Data) ? BuildLogXmlHelper.Deserialize<LogModelDeserialize>(log.Data).FirstOrDefault() : new LogModelDeserialize();
                logDisplayModel.Add(new LogDisplayModel
                {
                    Action = log.Action,
                    OldValue = mapper.Map<MaxBetLogSettingModel>(productBeforeChange),
                    NewValue = mapper.Map<MaxBetLogSettingModel>(productAfterChange),
                    ModifyDate = log.ModifiedDate.ToGmt8().ToString("MM/dd/yyyy hh:mm tt"),
                    ModifyBy = log.ModifiedBy
                });
            }
            return logDisplayModel.OrderByDescending(x => x.ModifyDate).ToList();
        }
    }
}