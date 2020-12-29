namespace OWP.OtherSport
{
    using System;
    using System.Reflection;
    using AutoMapper;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Validation.Models.AssignmentModelInExcel, Validation.Models.AssignmentModel>()
                .ForMember(dest => dest.HashKey, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => DateTime.Parse(src.DateTime)))
                .IgnoreAllNonExisting();
        }
    }

    public static class AutoMapperExtension
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>
            (this IMappingExpression<TSource, TDestination> expression)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var destinationProperties = typeof(TDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                if (property.Name.StartsWith("Trader", StringComparison.CurrentCultureIgnoreCase) ||
                    property.Name.StartsWith("OtherTrader", StringComparison.CurrentCultureIgnoreCase))
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }
    }
}