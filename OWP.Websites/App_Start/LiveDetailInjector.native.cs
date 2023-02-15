namespace OWP.Websites.App_Start;

using LiveDetail.Agent;
using LiveDetail.Assignment;
using LiveDetail.Common;
using LiveDetail.InfositeMatch;
using LiveDetail.MatchInfo;
using LiveDetail.MatchNote;
using LiveDetail.MatchOdd;
using LiveDetail.MatchSetting;
using LiveDetail.OtherProvider;
using LiveDetail.Provider;
using LiveDetail.Television;
using LiveDetail.User;

public static class LiveDetailInjector
{
    public static void AddLiveDetailService(this IServiceCollection services)
    {
        services.AddScoped<AgentRepository, AgentRepositoryImp>();
        services.AddScoped<AssignmentRepository, AssignmentRepositoryImp>();
        services.AddScoped<LiveDetailCacheManipulator, LiveDetailCacheManipulatorImp>();
        services.AddScoped<InfositeMatchRepository, InfositeMatchRepositoryImp>();
        services.AddScoped<MatchInfoRepository, MatchInfoRepositoryImp>();
        services.AddScoped<MatchNoteRepository, MatchNoteRepositoryImp>();
        services.AddScoped<MatchOddRepository, MatchOddRepositoryImp>();
        services.AddScoped<MatchSettingRepository, MatchSettingRepositoryImp>();
        services.AddScoped<OtherProviderRepository, OtherProviderRepositoryImp>();
        services.AddScoped<ProviderRepository, ProviderRepositoryImp>();
        services.AddScoped<TelevisionRepository, TelevisionRepositoryImp>();
        services.AddScoped<UserRepository, UserRepositoryImp>();
    }

}