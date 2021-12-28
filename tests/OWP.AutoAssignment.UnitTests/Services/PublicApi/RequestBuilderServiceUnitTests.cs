namespace OWP.AutoAssignment.UnitTests.Services.PublicApi
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoAssignment.Services.PublicApi;
    using Models.PublicApi;
    using NSubstitute;
    using Xunit;
    using Xunit.Asserts.Compare;
    public class RequestBuilderServiceUnitTests
    {
        private readonly Repository repository;
        private readonly SchedulingMatchBuilderService matchService;
        private readonly LAUserService laService;
        private readonly TraderUserService traderService;

        public RequestBuilderServiceUnitTests()
        {
            repository = Substitute.For<Repository>();
            matchService = Substitute.For<SchedulingMatchBuilderService>();
            laService = Substitute.For<LAUserService>();
            traderService = Substitute.For<TraderUserService>();
        }

        [Fact]
        public async Task BuildRequestSenderModel_Always_ReturnSender()
        {
            var request = new List<RequestModel>()
            {
                new ()
                {
                    RequestId = "johnny",
                    CreatedDate = DateTime.Parse("2021-12-08"),
                    EventDate = DateTime.Parse("2021-12-08")
                }
            };
            var service = new RequestBuilderImp(repository, matchService, laService, traderService);

            matchService.GetMatchesInfo(Arg.Compat.Any<DateTime>()).Returns(MockMatchService());
            laService.GetLAUsersInfo(Arg.Compat.Any<DateTime>()).Returns(MockLaService());
            traderService.GetTradersInfo(Arg.Compat.Any<DateTime>()).Returns(MockTraderService());
            var expected = MockRequestSenders();
            var actual = await service.BuildRequestSenderModel(request);

            DeepAssert.Equal(expected, actual);

        }

        public List<RequestSender> MockRequestSenders()
            => new()
            {
                new()
                {
                    Action = "New",
                    EventDate = DateTime.Parse("2021-12-08"),
                    RequestId = "johnny",
                    DataHolder = new DataHolder()
                    {
                        Matches = new SchedulingMatch()
                        {
                            Real = new List<RealMatchSender>
                            {
                                new ()
                                {
                                    KOTime=  DateTime.Parse("2021-12-08").ToString(),
                                    MatchId=1234,
                                    MaxBetMainMatch=12000,
                                }
                            },
                            Saba = new List<SabaMatchSender>
                            {
                                new ()
                                {
                                    KOTime=  DateTime.Parse("2021-12-08").ToString(),
                                    MatchId=9999,
                                    SabaType=1,
                                    MaxBet=12000
                                }
                            }
                        },
                        Suspenders = new List<SchedulingLAUser>()
                        {
                            new()
                            {
                                Level = 1,
                                Location = "SMS",
                                SpeakChinese = true,
                                SpeakEnglish = true,
                                UserId = 8,
                                WorkingShifts = new []
                                {
                                    new SchedulingWorkingShift
                                    {
                                        InTime = "2021-12-08T00:00:00",
                                        OutTime = "2021-12-09T00:00:00"
                                    }
                                }
                            }
                        },
                        Traders = new List<SchedulingTraderInfo>()
                        {
                            new()
                            {
                                Level = 1,
                                BettypesPreferences = "Booking",
                                MaxGames = 1,
                                MultipleGames = "1G",
                                Role = "Employee",
                                TraderId = 8,
                                WorkingShifts = new []
                                {
                                    new SchedulingWorkingShift
                                    {
                                        InTime= "2021-12-08T00:00:00",
                                        OutTime="2021-12-09T00:00:00"
                                    }
                                }
                            }
                        }
                    }
                }
            };

        public SchedulingMatch MockMatchService()
            => new()
            {
                Real = new List<RealMatchSender>
                {
                    new ()
                    {
                        KOTime=  DateTime.Parse("2021-12-08").ToString(),
                        MatchId=1234,
                        MaxBetMainMatch=12000
                    }
                },
                Saba = new List<SabaMatchSender>
                {
                    new ()
                    {
                        KOTime=  DateTime.Parse("2021-12-08").ToString(),
                        MatchId=9999,
                        SabaType=1,
                        MaxBet=12000
                    }
                }
            };

        public List<SchedulingLAUser> MockLaService()
            => new()
            {
                new()
                {
                    Level = 1,
                    Location = "SMS",
                    SpeakChinese = true,
                    SpeakEnglish = true,
                    UserId = 8,
                    WorkingShifts = new[]
                    {
                        new SchedulingWorkingShift
                        {
                            InTime = "2021-12-08T00:00:00",
                            OutTime = "2021-12-09T00:00:00"
                        }
                    }
                }
            };

        public List<SchedulingTraderInfo> MockTraderService()
            => new()
            {
                new()
                {
                    Level = 1,
                    BettypesPreferences = "Booking",
                    MaxGames = 1,
                    MultipleGames = "1G",
                    Role = "Employee",
                    TraderId = 8,
                    WorkingShifts = new[]
                    {
                        new SchedulingWorkingShift
                        {
                            InTime= "2021-12-08T00:00:00",
                            OutTime="2021-12-09T00:00:00"
                        }
                    }
                }
            };
    }
}
