namespace OWP.LeagueManagement;

using AutoMapper;
using Criteria;
using Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<SpecialProduct, UpdateSpecialProductCriteria>();
        CreateMap<LeagueInfoQuery, GetAllLeagueInfoCriteria>();
    }
}