namespace OWP.PreMatchAssignment
{
    using AutoMapper;
    using Criteria;
    using Models;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LeagueAssignment, UpdateAssignmentCriteria>();

            CreateMap<PreMatchLeague, LeagueAssignment>();
        }
    }
}
