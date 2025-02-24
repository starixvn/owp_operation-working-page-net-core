using AutoMapper;
using OWP.PrematchAssignment.AutoAssignment.Models;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.QueryAndSave.Models;

namespace OWP.PrematchAssignment;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<LeagueAssignment, UpdateAssignmentCriteria>();
        CreateMap<PreMatchLeague, LeagueAssignment>();
        CreateMap<PreMatchLeagueAssignment, PreMatchLeagueAssignment>();
    }
}