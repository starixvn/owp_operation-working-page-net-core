namespace OWP.OtherSport.Notification.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Models;
    using OWP.OtherSport.Submit.Contants;
    using Submit.Constants;
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
            var changedList = GetListValueChangedWithDifferentBettype(oldMatchSettings, newMatchSettings);

            var changedValueList = new List<ChangedValue>();

            foreach (var changed in changedList)
            {
                var mainMatch = snapshot.NewSnapshot.MaxBet.GetValue(snapshot.NewMatch.ParentMatchId, snapshot.NewMatch);
                var bettypeName = BettypeNameExtensions.GetBettypeNameForMatchSetting(changed.BetTypeId, snapshot.NewMatch.MatchType, snapshot.NewMatch.MinorBettypeName);
                var changedValues = existingAssignees.Select(a => new ChangedValue
                {
                    MessageTypeId = changed.MessageTypeId,
                    NewValue = changed.NewValue,
                    OldValue = changed.OldValue,
                    BetTypeName = bettypeName,
                    MatchNo = mainMatch.MatchNo,
                    League = mainMatch.LeagueName,
                    EventName = mainMatch.Home + EVENT_VS + mainMatch.Away,
                    AssigneeId = a.AssigneeId,
                    AssigneeName = a.AssigneeName
                });

                changedValueList.AddRange(changedValues);
            }

            return changedValueList;
        }

        public static IEnumerable<ChangedValue> GetListValueChangedWithDifferentBettype(List<MatchMaxBet> oldMatchSettings, List<MatchMaxBet> newMatchSettings)
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
                var listMessage = GetListValueChangedOfMatchSetting(oldMatchWithBettype, newMatchWithBettype, bettype);
                changedValues.AddRange(listMessage);
            }

            return changedValues;
        }

        public static IEnumerable<ChangedValue> GetListValueChangedOfMatchSetting(MatchMaxBet oldMatchSetting, MatchMaxBet newMatchSetting, string bettype)
        {
            if (!Equals(oldMatchSetting.MaxBet, newMatchSetting.MaxBet))
            {
                yield return new ChangedValue
                {
                    OldValue = oldMatchSetting.MaxBet.ToOWPMaxBetFormat(),
                    NewValue = newMatchSetting.MaxBet.ToOWPMaxBetFormat(),
                    BetTypeName = bettype,
                    MessageTypeId = (int)MsgTemplateCode.EditMaxbet
                };
            }
        }
    }
}