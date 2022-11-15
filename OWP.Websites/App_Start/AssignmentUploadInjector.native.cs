using OWP.AssignmentUpload.SyncInfoSite;
using OWP.AssignmentUpload.SyncInfoSite.Services;
using OWP.AssignmentUpload.Validation.Services;
using CacheManipulator = OWP.AssignmentUpload.Validation.Services.CacheManipulator;
using CacheManipulatorImp = OWP.AssignmentUpload.Validation.Services.CacheManipulatorImp;
using CommandRepository = OWP.AssignmentUpload.Submit.Services.CommandRepository;
using CommandRepositoryImp = OWP.AssignmentUpload.Submit.Services.CommandRepositoryImp;
using OWP.AssignmentUpload;
using OWP.AssignmentUpload.Submit.Services;
using OWP.AssignmentUpload.Notification.Services;

namespace OWP.Websites.App_Start;

public static class AssignmentUploadInjector
{
    public static void AddAssignmentUploadService(this IServiceCollection services)
    {
        services.Add(new ServiceDescriptor(typeof(Repository), typeof(RepositoryImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(AssignmentMapper), typeof(AssignmentMapperImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(InfoSiteMatchUpdater), typeof(InfoSiteMatchUpdaterImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(AssignmentValidator), typeof(AssignmentValidatorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(ExcelReader), typeof(ExcelReaderImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(ExcelValidator), typeof(ExcelValidatorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(MinorMatchDetector), typeof(MinorMatchDetectorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(MatchStatusValidator), typeof(MatchStatusValidatorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(ImplicitMinorMatchGenerater), typeof(GenerateImplicitMinorMatchImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(AssignmentBuilder), typeof(AssignmentBuilderImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(CacheManipulator), typeof(CacheManipulatorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(Analyzer), typeof(AnalyzerImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(ExplicitMinorMatchRule1), typeof(ExplicitMinorMatchRule1Imp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(ExplicitMinorMatchRule2), typeof(ExplicitMinorMatchRule2Imp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(SyncInfoSiteRepository), typeof(SyncInfoSiteRepositoryImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(InfositeMappingService), typeof(InfositeMappingServiceImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(SyncInfoSiteService), typeof(SyncInfoSiteIdServiceImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(InfoSiteValidation), typeof(InfoSiteValidationImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(AssignmentUpload.Submit.Services.CacheManipulator), typeof(AssignmentUpload.Submit.Services.CacheManipulatorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(QueryRepository), typeof(QueryRepositoryImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(CommandRepository), typeof(CommandRepositoryImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(AssignmentAnalyzer), typeof(AssignmentAnalyzerImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(AssigneeChangedValueFilter), typeof(AssigneeMessageFilterImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(AssignmentUpload.Notification.Services.CommandRepository), typeof(AssignmentUpload.Notification.Services.CommandRepositoryImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(AssignmentUpload.Notification.Services.CacheManipulator), typeof(AssignmentUpload.Notification.Services.CacheManipulatorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(ChangedDetector), typeof(ChangedDetectorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(ChangedMatchChangedValueGenerator), typeof(ChangedMatchChangedValueGeneratorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(ChangedValueAggregator), typeof(ChangedValueAggregatorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(ChangedValueBySnapshot), typeof(ChangedValueBySnapshotImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(ChangedValueByCacheAndDatabase), typeof(ChangedValueCacheAndDatabaseImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(DeletedMatchChangedValueGenerator), typeof(DeletedMatchChangedValueGeneratorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(InsertedMatchChangedValueGenerator), typeof(InsertedMatchChangedValueGeneratorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(MessageFactory), typeof(MessageFactoryImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(NotificationSettingQueryRepository), typeof(NotificationSettingQueryRepositoryImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(WatcherChangedValueGenerator), typeof(WatcherChangedValueGeneratorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(ChangedMatchSettingGenerator), typeof(ChangedMatchSettingGeneratorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(ChangedMatchStatusGenerator), typeof(ChangedMatchStatusGeneratorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(ChangedMatchInfoGenerator), typeof(ChangedMatchInfoGeneratorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(ChangedAssignmentGenerator), typeof(ChangedAssignmentGeneratorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(Notify), typeof(NotifyImp), ServiceLifetime.Transient));

    }
}