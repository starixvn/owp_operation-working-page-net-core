namespace OWP.WorkingShift.UnitTests.DailyWorkingShift.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Criteria;
    using DailyWorkingShift.Repositories;
    using Fanex.Data.Repository;
    using Models;
    using NSubstitute;
    using OWP.Models;
    using Xunit;

    public class RepositoryUnitTests
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly Repository repository;

        public RepositoryUnitTests()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            repository = new RepositoryImp(dynamicRepository);
        }

        [Fact]
        public async Task Query_Returns_ValidResult()
        {
            var mockDate = new DateTime(2019, 05, 20);
            // Arrange
            var queryUsers = new List<QueryUser>
            {
                new QueryUser { EmployeeId = 1, UserName = "u1", SectionName = "s1" },
                new QueryUser { EmployeeId = 2, UserName = "u2", SectionName = "s2" }
            };
            var queryUserShifts = new List<QueryUserShift>
            {
                new QueryUserShift { EmployeeId = 1, ShiftId = 1, ShiftDate = DateTime.Parse("2019-05-20") },
                new QueryUserShift { EmployeeId = 2, ShiftId = 2, ShiftDate = DateTime.Parse("2019-05-20") }
            };
            var queryUserMatches = new List<QueryUserMatch>
            {
                new QueryUserMatch { OWPMatchId = 1, Bettype = 1, UserId = 1 }
            };
            var queryMatchInfos = new List<QueryMatchInfo>
            {
                new QueryMatchInfo { OWPMatchId = 1, HomeName = "h1", AwayName = "h2", LeagueName = "l3" }
            };
            var queryEmployeeWorkingShiftNote = new List<EmployeeWorkingShiftNote>
            {
                new EmployeeWorkingShiftNote{ UserId=1, Note="note", ShiftDate=mockDate, LastUpdated=mockDate, Username="sa", EmployeeWorkingShiftNoteId=1}
            };

            var queryResult = new IEnumerable<object>[] { queryUsers, queryUserShifts, queryUserMatches, queryMatchInfos, queryEmployeeWorkingShiftNote };
            dynamicRepository.FetchMultipleAsync(Arg.Compat.Any<QueryCriteria>(), Arg.Compat.Any<Type[]>()).Returns(Task.FromResult(queryResult));

            var queryAssignments = new List<Assignment>
            {
                new Assignment { AssignmentId = 1, OWPMatchId = 1, Bettype = 1, UserId = 1 }
            };
            var queryOWPMatches = new List<OWPMatch>
            {
                new OWPMatch { OWPMatchId = 1, EventDateTime = new DateTime(2020,06,11,23,31,00), IsMinorMatch = false, MinorBettypeId = null, ParentMatchId =1 }
            };

            var assignmentQueryResult = new IEnumerable<object>[] { queryAssignments, queryOWPMatches };
            dynamicRepository.FetchMultipleAsync(Arg.Compat.Any<QueryAssignmentStatsCriteria>(), Arg.Compat.Any<Type[]>()).Returns(Task.FromResult(assignmentQueryResult));

            var stats = new AssignmentStatistic
            {
                LAAssigned = new List<int> { 0 },
                MatchAssigned = new List<int> { 1 },
                MatchSelected = new List<int> { 1 },
                TraderAssigned = new List<int> { 1 }
            };

            for (var i = 1; i < 24; i++)
            {
                stats.LAAssigned.Add(0);
                stats.MatchAssigned.Add(0);
                stats.MatchSelected.Add(0);
                stats.TraderAssigned.Add(0);
            }

            // Act
            var queryModel = new QueryModel
            {
                Date = DateTime.Now,
                SectionId = "1,2",
                LocationCode = "CTI",
                ShiftId = "1,2,3",
                User = "starix",
                ExcludeClosedAccount = true,
                Permission = new DailyWorkingShiftPermission
                {
                    DailyWorkingShiftAllSections = new PermissionModel(true, false),
                    FullTimeView = new PermissionModel(false, false)
                }
            };

            var outputResult = await repository.Query(queryModel);

            // Assert
            var expectedUserResult = new List<QueryUserGroup>
            {
                new QueryUserGroup
                {
                    UserId = 1,
                    SectionName = "s1",
                    UserName = "u1",
                    ShiftId = 0,
                    Shifts = new List<QueryUserShift>
                    {
                        new QueryUserShift { EmployeeId = 1, ShiftId = 1, ShiftDate = DateTime.Parse("2019-05-20") }
                    },
                    Matches = new List<QueryUserMatchGroup>
                    {
                        new QueryUserMatchGroup { BetTypes = new List<int> { 1 }, OWPMatchId = 1 }
                    }
                },
                new QueryUserGroup
                {
                    UserId = 2,
                    SectionName = "s2",
                    UserName = "u2",
                    ShiftId = 2,
                    Shifts = new List<QueryUserShift>
                    {
                        new QueryUserShift { EmployeeId = 2, ShiftId = 2, ShiftDate = DateTime.Parse("2019-05-20") }
                    }
                }
            };
            var expectedMatchResult = new List<QueryMatchInfo>
            {
                new QueryMatchInfo { OWPMatchId = 1, HomeName = "h1", AwayName = "h2", LeagueName = "l3" }
            };
            var expectedResult = new QueryResult
            {
                Users = expectedUserResult,
                Matches = expectedMatchResult,
                AssignmentStatistics = stats
            };

            Equals(expectedResult, outputResult);
        }
    }
}