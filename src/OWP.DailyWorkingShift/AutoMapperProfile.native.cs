using OWP.DailyWorkingShift.Criteria;

namespace OWP.DailyWorkingShift
{

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<QueryModel, QueryCriteria>();
        }
    }
}
