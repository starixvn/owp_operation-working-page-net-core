using OWP.WorkingShift.DailyWorkingShift.Criteria;

namespace OWP.WorkingShift.DailyWorkingShift
{

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<QueryModel, QueryCriteria>();
        }
    }
}
