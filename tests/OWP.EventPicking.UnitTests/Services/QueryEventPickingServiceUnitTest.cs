using Fanex.NAP.Client.Models.ValueObject;
using NSubstitute;
using OWP.Constants;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Middleware.Models;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using OWP.Models;
using OWP.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Services;

public class QueryEventPickingServiceImpTests
{
    private readonly TGRSService _mockTGRSService;
    private readonly BuildMainAndChildMatches _mockBuildMainAndChildMatches;
    private readonly PermissionService _mockPermissionService;
    private readonly QueryEventPickingServiceImp _service;

    public QueryEventPickingServiceImpTests()
    {
        _mockTGRSService = Substitute.For<TGRSService>();
        _mockBuildMainAndChildMatches = Substitute.For<BuildMainAndChildMatches>();
        _mockPermissionService = Substitute.For<PermissionService>();
        _service = new QueryEventPickingServiceImp(_mockTGRSService, _mockBuildMainAndChildMatches, _mockPermissionService);
    }

    [Fact]
    public async Task Execute_ShouldReturnExpectedResult_WhenConditionsAreMet()
    {
        // Arrange
        var request = new EventPickingRequest
        {
            Date = DateTime.Now,
            SelectGame = [1],
            FilterMatch = ["Red"],
            HasProvider = ["yes"],
            HasOtherProvider = ["yes"],
            FilterMatchCategory = [1],
            UpdateType = ["1"],
            Period = new Period
            {
                OneMonth = new TimeRange { FromDate = DateTime.Now.AddMonths(-1), ToDate = DateTime.Now },
                ThreeMonths = new TimeRange { FromDate = DateTime.Now.AddMonths(-3), ToDate = DateTime.Now },
                SixMonths = new TimeRange { FromDate = DateTime.Now.AddMonths(-6), ToDate = DateTime.Now },
                TwelveMonths = new TimeRange { FromDate = DateTime.Now.AddMonths(-12), ToDate = DateTime.Now }
            }
        };

        var matches = new List<EventPickingModel>
        {
            new()
            {
                LeagueName = "TestLeague SABA ",
                OWPLeagueId = 1,
                IsNoOffer = true,
                RowColor = "Red",
                Provider = "yes",
                OtherProvider = "yes",
                RAndDInfo = [new() { UpdateTypeId = 1, Group = "1"}]
            }
        };

        _mockBuildMainAndChildMatches.GetMainEvent(request.Date).Returns(matches);
        _mockBuildMainAndChildMatches.GetChildEventEventPicking(request.Date).Returns(new List<EventPickingModel>());

        var permissions = new PermissionModel(true, false);
        var functionPolicy = new List<FunctionPolicy>();
        _mockPermissionService.GetPermissions(FunctionKeys.EventPickingNew).Returns(functionPolicy);
        _mockPermissionService.GetPermissionModel(Arg.Any<string>()).Returns(permissions);

        _mockTGRSService.GetTurnOverWinLoss(Arg.Any<GetTurnOverWinLossCriteria>())
            .Returns(Task.FromResult<(IList<TurnOverWinLossModel>, bool)>(
                (new List<TurnOverWinLossModel>
                {
                    new() { LeagueName = "TestLeague", Turnover = 1000, WinLoss = 500 }
                }, true)));

        // Act
        var result = await _service.Execute(request,new List<short>() );

        // Assert
        Assert.NotNull(result);
        Assert.Equal(matches, result.FilteredMatches);
    }

    [Fact]
    public async Task CalculateTurnOver_Margin_ShouldCalculateAndInitializeMarginInfo()
    {
        // Arrange
        var request = new EventPickingRequest
        {
            Date = DateTime.Now,
            SelectGame = [1],
            FilterMatch = ["Red"],
            HasProvider = ["yes"],
            HasOtherProvider = ["yes"],
            FilterMatchCategory = [1],
            UpdateType = ["1"],
            Period = new Period
            {
                OneMonth = new TimeRange { FromDate = DateTime.Now.AddMonths(-1), ToDate = DateTime.Now },
                ThreeMonths = new TimeRange { FromDate = DateTime.Now.AddMonths(-3), ToDate = DateTime.Now },
                SixMonths = new TimeRange { FromDate = DateTime.Now.AddMonths(-6), ToDate = DateTime.Now },
                TwelveMonths = new TimeRange { FromDate = DateTime.Now.AddMonths(-12), ToDate = DateTime.Now }
            }
        };

        var matches = new List<EventPickingModel>
            {
                new EventPickingModel
                {
                    LeagueName = "TestLeague SABA",
                    OWPLeagueId = 1,
                    IsNoOffer = false,
                    RowColor = "Red",
                    Provider = "yes",
                    OtherProvider = "yes",
                    RAndDInfo = [new RAndDInfo { UpdateTypeId = 1 }]
                }
            };

        var turnoverWinLossModels = new List<TurnOverWinLossModel>
            {
                new TurnOverWinLossModel { LeagueName = "TestLeague", Turnover = 1000, WinLoss = 500 }
            };

        _mockTGRSService.GetTurnOverWinLoss(Arg.Any<GetTurnOverWinLossCriteria>())
            .Returns(Task.FromResult<(IList<TurnOverWinLossModel>, bool)>((turnoverWinLossModels, true)));

        // Act
        await _service.CalculateTurnOver_Margin(request, matches);

        // Assert
        await _mockTGRSService.Received().GetTurnOverWinLoss(Arg.Is<GetTurnOverWinLossCriteria>(x =>
            x.FromDate == request.Period.OneMonth.FromDate &&
            x.ToDate == request.Period.OneMonth.ToDate
        ));
        await _mockTGRSService.Received().GetTurnOverWinLoss(Arg.Is<GetTurnOverWinLossCriteria>(x =>
            x.FromDate == request.Period.ThreeMonths.FromDate &&
            x.ToDate == request.Period.ThreeMonths.ToDate
        ));
        await _mockTGRSService.Received().GetTurnOverWinLoss(Arg.Is<GetTurnOverWinLossCriteria>(x =>
            x.FromDate == request.Period.SixMonths.FromDate &&
            x.ToDate == request.Period.SixMonths.ToDate
        ));
        await _mockTGRSService.Received().GetTurnOverWinLoss(Arg.Is<GetTurnOverWinLossCriteria>(x =>
            x.FromDate == request.Period.TwelveMonths.FromDate &&
            x.ToDate == request.Period.TwelveMonths.ToDate
        ));
    }


