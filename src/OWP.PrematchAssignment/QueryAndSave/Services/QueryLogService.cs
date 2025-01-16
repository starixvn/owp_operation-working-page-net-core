using Fanex.Data.Repository;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.Handlers;
using OWP.PrematchAssignment.Misc;
using OWP.PrematchAssignment.Models;

namespace OWP.PrematchAssignment.Services;

public interface QueryLogService
{
    Task<QueryLogViewModel> Query(QueryLogRequest request);
}

public class QueryLogServiceImp : QueryLogService
{
    private readonly IDynamicRepository dynamicRepo;

    private readonly LogFactory logFactory;

    public QueryLogServiceImp(IDynamicRepository dynamicRepo, LogFactory logFactory)
    {
        this.dynamicRepo = dynamicRepo;
        this.logFactory = logFactory;
    }

    public async Task<QueryLogViewModel> Query(QueryLogRequest request)
    {
        var queryLogCriteria = new QueryLogCriteria { PageNumber = request.PageNumber, PageSize = request.PageSize, IsDay = request.IsDay };
        var logs = await dynamicRepo.FetchMultipleAsync<Log, bool>(queryLogCriteria);

        foreach (var log in logs.Item1)
        {
            var modifiedDate = log.LastModified.AddHours(12).ToString("MM/dd/yyyy hh:mm tt");
            var newData = logFactory.BuildColumn(log.Action, log.NewValue);
            var oldData = logFactory.BuildColumn(log.Action, log.OldValue);

            log.LastModifiedInString = modifiedDate;
            log.Action = logFactory.GetLogType(log.Action, string.IsNullOrEmpty(log.NewValue) ? log.OldValue : log.NewValue);
            log.NewValue = newData;
            log.OldValue = oldData;
        }

        return new QueryLogViewModel { Logs = logs.Item1, IsLastPage = logs.Item2.First() };
    }
}