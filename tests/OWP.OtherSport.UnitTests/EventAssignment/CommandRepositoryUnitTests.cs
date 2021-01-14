namespace OWP.OtherSport.UnitTests.EventAssignment
{
    using Fanex.Data.Repository;
    using NSubstitute;
    using OWP.OtherSport.EventAssignment;
    using OWP.OtherSport.EventAssignment.Models;
    using OWP.OtherSport.EventAssignment.Services;
    using OWP.OtherSport.Submit.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class CommandRepositoryUnitTests
    {
        private readonly IDynamicRepository dynamicRepository;

        private readonly CommandRepositoryImp commandRepository;

        public CommandRepositoryUnitTests()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();

            commandRepository = new CommandRepositoryImp(dynamicRepository);
        }

        [Fact]
        public async Task Query_Always_ReturnListEventAssignmentModel()
        {
            var request = new QueryEventAssignmentRequest
            {
                FromDate = DateTime.Parse("2021-01-01"),
                ToDate = DateTime.Today
            };

            var matches = FakeMatchInfoList();
            var assignments = FakeAssignments();
            var maxbets = FakeMaxbets();
            var queryResult = new ValueTuple<IEnumerable<MatchInfo>, IEnumerable<MatchAssignment>, IEnumerable<MatchMaxBet>>(matches, assignments, maxbets);
            dynamicRepository.FetchMultipleAsync<MatchInfo, MatchAssignment, MatchMaxBet>(Arg.Compat.Any<ICriteria>())
                .ReturnsForAnyArgs(queryResult);

            var expected = GetEventAssignmentModel();

            var actual = await commandRepository.Query(request);

            DeepAssert.Equal(expected, actual);
        }

        private static IEnumerable<MatchInfo> FakeMatchInfoList()
        {
            return new List<MatchInfo>
            {
                new MatchInfo
                {
                    OSMatchId = 123,
                    League = "*SPAIN PRIMERA LALIGA",
                    MatchStatus = "Live",
                    DateTime = DateTime.Parse("2021-01-12"),
                    Home = "CD Universidad Catolica(V)_Home",
                    Away = "AEK Athens (V)_Away",
                    Remark = "TA ROBOT_Draw will PK",
                    IPTV= "ipTv aaaaa",
                    WB= "Streaming bbbbbbb",
                    Provider= "Provider JAck",
                    AutoLink="Yes",
                    Category="Category",
                    NoHDP="NoHDP",
                    MatchOffer="MatchOffer",
                    OtherProvider="OtherProvider",
                    Service="Medium",
                    Sport = "Football"
                },

                new MatchInfo
                {
                    OSMatchId = 124,
                    League = "NBA",
                    MatchStatus = "Live",
                    DateTime = DateTime.Parse("2021-01-12"),
                    Home = "Golden State Warrior",
                    Away = "Brooklyn NET",
                    Remark = "TA ROBOT_Draw will PK",
                    IPTV= "ipTv aaaaa",
                    WB= "Streaming bbbbbbb",
                    Provider= "Provider",
                    AutoLink="Yes",
                    Category="Category",
                    NoHDP="NoHDP",
                    MatchOffer="MatchOffer",
                    OtherProvider="OtherProvider",
                    Service= "Medium",
                    Sport = "Basketball"
                }
            };
        }

        private static IEnumerable<MatchAssignment> FakeAssignments()
        {
            return new List<MatchAssignment> {
                new MatchAssignment {
                    AssignmentNote = null,
                    AssignmentType =  1,
                    BetTypeName = "1Q & 3Q (OU/OE)",
                    OSMatchId  = 123,
                    UserId = 8 ,
                    Username  =  "sa"
                },
                new MatchAssignment {
                    AssignmentNote = "check other #<user>",
                    AssignmentType =  2,
                    BetTypeName = "1Q & 3Q (OU/OE)",
                    OSMatchId  = 123,
                    UserId = 8 ,
                    Username  =  "sa"
                }
            };
        }

        private static IEnumerable<MatchMaxBet> FakeMaxbets()
        {
            return new List<MatchMaxBet>
            {
                new MatchMaxBet
                {
                    BetTypeName = "FT Run Line (HDP)",
                    OSMatchId = 124,
                    MaxBet = 100000.00m
                },
                new MatchMaxBet
                {
                    BetTypeName = "1H 1st 5 innings (HDP)",
                    OSMatchId = 124,
                    MaxBet = 1018.00m
                },
                new MatchMaxBet
                {
                    BetTypeName = "1H (OU/OE)",
                    OSMatchId = 123,
                    MaxBet = 1018.00m
                }
            };
        }

        private static IEnumerable<EventAssignmentModel> GetEventAssignmentModel()
        {
            return new List<EventAssignmentModel>
            {
                new EventAssignmentModel
                {
                    OSMatchId = 123,
                    League = "*SPAIN PRIMERA LALIGA",
                    MatchStatus = "Live",
                    DateTime = DateTime.Parse("2021-01-12"),
                    Home = "CD Universidad Catolica(V)_Home",
                    Away = "AEK Athens (V)_Away",
                    Remark = "TA ROBOT_Draw will PK",
                    IPTV= "ipTv aaaaa",
                    WB= "Streaming bbbbbbb",
                    Provider= "Provider JAck",
                    AutoLink="Yes",
                    Category="Category",
                    NoHDP="NoHDP",
                    MatchOffer="MatchOffer",
                    OtherProvider="OtherProvider",
                    Service="Medium",
                    Sport = "Football",
                    TraderMaxbetModels = new Dictionary<string, List<TraderMaxBet>>()
                    {
                        {
                            "1Q & 3Q (OU/OE)",
                            new List<TraderMaxBet>
                            {
                                new MatchAssignment
                                {
                                AssignmentNote = null,
                                AssignmentType =  1,
                                BetTypeName = "1Q & 3Q (OU/OE)",
                                OSMatchId  = 123,
                                UserId = 8 ,
                                Username  =  "sa"
                                },
                                new MatchAssignment
                                {
                                    AssignmentNote = "check other #<user>",
                                    AssignmentType =  2,
                                    BetTypeName = "1Q & 3Q (OU/OE)",
                                    OSMatchId  = 123,
                                    UserId = 8 ,
                                    Username  =  "sa"
                                }
                             }
                        },
                        {
                            "1H (OU/OE)",
                            new List<TraderMaxBet>
                            {
                                new MatchMaxBet
                                {
                                    BetTypeName = "1H (OU/OE)",
                                    OSMatchId = 123,
                                    MaxBet = 1018.00m
                                }
                             }
                         }
                    }
                },

                new EventAssignmentModel
                {
                    OSMatchId = 124,
                    League = "NBA",
                    MatchStatus = "Live",
                    DateTime = DateTime.Parse("2021-01-12"),
                    Home = "Golden State Warrior",
                    Away = "Brooklyn NET",
                    Remark = "TA ROBOT_Draw will PK",
                    IPTV= "ipTv aaaaa",
                    WB= "Streaming bbbbbbb",
                    Provider= "Provider",
                    AutoLink="Yes",
                    Category="Category",
                    NoHDP="NoHDP",
                    MatchOffer="MatchOffer",
                    OtherProvider="OtherProvider",
                    Service="Medium",
                    Sport = "Basketball",
                    TraderMaxbetModels = new Dictionary<string, List<TraderMaxBet>>()
                    {                       
                        {
                            "FT Run Line (HDP)",
                            new List<TraderMaxBet>
                            {
                                 new MatchMaxBet
                                 {
                                    BetTypeName = "FT Run Line (HDP)",
                                    OSMatchId = 124,
                                    MaxBet = 100000.00m
                                 }
                            }
                        },

                        {
                            "1H 1st 5 innings (HDP)",
                            new List<TraderMaxBet>
                            {
                                 new MatchMaxBet
                                 {
                                    BetTypeName = "1H 1st 5 innings (HDP)",
                                    OSMatchId = 124,
                                    MaxBet = 1018.00m
                                 }
                            }
                        }
                    }
                }
            };
        }
    }
}
