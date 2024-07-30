namespace OWP.WorkingShift.Common;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UpdateDefaultShiftModel, UpdateDefaultShiftCriteria>();
        CreateMap<CopyWholeShiftsModel, CopyWholeShiftsCriteria>();
        CreateMap<DeleteWholeShiftsModel, DeleteWholeShiftsCriteria>();
        CreateMap<CopySingleShiftModel, CopySingleShiftCriteria>();
        CreateMap<RemoveSingleShiftModel, RemoveSingleShiftCriteria>();
        CreateMap<PublishWeeklyWorkingShiftModel, PublishWeeklyWorkingShiftCriteria>();
        CreateMap<WorkingShiftQueryByUserModel, QueryWorkingShiftByUserCriteria>();
        CreateMap<WorkingShiftQueryBySectionModel, QueryWorkingShiftBySectionCriteria>();
        CreateMap<CopyWorkingShiftWeeklyPreviousModel, CopyWorkingShiftWeeklyPreviousCriteria>();
        CreateMap<UpdateEmployeeWorkingShiftOrderModel, UpdateEmployeeWorkingShiftOrderCriteria>();
    }
}