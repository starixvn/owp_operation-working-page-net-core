using AutoMapper;
using OWP.AssignmentUpload.Validation.Models;

namespace OWP.AssignmentUpload;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AssignmentModelInExcel, AssignmentModel>();
    }
}