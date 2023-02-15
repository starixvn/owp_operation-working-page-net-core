namespace OWP.PrematchAssignment;

using AutoMapper;
using PreMatchAssignment.Criteria;
using PreMatchAssignment.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<LeagueAssignment, UpdateAssignmentCriteria>();
        CreateMap<PreMatchLeague, LeagueAssignment>();
    }
}