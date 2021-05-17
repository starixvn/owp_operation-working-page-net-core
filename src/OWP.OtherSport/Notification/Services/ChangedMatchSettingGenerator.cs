namespace OWP.OtherSport.Notification.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Constants;
    using Models;
    using OWP.Extensions;
    using OWP.OtherSport.Notification.Extensions;
    using OWP.OtherSport.Submit.Contants;
    using Submit.Models;
    using Utils;

    public interface ChangedMatchSettingGenerator
    {
        List<ChangedValue> Create(ChangedMatchSnapshot snapshot, int matchId, List<Assignee> existingAssignees);
    }

    public class ChangedMatchSettingGeneratorImp : ChangedMatchSettingGenerator
    {
        private const string EVENT_VS = " -vs- ";
        public List<ChangedValue> Create(ChangedMatchSnapshot snapshot, int matchId, List<Assignee> existingAssignees)
        {

            var newMatchSettings = snapshot.NewSnapshot.MaxBet.GetValue(matchId, new List<MatchMaxBet>());
            var oldMatchSettings = snapshot.OldSnapshot.MaxBet.GetValue(matchId, new List<MatchMaxBet>());
            var changedList = GetListValueChangedWithDifferentBettype(oldMatchSettings, newMatchSettings, matchId);

            var changedValueList = new List<ChangedValue>();

            foreach (var changed in changedList)
            {
                var matchInfo = snapshot.NewSnapshot.Matches.GetValue(changed.OSMatchId, snapshot.NewMatch);
                var changedValues = existingAssignees.Select(a => new ChangedValue
                {
                    MessageTypeId = changed.MessageTypeId,
                    NewValue = changed.NewValue,
                    OldValue = changed.OldValue,
                    BetTypeName = changed.BetTypeName,
                    MatchNo = matchInfo.MatchNo.ToString(CultureInfo.InvariantCulture),
                    League = matchInfo.League,
                    EventName = matchInfo.Home + EVENT_VS + matchInfo.Away,
                    UserId = a.AssigneeId,
                    Username = a.AssigneeName,
                    OSMatchId= matchId
                });

                changedValueList.AddRange(changedValues);
            }

            return changedValueList;
        }

        public static IEnumerable<ChangedValue> GetListValueChangedWithDifferentBettype(List<MatchMaxBet> oldMatchSettings, List<MatchMaxBet> newMatchSettings, int matchId)
        {
            var bettypes = new List<string> {
                 Bettype.FTHDP,
                 Bettype.FirstHHDP,
                 Bettype.FirstQ3QHDP ,
                 Bettype.FTOUOE ,
                 Bettype.FirstHOUOE ,
                 Bettype.FirstHOU ,
                 Bettype.FirstHOsU ,
                 Bettype.FirstQ3QOUOE ,
                 Bettype.FirstQ3QOU,
                 Bettype.FirstQHDP ,
                 Bettype.FirstQOU ,
                 Bettype.FTOU ,
                 Bettype.FTOsU ,
                 Bettype.First2ndPeriodsHDP ,
                 Bettype.First2ndPeriodsOU ,
                 Bettype.MatchWinner ,
                 Bettype.LegsHDP ,
                 Bettype.Most180sHDP ,
                 Bettype.MatchWinnerHDP ,
                 Bettype.RacksWinnerHDP ,
                 Bettype.SetWinnerHDP ,
                 Bettype.SetXWinnerHDP ,
                 Bettype.SetWinnerHDPOE ,
                 Bettype.SetsXPointWinnerHDP ,
                 Bettype.MatchWinnerInningsOU ,
                 Bettype.NextOverOUPlayerOU ,
                 Bettype.FT1X2 ,
                 Bettype.MatchWinnerHDPOU ,
                 Bettype.FramesWinnerHDPOUOE ,
                 Bettype.First5FramesHDP ,
                 Bettype.First8FramesHDP,
                 Bettype.First9FramesHDP ,
                 Bettype.FTRunLineHDP ,
                 Bettype.FirstH1st5inningsHDP ,
                 Bettype.FTTotalRunOU ,
                 Bettype.FirstH1st5inningsOU ,
                 Bettype.InningHitsAndRuns ,
                 Bettype.AllProductSetWinner ,
                 Bettype.AllProductPointWinner ,
                 Bettype.RacetoPoints7911 ,
                 Bettype.NextPointWinner ,
                 Bettype.MatchHandicap ,
                 Bettype.AllProductLS ,
                 Bettype.NextGame ,
                 Bettype.MostAces ,
                 Bettype.MostDoubleFaults
            };

            var changedValues = new List<ChangedValue>();
            foreach (var bettype in bettypes)
            {
                var oldMatchWithBettype = oldMatchSettings.SingleOrDefault(a => string.Equals(a.BetTypeName, bettype)) ?? new MatchMaxBet();
                var newMatchWithBettype = newMatchSettings.SingleOrDefault(a => string.Equals(a.BetTypeName, bettype)) ?? new MatchMaxBet();
                var listMessage = GetListValueChangedOfMatchSetting(oldMatchWithBettype, newMatchWithBettype, bettype, matchId);
                changedValues.AddRange(listMessage);
            }

            return changedValues;
        }

        public static IEnumerable<ChangedValue> GetListValueChangedOfMatchSetting(MatchMaxBet oldMatchSetting, MatchMaxBet newMatchSetting, string bettype, int matchId)
        {
            if (!Equals(oldMatchSetting.MaxBet, newMatchSetting.MaxBet))
            {
                yield return new ChangedValue
                {
                    OldValue = oldMatchSetting.MaxBet.ToOWPMaxBetFormat(),
                    NewValue = newMatchSetting.MaxBet.ToOWPMaxBetFormat(),
                    BetTypeName = bettype,
                    MessageTypeId = (int)MsgTemplateCode.EditMaxbet,
                    OSMatchId = matchId
                };
            }
        }
    }
}