using Microsoft.Extensions.DependencyInjection;
using Fanex.Data.Repository;
using PreMatchNotificationServices = OWP.PreMatchNotification.Services;
using LinkedMatch = OWP.PreMatchNotification.LinkedMatch.Services;
using MyAssignment = OWP.PreMatchNotification.Assignment.Services.MyAssignment;
using OtherAssignment = OWP.PreMatchNotification.Assignment.Services.OtherAssignment;
using InfoSiteMatchInfo = OWP.PreMatchNotification.InfoSiteMatchInfo.Services;
using NewLeague = OWP.PreMatchNotification.NewLeague.Services;

namespace OWP.Websites;

public static class PreMatchNotificationInjector
{
    public static void AddPreMatchNotification(this IServiceCollection services)
    {
        services.AddScoped<PreMatchNotificationServices.CommandRepository, PreMatchNotificationServices.CommandRepositoryImp>();
        services.AddScoped<PreMatchNotificationServices.MessageFactory, PreMatchNotificationServices.MessageFactoryImp>();
        services.AddScoped<PreMatchNotificationServices.NotifyServices, PreMatchNotificationServices.NotifyServicesImp>();
        services.AddScoped<PreMatchNotificationServices.CacheManipulatorCommon, PreMatchNotificationServices.CacheManipulatorCommonImp>();
        services.AddScoped<PreMatchNotificationServices.DateTimeHelper, PreMatchNotificationServices.DateTimeHelperImp>();

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
