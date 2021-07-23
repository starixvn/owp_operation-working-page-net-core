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
            var oldMatchSetting = new MatchSetting
            {
                MaxBet = Convert.ToDecimal("120.00"),
                Spread = Convert.ToDecimal(0.32),
            };
            var newMatchSetting = new MatchSetting
            {
                MaxBet = Convert.ToDecimal("11000.00"),
                Spread = Convert.ToDecimal(0.31),
            };
            var expected = new List<ChangedValue>
            {
              new ChangedValue {
                  BetTypeName = "FT (HDP)",
                  OldValue = "120",
                  NewValue = "11000",
                  MessageTypeId = (int)MsgTemplateCode.EditMaxbet,
                  OSMatchId = 123
                },
              new ChangedValue {
                  BetTypeName = "FT (HDP)",
                  OldValue = "32%",
                  NewValue = "31%",
                  MessageTypeId = (int)MsgTemplateCode.EditSpread,
                  OSMatchId = 123
                },
            };

            var actual = ChangedMatchSettingGeneratorImp.GetListValueChangedOfMatchSetting(oldMatchSetting, newMatchSetting, "FT (HDP)", 123);

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void GetListMatchChangeMaxBet_WithBettypeFTHDP_ReturnListMatch_Has_MatxbetChanged()
        {
            var oldMatchSettings = new List<MatchSetting>
            {
                new MatchSetting
                {
                    OSMatchId=123,
                    MaxBet=Convert.ToDecimal("11000.00"),
                    Spread= Convert.ToDecimal(0.02),
                    BetTypeName= "FT (HDP)"
                }
            };
            var newMatchSettings = new List<MatchSetting>
            {
                new MatchSetting
                {
                    OSMatchId=123,
                    MaxBet=Convert.ToDecimal("12000.00"),
                    Spread= Convert.ToDecimal(0.021),
                    BetTypeName= "FT (HDP)"
                }
            };
            var expected = new List<ChangedValue>
            {
                new ChangedValue
                {
                    MessageTypeId=(int)MsgTemplateCode.EditMaxbet,
                    BetTypeName="FT (HDP)",
                    OldValue="11000",
                    NewValue="12000",
                    OSMatchId=123
                },
                new ChangedValue
                {
                    MessageTypeId=(int)MsgTemplateCode.EditSpread,
                    BetTypeName="FT (HDP)",
                    OldValue="2%",
                    NewValue="2.1%",
                    OSMatchId=123
                },
            };

            var actual = ChangedMatchSettingGeneratorImp.GetListValueChangedWithDifferentBettype(oldMatchSettings, newMatchSettings, 123);

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void CreateListMatchSettingChange_Old_New_Snapshot_Has_Diff_MatchSetting_Return_ListMatchChanged()
        {
            var changedMatchSettingGeneratorImp = new ChangedMatchSettingGeneratorImp();
            var snapshot = MockChangeSnapshot();
            var assignees = new List<Assignee>
            {
                new Assignee
                {
                    AssigneeId=8,
                    AssigneeName="sa"
                }
            };
            var expected = GetExpect();

            var actual = changedMatchSettingGeneratorImp.Create(snapshot, 123, assignees);

            DeepAssert.Equal(expected, actual);
        }

        private ChangedMatchSnapshot MockChangeSnapshot()
        => new ChangedMatchSnapshot
        {
            OldSnapshot = new AssignmentSnapshot
            {
                MatchSettings = new Dictionary<int, List<MatchSetting>>
                    {
                        {123, new List<MatchSetting>
                        {
                            new MatchSetting
                            {
                                OSMatchId=123,
                                BetTypeName= "FT (HDP)",
                                MaxBet=Convert.ToDecimal("12000.00"),
                                Spread=Convert.ToDecimal(0.21),
                            },
                            new MatchSetting
                            {
                                OSMatchId=123,
                                BetTypeName= "FT (OE)",
                                MaxBet=Convert.ToDecimal("17000.00"),
                                Spread=Convert.ToDecimal(0.01),
                            }
                        } }
                    }
            },
            NewSnapshot = new AssignmentSnapshot
            {
                MatchSettings = new Dictionary<int, List<MatchSetting>>
                    {
                        {123, new List<MatchSetting>
                        {
                            new MatchSetting
                            {
                                OSMatchId=123,
                                BetTypeName= "FT (HDP)",
                                MaxBet= Convert.ToDecimal("11000.00"),
                                Spread=Convert.ToDecimal(0.22)
                            },
                            new MatchSetting
                            {
                                OSMatchId=123,
                                BetTypeName= "FT (OE)",
                                MaxBet=Convert.ToDecimal( "19000.00"),
                                Spread=Convert.ToDecimal(0.02),
                            }
                        } }
                    },
                Matches = new Dictionary<int, MatchInfo>
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

        private List<ChangedValue> GetExpect()
        => new List<ChangedValue>
            {
                new ChangedValue
                {
                    UserId=8,
                    Username="sa",
                    BetTypeName="FT (HDP)",
                    EventName="Binh Dinh -vs- Ha Noi",
                    League="Vleague",
                    MatchNo="1234",
                    MessageTypeId=(int)MsgTemplateCode.EditMaxbet,
                    NewValue="11000",
                    OldValue="12000",
                    OSMatchId=123
                },
                new ChangedValue
                {
                    UserId=8,
                    Username="sa",
                    BetTypeName="FT (HDP)",
                    EventName="Binh Dinh -vs- Ha Noi",
                    League="Vleague",
                    MatchNo="1234",
                    MessageTypeId=(int)MsgTemplateCode.EditSpread,
                    NewValue="22%",
                    OldValue="21%",
                    OSMatchId=123
                },
                new ChangedValue
                {
                    UserId=8,
                    Username="sa",
                    BetTypeName="FT (OE)",
                    EventName="Binh Dinh -vs- Ha Noi",
                    League="Vleague",
                    MatchNo="1234",
                    MessageTypeId=(int)MsgTemplateCode.EditMaxbet,
                    NewValue="19000",
                    OldValue="17000",
                    OSMatchId=123
                },
                new ChangedValue
                {
                    UserId=8,
                    Username="sa",
                    BetTypeName="FT (OE)",
                    EventName="Binh Dinh -vs- Ha Noi",
                    League="Vleague",
                    MatchNo="1234",
                    MessageTypeId=(int)MsgTemplateCode.EditSpread,
                    NewValue="2%",
                    OldValue="1%",
                    OSMatchId=123
                }
            };
    }
}