using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Services;
using Newtonsoft.Json;
using OWP.Report.OvertimeReport.Criteria;
using OWP.Report.OvertimeReport.Models;

namespace OWP.Report.OvertimeReport.Services;

public interface CommandRepository
{
    Task<(IEnumerable<User>, IEnumerable<OvertimeData>)> GetReportByMonth(OverTimeRequest request);

    Task<(IEnumerable<User>, IEnumerable<OvertimeData>)> GetReportByYear(OverTimeRequest request);
}

public class CommandRepositoryImp : CommandRepository
{
    private readonly IDynamicRepository repository;
    private readonly INapSettings napSetting;

    private readonly IUserService userService;

    public CommandRepositoryImp(IDynamicRepository repository, INapSettings napSetting, IUserService userService)
    {
        this.repository = repository;
        this.napSetting = napSetting;
        this.userService = userService;
    }

    public async Task<(IEnumerable<User>, IEnumerable<OvertimeData>)> GetReportByMonth(OverTimeRequest request)
    {
        var endMonthDay = DateTime.DaysInMonth(request.Year, request.Month);
        var fromDate = new DateTime(request.Year, request.Month, 1);
        var toDate = new DateTime(request.Year, request.Month, endMonthDay);

        return await GetReportByDateRange(request, fromDate, toDate);
    }

    public async Task<(IEnumerable<User>, IEnumerable<OvertimeData>)> GetReportByYear(OverTimeRequest request)
    {
        var fromDate = new DateTime(request.Year, 1, 1);
        var toDate = new DateTime(request.Year, 12, 31);

        return await GetReportByDateRange(request, fromDate, toDate);
    }

    public async Task<(IEnumerable<User>, IEnumerable<OvertimeData>)> GetReportByDateRange(OverTimeRequest request, DateTime fromDate, DateTime toDate)
    {
        var allUserActive = await userService.GetBySiteAsync(napSetting.SiteId, true);
        var criteria = new GetReportByDateRangeCriteria
        {
            TraderName = request.Trader ?? string.Empty,
            Location = request.Location ?? string.Empty,
            FromDate = fromDate,
            ToDate = toDate,
            AllUserJson = JsonConvert.SerializeObject(allUserActive.Select(u => new
            {
                UserId = u.Id,
                IsActive = true
            }))
        };

        return await repository.FetchMultipleAsync<User, OvertimeData>(criteria);
    }
}