namespace OWP.Websites
{
    using Microsoft.Extensions.DependencyInjection;
    using Submit = OtherSport.Submit.Services;
    using Download = OtherSport.Download.Services;
    using Validation = OtherSport.Validation.Services;
    using EventAssignment = OtherSport.EventAssignment.Services;
    using OWP.OtherSport.Notification.Services;

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
            services.AddScoped<EventAssignment.AssignmentBuilder, EventAssignment.AssignmentBuilderImp>();
            services.AddScoped<EventAssignment.MatchSettingBuilder, EventAssignment.MatchSettingBuilderImp>();
            services.AddScoped<EventAssignment.EventAssignmentBuilder, EventAssignment.EventAssignmentBuilderImp>();

            services.AddScoped<Download.BuildExcelModelService, Download.BuildExcelModelServiceImp>();
            services.AddScoped<Download.ExportExcelService, Download.ExportExcelServiceImp>();
            services.AddScoped<Download.CommandRepository, Download.CommandRepositoryImp > ();

            // Notification
            services.AddScoped<MessageFactory, MessageFactoryImp>();
            services.AddScoped<ChangedMatchInfoGenerator, ChangedMatchInfoGeneratorImp>();
            services.AddScoped<ChangedMatchStatusGenerator, ChangedMatchStatusGeneratorImp>();
            services.AddScoped<ChangedMatchSettingGenerator, ChangedMatchSettingGeneratorImp>();
            services.AddScoped<NotifyServices, NotifyServicesImp>();

            services.AddScoped<AssigneeChangedValueFilter, AssigneeMessageFilterImp>();
            services.AddScoped<CacheManipulator, CacheManipulatorImp>();
            services.AddScoped<ChangedAssignmentGenerator, ChangedAssignmentGeneratorImp>();
            services.AddScoped<ChangedDetector, ChangedDetectorImp>();
            services.AddScoped<ChangedMatchChangedValueGenerator, ChangedMatchChangedValueGeneratorImp>();
            services.AddScoped<ChangedValueAggregator, ChangedValueAggregatorImp>();
            services.AddScoped<ChangedValueBySnapshot, ChangedValueBySnapshotImp>();
            services.AddScoped<ChangedValueByCacheAndDatabase, ChangedValueByCacheAndDatabaseImp>();
            services.AddScoped<ChangedAssignmentNoteGenerator, ChangedAssignmentNoteGeneratorImp>();
            services.AddScoped<DeletedMatchChangedValueGenerator, DeletedMatchChangedValueGeneratorImp>();
            services.AddScoped<InsertedMatchChangedValueGenerator, InsertedMatchChangedValueGeneratorImp>();
            services.AddScoped<OtherSport.Notification.Services.CommandRepository, OtherSport.Notification.Services.CommandRepositoryImp>();
            services.AddScoped<NotificationSettingQueryRepository, NotificationSettingQueryRepositoryImp>();
        }
    }
}