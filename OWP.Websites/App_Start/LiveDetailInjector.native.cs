using OWP.LiveDetail.Agent;
using OWP.LiveDetail.Assignment;
using OWP.LiveDetail.Common;
using OWP.LiveDetail.InfositeMatch;
using OWP.LiveDetail.MatchInfo;
using OWP.LiveDetail.MatchNote;
using OWP.LiveDetail.MatchOdd;
using OWP.LiveDetail.MatchSetting;
using OWP.LiveDetail.OtherProvider;
using OWP.LiveDetail.Provider;
using OWP.LiveDetail.Television;
using OWP.LiveDetail.User;
using OWP.Services;

namespace OWP.Websites.App_Start
{
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
            services.AddScoped<CheckIsLiveStatusServices, CheckIsLiveStatusServicesImp>();
            services.AddScoped<GetAdminSiteMatchIdService, GetAdminSiteMatchIdServiceImp>();

        }

    }
}