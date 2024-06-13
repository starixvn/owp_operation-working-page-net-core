
using OWP.WorkingShift.ViewLog.Models;

namespace OWP.WorkingShift.WeeklyWorkingShift.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Criteria;
using Fanex.Data.Repository;
using Models;
using Utils;
using OWP.WorkingShift.Common.Repositories;
using ViewLog;


public interface Repository
{
    Task<List<WorkingShiftUserModel>> SearchWorkingShiftUser(QueryModel model);

    Task<QueryResult> SearchWorkingShiftWeekly(QueryModel model);

    Task<List<UserLogModel>> SearchUserLog(ViewLogModel model);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly WorkingShiftColorRepository workingShiftColorRepository;
    private readonly WorkingShiftDefaultRepository workingShiftDefaultRepository;
    private readonly WorkingShiftPublishRepository workingShiftPublishRepository;
    private readonly WorkingShiftRepository workingShiftRepository;
    private readonly INapSession currentSession;
    private readonly UserRepository userRepository;
    private readonly INapSettings napSettings;

    public RepositoryImp(IDynamicRepository dynamicRepository,
        WorkingShiftColorRepository workingShiftColorRepository,
        WorkingShiftDefaultRepository workingShiftDefaultRepository,
        WorkingShiftPublishRepository workingShiftPublishRepository,
        WorkingShiftRepository workingShiftRepository,
        INapSession currentSession,
        UserRepository userRepository, INapSettings napSettings)
    {
        this.dynamicRepository = dynamicRepository;
        this.workingShiftColorRepository = workingShiftColorRepository;
        this.workingShiftDefaultRepository = workingShiftDefaultRepository;
        this.workingShiftPublishRepository = workingShiftPublishRepository;
        this.workingShiftRepository = workingShiftRepository;
        this.currentSession = currentSession;
        this.userRepository = userRepository;
        this.napSettings = napSettings;
    }

    public async Task<List<WorkingShiftUserModel>> SearchWorkingShiftUser(QueryModel model)
    {
        var criteria = new SearchEmployeeWorkingShiftCriteria
        {
            SiteId = napSettings.SiteId,
            FromDate = model.FromDate,
            ToDate = model.ToDate,
            User = model.User,
            SectionId = model.Section,
            DepartmentId = model.DepartmentId,
            CategoryId = model.Category,
            LocationCode = model.Location,
            ExcludeClosedAccount = model.ExcludeClosedAccount,
            OrderUserId = model.OrderUserId
        };

        var searchUsers = await dynamicRepository.FetchAsync<WorkingShiftUserModel>(criteria);
        if (!model.IsFrame)
        {
            var user = await userRepository.GetUserInfoById(currentSession.UserId);
            if (!model.Permission.WorkingShiftManagementAllSections.IsRead && user.SectionId == 0)
            {
                searchUsers = searchUsers.Where(x => x.UserId == user.UserId);
            }
        }

        return searchUsers.ToList().Order();
    }

    public async Task<QueryResult> SearchWorkingShiftWeekly(QueryModel model)
    {
        var allPublishSections = await workingShiftPublishRepository.GetSectionPublishWorkingShift(model.FromDate, model.ToDate, model.Section);
        if (model.IsFrame)
        {
            allPublishSections = allPublishSections.Where(x => x.IsPublished == true).ToList();
            var sectionIds = allPublishSections.Select(x => x.SectionId).ToArray();
            model.Section = string.Join(",", sectionIds);
        }

        var searchUsers = string.IsNullOrEmpty(model.Section)
            ? new List<WorkingShiftUserModel>()
            : await SearchWorkingShiftUser(model);

        IList<WorkingShiftDefaultModel> workingShiftDefaults = new List<WorkingShiftDefaultModel>();
        IList<WorkingShiftModel> workingShifts = new List<WorkingShiftModel>();
        IList<WorkingShiftColorModel> workingShiftColors = new List<WorkingShiftColorModel>();

        if (searchUsers.Any())
        {
            var userIds = string.Join(",", searchUsers.Select(x => x.UserId));
            workingShiftColors = await workingShiftColorRepository.GetAllByTimeZone(model.TimeZoneOffset);
            if (!model.IsFrame)
            {
                workingShiftDefaults = await workingShiftDefaultRepository.QueryByUser(userIds);
            }

            workingShifts = await workingShiftRepository.QueryByUser(new WorkingShiftQueryByUserModel
            {
                UserIds = userIds,
                FromDate = model.FromDate,
                ToDate = model.ToDate
            });
        }

        searchUsers = searchUsers.AddWorkingShifts(model.FromDate, model.ToDate, workingShifts)
            .AddWorkingShiftDefaults(model.FromDate, model.ToDate, workingShiftDefaults);
        return new QueryResult
        {
            WorkingShiftUsers = searchUsers,
            WorkingShiftColors = workingShiftColors,
            ListSectionPublish = allPublishSections
        };
    }

    public async Task<List<UserLogModel>> SearchUserLog(ViewLogModel model)
    {
        var criteria = new EmployeeWorkingShiftViewLogCriteria
        {
            ObjectId = model.ObjectId,
            PageSize = model.PageSize
        };

        return (await dynamicRepository.FetchAsync<UserLogModel>(criteria)).ToList();
    }
}