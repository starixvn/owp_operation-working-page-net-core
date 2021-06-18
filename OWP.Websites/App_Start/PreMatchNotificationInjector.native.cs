namespace OWP.Websites.App_Start
{
    using Microsoft.Extensions.DependencyInjection;
    using PreMatchNotification = PreMatchNotification.Services;
    using LinkedMatch = PreMatchNotification.LinkedMatch.Services;
    using Assignment = PreMatchNotification.Assignment.Services;
    using InfoSiteMatchInfo = PreMatchNotification.InfoSiteMatchInfo.Services;
    using NewLeague = PreMatchNotification.NewLeague.Services;

    public static class PreMatchNotificationInjector
    {
        public static void InjectPreMatchNotification(this IServiceCollection services)
        {
            services.AddScoped<PreMatchNotification.CommandRepository, PreMatchNotification.CommandRepositoryImp>();
            services.AddScoped<PreMatchNotification.MessageFactory, PreMatchNotification.MessageFactoryImp>();
            services.AddScoped<PreMatchNotification.NotifyServices, PreMatchNotification.NotifyServicesImp>();
            services.AddScoped<PreMatchNotification.CacheManipulatorCommon, PreMatchNotification.CacheManipulatorCommonImp>();
            
            services.AddScoped<Assignment.MyAssignmentGenerator, Assignment.MyAssignmentGeneratorImp>();
            services.AddScoped<Assignment.MyAssignmentDetector, Assignment.MyAssignmentDetectorImp>();
          
            // LinkedMatch
            services.AddScoped<LinkedMatch.CacheManipulator, LinkedMatch.CacheManipulatorImp>();
            services.AddScoped<LinkedMatch.ChangedLinkedMatchDetector, LinkedMatch.ChangedLinkedMatchDetectorImp>();
            services.AddScoped<LinkedMatch.ChangedLiveMatchGenerator, LinkedMatch.ChangedLiveMatchGeneratorImp>();
            services.AddScoped<LinkedMatch.ChangedMatchStatusGenerator, LinkedMatch.ChangedMatchStatusGeneratorImp>();
            services.AddScoped<LinkedMatch.Repository, LinkedMatch.RepositoryImp>();

            // InfoSiteMatchInfo

            // NewLeague

        }
    }
}