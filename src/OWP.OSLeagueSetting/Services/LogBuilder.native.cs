namespace OWP.OSLeagueSetting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using OWP.Extensions;
    using OWP.Utils;
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
}