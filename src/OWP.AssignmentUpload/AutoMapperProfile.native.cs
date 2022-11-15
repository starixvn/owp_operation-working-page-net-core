namespace OWP.AssignmentUpload
{
    using AutoMapper;
    using Notification.Models;
    using Validation.Models;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AssignmentModelInExcel, AssignmentModel>();
            CreateMap<ChangedValue, ChangedValue>();
        }
    }
}
