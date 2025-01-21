using MediatR;
using OWP.WorkingShift.Common.Utils;

namespace OWP.WorkingShift.SyncWorkingShift.Handlers;

public class QueryEmployeeWorkingShiftRequest : IRequest<IEnumerable<SyncWorkingShiftModel>>
{
    public string LocationCode { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public short Timezone { get; set; }
}

public class QueryEmployeeWorkingShiftHandler(
    IMapper mapper,
    WorkingShiftRepository commandRepository) : IRequestHandler<QueryEmployeeWorkingShiftRequest, IEnumerable<SyncWorkingShiftModel>>
{
    private const short Timezone_GMT7 = 7;

    public async Task<IEnumerable<SyncWorkingShiftModel>> Handle(QueryEmployeeWorkingShiftRequest request, CancellationToken cancellationToken)
    {
        var workingShifts = await commandRepository.QueryWorkingShift(request);
        foreach (var workingShift in workingShifts)
        {
            if (request.Timezone == Timezone_GMT7)
            {
                workingShift.ShiftStartTime = workingShift.ShiftStartTime?.AddHours(-1) ?? workingShift.ShiftStartTime;
                workingShift.ShiftEndTime = workingShift.ShiftEndTime?.AddHours(-1) ?? workingShift.ShiftEndTime;
                workingShift.LastUpdated = workingShift.LastUpdated?.AddHours(-1) ?? workingShift.LastUpdated;
            }

            RecalculateShiftCode(workingShift);
        }

        return mapper.Map<IEnumerable<SyncWorkingShiftModel>>(workingShifts);
    }

    private static void RecalculateShiftCode(EmployeeWorkingShiftModel workingShift)
    {
        if (!workingShift.IsTimeShift)
        {
            return;
        }

        var startTime = workingShift.ShiftStartTime?.TimeOfDay ?? TimeSpan.Zero;
        var endTime = workingShift.ShiftEndTime?.TimeOfDay ?? TimeSpan.Zero;

        var startTimeFormat = startTime.Minutes == 0 ? "hh" : @"hh\:mm";
        var endTimeFormat = endTime.Minutes == 0 ? "hh" : @"hh\:mm";
        workingShift.ShiftCode = startTime.ToString(startTimeFormat) + " >" + endTime.ToString(endTimeFormat);
        if (workingShift.Extension.HasValue)
        {
            var extension = workingShift.Extension.Value.ToExtension();
            workingShift.ShiftCode = workingShift.ShiftCode + " " + extension;
        }

        workingShift.ShiftCode = workingShift.ShiftCode.Trim();
    }
}