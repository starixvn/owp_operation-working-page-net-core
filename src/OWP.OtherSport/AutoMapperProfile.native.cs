namespace OWP.OtherSport;

using System.Reflection;
using AutoMapper;
using Download.Models;
using EventAssignment.Models;
using Submit.Models;
using Validation.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AssignmentModelInExcel, AssignmentModel>()
            .ForMember(dest => dest.HashKey, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.DateTime, opt => opt.Ignore())
            .IgnoreMaxbetsAndSpreads()
            .IgnoreTraders();
        CreateMap<MatchInfo, EventAssignmentModel>();
        CreateMap<MatchAssignment, ExcelMatchAssignment>();
        CreateMap<MatchSetting, ExcelMatchMaxBetSpread>();
    }
}

public static class AutoMapperExtension
{
    public static IMappingExpression<AssignmentModelInExcel, AssignmentModel> IgnoreTraders<AssignmentModelInExcel, AssignmentModel>
        (this IMappingExpression<AssignmentModelInExcel, AssignmentModel> expression)
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
        var destinationProperties = typeof(AssignmentModel).GetProperties(flags);

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

    public static IMappingExpression<AssignmentModelInExcel, AssignmentModel> IgnoreMaxbetsAndSpreads<AssignmentModelInExcel, AssignmentModel>
        (this IMappingExpression<AssignmentModelInExcel, AssignmentModel> expression)
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
        var destinationProperties = typeof(AssignmentModel).GetProperties(flags);

        foreach (var property in destinationProperties)
        {
            if (property.Name.StartsWith("Maxbet", StringComparison.CurrentCultureIgnoreCase) ||
                property.Name.StartsWith("Spread", StringComparison.CurrentCultureIgnoreCase))
            {
                expression.ForMember(property.Name, opt => opt.Ignore());
            }
        }
        return expression;
    }
}