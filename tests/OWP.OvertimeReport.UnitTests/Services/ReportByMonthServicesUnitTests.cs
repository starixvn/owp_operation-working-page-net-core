namespace OWP.OvertimeReport.UnitTests.Services
{
    using Models;
    using NSubstitute;
    using OWP.OvertimeReport.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class ReportByMonthServicesUnitTests
    {
        private readonly CommandRepository commandRepository;
        private readonly ReportByMonthServices service;

        public ReportByMonthServicesUnitTests()
        {
            commandRepository = Substitute.For<CommandRepository>();

            service = new ReportByMonthServicesImp(commandRepository);
        }

        [Fact]
        public async Task Handle_WithCorrectRequestModel_ReturnData()
        {
            commandRepository.GetReportByMonth(Arg.Any<OverTimeRequest>())
                .Returns((MockUsers(), MockData()));
            var request = new OverTimeRequest
            {
                Location = string.Empty,
                Trader = string.Empty,
                Month = 10,
                Year = 2021
            };

            var actual = await service.GetReportByMonth(request);

            DeepAssert.Equal(MockExpectDateColumnInfos(), actual.DateColumns.ToList());
            DeepAssert.Equal(16, actual.TotalHours);
            DeepAssert.Equal(MockTraderExpected(), actual.TraderOvertime.ToList());
            DeepAssert.Equal(MockTotalHoursByDate(), actual.TotalHoursByDate.ToList());
        }

        private IEnumerable<User> MockUsers()
            => new List<User>
            {
                new User
                {
                    UserId = 1,
                    Username = "sa",
                    Location = "SMS"
                },
                new User
                {
                    UserId = 2,
                    Username = "sa2",
                    Location = "SMS"
                },
                new User
                {
                    UserId = 3,
                    Username = "sa3",
                    Location = "SMS"
                },
                new User
                {
                    UserId = 4,
                    Username = "sa5",
                    Location = "Phil"
                }
            };

        private IEnumerable<OvertimeData> MockData()
            => new List<OvertimeData>
            {
                new OvertimeData
                {
                    UserId = 1,
                    Hour = 2,
                    IsOffDay = true,
                    ReportDate = new DateTime(2021, 10, 2)
                },
                new OvertimeData
                {
                    UserId = 1,
                    Hour = 10,
                    IsOffDay = false,
                    ReportDate = new DateTime(2021, 10, 10)
                },
                new OvertimeData
                {
                    UserId = 1,
                    Hour = 1,
                    IsOffDay = true,
                    ReportDate = new DateTime(2021, 10, 20)
                },
                new OvertimeData
                {
                    UserId = 1,
                    Hour = 1,
                    IsOffDay = true,
                    ReportDate = new DateTime(2021, 10, 21)
                },
                new OvertimeData
                {
                    UserId = 3,
                    Hour = 1,
                    IsOffDay = true,
                    ReportDate = new DateTime(2021, 10, 20)
                },
                new OvertimeData
                {
                    UserId = 3,
                    Hour = 1,
                    IsOffDay = true,
                    ReportDate = new DateTime(2021, 10, 21)
                }
            };

        private IEnumerable<DateColumnInfo> MockExpectDateColumnInfos()
            => new List<DateColumnInfo>
            {
                new() { Date = "01-Oct", DayOfWeek = "Fri", IsWeekend = false },
                new() { Date = "02-Oct", DayOfWeek = "Sat", IsWeekend = true },
                new() { Date = "03-Oct", DayOfWeek = "Sun", IsWeekend = true },
                new() { Date = "04-Oct", DayOfWeek = "Mon", IsWeekend = false },
                new() { Date = "05-Oct", DayOfWeek = "Tue", IsWeekend = false },
                new() { Date = "06-Oct", DayOfWeek = "Wed", IsWeekend = false },
                new() { Date = "07-Oct", DayOfWeek = "Thu", IsWeekend = false },
                new() { Date = "08-Oct", DayOfWeek = "Fri", IsWeekend = false },
                new() { Date = "09-Oct", DayOfWeek = "Sat", IsWeekend = true },
                new() { Date = "10-Oct", DayOfWeek = "Sun", IsWeekend = true },
                new() { Date = "11-Oct", DayOfWeek = "Mon", IsWeekend = false },
                new() { Date = "12-Oct", DayOfWeek = "Tue", IsWeekend = false },
                new() { Date = "13-Oct", DayOfWeek = "Wed", IsWeekend = false },
                new() { Date = "14-Oct", DayOfWeek = "Thu", IsWeekend = false },
                new() { Date = "15-Oct", DayOfWeek = "Fri", IsWeekend = false },
                new() { Date = "16-Oct", DayOfWeek = "Sat", IsWeekend = true },
                new() { Date = "17-Oct", DayOfWeek = "Sun", IsWeekend = true },
                new() { Date = "18-Oct", DayOfWeek = "Mon", IsWeekend = false },
                new() { Date = "19-Oct", DayOfWeek = "Tue", IsWeekend = false },
                new() { Date = "20-Oct", DayOfWeek = "Wed", IsWeekend = false },
                new() { Date = "21-Oct", DayOfWeek = "Thu", IsWeekend = false },
                new() { Date = "22-Oct", DayOfWeek = "Fri", IsWeekend = false },
                new() { Date = "23-Oct", DayOfWeek = "Sat", IsWeekend = true },
                new() { Date = "24-Oct", DayOfWeek = "Sun", IsWeekend = true },
                new() { Date = "25-Oct", DayOfWeek = "Mon", IsWeekend = false },
                new() { Date = "26-Oct", DayOfWeek = "Tue", IsWeekend = false },
                new() { Date = "27-Oct", DayOfWeek = "Wed", IsWeekend = false },
                new() { Date = "28-Oct", DayOfWeek = "Thu", IsWeekend = false },
                new() { Date = "29-Oct", DayOfWeek = "Fri", IsWeekend = false },
                new() { Date = "30-Oct", DayOfWeek = "Sat", IsWeekend = true },
                new() { Date = "31-Oct", DayOfWeek = "Sun", IsWeekend = true },
            };

        private IEnumerable<TraderOvertime> MockTraderExpected()
            => new List<TraderOvertime>
            {
                new TraderOvertime
                {
                    Trader = "sa5",
                    Location = "Phil",
                    Overtime = new Dictionary<string, OverTimeInfo>(),
                    Total = 0
                },
                new TraderOvertime
                {
                    Trader = "sa",
                    Location = "SMS",
                    Overtime = new Dictionary<string, OverTimeInfo>()
                    {
                        { "02-Oct", new OverTimeInfo { IsOffDay = true, Hour = 2 } },
                        { "10-Oct", new OverTimeInfo { IsOffDay = false, Hour = 10 } },
                        { "20-Oct", new OverTimeInfo { IsOffDay = true, Hour = 1} },
                        { "21-Oct", new OverTimeInfo { IsOffDay = true, Hour = 1} }
                    },
                    Total = 14
                },
                new TraderOvertime
                {
                    Trader = "sa2",
                    Location = "SMS",
                    Overtime = new Dictionary<string, OverTimeInfo>(),
                    Total = 0
                },
                new TraderOvertime
                {
                    Trader = "sa3",
                    Location = "SMS",
                    Overtime = new Dictionary<string, OverTimeInfo>()
                    {
                        { "20-Oct", new OverTimeInfo { IsOffDay = true, Hour = 1 } },
                        { "21-Oct", new OverTimeInfo { IsOffDay = true, Hour = 1 } },
                    },
                    Total = 2
                }
            };

        private List<decimal> MockTotalHoursByDate()
            => new List<decimal> { 0, 2, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }
}
