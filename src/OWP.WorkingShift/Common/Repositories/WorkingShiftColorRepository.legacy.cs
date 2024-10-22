using Fanex.NAP.Client;
using OWP.WorkingShift.Common.Utils;

namespace OWP.WorkingShift.Common.Repositories;

public interface WorkingShiftColorRepository
{
    Task<ResultDataObject> ValidateBeforeCreateAsync(string shiftCode);

    Task<ResultDataObject> Insert(QuickInsertWorkingShiftColorModel model);

    Task<ResultDataObject> Update(WorkingShiftColorModel shift);

    Task<ResultDataObject> ValidateBeforeDeleteAsync(int shiftId);

    Task<ResultDataObject> Delete(int shiftId);

    Task<List<WorkingShiftColorModel>> GetAll();

    Task<List<WorkingShiftColorModel>> GetAllByTimeZone(int? timeZoneOffset);

    Task<List<WorkingShiftColorModel>> GetAllGmt8();
}

public class RepositoryWorkingShiftColorImp : WorkingShiftColorRepository
{
    private readonly IDynamicRepository repository;
    private readonly INapSession currentSession;
    private readonly WorkingShiftTimezoneSettingRepository workingShiftTimezoneSettingRepository;

    private const string InvalidShiftCodeErrorMessage = "Shift should not contain specific characters like < or ,";

    public RepositoryWorkingShiftColorImp(IDynamicRepository repository, INapSession currentSession, WorkingShiftTimezoneSettingRepository workingShiftTimezoneSettingRepository)
    {
        this.repository = repository;
        this.currentSession = currentSession;
        this.workingShiftTimezoneSettingRepository = workingShiftTimezoneSettingRepository;
    }

    public async Task<List<WorkingShiftColorModel>> GetAll()
    {
        var criteria = new GetAllWorkingShiftColorCriteria
        {
            UserId = currentSession.UserId
        };
        return (await GetAll(criteria)).ToList();
    }

    public async Task<List<WorkingShiftColorModel>> GetAllGmt8()
    {
        var criteria = new GetAllWorkingShiftColorCriteria
        {
            UserId = 0
        };
        return (await GetAll(criteria)).ToList();
    }

    public async Task<List<WorkingShiftColorModel>> GetAllByTimeZone(int? timeZoneOffset)
    {
        var criteria = new GetAllWorkingShiftColorCriteria
        {
            UserId = currentSession.UserId,
            TimeZoneOffset = timeZoneOffset
        };
        return (await GetAll(criteria)).ToList();
    }

#pragma warning disable S138 // Functions should not have too many lines of code
#pragma warning disable S1541 // Methods and properties should not be too complex
#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
    public async Task<ResultDataObject> ValidateBeforeCreateAsync(string shiftCode)
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
#pragma warning restore S1541 // Methods and properties should not be too complex
#pragma warning restore S138 // Functions should not have too many lines of code
    {
        shiftCode = shiftCode.Trim().ToUpperInvariant();
        if (!shiftCode.IsValidShiftCode())
        {
            return new ResultDataObject
            {
                Code = (int)ErrorCode.Error,
                Message = InvalidShiftCodeErrorMessage
            };
        }
        var extension = 0;
        ShiftTimeSpan shiftTimeSpan = null;
        if (shiftCode.Contains('>'))
        {
            var currentTimeZone = await workingShiftTimezoneSettingRepository.GetSelectedTimezoneSetting();
            var getHourOfTimeZone = currentTimeZone.Offset / 60;
            var minHour = 7 + getHourOfTimeZone;
            var maxHour = 24 + getHourOfTimeZone;

            var shiftCodeWithExtension = shiftCode.ToShiftCodeWithExtension();

            var newShiftCode = shiftCodeWithExtension.Item1;

            extension = shiftCodeWithExtension.Item2;
            if (extension == (int)WorkingShiftExtension.Real || extension == (int)WorkingShiftExtension.Special)
            {
                shiftTimeSpan = HelperService.ParseShiftTimeSpan(newShiftCode);
                if (shiftTimeSpan == null)
                {
                    return new ResultDataObject
                    {
                        Code = (int)ErrorCode.Error,
                        Message = "Incorrect format time!"
                    };
                }
            }

            if (extension == (int)WorkingShiftExtension.Real)
            {
                if (shiftTimeSpan.StartTime.Hours < maxHour && !(shiftTimeSpan.StartTime.Hours >= 0 && shiftTimeSpan.StartTime.Hours < minHour))
                {
                    return new ResultDataObject
                    {
                        Code = (int)ErrorCode.Error,
                        Message = "The hour(IN) of REAL shift violates the rules! You cannot add this shift!"
                    };
                }
            }
            else if (extension == (int)WorkingShiftExtension.Special)
            {
                if (!(shiftTimeSpan.StartTime.Hours >= minHour && shiftTimeSpan.StartTime.Hours < maxHour))
                {
                    return new ResultDataObject
                    {
                        Code = (int)ErrorCode.Error,
                        Message = "The hour(IN) of SPECIAL shift violates the rules! You cannot add this shift!"
                    };
                }
            }
            else
            {
                shiftTimeSpan = HelperService.ParseShiftTimeSpan(newShiftCode);
                if (shiftTimeSpan == null)
                {
                    return new ResultDataObject
                    {
                        Code = (int)ErrorCode.WarningToContinue,
                        Message = "Shift time is incorrect format. Are you sure you want to continue?"
                    };
                }
            }
        }

        var shifts = await GetAll();
        var isShiftExisted = shifts.Exists(shift => IsShiftExisted(shiftCode, shift, extension, shiftTimeSpan));
        if (isShiftExisted)
        {
            return new ResultDataObject
            {
                Code = (int)ErrorCode.Error,
                Message = "Shift existed"
            };
        }

        var nonTimeShifts = shifts.Where(s => !s.IsTimeShift);
        var plainShifts = nonTimeShifts.Select(shift => new
        {
            shift.ShiftCode,
            PlainShiftCode = shift.ShiftCode.RemoveSpecificCharacters()
        });

        var plainShiftCode = shiftCode.RemoveSpecificCharacters();
        var isExistSimilarShift = plainShifts.Any(shift => shift.PlainShiftCode == plainShiftCode);
        if (isExistSimilarShift)
        {
            return new ResultDataObject
            {
                Code = (int)ErrorCode.WarningToContinue,
                Message = "Shift name is nearly the same as the existed one. Are you sure you want to continue?"
            };
        }

        return ResultDataObject.Ok;
    }

