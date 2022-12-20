namespace OWP.AssignmentUpload
{
    using AutoMapper;
    using Validation.Models;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AssignmentModelInExcel, AssignmentModel>();
        }
    }
}
