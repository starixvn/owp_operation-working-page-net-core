using OWP.AutoAssignment.Constant;
using OWP.AutoAssignment.Criteria.PublicApi;
using OWP.AutoAssignment.Models.PublicApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.PublicApi
{
    public interface RealMatchService
    {
        Task<IList<RealMatchSender>> BuildRealMatchInfo(IList<MatchQueried> matchsQueried);
    }

    public class RealMatchServiceImp : RealMatchService
    {
        private readonly Repository repository;


        private readonly ExtraTimeDetectorService extraTimeService;

        private readonly ProviderDetectorService providerService;

        public RealMatchServiceImp(
            Repository repository, 
            ExtraTimeDetectorService extraTimeService,
            ProviderDetectorService providerService)
        {
            this.repository = repository;
            this.extraTimeService = extraTimeService;
            this.providerService = providerService;
        }

        public async Task<IList<RealMatchSender>> BuildRealMatchInfo(IList<MatchQueried> matchsQueried)
        {
            var matchIds = matchsQueried.Select(i => i.InfositeMatchId).ToList();

            var realMatchSenders = new List<RealMatchSender>();
            var realMatches = await repository.GetRealMatchCommonInfoByIds(matchIds);

            var maxBetByMatchIds = await repository.GetMaxMaxbetFTHDP(matchIds);
            var maxBetCorners = await repository.GetMaxBetCorner(maxBetByMatchIds.ToList());
           
            foreach (var match in realMatches)
            {
                var commonInfo = GetCommonInfo(match,maxBetCorners.ToList());
                realMatchSenders.Add(commonInfo);
            }
            return realMatchSenders;
        }

        private RealMatchSender GetCommonInfo(RealMatchCommonInfo match, List<MaxBetCorner> maxBetCorners)
        {
            var leagueName = match.LeagueName;
            return new RealMatchSender
            {
                MatchId = match.InfoSiteId,
                IsFutsal = IsFutsal(leagueName),
                HasTV = string.IsNullOrWhiteSpace(match.Tv),
                KOTime = match.KOTime.ToString(AutoConstant.FormatDateTime),
                HasReporter = HasReporter(match.Provider),
                RequireChinese = RequireChinese(match.Provider),
                RequireEnglish = RequireEnglish(match.Provider),
                IsSoloMatch = match.IsSoloMatch,
                IsMajorLeague = match.IsMajorLeague,
                ExtraTime = extraTimeService.DetectExtraTime(match.Remark),
                Location = match.Location,
                Provider= providerService.DetectProviderType(match.OtherProvider),
                IsFantasy = IsFantasy(match.LeagueName),
                MaxBetNextCorner = GetMBNextCorner(maxBetCorners,match.InfoSiteId),
                MaxBetNoOfCorner = GetMBNoOfCorner(maxBetCorners, match.InfoSiteId)
            };
        }

        private bool IsFutsal(string leagueName)
            => leagueName.Contains(AutoConstant.Futsal, StringComparison.OrdinalIgnoreCase)
            && !leagueName.Contains(AutoConstant.Saba, StringComparison.OrdinalIgnoreCase);

        private bool HasReporter(string provider)
        => provider.Contains(AutoConstant.Woo, StringComparison.OrdinalIgnoreCase)
            || provider.Contains(AutoConstant.Al, StringComparison.OrdinalIgnoreCase)
            || provider.Contains(AutoConstant.Slr, StringComparison.OrdinalIgnoreCase);

        private bool RequireChinese(string provider)
            => provider.Contains(AutoConstant.Woo, StringComparison.OrdinalIgnoreCase)
            || provider.Contains(AutoConstant.Al, StringComparison.OrdinalIgnoreCase);

        private bool RequireEnglish(string provider)
            => provider.Contains(AutoConstant.Slr, StringComparison.OrdinalIgnoreCase);

        private bool IsFantasy(string leagueName)
            => leagueName.Contains(AutoConstant.Fantasy, StringComparison.OrdinalIgnoreCase);

        private decimal? GetMBNextCorner(List<MaxBetCorner> maxBetCorners, int matchId)
            => maxBetCorners
                .FirstOrDefault(m => m.InfoSiteMatchId == matchId)?
                .MaxBetNextCorner;
        private decimal? GetMBNoOfCorner(List<MaxBetCorner> maxBetCorners, int matchId)
            => maxBetCorners
                .FirstOrDefault(m => m.InfoSiteMatchId == matchId)?
                .MaxBetNoOfCorner;
    }
}