using AutoMapper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AssignmentModel, AssignmentModel>();
    }
}