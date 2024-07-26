using Fanex.Logging;
using MediatR;
using WorkingShiftModel = OWP.WorkingShift.SyncWorkingShift.Models.WorkingShiftModel;

namespace OWP.WorkingShift.SyncWorkingShift.Handlers;

public class QueryWorkingShiftRequest : IRequest<IEnumerable<WorkingShiftModel>>
{
    public DateTime From { get; set; }

    public DateTime To { get; set; }
}

public class Handler : IRequestHandler<QueryWorkingShiftRequest, IEnumerable<WorkingShiftModel>>
{
    private readonly WorkingShiftRepository commandRepository;
    private readonly ILogger logger;

    public Handler(WorkingShiftRepository commandRepository, ILogger logger)
    {
        this.commandRepository = commandRepository;
        this.logger = logger;
    }

    public async Task<IEnumerable<WorkingShiftModel>> Handle(QueryWorkingShiftRequest request, CancellationToken cancellationToken)
    {
        var workingShiftModels = (await commandRepository.Query(request)).ToList();
        var wss = workingShiftModels.FirstOrDefault(x => x.EmployeeID == "EM0165");

        if (wss is not null)
        {
            await logger.InfoAsync("wss=>LogShiftCode:" + wss.LogShiftCode + "LogIsTimeShift:" + wss.LogIsTimeShift+ " ShiftStartTime:" + wss.ShiftStartTime+ "ShiftStartTime: " + wss.ShiftStartTime);
        }
        foreach (var shift in workingShiftModels)
        {
            shift.RecalculateShiftCode();
        }


        if (wss is not null)
        {
            await logger.InfoAsync("wss=>LogShiftCode:" + wss.LogShiftCode + "LogIsTimeShift:" + wss.LogIsTimeShift + " ShiftStartTime:" + wss.ShiftStartTime + "ShiftStartTime: " + wss.ShiftStartTime);
        }
        return workingShiftModels;
    }
}