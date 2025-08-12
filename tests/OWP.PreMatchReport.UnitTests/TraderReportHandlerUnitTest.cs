using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NSubstitute;
using OWP.Constants;
using OWP.Report.Models;
using OWP.Report.TraderReport.Handlers;
using OWP.Report.TraderReport.Models;
using OWP.Report.TraderReport.Services;
using Xunit;

namespace OWP.PreMatchReport.UnitTests;

public class TraderReportHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldGetReportData_AndAssignTraderPerformance_AndReturnOrderedResults()
    {
        // Arrange
        var repositoryService = Substitute.For<RepositoryService>();
        var tGRSReportService = Substitute.For<TGRSReportService>();
        var handler = new Handler(repositoryService, tGRSReportService);

        var request = new ReportRequest
        {
            FromDate = System.DateTime.Now.AddDays(-7),
            ToDate = System.DateTime.Now
        };

        var dataReport = new List<ReportModel>
        {
            new() { UserId = 1, SectionName = "Section A", Username = "user1" },
            new() { UserId = 2, SectionName = "Section B", Username = "user2" },
            new() { UserId = 1, SectionName = "Section A", Username = "user1" }
        };

        var traderPerformanceDetails = new List<TraderPerformanceDetail>
        {
            new() { TraderId = 1, BetType = Bettype.Handicap, Turnover = 1000, WinLoss = 100 },
            new() { TraderId = 1, BetType = Bettype.OverUnder, Turnover = 2000, WinLoss = 200 },
            new() { TraderId = 2, BetType = Bettype.FirstHalfHandicap, Turnover = 1500, WinLoss = 150 }
        };

        repositoryService.GetLstReportDataByDate(request).Returns(dataReport);
        tGRSReportService.GetTraderPerformanceDetails(Arg.Any<List<int>>(), request).Returns(traderPerformanceDetails);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repositoryService.Received(1).GetLstReportDataByDate(request);
        await tGRSReportService.Received(1).GetTraderPerformanceDetails(
            Arg.Is<List<int>>(ids => ids.Count == 2 && ids.Contains(1) && ids.Contains(2)),
            request);

        var resultList = result.ToList();
        Assert.Equal(3, resultList.Count);

        // Check that results are ordered by SectionName then Username
        Assert.Equal("Section A", resultList[0].SectionName);
        Assert.Equal("Section A", resultList[1].SectionName);
        Assert.Equal("Section B", resultList[2].SectionName);

        // Check that trader performance is assigned
        var user1Reports = resultList.Where(r => r.UserId == 1).ToList();
        Assert.Equal(1000, user1Reports[0].TurnOver_FT_HDP);
        Assert.Equal(2000, user1Reports[0].TurnOver_FT_OU);
        Assert.Equal(100, user1Reports[0].WinLoss_FT_HDP);
        Assert.Equal(200, user1Reports[0].Winloss_FT_OU);
    }

    [Fact]
    public async Task Handle_WithEmptyData_ShouldReturnEmptyList()
    {
        // Arrange
        var repositoryService = Substitute.For<RepositoryService>();
        var tGRSReportService = Substitute.For<TGRSReportService>();
        var handler = new Handler(repositoryService, tGRSReportService);

        var request = new ReportRequest
        {
            FromDate = System.DateTime.Now.AddDays(-7),
            ToDate = System.DateTime.Now
        };

        repositoryService.GetLstReportDataByDate(request).Returns(new List<ReportModel>());
        tGRSReportService.GetTraderPerformanceDetails(Arg.Any<List<int>>(), request).Returns(new List<TraderPerformanceDetail>());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repositoryService.Received(1).GetLstReportDataByDate(request);
        await tGRSReportService.Received(1).GetTraderPerformanceDetails(Arg.Is<List<int>>(ids => ids.Count == 0), request);

        Assert.Empty(result);
    }
}