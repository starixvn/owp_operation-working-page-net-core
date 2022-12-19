using OWP.EventPicking.Models;
using OWP.Extensions;
using OWP.MaxbetSetting.Models;

namespace OWP.EventPicking.Services
{
    public interface BuildMatchOffer
    {
        void Build(IEnumerable<EventPickingModel> mainMatchesHasMaxBet, IEnumerable<EventPickingModel> childMatches, Dictionary<decimal, MaxBetSettingModel> maxBetDefault);
    }

    public class BuildMatchOfferImp : BuildMatchOffer
    {
        public void Build(IEnumerable<EventPickingModel> mainMatchesHasMaxBet, IEnumerable<EventPickingModel> childMatches, Dictionary<decimal, MaxBetSettingModel> maxBetDefault)
        {

        }

        private static void BuildMatchOfferMainMatches(IEnumerable<EventPickingModel> mainMatches, Dictionary<decimal, MaxBetSettingModel> liveSettingsDefault)
        {
            foreach (var mainMatch in mainMatches)
            {
                if (mainMatch.LeagueName.IsContains("FANTASY") && mainMatch.MaxBetHDPNumber.HasValue)
                {
                    var liveSetting = GetLiveSetting(liveSettingsDefault, mainMatch);

                    mainMatch.MatchOffer = liveSetting.SecondAccept_Fantasy;
                }
            }
        }

        private static MaxBetSettingModel GetLiveSetting(Dictionary<decimal, MaxBetSettingModel> liveSettingsDefault, EventPickingModel mainMatch)
        {
            return liveSettingsDefault.ContainsKey((decimal)mainMatch.MaxBetHDPNumber)
                                    ? liveSettingsDefault.GetValue((decimal)mainMatch.MaxBetHDPNumber)
                                    : liveSettingsDefault.GetValue((-1000m));
        }
    }
}
