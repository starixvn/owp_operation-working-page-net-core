using OWP.AssignmentUpload.Submit.Services;
using OWP.AssignmentUpload.SyncInfoSite.Services;
using OWP.AssignmentUpload.Validation.Services;
using CacheManipulator = OWP.AssignmentUpload.Validation.Services.CacheManipulator;
using CacheManipulatorImp = OWP.AssignmentUpload.Validation.Services.CacheManipulatorImp;

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
        services.Add(new ServiceDescriptor(typeof(ImplicitMinorMatchGenerator), typeof(GenerateImplicitMinorMatchImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(AssignmentBuilder), typeof(AssignmentBuilderImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(CacheManipulator), typeof(CacheManipulatorImp), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(AssignmentUpload.Validation.Services.Analyzer), typeof(AnalyzerImp), ServiceLifetime.Transient));
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


    }
}