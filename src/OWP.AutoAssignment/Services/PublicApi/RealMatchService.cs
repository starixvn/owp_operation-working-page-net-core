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
        
            var maxBetHdpOptions = (await repository.GetMaxMaxBetFTHDPInfo(matchIds)).ToList();
            var maxBetFtHdpByMatchIds = maxBetHdpOptions.Select(i => i.MatchId).Distinct().ToList();
            var realMatches = (await repository.GetRealMatchCommonInfoByIds(maxBetFtHdpByMatchIds)).ToList();
            var maxBetHdpInfo = BuildMaxBetMainMatch(maxBetHdpOptions, realMatches).ToList();
            var maxBetCorners = (await repository.GetMaxBetCorner(maxBetHdpInfo)).ToList();
            var multipleRealMatch= new MultipleRealMatchBuilder
            {
                RealMatches= realMatches,
                MaxBetHdpInfo= maxBetHdpInfo,
                MaxBetCorners= maxBetCorners,
            };
            BuildMultipleRealMatchSender(multipleRealMatch, realMatchSenders);
            return realMatchSenders;
        }

        private void BuildMultipleRealMatchSender(MultipleRealMatchBuilder multipleRealMatch,
            IList<RealMatchSender> realMatchSenders)
        {
          
            foreach (var match in multipleRealMatch.RealMatches)
            {
                var mbHdp = multipleRealMatch.MaxBetHdpInfo.FirstOrDefault(x => x.InfoSiteMatchId == match.InfoSiteId);
                var mbCorner = multipleRealMatch.MaxBetCorners.FirstOrDefault(x => x.InfoSiteMatchId == match.InfoSiteId);

                var singleRealMatchBuilder = new SingleRealMatchBuilder
                {
                    Match= match,
                    MaxBetHdp= mbHdp,
                    MaxBetCorners=mbCorner
                };
                var commonInfo = BuildSingleRealMatchSender(singleRealMatchBuilder);
                realMatchSenders.Add(commonInfo);
            }
        }

        private RealMatchSender BuildSingleRealMatchSender(SingleRealMatchBuilder singleRealMatchBuilder)
        {
            var match = singleRealMatchBuilder.Match;
            var maxBetCorners = singleRealMatchBuilder.MaxBetCorners;
            var maxBetHdp = singleRealMatchBuilder.MaxBetHdp;
            var leagueName = match.LeagueName;
            return new RealMatchSender
            {
                MatchId = match.InfoSiteId,
                IsFutsal = IsFutsal(leagueName),
                HasTV = !string.IsNullOrWhiteSpace(match.Tv),
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
                MaxBetNextCorner = maxBetCorners?.MaxBetNextCorner,
                MaxBetNoOfCorner = maxBetCorners?.MaxBetNoOfCorner,
                MaxBetMainMatch = maxBetHdp?.MaxBetFTHDP
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

        public IEnumerable<MaxBetOfFTHDP> BuildMaxBetMainMatch(IList<MaxBetFTHDPInfo> maxBetOptions,
            IList<RealMatchCommonInfo> realMatches)
        {
            foreach (var match in realMatches)
            {
                var hasMatchTv = !string.IsNullOrWhiteSpace(match.Tv);
                var mbHdpOption = maxBetOptions.Where(i => i.MatchId == match.InfoSiteId).ToList();
                yield return new MaxBetOfFTHDP
                {
                    InfoSiteMatchId = match.InfoSiteId,
                    MaxBetFTHDP = GetMaxBetHdp(mbHdpOption, hasMatchTv)
                };
            }
        }

        private decimal? GetMaxBetHdp(IList<MaxBetFTHDPInfo> maxBetOption, bool hasMatchTv)
            => maxBetOption.Any(i => i.HasTv == hasMatchTv)
                ? maxBetOption.FirstOrDefault(i => i.HasTv == hasMatchTv)?.MaxBet
                : maxBetOption.FirstOrDefault()?.MaxBet;
    }
}
