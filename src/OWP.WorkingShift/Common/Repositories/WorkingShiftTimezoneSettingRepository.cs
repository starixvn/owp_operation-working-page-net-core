using OWP.WorkingShift.Common.Models;

namespace OWP.WorkingShift.Common.Repositories
{

    public interface WorkingShiftTimezoneSettingRepository
    {
        Task<ResultDataObject> Update(TimezoneUpdateModel model);

        Task<IList<TimezoneSettingModel>> GetTimezoneSettings();

        Task<TimezoneSettingModel> GetSelectedTimezoneSetting();

        Task<TimezoneSettingModel> GetTimezoneSettingByOffset(int offset);
    }

    public class WorkingShiftTimezoneSettingRepositoryImp : WorkingShiftTimezoneSettingRepository
    {
        private readonly IDynamicRepository repository;
        private readonly INapSession currentSession;

        public WorkingShiftTimezoneSettingRepositoryImp(IDynamicRepository repository, INapSession currentSession)
        {
            this.repository = repository;
            this.currentSession = currentSession;
        }

        public async Task<IList<TimezoneSettingModel>> GetTimezoneSettings()
        {
            var criteria = new GetTimezoneSettingCriteria
            {
                UserId = currentSession?.UserId ?? 0
            };

            return (await repository.FetchAsync<TimezoneSettingModel>(criteria)).ToList();
        }

        public async Task<TimezoneSettingModel> GetSelectedTimezoneSetting()
        {
            var timezoneSettings = await GetTimezoneSettings();
            return timezoneSettings.First(x => x.IsSelected);
        }

        public async Task<ResultDataObject> Update(TimezoneUpdateModel model)
        {
            var criteria = new UpdateTimezoneCriteria
            {
                SettingId = model.SettingId,
                UserId = currentSession.UserId
            };

            await repository.ExecuteAsync(criteria);
            return ResultDataObject.Ok;
        }

        public async Task<TimezoneSettingModel> GetTimezoneSettingByOffset(int offset)
        {
            var timezoneSettings = await GetTimezoneSettings();
            return timezoneSettings.First(x => x.Offset == offset);
        }
    }
}