    [Theory]
    [MemberData(nameof(ValidMatchTestData))]
    public void ValidMatch_ShouldReturnExpectedResult(EventPickingModel match, EventPickingRequest condition, bool expectedResult)
    {
        // Act
        var result = QueryEventPickingServiceImp.ValidMatch(match, condition);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    public static IEnumerable<object[]> ValidMatchTestData()
    {
        // Test case 1: All conditions met
        yield return
        [
            new EventPickingModel
                {
                    IsNoOffer = true,
                    LeagueName = "SABA ",
                    RowColor = "Red",
                    Provider = "yes",
                    OtherProvider = "yes",
                    OWPLeagueId = 1,
                    RAndDInfo = [new RAndDInfo { UpdateTypeId = 1, Group = "1"}]
                },
                new EventPickingRequest
                {
                    SelectGame = [1],
                    FilterMatchCategory = [1],
                    FilterMatch = ["Red"],
                    HasProvider = ["yes"],
                    HasOtherProvider = ["yes"],

                    UpdateType = ["1"]
                },
                true
        ];

        // Test case 2: Condition 1 fails
        yield return
        [
            new EventPickingModel
                {
                    IsNoOffer = false,
                    LeagueName = "SABA ",
                    RowColor = "Red",
                    Provider = "yes",
                    OtherProvider = "yes",
                    OWPLeagueId = 1,
                    RAndDInfo = [new RAndDInfo { UpdateTypeId = 1 }]
                },
                new EventPickingRequest
                {
                    SelectGame = [1],
                    FilterMatchCategory = [1],
                    FilterMatch = ["Red"],
                    HasProvider = ["yes"],
                    HasOtherProvider = ["yes"],

                    UpdateType = ["1"]
                },
                false
        ];

        // Test case 3: Condition 2 fails
        yield return
        [
            new EventPickingModel
                {
                    IsNoOffer = true,
                    LeagueName = "SABA ",
                    RowColor = "Blue",
                    Provider = "yes",
                    OtherProvider = "yes",
                    OWPLeagueId = 1,
                    RAndDInfo = [new RAndDInfo { UpdateTypeId = 1 }]
                },
                new EventPickingRequest
                {
                    SelectGame = [1],
                    FilterMatchCategory = [1],
                    FilterMatch = ["Red"],
                    HasProvider = ["yes"],
                    HasOtherProvider = ["yes"],

                    UpdateType = ["1"]
                },
                false
        ];

        // Test case 4: Condition 3 fails
        yield return
        [
            new EventPickingModel
                {
                    IsNoOffer = true,
                    LeagueName = "SABA ",
                    RowColor = "Red",
                    Provider = "yes",
                    OtherProvider = "yes",
                    OWPLeagueId = 2,
                    RAndDInfo = [new RAndDInfo { UpdateTypeId = 1 }]
                },
                new EventPickingRequest
                {
                    SelectGame = [1],
                    FilterMatchCategory = [1],
                    FilterMatch = ["Red"],
                    HasProvider = ["yes"],
                    HasOtherProvider = ["yes"],

                    UpdateType = ["1"]
                },
                false
        ];

        // Test case 5: Condition 3, UpdateType is null in request
        yield return
        [
            new EventPickingModel
                {
                    IsNoOffer = true,
                    LeagueName = "SABA ",
                    RowColor = "Red",
                    Provider = "yes",
                    OtherProvider = "yes",
                    OWPLeagueId = 1,
                    RAndDInfo = [new RAndDInfo { Group = "1" }]
                },
                new EventPickingRequest
                {
                    SelectGame = [1],
                    FilterMatchCategory = [1],
                    FilterMatch = ["Red"],
                    HasProvider = ["yes"],
                    HasOtherProvider = ["yes"],

                    UpdateType = [null]
                },
                false
        ];

        // Test case 6: Condition 3, no RAndDInfos and UpdateType contains null
        yield return
        [
            new EventPickingModel
                {
                    IsNoOffer = true,
                    LeagueName = "SABA ",
                    RowColor = "Red",
                    Provider = "yes",
                    OtherProvider = "yes",
                    OWPLeagueId = 1,
                    RAndDInfo = []
                },
                new EventPickingRequest
                {
                    SelectGame = [1],
                    FilterMatchCategory = [1],
                    FilterMatch = ["Red"],
                    HasProvider = ["yes"],
                    HasOtherProvider = ["yes"],

                    UpdateType = [null]
                },
                true
        ];
    }
}