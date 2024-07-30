using Fanex.Data.Repository;
using OWP.NotificationSetting.Criteria;
using OWP.NotificationSetting.Models;
namespace OWP.NotificationSetting;

public interface Repository
{
    Task<IEnumerable<NotificationSettingMapping>> GetNotificationSettingByUserId(int userId);

    Task Update(NotificationSettingUpdateModel model);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository repository;

    public RepositoryImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<NotificationSettingMapping>> GetNotificationSettingByUserId(int userId)
    {
        var criteria = new NotificationSettingByUserIdCriteria
        {
            UserId = userId
        };

        return await repository.FetchAsync<NotificationSettingMapping>(criteria);
    }

    public async Task Update(NotificationSettingUpdateModel model)
    {
        var criteria = new NotificationSettingUpdateCriteria
        {
            UserId = model.UserId,
            Value = model.Value,
            NotificationSettingId = model.NotificationSettingId
        };


        await repository.ExecuteAsync(criteria);
    }
}