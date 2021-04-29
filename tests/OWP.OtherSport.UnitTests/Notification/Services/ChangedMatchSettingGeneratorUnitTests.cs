using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWP.OtherSport.Notification.Constants;
using OWP.OtherSport.Notification.Models;
using OWP.OtherSport.Notification.Services;
using OWP.OtherSport.Submit.Models;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.OtherSport.UnitTests.Notification.Services
{
    public class ChangedMatchSettingGeneratorUnitTests
    {
        [Fact]
        public void GetListValueChangedOfMatchSetting_OldvalueHasMaxbetChange_NewvalueHasMaxbetChange()
        {
            var oldMatchSetting = new MatchMaxBet
            {
                MaxBet = 120000,
            };
            var newMatchSetting = new MatchMaxBet
            {
                MaxBet = 110000,
            };
            var actual = ChangedMatchSettingGeneratorImp.GetListValueChangedOfMatchSetting(oldMatchSetting, newMatchSetting, "FT (HDP)", 123).FirstOrDefault();
            var expected = new ChangedValue
            {
                BetTypeName = "FT (HDP)",
                OldValue = "120K",
                NewValue = "110K",
                MessageTypeId = 10,
                OSMatchId=123
            };
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void GetListMatchChangeMaxBet_WithBettypeFTHDP_ReturnListMatch_Has_MatxbetChanged()
        {
            var oldMatchSettings = new List<MatchMaxBet>
            {
                new MatchMaxBet
                {
                    OSMatchId=123,
                    MaxBet=12000,
                    BetTypeName= "FT (HDP)"
                }
            };

            var newMatchSettings = new List<MatchMaxBet>
            {
                new MatchMaxBet
                {
                    OSMatchId=123,
                    MaxBet=11000,
                    BetTypeName= "FT (HDP)"
                }
            };
            var actual = ChangedMatchSettingGeneratorImp.GetListValueChangedWithDifferentBettype(oldMatchSettings, newMatchSettings, 123);
            var expected = new List<ChangedValue>
            {
                new ChangedValue
                {
                    MessageTypeId=(int)MsgTemplateCode.EditMaxbet,
                    BetTypeName="FT (HDP)",
                    OldValue="12K",
                    NewValue="11K",
                    OSMatchId=123
                }
            };
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void CreateListMatchSettingChange_Old_New_Snapshot_Has_Diff_MatchSetting_Return_ListMatchChanged()
        {
            var changedMatchSettingGeneratorImp = new ChangedMatchSettingGeneratorImp();
            var snapshot = new ChangedMatchSnapshot
            {
                OldSnapshot = new AssignmentSnapshot
                {
                    MaxBet = new Dictionary<int, List<MatchMaxBet>>
                    {
                        {123, new List<MatchMaxBet>
                        {
                            new MatchMaxBet
                            {
                                OSMatchId=123,
                                BetTypeName= "FT (HDP)",
                                MaxBet=12000
                            }
                        } }
                    }
                },
                NewSnapshot = new AssignmentSnapshot
                {
                    MaxBet = new Dictionary<int, List<MatchMaxBet>>
                    {
                        {123, new List<MatchMaxBet>
                        {
                            new MatchMaxBet
                            {
                                OSMatchId=123,
                                BetTypeName= "FT (HDP)",
                                MaxBet=11000
                            }
                        } }
                    },
                    Matches= new Dictionary<int, MatchInfo>
                    {
                        {123, new MatchInfo
                        {
                            MatchNo=1234,
                            League="Vleague",
                            Home="Binh Dinh",
                            Away= "Ha Noi"
                        } }
                    }
                }
            };
            var assignees = new List<Assignee>
            {
                new Assignee
                {
                    AssigneeId=8,
                    AssigneeName="sa"
                }
            };
            var actual = changedMatchSettingGeneratorImp.Create(snapshot, 123, assignees);
            var expected = new List<ChangedValue>
            {
                new ChangedValue
                {
                    AssigneeId=8,
                    AssigneeName="sa",
                    BetTypeName="FT (HDP)",
                    EventName="Binh Dinh -vs- Ha Noi",
                    League="Vleague",
                    MatchNo="1234",
                    MessageTypeId=10,
                    NewValue="11K",
                    OldValue="12K",
                    OSMatchId=123
                }
            };
            DeepAssert.Equal(expected, actual);
        }
    }
}
