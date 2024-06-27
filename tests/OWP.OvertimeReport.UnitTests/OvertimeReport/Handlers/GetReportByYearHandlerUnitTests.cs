using AutoMapper;
using NSubstitute;
using OWP.Report.OvertimeReport.Handlers;
using OWP.Report.OvertimeReport.Models;
using OWP.Report.OvertimeReport.Services;
using Xunit;

namespace OWP.Report.UnitTests.OvertimeReport.Handlers;

using Xunit.Asserts.Compare;

public class GetReportByYearHandlerUnitTests
{
    [Fact]
    public async Task Handle_WithCorrectRequestModel_ReturnData()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<QueryReportByYearRequest, OverTimeRequest>();
        });
        var iMapper = config.CreateMapper();
        var commandRepository = Substitute.For<CommandRepository>();

        commandRepository.GetReportByYear(Arg.Any<OverTimeRequest>())
            .Returns((MockUsers(), MockData()));
        var services = new GetReportByYearHandler(commandRepository, iMapper);

        var actual = await services.Handle(new QueryReportByYearRequest { Location = string.Empty, Trader = string.Empty, Year = 2021 }, new CancellationToken(false));

        DeepAssert.Equal(MockExpected(), actual);
    }

    private IEnumerable<User> MockUsers()
        => new List<User>
        {
            new()
            {
                UserId = 1,
                Username = "sa",
                Location = "SMS"
            },
            new()
            {
                UserId = 2,
                Username = "sa2",
                Location = "SMS"
            },
            new()
            {
                UserId = 3,
                Username = "sa3",
                Location = "SMS"
            },
            new()
            {
                UserId = 4,
                Username = "sa5",
                Location = "Phil"
            }
        };

    private IEnumerable<OvertimeData> MockData()
        => new List<OvertimeData>
        {
            new()
            {
                UserId = 1,
                Hour = 2,
                IsOffDay = true,
                ReportDate = new DateTime(2021, 10, 2)
            },
            new()
            {
                UserId = 1,
                Hour = 10,
                IsOffDay = false,
                ReportDate = new DateTime(2021, 10, 10)
            },
            new()
            {
                UserId = 1,
                Hour = 1,
                IsOffDay = true,
                ReportDate = new DateTime(2021, 11, 20)
            },
            new()
            {
                UserId = 1,
                Hour = 1,
                IsOffDay = true,
                ReportDate = new DateTime(2021, 12, 20)
            },
            new()
            {
                UserId = 3,
                Hour = 1,
                IsOffDay = true,
                ReportDate = new DateTime(2021, 12, 20)
            },
            new()
            {
                UserId = 3,
                Hour = 1,
                IsOffDay = true,
                ReportDate = new DateTime(2021, 12, 21)
            }
        };

    private IEnumerable<OvertimeByYear> MockExpected()
        => new List<OvertimeByYear>
        {
            new()
            {
                Trader = "sa5",
                Location = "Phil",
                OvertimeData = [null, null, null, null, null, null, null, null, null, null, null, null],
                Total = null
            },
            new()
            {
                Trader = "sa",
                Location = "SMS",
                OvertimeData = [null, null, null, null, null, null, null, null, null, 12, 1, 1],
                Total = 14
            },
            new()
            {
                Trader = "sa2",
                Location = "SMS",
                OvertimeData = [null, null, null, null, null, null, null, null, null, null, null, null],
                Total = null
            },
            new()
            {
                Trader = "sa3",
                Location = "SMS",
                OvertimeData = [null, null, null, null, null, null, null, null, null, null, null, 2],
                Total = 2
            }
        };
}