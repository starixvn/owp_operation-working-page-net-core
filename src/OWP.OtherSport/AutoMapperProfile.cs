namespace OWP.OtherSport
{
    using System;
    using AutoMapper;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Validation.Models.AssignmentModelInExcel, Validation.Models.AssignmentModel>()
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => DateTime.Parse(src.DateTime)))
                .ForMember(dest => dest.HashKey, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
        }
    }
}