using AutoMapper;
using OWP.LeagueManagement.Criteria;
using OWP.LeagueManagement.Models;

namespace OWP.LeagueManagement;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<SpecialProduct, UpdateSpecialProductCriteria>();
        CreateMap<LeagueInfoQuery, GetAllLeagueInfoCriteria>();
    }
}