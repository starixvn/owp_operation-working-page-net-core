using AutoMapper;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.Models;

namespace OWP.PrematchAssignment;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<LeagueAssignment, UpdateAssignmentCriteria>();
        CreateMap<PreMatchLeague, LeagueAssignment>();
    }
}