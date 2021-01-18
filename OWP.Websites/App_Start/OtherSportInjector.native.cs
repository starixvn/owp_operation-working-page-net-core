namespace OWP.Websites
{
    using Microsoft.Extensions.DependencyInjection;
    using Submit = OtherSport.Submit.Services;
    using Validation = OtherSport.Validation.Services;
    using EventAssignment = OtherSport.EventAssignment.Services;
    using OWP.OtherSport.EventAssignment.Builder;

    public static class OtherSportInjector
    {
        public static void InjectOtherSportService(this IServiceCollection services)
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
            services.AddScoped<AssignmentBuilder, AssignmentBuilderImp>();
            services.AddScoped<BettypeBuilder, BettypeBuilderImp>();
            services.AddScoped<MaxbetBuilder, MaxbetBuilderImp>();
            services.AddScoped<AssignmentMaxbetBuilder, AssignmentMaxbetBuilderImp>();
        }
    }
}