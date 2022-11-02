namespace OWP.OtherSport.UnitTests.Download.Services
{
    using AutoMapper;
    using NSubstitute;
    using OWP.OtherSport.Download;
    using OWP.OtherSport.Download.Models;
    using OWP.OtherSport.Download.Services;
    using OWP.OtherSport.Shared.Services;
    using OWP.OtherSport.Submit.Contants;
    using OWP.OtherSport.Submit.Models;

    using Xunit;
    using Xunit.Asserts.Compare;

    public class BuildExcelModelServiceUnitTests
    {
        private readonly BuildExcelModelServiceImp service;

        private readonly CommandRepository commandRepository;

        private readonly MatchStatusCalculator matchStatusCalculator;

        public BuildExcelModelServiceUnitTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MatchAssignment, ExcelMatchAssignment>();
                cfg.CreateMap<MatchSetting, ExcelMatchMaxBetSpread>();
            });
            var iMapper = config.CreateMapper();
            commandRepository = Substitute.For<CommandRepository>();
            matchStatusCalculator = Substitute.For<MatchStatusCalculator>();

            service = new BuildExcelModelServiceImp(commandRepository, matchStatusCalculator, iMapper);
        }

        [Fact]
        public async Task BuildExcelModel()
        {
            var request = new DownloadAssignmentRequest
            {
                From = new DateTime(2021, 03, 25),
                To = new DateTime(2021, 03, 25),
                Employee = "sa",
                Sports = "Basketball",
                OfferLives = "Live"
            };
            var assignmentSnapshot = GetAssignmentSnapshot();
            commandRepository.Query(request).Returns(assignmentSnapshot);

            var expectedHeader = new ExcelHeaderInfoModel
            {
                FromDate = "Mar 25, 2021",
                ToDate = "Mar 25, 2021",
                Employee = "sa",
                Sports = "Basketball",
                OfferLive = "Live"
            };

            var expectedAssignments = new ExcelAssignmentModel
            {
                Matches = new List<MatchInfo>
                {
                    new MatchInfo { OSMatchId = 111, Sport = Sport.Basketball, Home = "home111", League = "league111" }
                },
                MatchAssignments = new Dictionary<int, List<ExcelMatchAssignment>>
                {
                    { 111, new List<ExcelMatchAssignment> {
                        new ExcelMatchAssignment { ColumnIndex=20,Value="", AssignmentType = 1, BetTypeName = Bettype.FTHDP, UserId = 8, Username="sa" },
                        new ExcelMatchAssignment { ColumnIndex=88,Value="#starix15 mynote", AssignmentType = 2, BetTypeName = Bettype.FirstHHDP,
                            UserId = 123, Username="starix15", AssignmentNote = "#<user> mynote" }
                    } }
                },
                MaxBetSpread = new Dictionary<int, List<ExcelMatchMaxBetSpread>>
                {
                    { 111, new List<ExcelMatchMaxBetSpread> {
                        new ExcelMatchMaxBetSpread {
                            ColumnIndexMaxbet=154,
                            ValueMaxbet="1234",
                            BetTypeName = Bettype.FTHDP,
                            MaxBet = 1234,
                            Spread =12,
                            ColumnIndexSpread = 225,
                            ValueSpread = "12"
                        },
                        new ExcelMatchMaxBetSpread {
                            ColumnIndexMaxbet=155,
                            ValueMaxbet="222.05",
                            BetTypeName = Bettype.FTOUOE,
                            MaxBet = 222.05m,
                            Spread = 13,
                            ColumnIndexSpread = 226,
                            ValueSpread = "13"
                        }
                    } }
                }
            };

            var actual = await service.BuildExcelModel(request);

            DeepAssert.Equal(expectedHeader, actual.HeaderInfo);

            Assert.Equal(expectedAssignments.Matches.Count, actual.ExcelAssignmentModels.Matches.Count);
            DeepAssert.Equal(expectedAssignments.Matches, actual.ExcelAssignmentModels.Matches);
            DeepAssert.Equal(expectedAssignments.MatchAssignments, actual.ExcelAssignmentModels.MatchAssignments);
            DeepAssert.Equal(expectedAssignments.MaxBetSpread, actual.ExcelAssignmentModels.MaxBetSpread);
        }

        public AssignmentSnapshot GetAssignmentSnapshot()
        {
            return new AssignmentSnapshot
            {
                Matches = new Dictionary<int, MatchInfo>
                {
                    { 111, new MatchInfo { OSMatchId = 111, Sport = Sport.Basketball, Home = "home111", League = "league111" } },
                    { 222, new MatchInfo { OSMatchId = 222, Sport = Sport.Baseball, Home = "home222", League = "league222" } },
                    { 333, new MatchInfo { OSMatchId = 333, Sport = Sport.Tennis, Home = "home222", League = "league222" } }
                },
                MatchAssignments = new Dictionary<int, List<MatchAssignment>>
                {
                    { 111, new List<MatchAssignment> {
                        new MatchAssignment { AssignmentType = 1, BetTypeName = Bettype.FTHDP, UserId = 8, Username="sa" },
                        new MatchAssignment { AssignmentType = 2, BetTypeName = Bettype.FirstHHDP, UserId = 123, Username="starix15",
                            AssignmentNote = "#<user> mynote" }
                    } },
                    { 222, new List<MatchAssignment> {
                        new MatchAssignment { AssignmentType = 1, BetTypeName = Bettype.FTHDP, UserId = 2228, Username="tr", Nickname="abc" },
                    } }
                },
                MatchSettings = new Dictionary<int, List<MatchSetting>>
                {
                    { 111, new List<MatchSetting> {
                        new MatchSetting { BetTypeName = Bettype.FTHDP, MaxBet = 1234, Spread = 12 },
                        new MatchSetting { BetTypeName = Bettype.FTOUOE, MaxBet = 222.05m, Spread = 13 }
                    } }
                }
            };
        }
    }
}