using OWP.WorkingShift.Common.Repositories;

namespace OWP.WorkingShift.WeeklyWorkingShift.Services;

using System.Linq;
using System.Threading.Tasks;
using Models;
using Repositories;
using ViewLog.Models;
using ViewLog.Services;
    

public interface ViewLogService
{
    Task<string> ViewLog(ViewLogModel model);
}

public class ViewLogServiceImp : ViewLogService
{
    private readonly Repository repository;
    private readonly WorkingShiftColorRepository colorRepository;
    private readonly TemplateService templateService;

    public ViewLogServiceImp(Repository repository, WorkingShiftColorRepository colorRepository, TemplateService templateService)
    {
        this.repository = repository;
        this.colorRepository = colorRepository;
        this.templateService = templateService;
    }

    public async Task<string> ViewLog(ViewLogModel model)
    {
        var shifts = await colorRepository.GetAll();
        var userLogs = await repository.SearchUserLog(model);
        return templateService.BuildTemplate(new TemplateModel
        {
            UserLogs = userLogs,
            Shifts = shifts.ToDictionary(shift => shift.ShiftId, shift => shift.ShiftCode)
        });
    }
}