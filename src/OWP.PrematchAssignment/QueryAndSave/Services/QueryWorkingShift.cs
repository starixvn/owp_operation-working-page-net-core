using Fanex.Data.Repository;
using Fanex.NAP.Client;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.QueryAndSave.Models;

namespace OWP.PrematchAssignment.QueryAndSave.Services;

public interface QueryWorkingShift
{
    Task<List<WorkingShift>> QueryWorkingShift(DateTime eventDate);
}

public class QueryWorkingShiftImp : QueryWorkingShift
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly INapSession currentSession;

    public QueryWorkingShiftImp(IDynamicRepository dynamicRepository, INapSession currentSession)
    {
        this.dynamicRepository = dynamicRepository;
        this.currentSession = currentSession;
    }

    private async Task<List<WorkingShiftModel>> GetWorkingShiftByDate(DateTime eventDate)
    {
        var criteria = new GetWorkingShiftByDateCriteria
        {
            EventDate = eventDate,
            UserId = currentSession.UserId
        };

        var result = (await dynamicRepository.FetchAsync<WorkingShiftModel>(criteria)).ToList();

        return result;
    }

    public async Task<List<WorkingShift>> QueryWorkingShift(DateTime eventDate)
    {
        var workingShiftByDate = await GetWorkingShiftByDate(eventDate);
        foreach (var shiftCode in workingShiftByDate)
        {
            shiftCode.RecalculateShiftCode();
        }
        var workingShifts = workingShiftByDate.GroupBy(a => a.ShiftCode).ToDictionary(a => a.Key, a => a.Select(b => b.NickName).ToList());

        var workingModel = workingShifts.Select(workingShift =>
            new WorkingShift
            {
                Shifts = workingShift.Key,
                Usernames = string.Join(", ", workingShift.Value.Distinct())
            }).ToList();

        return workingModel.OrderBy(x => x.Shifts).ToList();
    }
}