using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.OtherSport.EventAssignment.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using OWP.Extensions;
    using OWP.OtherSport.EventAssignment.Models;
    using OWP.OtherSport.Submit.Models;

    public interface MatchSettingBuilder
    {
        public void Build(List<EventAssignmentModel> eventAssignmentModels, Dictionary<int, List<MatchSetting>> matchSettings);
    }

    public class MatchSettingBuilderImp : MatchSettingBuilder
    {
        public void Build(List<EventAssignmentModel> eventAssignmentModels, Dictionary<int, List<MatchSetting>> matchSettings)
        {
            foreach (var eventAssignmentModel in eventAssignmentModels)
            {
                var matchSettingsByOWPMatchId = matchSettings.GetValue(eventAssignmentModel.OSMatchId, new List<MatchSetting>());

                CreateMatchSetting(matchSettingsByOWPMatchId, eventAssignmentModel.TraderMaxbetModels);
            }
        }

        public void CreateMatchSetting(List<MatchSetting> matchSettings, IList<TraderMaxBetModel> traderMaxbetModels)
        {
            foreach (var matchSetting in matchSettings)
            {
                var eventAssignmentByBettype = traderMaxbetModels.FirstOrDefault(x => x.BetTypeName == matchSetting.BetTypeName);
                if (eventAssignmentByBettype != null)
                {
                    eventAssignmentByBettype.Maxbet = new MatchSetting
                    {
                        MaxBet = matchSetting.MaxBet
                    };
                }
            }
        }

        public Dictionary<string, string> SpecialBettypes = new Dictionary<string, string>
        {
            {"FT (OE)", "FT (OU/OE)"},
            {"Set Winner (HDP/OE)", " Set Winner (HDP/OE)" },
            {"1H (OE)", "1H (OU/OE)" }
        };
    }
}
