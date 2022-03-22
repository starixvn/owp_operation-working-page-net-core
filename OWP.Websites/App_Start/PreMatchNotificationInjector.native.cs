using InfoSiteMatchInfo = OWP.PreMatchNotification.InfoSiteMatchInfo.Services;
using LinkedMatch = OWP.PreMatchNotification.LinkedMatch.Services;
using MyAssignment = OWP.PreMatchNotification.Assignment.Services.MyAssignment;
using NewLeague = OWP.PreMatchNotification.NewLeague.Services;
using OtherAssignment = OWP.PreMatchNotification.Assignment.Services.OtherAssignment;
using PreMatchNotificationServices = OWP.PreMatchNotification.Services;

namespace OWP.Websites;

public static class PreMatchNotificationInjector
{
  public static void AddPreMatchNotification(this IServiceCollection services)
  {
    services.AddTransient<PreMatchNotificationServices.CommandRepository, PreMatchNotificationServices.CommandRepositoryImp>();
    services.AddTransient<PreMatchNotificationServices.MessageFactory, PreMatchNotificationServices.MessageFactoryImp>();
    services.AddTransient<PreMatchNotificationServices.NotifyServices, PreMatchNotificationServices.NotifyServicesImp>();
    services.AddTransient<PreMatchNotificationServices.CacheManipulatorCommon, PreMatchNotificationServices.CacheManipulatorCommonImp>();
    services.AddTransient<PreMatchNotificationServices.DateTimeHelper, PreMatchNotificationServices.DateTimeHelperImp>();

    services.AddTransient<MyAssignment.MyAssignmentGenerator, MyAssignment.MyAssignmentGeneratorImp>();
    services.AddTransient<MyAssignment.MyAssignmentDetector, MyAssignment.MyAssignmentDetectorImp>();
    services.AddTransient<MyAssignment.ChangedValueHdpDetector, MyAssignment.ChangedValueHdpDetectorImp>();
    services.AddTransient<MyAssignment.ChangedValueOuDetector, MyAssignment.ChangedValueOuDetectorImp>();
    services.AddTransient<MyAssignment.ChangedValueHdpTakeOverDetector, MyAssignment.ChangedValueHdpTakeOverDetectorImp>();
    services.AddTransient<MyAssignment.ChangedValueOuTakeOverDetector, MyAssignment.ChangedValueOuTakeOverDetectorImp>();

    services.AddTransient<OtherAssignment.OtherAssignmentGenerator, OtherAssignment.OtherAssignmentGeneratorImp>();
    services.AddTransient<OtherAssignment.OtherAssignmentDetector, OtherAssignment.OtherAssignmentDetectorImp>();
    services.AddTransient<OtherAssignment.ChangedValueHdpDetector, OtherAssignment.ChangedValueHdpDetectorImp>();
    services.AddTransient<OtherAssignment.ChangedValueOuDetector, OtherAssignment.ChangedValueOuDetectorImp>();
    services.AddTransient<OtherAssignment.ChangedValueHdpTakeOverDetector, OtherAssignment.ChangedValueHdpTakeOverDetectorImp>();
    services.AddTransient<OtherAssignment.ChangedValueOuTakeOverDetector, OtherAssignment.ChangedValueOuTakeOverDetectorImp>();

    // LinkedMatch
    services.AddTransient<LinkedMatch.CacheManipulator, LinkedMatch.CacheManipulatorImp>();
    services.AddTransient<LinkedMatch.ChangedLinkedMatchDetector, LinkedMatch.ChangedLinkedMatchDetectorImp>();
    services.AddTransient<LinkedMatch.ChangedLiveMatchGenerator, LinkedMatch.ChangedLiveMatchGeneratorImp>();
    services.AddTransient<LinkedMatch.ChangedMatchStatusGenerator, LinkedMatch.ChangedMatchStatusGeneratorImp>();
    services.AddTransient<LinkedMatch.Repository, LinkedMatch.RepositoryImp>();

    // InfoSiteMatchInfo
    services.AddTransient<InfoSiteMatchInfo.Repository, InfoSiteMatchInfo.RepositoryImp>();
    services.AddTransient<InfoSiteMatchInfo.ChangedValueGenerator, InfoSiteMatchInfo.ChangedValueGeneratorImp>();

    // NewLeague
    services.AddTransient<NewLeague.NewLeagueValueGenerator, NewLeague.NewLeagueValueGeneratorImp>();
  }
}