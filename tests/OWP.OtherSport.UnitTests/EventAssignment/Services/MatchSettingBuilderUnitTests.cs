using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.EventAssignment.Services;
using OWP.OtherSport.Submit.Constants;
using OWP.OtherSport.Submit.Models;
using Xunit;

namespace OWP.OtherSport.UnitTests.EventAssignment.Services;

using Xunit.Asserts.Compare;

public class MatchSettingBuilderUnitTests
{
    private readonly MatchSettingBuilderImp services;

    public MatchSettingBuilderUnitTests()
    {
        services = new MatchSettingBuilderImp();
    }

    [Fact]
    public void Build_MatchSetting_MapMatchSetting()
    {
        var eventAssignmentModels = new List<EventAssignmentModel>
        {
            new()
            {
                OSMatchId = 111,
                Sport = "Basketball",
                TraderMaxbetModels =
                [
                    new TraderMaxBetModel
                    {
                        BetTypeName = Bettype.FTOUOE, TraderAssignment = new TraderAssignment { MainTrader = "FTOUOE" }
                    },
                    new TraderMaxBetModel
                    {
                        BetTypeName = Bettype.FTHDP, TraderAssignment = new TraderAssignment { MainTrader = "FTHDP" }
                    }
                ]
            },
            new()
            {
                OSMatchId = 222,
                Sport = "Snooker",
                TraderMaxbetModels =
                [
                    new TraderMaxBetModel
                    {
                        BetTypeName = Bettype.FramesWinnerHDPOUOE,
                        TraderAssignment = new TraderAssignment { MainTrader = "FramesWinnerHDPOUOE" }
                    },
                    new TraderMaxBetModel
                    {
                        BetTypeName = Bettype.First5FramesHDP,
                        TraderAssignment = new TraderAssignment { MainTrader = "First5FramesHDP" }
                    },
                    new TraderMaxBetModel
                    {
                        BetTypeName = Bettype.MatchWinnerHDPOU,
                        TraderAssignment = new TraderAssignment { MainTrader = "First5FramesHDP" }
                    }
                ]
            }
        };
        var matchSettings = new Dictionary<int, List<MatchSetting>>
        {
            {   111, [
                    new MatchSetting { BetTypeName = Bettype.FTHDP, MaxBet = 2000, Spread = null },
                    new MatchSetting { BetTypeName = Bettype.FTOUOE, MaxBet = 1000, Spread = 0.02m },
                    new MatchSetting { BetTypeName = Bettype.FTOE, MaxBet = 555, Spread = 0.03m }
                ]
            },
            {   222, [
                    new MatchSetting { BetTypeName = Bettype.FramesWinnerOU, MaxBet = 2000, Spread = null },
                    new MatchSetting { BetTypeName = Bettype.FramesWinnerOE, MaxBet = 1000, Spread = 0.03m },
                    new MatchSetting { BetTypeName = Bettype.FramesWinnerHDPOUOE, MaxBet = 2000, Spread = null },
                    new MatchSetting { BetTypeName = Bettype.MatchWinnerOU, MaxBet = 555, Spread = 0.02m }
                ]
            }
        };

        var expected = new List<EventAssignmentModel>
        {
            new()
            {
                OSMatchId = 111,
                Sport = "Basketball",
                TraderMaxbetModels =
                [
                    new TraderMaxBetModel
                    {
                        BetTypeName = Bettype.FTOUOE,
                        TraderAssignment = new TraderAssignment { MainTrader = "FTOUOE" },
                        MatchSettings =
                        [
                            new MatchSettingModel { BetTypeName = "OU", MaxBet = 1000, Spread = "2%" },
                            new MatchSettingModel { BetTypeName = "OE", MaxBet = 555, Spread = "3%" }
                        ]
                    },

                    new TraderMaxBetModel
                    {
                        BetTypeName = Bettype.FTHDP,
                        TraderAssignment = new TraderAssignment { MainTrader = "FTHDP" },
                        MatchSettings = [new MatchSettingModel { BetTypeName = "", MaxBet = 2000, Spread = "" }]
                    }
                ]
            },
            new()
            {
                OSMatchId = 222,
                Sport = "Snooker",
                TraderMaxbetModels =
                [
                    new TraderMaxBetModel
                    {
                        BetTypeName = Bettype.FramesWinnerHDPOUOE,
                        TraderAssignment = new TraderAssignment { MainTrader = "FramesWinnerHDPOUOE" },
                        MatchSettings =
                        [
                            new MatchSettingModel { BetTypeName = "HDP", MaxBet = 2000, Spread = "" },
                            new MatchSettingModel { BetTypeName = "OU", MaxBet = 2000, Spread = "" },
                            new MatchSettingModel { BetTypeName = "OE", MaxBet = 1000, Spread = "3%" }
                        ]
                    },

                    new TraderMaxBetModel
                    {
                        BetTypeName = Bettype.First5FramesHDP,
                        TraderAssignment = new TraderAssignment { MainTrader = "First5FramesHDP" }
                    },

                    new TraderMaxBetModel
                    {
                        BetTypeName = Bettype.MatchWinnerHDPOU,
                        TraderAssignment = new TraderAssignment { MainTrader = "First5FramesHDP" },
                        MatchSettings = [new MatchSettingModel { BetTypeName = "OU", MaxBet = 555, Spread = "2%" }]
                    }
                ]
            }
        };

        services.Build(eventAssignmentModels, matchSettings);

        DeepAssert.Equal(expected[0], eventAssignmentModels[0]);
        DeepAssert.Equal(expected[1], eventAssignmentModels[1]);
    }
}