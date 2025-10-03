using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using OWP.Api.Maxbet;
using OWP.Api.Maxbet.Models;
using OWP.Api.Maxbet.Services;
using Olympus.OWP.Constants;

namespace OWP.Api.UnitTests.Maxbet;

public class MaxbetControllerUnitTests
{
    private readonly MainMatchService _mainMatchService;
    private readonly SpecialProductService _specialProductService;
    private readonly ETPKMatchService _etpkMatchService;
    private readonly MaxbetController _controller;

    public MaxbetControllerUnitTests()
    {
        _mainMatchService = Substitute.For<MainMatchService>();
        _specialProductService = Substitute.For<SpecialProductService>();
        _etpkMatchService = Substitute.For<ETPKMatchService>();
        _controller = new MaxbetController(_mainMatchService, _specialProductService, _etpkMatchService);
    }

    [Fact]
    public async Task GetMaxbet_WithValidMatchId_ReturnsOkWithMaxbetInfo()
    {
        // Arrange
        var matchId = 123;
        var owpMatchId = 456;
        var expectedMaxbetInfo = new MaxbetInfo
        {
            AdminsiteID = matchId,
            SecondAcceptValue = 1000,
            Spread = new MaxbetSpread(),
            Maxbet = new MaxbetSpread(),
            NumberOfLines = new NumberOfLines()
        };

        _mainMatchService.GetMatchIdByAdminSiteId(matchId).Returns(owpMatchId);
        _mainMatchService.GetMaxbetInfo(owpMatchId, matchId).Returns(expectedMaxbetInfo);

        // Act
        var result = await _controller.GetMaxbet(matchId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMaxbetInfo = Assert.IsType<MaxbetInfo>(okResult.Value);

        Assert.Equal(expectedMaxbetInfo.AdminsiteID, returnedMaxbetInfo.AdminsiteID);
        Assert.Equal(expectedMaxbetInfo.SecondAcceptValue, returnedMaxbetInfo.SecondAcceptValue);

        await _mainMatchService.Received(1).GetMatchIdByAdminSiteId(matchId);
        await _mainMatchService.Received(1).GetMaxbetInfo(owpMatchId, matchId);
    }

    [Fact]
    public async Task GetMaxbet_WithInvalidMatchId_ReturnsNotFound()
    {
        // Arrange
        var matchId = 999;
        _mainMatchService.GetMatchIdByAdminSiteId(matchId).Returns(0);

        // Act
        var result = await _controller.GetMaxbet(matchId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        await _mainMatchService.Received(1).GetMatchIdByAdminSiteId(matchId);
        await _mainMatchService.DidNotReceive().GetMaxbetInfo(Arg.Any<int>(), Arg.Any<int>());
    }

    [Fact]
    public async Task GetSpecialProduct_WithETPKSubType_ReturnsETPKMaxbetInfo()
    {
        // Arrange
        var matchId = 123;
        var subType = 100; // MainMatchET - ETPK subtype
        var expectedMaxbetInfo = new MaxbetInfo
        {
            AdminsiteID = matchId,
            SecondAcceptValue = 2000
        };

        _etpkMatchService.GetMaxBetETPK(matchId, subType).Returns(expectedMaxbetInfo);

        // Act
        var result = await _controller.GetSpecialProduct(matchId, subType);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMaxbetInfo = Assert.IsType<MaxbetInfo>(okResult.Value);

        Assert.Equal(expectedMaxbetInfo.AdminsiteID, returnedMaxbetInfo.AdminsiteID);
        Assert.Equal(expectedMaxbetInfo.SecondAcceptValue, returnedMaxbetInfo.SecondAcceptValue);

        await _etpkMatchService.Received(1).GetMaxBetETPK(matchId, subType);
    }

    [Fact]
    public async Task GetSpecialProduct_WithETPKSubType_WhenETPKReturnsNull_ReturnsNotFound()
    {
        // Arrange
        var matchId = 123;
        var subType = 100; // MainMatchET - ETPK subtype

        _etpkMatchService.GetMaxBetETPK(matchId, subType).Returns((MaxbetInfo?)null);

        // Act
        var result = await _controller.GetSpecialProduct(matchId, subType);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        await _etpkMatchService.Received(1).GetMaxBetETPK(matchId, subType);
    }

    [Fact]
    public async Task GetSpecialProduct_WithNonETPKSubType_ReturnsSpecialProductInfo()
    {
        // Arrange
        var matchId = 123;
        var subType = 5; // Non-ETPK subtype
        var bettypeId = MinorBettype.GetMinorBettypeIdBySubtype(subType);
        var owpMatch = new MinorMatchInfo
        {
            MinorMatchId = 789,
            ParentMatchId = 456
        };
        var expectedMaxbetInfo = new MaxbetInfo
        {
            AdminsiteID = matchId,
            SecondAcceptValue = 1500
        };

        _specialProductService.GetMatchIdByAdminSiteId(matchId, bettypeId).Returns(owpMatch);
        _specialProductService.GetSpecialProductInfo(owpMatch, matchId, bettypeId).Returns(expectedMaxbetInfo);

        // Act
        var result = await _controller.GetSpecialProduct(matchId, subType);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMaxbetInfo = Assert.IsType<MaxbetInfo>(okResult.Value);

        Assert.Equal(expectedMaxbetInfo.AdminsiteID, returnedMaxbetInfo.AdminsiteID);
        Assert.Equal(expectedMaxbetInfo.SecondAcceptValue, returnedMaxbetInfo.SecondAcceptValue);

        await _specialProductService.Received(1).GetMatchIdByAdminSiteId(matchId, bettypeId);
        await _specialProductService.Received(1).GetSpecialProductInfo(owpMatch, matchId, bettypeId);
    }

    [Fact]
    public async Task GetSpecialProduct_WithNonETPKSubType_WhenMatchNotFound_ReturnsNotFound()
    {
        // Arrange
        var matchId = 123;
        var subType = 5; // Non-ETPK subtype
        var bettypeId = MinorBettype.GetMinorBettypeIdBySubtype(subType);

        _specialProductService.GetMatchIdByAdminSiteId(matchId, bettypeId).Returns((MinorMatchInfo?)null);

        // Act
        var result = await _controller.GetSpecialProduct(matchId, subType);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        await _specialProductService.Received(1).GetMatchIdByAdminSiteId(matchId, bettypeId);
        await _specialProductService.DidNotReceive().GetSpecialProductInfo(Arg.Any<MinorMatchInfo>(), Arg.Any<int>(), Arg.Any<int>());
    }

    [Theory]
    [InlineData(100, true)]  // MainMatchET - ETPK subtype
    [InlineData(200, true)]  // MainMatchPK - ETPK subtype
    [InlineData(5, false)]   // Non-ETPK subtype
    [InlineData(10, false)]  // Non-ETPK subtype
    public async Task GetSpecialProduct_WithDifferentSubTypes_CallsCorrectService(int subType, bool isETPK)
    {
        // Arrange
        var matchId = 123;
        var bettypeId = MinorBettype.GetMinorBettypeIdBySubtype(subType);
        var owpMatch = new MinorMatchInfo { MinorMatchId = 789, ParentMatchId = 456 };
        var expectedMaxbetInfo = new MaxbetInfo { AdminsiteID = matchId };

        if (isETPK)
        {
            _etpkMatchService.GetMaxBetETPK(matchId, subType).Returns(expectedMaxbetInfo);
        }
        else
        {
            _specialProductService.GetMatchIdByAdminSiteId(matchId, bettypeId).Returns(owpMatch);
            _specialProductService.GetSpecialProductInfo(owpMatch, matchId, bettypeId).Returns(expectedMaxbetInfo);
        }

        // Act
        var result = await _controller.GetSpecialProduct(matchId, subType);

        // Assert
        Assert.IsType<OkObjectResult>(result);

        if (isETPK)
        {
            await _etpkMatchService.Received(1).GetMaxBetETPK(matchId, subType);
            await _specialProductService.DidNotReceive().GetMatchIdByAdminSiteId(Arg.Any<int>(), Arg.Any<int>());
        }
        else
        {
            await _specialProductService.Received(1).GetMatchIdByAdminSiteId(matchId, bettypeId);
            await _etpkMatchService.DidNotReceive().GetMaxBetETPK(Arg.Any<int>(), Arg.Any<int>());
        }
    }
}