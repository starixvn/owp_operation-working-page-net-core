namespace OWP.Websites.App_Start
{
    using Microsoft.Extensions.DependencyInjection;
    using Fanex.Data.Repository;
    using PreMatchNotification = PreMatchNotification.Services;
    using LinkedMatch = PreMatchNotification.LinkedMatch.Services;
    using MyAssignment = PreMatchNotification.Assignment.Services.MyAssignment;
    using OtherAssignment = PreMatchNotification.Assignment.Services.OtherAssignment;
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
            services.AddScoped<PreMatchNotification.DateTimeHelper, PreMatchNotification.DateTimeHelperImp>();

            services.AddScoped<MyAssignment.MyAssignmentGenerator, MyAssignment.MyAssignmentGeneratorImp>();
            services.AddScoped<MyAssignment.MyAssignmentDetector, MyAssignment.MyAssignmentDetectorImp>();
            services.AddScoped<MyAssignment.ChangedValueHdpDetector, MyAssignment.ChangedValueHdpDetectorImp>();
            services.AddScoped<MyAssignment.ChangedValueOuDetector, MyAssignment.ChangedValueOuDetectorImp>();
            services.AddScoped<MyAssignment.ChangedValueHdpTakeOverDetector, MyAssignment.ChangedValueHdpTakeOverDetectorImp>();
            services.AddScoped<MyAssignment.ChangedValueOuTakeOverDetector, MyAssignment.ChangedValueOuTakeOverDetectorImp>();

            services.AddScoped<OtherAssignment.OtherAssignmentGenerator, OtherAssignment.OtherAssignmentGeneratorImp>();
            services.AddScoped<OtherAssignment.OtherAssignmentDetector, OtherAssignment.OtherAssignmentDetectorImp>();
            services.AddScoped<OtherAssignment.ChangedValueHdpDetector, OtherAssignment.ChangedValueHdpDetectorImp>();
            services.AddScoped<OtherAssignment.ChangedValueOuDetector, OtherAssignment.ChangedValueOuDetectorImp>();
            services.AddScoped<OtherAssignment.ChangedValueHdpTakeOverDetector, OtherAssignment.ChangedValueHdpTakeOverDetectorImp>();
            services.AddScoped<OtherAssignment.ChangedValueOuTakeOverDetector, OtherAssignment.ChangedValueOuTakeOverDetectorImp>();

            // LinkedMatch
            services.AddScoped<LinkedMatch.CacheManipulator, LinkedMatch.CacheManipulatorImp>();
            services.AddScoped<LinkedMatch.ChangedLinkedMatchDetector, LinkedMatch.ChangedLinkedMatchDetectorImp>();
            services.AddScoped<LinkedMatch.ChangedLiveMatchGenerator, LinkedMatch.ChangedLiveMatchGeneratorImp>();
            services.AddScoped<LinkedMatch.ChangedMatchStatusGenerator, LinkedMatch.ChangedMatchStatusGeneratorImp>();
            services.AddScoped<LinkedMatch.Repository, LinkedMatch.RepositoryImp>();

            // InfoSiteMatchInfo
            services.AddScoped<InfoSiteMatchInfo.Repository, InfoSiteMatchInfo.RepositoryImp>();
            services.AddScoped<InfoSiteMatchInfo.ChangedValueGenerator, InfoSiteMatchInfo.ChangedValueGeneratorImp>();

            // NewLeague
            services.AddScoped<NewLeague.NewLeagueValueGenerator, NewLeague.NewLeagueValueGeneratorImp>();
        }
    }
}