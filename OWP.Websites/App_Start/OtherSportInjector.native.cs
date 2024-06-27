using Download = OWP.OtherSport.Download.Services;
using EventAssignment = OWP.OtherSport.EventAssignment.Services;
using Shared = OWP.OtherSport.Shared.Services;
using Submit = OWP.OtherSport.Submit.Services;
using Validation = OWP.OtherSport.Validation.Services;

namespace OWP.Websites;

public static class OtherSportInjector
{
    public static void AddOtherSport(this IServiceCollection services)
    {
        // Submit
        services.AddScoped<Submit.CacheManipulator, Submit.CacheManipulatorImp>();
        services.AddScoped<Submit.AssignmentAnalyzer, Submit.AssignmentAnalyzerImp>();
        services.AddScoped<Submit.AssignmentMaxbetMapper, Submit.AssignmentMaxbetMapperImp>();
        services.AddScoped<Submit.CommandRepository, Submit.CommandRepositoryImp>();

        services.AddScoped<Validation.CacheManipulator, Validation.CacheManipulatorImp>();
        services.AddScoped<Validation.Analyzer, Validation.AnalyzerImp>();
        services.AddScoped<Validation.AssignmentBuilder, Validation.AssignmentBuilderImp>();
        services.AddScoped<Validation.AssignmentMapper, Validation.AssignmentMapperImp>();
        services.AddScoped<Validation.AssignmentValidator, Validation.AssignmentValidatorImp>();
        services.AddScoped<Validation.ExcelReader, Validation.ExcelReaderImp>();
        services.AddScoped<Validation.ExcelValidator, Validation.ExcelValidatorImp>();
        services.AddScoped<Validation.Repository, Validation.RepositoryImp>();

        services.AddScoped<EventAssignment.CommandRepository, EventAssignment.CommandRepositoryImp>();
        services.AddScoped<EventAssignment.AssignmentBuilder, EventAssignment.AssignmentBuilderImp>();
        services.AddScoped<EventAssignment.MatchSettingBuilder, EventAssignment.MatchSettingBuilderImp>();
        services.AddScoped<EventAssignment.EventAssignmentBuilder, EventAssignment.EventAssignmentBuilderImp>();
        services.AddScoped<Shared.MatchStatusCalculator, Shared.MatchStatusCalculatorImp>();

        services.AddScoped<Download.BuildExcelModelService, Download.BuildExcelModelServiceImp>();
        services.AddScoped<Download.ExportExcelService, Download.ExportExcelServiceImp>();
        services.AddScoped<Download.CommandRepository, Download.CommandRepositoryImp>();
    }
}