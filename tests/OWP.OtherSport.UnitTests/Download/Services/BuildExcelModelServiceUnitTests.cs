namespace OWP.OtherSport.UnitTests.Download.Services
{
    using AutoMapper;
    using NSubstitute;
    using OWP.OtherSport.Download;
    using OWP.OtherSport.Download.Models;
    using OWP.OtherSport.Download.Services;
    using OWP.OtherSport.Submit.Contants;
    using OWP.OtherSport.Submit.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class BuildExcelModelServiceUnitTests
    {
        private readonly BuildExcelModelServiceImp service;

        private readonly CommandRepository commandRepository;

        public BuildExcelModelServiceUnitTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MatchAssignment, ExcelMatchAssignment>();
                cfg.CreateMap<MatchSetting, ExcelMatchMaxBet>();
            });
            var iMapper = config.CreateMapper();
            commandRepository = Substitute.For<CommandRepository>();

            service = new BuildExcelModelServiceImp(commandRepository, iMapper);
        }

        [Fact]
        public async Task BuildExcelModel()
        {
            var request = new DownloadAssignmentRequest
            {
                From = new DateTime(2021,03,25),
                To = new DateTime(2021, 03, 25),
                Employee = "sa",
                Sports = "Basketball",
                MatchStatuses = "Live"
            };
            var assignmentSnapshot = GetAssignmentSnapshot();
            commandRepository.Query(request).Returns(assignmentSnapshot);

            var expectedHeader = new ExcelHeaderInfoModel
            {
                FromDate = "Mar 25, 2021",
                ToDate = "Mar 25, 2021",
                Employee = "sa",
                Sports = "Basketball",
                MatchStatus = "Live"
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
                        new ExcelMatchAssignment { ColumnIndex=19,Value="", AssignmentType = 1, BetTypeName = Bettype.FTHDP, UserId = 8, Username="sa" },
                        new ExcelMatchAssignment { ColumnIndex=87,Value="#starix15 mynote", AssignmentType = 2, BetTypeName = Bettype.FirstHHDP,
                            UserId = 123, Username="starix15", AssignmentNote = "#<user> mynote" }
                    } }
                },
                MaxBet = new Dictionary<int, List<ExcelMatchMaxBet>>
                {
                    { 111, new List<ExcelMatchMaxBet> {
                        new ExcelMatchMaxBet { ColumnIndex=153,Value="1234", BetTypeName = Bettype.FTHDP, MaxBet = 1234 },
                        new ExcelMatchMaxBet { ColumnIndex=154,Value="222.05", BetTypeName = Bettype.FTOUOE, MaxBet = 222.05m }
                    } }
                }
            };

            var actual = await service.BuildExcelModel(request);

            DeepAssert.Equal(expectedHeader, actual.HeaderInfo);

            Assert.Equal(expectedAssignments.Matches.Count, actual.ExcelAssignmentModels.Matches.Count);
            DeepAssert.Equal(expectedAssignments.Matches, actual.ExcelAssignmentModels.Matches);
            DeepAssert.Equal(expectedAssignments.MatchAssignments, actual.ExcelAssignmentModels.MatchAssignments);
            DeepAssert.Equal(expectedAssignments.MaxBet, actual.ExcelAssignmentModels.MaxBet);
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
                        new MatchSetting { BetTypeName = Bettype.FTHDP, MaxBet = 1234 },
                        new MatchSetting { BetTypeName = Bettype.FTOUOE, MaxBet = 222.05m }
                    } }
                }
            };
        }
    }
}
