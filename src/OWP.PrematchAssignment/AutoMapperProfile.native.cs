namespace OWP.PrematchAssignment
{
    using AutoMapper;
    using OWP.PreMatchAssignment.Criteria;
    using OWP.PreMatchAssignment.Models;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LeagueAssignment, UpdateAssignmentCriteria>();
            CreateMap<PreMatchLeague, LeagueAssignment>();
        }
    }
}