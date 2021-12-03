using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OWP.AutoAssignment.Constant;
using OWP.AutoAssignment.Models.PublicApi;

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

            var maxBetHDPsInfo = (await repository.GetMaxMaxBetFTHDPForRealMatch(matchIds)).ToList();

            var maxBetCorners = (await repository.GetMaxBetCorner(maxBetHDPsInfo)).ToList();

            foreach (var match in realMatches)
            {
                var maxBetHDP = maxBetHDPsInfo.Where(x => x.InfoSiteMatchId == match.InfoSiteId).ToList();
                var mbNextCorner = maxBetCorners.FirstOrDefault(x => x.InfoSiteMatchId == match.InfoSiteId)?.MaxBetNextCorner;
                var mbNoOfCorner = maxBetCorners.FirstOrDefault(x => x.InfoSiteMatchId == match.InfoSiteId)?.MaxBetNoOfCorner;
                var mbCorners = new MaxBetCorner
                {
                    InfoSiteMatchId = match.InfoSiteId,
                    MaxBetNextCorner = mbNextCorner,
                    MaxBetNoOfCorner = mbNoOfCorner
                };
                var commonInfo = BuildMatchInfo(match, maxBetHDP, mbCorners);
                realMatchSenders.Add(commonInfo);
            }
            return realMatchSenders;
        }

        private RealMatchSender BuildMatchInfo(RealMatchCommonInfo match, List<RealMatchMaxBetFTHDP> maxBetHDPOption, MaxBetCorner maxBetCorners)
        {
            var leagueName = match.LeagueName;
            var hasTv = string.IsNullOrWhiteSpace(match.Tv);

            return new RealMatchSender
            {
                MatchId = match.InfoSiteId,
                IsFutsal = IsFutsal(leagueName),
                HasTV = hasTv,
                KOTime = match.KOTime.ToString(AutoConstant.FormatDateTime),
                HasReporter = HasReporter(match.Provider),
                RequireChinese = RequireChinese(match.Provider),
                RequireEnglish = RequireEnglish(match.Provider),
                IsSoloMatch = match.IsSoloMatch,
                IsMajorLeague = match.IsMajorLeague,
                ExtraTime = extraTimeService.DetectExtraTime(match.Remark),
                Location = match.Location,
                Provider = providerService.DetectProviderType(match.OtherProvider),
                IsFantasy = IsFantasy(match.LeagueName),
                MaxBetNextCorner = maxBetCorners.MaxBetNextCorner,
                MaxBetNoOfCorner = maxBetCorners.MaxBetNoOfCorner,
                MaxBetMainMatch = GetMaxBetMainMatch(maxBetHDPOption, hasTv)
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

        private decimal? GetMaxBetMainMatch(IList<RealMatchMaxBetFTHDP> maxBetOption, bool hasTv)
            => maxBetOption.Any(i => i.HasTv)
                ? maxBetOption.FirstOrDefault(x => x.HasTv == hasTv)?.MaxBetFTHDP
                : maxBetOption.FirstOrDefault()?.MaxBetFTHDP;

    }
}
