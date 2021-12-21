namespace OWP.PreMatchAssignment.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Criteria;
    using Fanex.Data.Repository;
    using Models;
    using NSubstitute;
    using PreMatchAssignment.Misc;
    using PreMatchAssignment.Services;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class QueryLogServiceUnitTests
    {
        [Fact]
        public async Task Query_ShouldReturnData()
        {
            //Arrange
            var dynamicRepo = Substitute.For<IDynamicRepository>();
            var logFactory = Substitute.For<LogFactory>();
            var logService = new QueryLogServiceImp(dynamicRepo, logFactory);
            dynamicRepo.FetchMultipleAsync<Log, bool>(Arg.Compat.Any<QueryLogCriteria>()).Returns(Task.FromResult(MockLogData()));
            logFactory.BuildColumn(LogActionConst.UpdateAssignment, "<r><z plid=\"2047\" uid=\"8\" un=\"sa\" bt=\"1\" ito=\"1\" at=\"1\" /></r>").Returns("HDP T.Over: #sa");
            logFactory.BuildColumn(LogActionConst.UpdateAssignment, string.Empty).Returns(string.Empty);
            logFactory.BuildColumn(LogActionConst.UpdateRemark, "<r><z plid=\"2019\" rm=\"\"/></r>").Returns(string.Empty);
            logFactory.BuildColumn(LogActionConst.UpdateRemark, "<r><z plid=\"2019\" rm=\"cdcd\"/></r>").Returns("cdcd");
            logFactory.BuildColumn(LogActionConst.ChangeStartTime, "<r><z d=\"2020-08-20\" t=\"14:20\"/></r>").Returns("Date: 08/20/2020, Start Time: 02:20 PM");

            logFactory.GetLogType(LogActionConst.ChangeStartTime, "<r><z d=\"2020-08-20\" t=\"14:20\"/></r>").Returns(LogActionConst.ChangeStartTime);
            logFactory.GetLogType(LogActionConst.UpdateAssignment, "<r><z plid=\"2047\" uid=\"8\" un=\"sa\" bt=\"1\" ito=\"1\" at=\"1\" /></r>").Returns(LogActionConst.UpdateAssignment);
            logFactory.GetLogType(LogActionConst.UpdateRemark, "<r><z plid=\"2019\" rm=\"\"/></r>").Returns("Update Remark");
            logFactory.GetLogType("Delete League", "<r><z plid=\"2007\"/></r>").Returns("Delete League");
            logFactory.GetLogType("Recover League", "<r><z plid=\"2007\"/></r>").Returns("Recover League");

            var expected = CreateExpectedData();

            //Act
            var actual = await logService.Query(new Handlers.QueryLogRequest { PageNumber = 1, PageSize = 10 });

            //Assert
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public async Task Query_ShouldReturnEmptyData()
        {
            //Arrange
            var dynamicRepo = Substitute.For<IDynamicRepository>();
            var logFactory = Substitute.For<LogFactory>();
            var logService = new QueryLogServiceImp(dynamicRepo, logFactory);
            var logData = new ValueTuple<IEnumerable<Log>, IEnumerable<bool>> { Item1 = new List<Log>(), Item2 = new List<bool> { false } };
            dynamicRepo.FetchMultipleAsync<Log, bool>(Arg.Compat.Any<QueryLogCriteria>()).Returns(Task.FromResult(logData));
            var expected = new QueryLogViewModel { IsLastPage = false, Logs = logData.Item1 };

            //Act
            var actual = await logService.Query(new Handlers.QueryLogRequest { PageNumber = 1, PageSize = 10 });

            //Assert
            DeepAssert.Equal(expected, actual);
        }

        private ValueTuple<IEnumerable<Log>, IEnumerable<bool>> MockLogData()
        {
            var logs = new List<Log>
            {
                new Log
                {
                    Action = LogActionConst.UpdateRemark,
                    LastModified = new DateTime(2020,8,20,4,3,50),
                    LeagueName = "BRAZIL CAMPEONATO POTIGUAR",
                    ModifiedBy = "sa",
                    NewValue = "<r><z plid=\"2019\" rm=\"\"/></r>",
                    OldValue = "<r><z plid=\"2019\" rm=\"cdcd\"/></r>"
                },

                new Log
                {
                    Action = "Delete League",
                    LastModified = new DateTime(2020,8,20,4,3,50),
                    LeagueName = "CLUB FRIENDLY",
                    ModifiedBy = "sa",
                    NewValue = string.Empty,
                    OldValue = "<r><z plid=\"2007\"/></r>"
                },

                new Log
                {
                    Action = LogActionConst.UpdateAssignment,
                    LastModified = new DateTime(2020,8,20,4,3,50),
                    LeagueName = "ICELAND U19 LEAGUE",
                    ModifiedBy = "sa",
                    NewValue = "<r><z plid=\"2047\" uid=\"8\" un=\"sa\" bt=\"1\" ito=\"1\" at=\"1\" /></r>",
                    OldValue = string.Empty
                },

                new Log
                {
                    Action = "Recover League",
                    LastModified = new DateTime(2020,8,20,4,3,50),
                    LeagueName = "CLUB FRIENDLY",
                    ModifiedBy = "sa",
                    NewValue = "<r><z plid=\"2007\"/></r>",
                    OldValue = string.Empty
                },

                new Log
                {
                    Action = LogActionConst.ChangeStartTime,
                    LastModified = new DateTime(2020,8,20,4,3,50),
                    LeagueName = "CLUB FRIENDLY",
                    ModifiedBy = "sa",
                    NewValue = "<r><z d=\"2020-08-20\" t=\"14:20\"/></r>",
                    OldValue = string.Empty
                }
            };

            var isLastPage = new List<bool> { true };

            return new ValueTuple<IEnumerable<Log>, IEnumerable<bool>> { Item1 = logs, Item2 = isLastPage };
        }

        private QueryLogViewModel CreateExpectedData()
        {
            var logs = new List<Log>
            {
                new Log
                {
                    Action = LogActionConst.UpdateRemark,
                    LastModified = new DateTime(2020,8,20,4,3,50),
                    LeagueName = "BRAZIL CAMPEONATO POTIGUAR",
                    ModifiedBy = "sa",
                    NewValue = string.Empty,
                    OldValue = "cdcd",
                    LastModifiedInString = "08/20/2020 04:03 PM"
                },

                new Log
                {
                    Action = "Delete League",
                    LastModified = new DateTime(2020,8,20,4,3,50),
                    LeagueName = "CLUB FRIENDLY",
                    ModifiedBy = "sa",
                    NewValue = string.Empty,
                    OldValue = string.Empty,
                    LastModifiedInString = "08/20/2020 04:03 PM"
                },

                new Log
                {
                    Action = LogActionConst.UpdateAssignment,
                    LastModified = new DateTime(2020,8,20,4,3,50),
                    LeagueName = "ICELAND U19 LEAGUE",
                    ModifiedBy = "sa",
                    NewValue = "HDP T.Over: #sa",
                    OldValue = string.Empty,
                    LastModifiedInString = "08/20/2020 04:03 PM"
                },

                new Log
                {
                    Action = "Recover League",
                    LastModified = new DateTime(2020,8,20,4,3,50),
                    LeagueName = "CLUB FRIENDLY",
                    ModifiedBy = "sa",
                    NewValue = string.Empty,
                    OldValue = string.Empty,
                    LastModifiedInString = "08/20/2020 04:03 PM"
                },

                new Log
                {
                    Action = LogActionConst.ChangeStartTime,
                    LastModified = new DateTime(2020,8,20,4,3,50),
                    LeagueName = "CLUB FRIENDLY",
                    ModifiedBy = "sa",
                    NewValue = "Date: 08/20/2020, Start Time: 02:20 PM",
                    OldValue = string.Empty,
                    LastModifiedInString = "08/20/2020 04:03 PM"
                }
            };

            return new QueryLogViewModel { Logs = logs, IsLastPage = true };
        }
    }
}