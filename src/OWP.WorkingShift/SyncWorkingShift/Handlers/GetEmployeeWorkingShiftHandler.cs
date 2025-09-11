using MediatR;
using OWP.WorkingShift.Common.Utils;

namespace OWP.WorkingShift.SyncWorkingShift.Handlers;

public class QueryEmployeeWorkingShiftRequest : IRequest<IEnumerable<SyncWorkingShiftModel>>
{
    public string LocationCode { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public float Timezone { get; set; }
}

public class QueryEmployeeWorkingShiftHandler(
    IMapper mapper,
    WorkingShiftRepository commandRepository) : IRequestHandler<QueryEmployeeWorkingShiftRequest, IEnumerable<SyncWorkingShiftModel>>
{
    public async Task<IEnumerable<SyncWorkingShiftModel>> Handle(QueryEmployeeWorkingShiftRequest request, CancellationToken cancellationToken)
    {
        const float timezone_GMT5_5 = 5.5F;
        const float timezone_GMT7 = 7;
        var workingShifts = await commandRepository.QueryWorkingShift(request);
        foreach (var workingShift in workingShifts)
        {
            switch (request.Timezone)
            {
                case timezone_GMT7:
                    workingShift.ShiftStartTime = ConvertDateTime(workingShift.ShiftStartTime, -1);
                    workingShift.ShiftEndTime = ConvertDateTime(workingShift.ShiftEndTime, -1);
                    workingShift.LastUpdated = ConvertDateTime(workingShift.LastUpdated, -1);
                    break;
                case timezone_GMT5_5:
                    workingShift.ShiftStartTime = ConvertDateTime(workingShift.ShiftStartTime, -2.5F);
                    workingShift.ShiftEndTime = ConvertDateTime(workingShift.ShiftEndTime, -2.5F);
                    workingShift.LastUpdated = ConvertDateTime(workingShift.LastUpdated, -2.5F);
                    break;
                default:
                    // do nothing
                    break;
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

    private static DateTime? ConvertDateTime(DateTime? date, float offset) => date?.AddHours(offset);
}