using OWP.WorkingShift.DailyWorkingShift.Criteria;

namespace OWP.WorkingShift;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<QueryModel, QueryCriteria>();
        CreateMap<EmployeeWorkingShiftModel, SyncWorkingShiftModel>();
    }
}