    private static bool IsShiftExisted(string shiftCode, WorkingShiftColorModel shift, int extension, ShiftTimeSpan shiftTimeSpan)
    {
        if (shiftTimeSpan == null)
        {
            return !shift.IsTimeShift && shift.ShiftCode == shiftCode;
        }

        return shiftTimeSpan.StartTime == shift.StartTime && shiftTimeSpan.EndTime == shift.EndTime && shift.Extension == extension;
    }

    public async Task<ResultDataObject> Insert(QuickInsertWorkingShiftColorModel model)
    {
        var shiftCode = model.ShiftCode.Trim().ToUpperInvariant();
        if (!shiftCode.IsValidShiftCode())
        {
            return new ResultDataObject
            {
                Code = (int)ErrorCode.Error,
                Message = InvalidShiftCodeErrorMessage
            };
        }

        var shiftCodeWithExtension = shiftCode.ToShiftCodeWithExtension();
        shiftCode = shiftCodeWithExtension.Item1;
        var extension = shiftCodeWithExtension.Item2;

        var background = shiftCode.GenerateBackgroundColor();
        var shiftTimeSpan = HelperService.ParseShiftTimeSpan(shiftCode);

        var criteria = new InsertWorkingShiftColorCriteria
        {
            ActorId = currentSession.UserId,
            ShiftCode = shiftCode,
            Background = background,
            Color = model.Color,
            StartTime = shiftTimeSpan?.StartTime,
            EndTime = shiftTimeSpan?.EndTime,
            Extension = extension
        };


        await repository.ExecuteAsync(criteria);
        WorkingShiftColorModel data = null;
        if (criteria.ShiftId > 0)
        {
            data = new WorkingShiftColorModel
            {
                ShiftId = criteria.ShiftId,
                ShiftCode = criteria.ShiftCode,
                StartTime = criteria.StartTime,
                EndTime = criteria.EndTime,
                Background = criteria.Background,
                Color = criteria.Color,
                Extension = extension,
                IsTimeShift = shiftTimeSpan != null
            };
            data.RecalculateShiftCode();
        }

        return new ResultDataObject
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg,
            Data = data
        };
    }

    public async Task<ResultDataObject> Update(WorkingShiftColorModel shift)
    {
        var criteria = new UpdateWorkingShiftColorCriteria
        {
            ActorId = currentSession.UserId,
            ShiftId = shift.ShiftId,
            Background = shift.Background,
            Color = shift.Color
        };
        await repository.ExecuteAsync(criteria);

        return new ResultDataObject
        {
            Code = (int)ErrorCode.Ok,
            Data = new
            {
                shift.ShiftId,
                shift.Background,
                shift.Color
            }
        };
    }

    public async Task<ResultDataObject> ValidateBeforeDeleteAsync(int shiftId)
    {
        var criteria = new ValidateBeforeDeleteWorkingShiftColorCriteria
        {
            ShiftId = shiftId
        };

        await repository.ExecuteAsync(criteria);

        return new ResultDataObject
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg
        };
    }

    public async Task<ResultDataObject> Delete(int shiftId)
    {
        var criteria = new DeleteWorkingShiftColorCriteria
        {
            ActorId = currentSession.UserId,
            ShiftId = shiftId
        };

        await repository.ExecuteAsync(criteria);

        return new ResultDataObject
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg
        };
    }

    private async Task<List<WorkingShiftColorModel>> GetAll(GetAllWorkingShiftColorCriteria criteria)
    {

        var data = await repository.FetchAsync<WorkingShiftColorModel>(criteria);
        foreach (var item in data)
        {
            item.RecalculateShiftCode();
        }

        return data.OrderBy(x => x.ShiftCode).ToList();
    }
}