using OWP.OtherSport.Submit.Constants;

namespace OWP.OtherSport.UnitTests.EventAssignment.Services;

using AutoMapper;
using NSubstitute;
using OtherSport.EventAssignment;
using OtherSport.EventAssignment.Models;
using OtherSport.EventAssignment.Services;
using OtherSport.Submit.Models;
using Xunit;
using Xunit.Asserts.Compare;

public class EventAssignmentBuilderUnitTest
{
    private readonly AssignmentBuilder assignmentBuilder;

    private readonly MatchSettingBuilder matchSettingBuilder;

    private readonly IMapper mapper;

    public EventAssignmentBuilderUnitTest()
    {
        assignmentBuilder = Substitute.For<AssignmentBuilderImp>();
        matchSettingBuilder = Substitute.For<MatchSettingBuilderImp>();
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<MatchInfo, EventAssignmentModel>();
        });

        mapper = configuration.CreateMapper();
    }

    [Fact]
    public void RemoveInvalidNote_Always_MapMatchSetting()
    {
        var matchAssignment = new Dictionary<int, List<MatchAssignment>>
        {
            {   117, new List<MatchAssignment>
                {
                    new(){AssignmentNote = "sa",UserId=0,BetTypeName="1H (HDP)", AssignmentType=1},
                    new(){AssignmentNote = "#<user>",UserId=0, BetTypeName="FT (HDP)",AssignmentType=2}
                }
            },
            {   222, new List<MatchAssignment>
                {
                    new(){AssignmentNote = "#<user>",UserId=12,BetTypeName="1H (HDP)", AssignmentType=2},
                    new(){AssignmentNote = "#<user>",UserId=0,BetTypeName="FT (HDP)", AssignmentType=1}
                }
            }
        };

        var expected = new Dictionary<int, List<MatchAssignment>>
        {
            {   117, new List<MatchAssignment>
                {
                    new(){AssignmentNote = "sa",UserId=0,BetTypeName="1H (HDP)", AssignmentType=1}
                }
            },
            {   222, new List<MatchAssignment>
                {
                    new(){AssignmentNote = "#<user>",UserId=0,BetTypeName="FT (HDP)", AssignmentType=1},
                    new(){AssignmentNote = "#<user>",UserId=12,BetTypeName="1H (HDP)", AssignmentType=2}
                }
            }
        };

        EventAssignmentBuilderImp.RemoveInvalidNote(matchAssignment);

        DeepAssert.Equal(expected, matchAssignment);
    }

    [Fact]
    public void Build_MapEventAssignmentModel_BuildMatchSettingAndMatchAssignment()
    {
        var request = new QueryEventAssignmentRequest
        {
            Employee = "Starix1",
            MatchStatuses = new List<string> { "In-play", "Early" }
        };

        var service = new EventAssignmentBuilderImp(assignmentBuilder, matchSettingBuilder, mapper);
        var assignmentSnapshot = new AssignmentSnapshot
        {
            MatchAssignments = new Dictionary<int, List<MatchAssignment>>
            {
                {   117, new List<MatchAssignment>
                    {
                        new(){Username="Starix1", AssignmentNote = "sa",UserId=0,BetTypeName=Bettype.FTOUOE, AssignmentType=1}
                    }
                },
                {   222, new List<MatchAssignment>
                    {
                        new(){Username="Starix1", AssignmentNote = "#<user>",UserId=12,BetTypeName=Bettype.First5FramesHDP, AssignmentType=2},
                        new(){Username="Starix1", AssignmentNote = "#<user>",UserId=0,BetTypeName=Bettype.FramesWinnerHDPOUOE, AssignmentType=1}
                    }
                }
            },
            Matches = new Dictionary<int, MatchInfo>
            {
                {   117, new MatchInfo
                    {
                        OSMatchId = 117,
                        Sport = "Basketball",
                        MatchNo=2,
                        MatchStatus = "In-play"
                    }
                },
                {   222, new MatchInfo
                    {
                        OSMatchId = 222,
                        Sport = "Snooker",
                        MatchNo=2,
                        MatchStatus = "Early"
                    }
                }
            },
            MatchSettings = new Dictionary<int, List<MatchSetting>>
            {
                {   117, new List<MatchSetting>
                    {
                        new(){BetTypeName=Bettype.FTOUOE,MaxBet=1000,Spread=0.021m},
                        new(){BetTypeName=Bettype.FTOE,MaxBet=500,Spread=0.02m}
                    }
                },
                {   222, new List<MatchSetting>
                    {
                        new(){BetTypeName=Bettype.FramesWinnerOU,MaxBet=2000,Spread=null},
                        new(){BetTypeName=Bettype.FramesWinnerOE,MaxBet=1000,Spread=0.03m},
                        new(){BetTypeName=Bettype.FramesWinnerHDPOUOE,MaxBet=2000,Spread=null},
                        new(){BetTypeName=Bettype.First5FramesHDP,MaxBet=1700,Spread=0.07m}
                    }
                }
            }
        };

        var expected = GetExpected();

        var actual = service.Build(assignmentSnapshot, request);

        DeepAssert.Equal(expected, actual);
    }

    public List<EventAssignmentModel> GetExpected()
        => new()
        {
            new EventAssignmentModel
            {
                OSMatchId = 117,
                Sport = "Basketball",
                MatchNo=2,
                MatchStatus = "In-play",
                TraderMaxbetModels = new List<TraderMaxBetModel>
                {
                    new()
                    {
                        BetTypeName = Bettype.FTOUOE,
                        TraderAssignment = new TraderAssignment{MainTrader="sa",AssignmentNote=""},
                        MatchSettings = new List<MatchSettingModel>
                        {
                            new(){BetTypeName="OU", MaxBet=1000, Spread="2.1%"},
                            new(){BetTypeName="OE", MaxBet=500, Spread="2%"}
                        }
                    }
                }
            },
            new EventAssignmentModel
            {
                OSMatchId = 222,
                Sport = "Snooker",
                MatchNo=2,
                MatchStatus = "Early",
                TraderMaxbetModels = new List<TraderMaxBetModel>
                {
                    new()
                    {
                        BetTypeName = "Frames Winner (HDP/OU/OE)",
                        TraderAssignment = new TraderAssignment{MainTrader="",AssignmentNote=""},
                        MatchSettings = new List<MatchSettingModel>
                        {
                            new(){BetTypeName="HDP",MaxBet=2000,Spread=""},
                            new(){BetTypeName="OU",MaxBet=2000,Spread=""},
                            new(){BetTypeName="OE",MaxBet=1000,Spread="3%"}
                        }
                    },
                    new()
                    {
                        BetTypeName = "First 5 Frames (HDP)",
                        TraderAssignment = new TraderAssignment{MainTrader="",AssignmentNote="#Starix1"},
                        MatchSettings = new List<MatchSettingModel>
                        {
                            new(){BetTypeName="",MaxBet=1700,Spread="7%"}
                        }
                    }
                }
            }
        };
}