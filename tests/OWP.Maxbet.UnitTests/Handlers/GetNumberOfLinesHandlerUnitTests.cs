namespace OWP.Maxbet.UnitTests.Handlers;

using Criteria;
using Enums;
using Fanex.Data.Repository;
using Maxbet.Handlers;
using Models;
using NSubstitute;
using Xunit;

public class GetNumberOfLinesHandlerUnitTests
{
    private IDynamicRepository dynamicRepository;
    private GetNumberOfLinesHandler.Handler handler;

    [Fact]
    public async Task Handler_MatchWithTwoPlusFourLinesNoOfHDP_ReturnData()
    {
        Setup();
        IEnumerable<MatchResource> noOfHdp = new List<MatchResource> { new() { ResourceId = (int)EnumNoOfHdp.TwoPlusFourLines } };
        dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchNoOfHdpGetCriteria>()).Returns(Task.FromResult(noOfHdp));
        var expected = new NumberOfLines { CreditFt = 2, CreditFirstHalf = 2, LicenseeFt = 4, LicenseeFirstHalf = 3 };

        var actual = await handler.Handle(new GetNumberOfLinesHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

        Equals(expected, actual);
    }

    [Fact]
    public async Task Handler_MatchWithThreePlusThreeLinesNoOfHDP_ReturnData()
    {
        Setup();
        IEnumerable<MatchResource> noOfHdp = new List<MatchResource> { new() { ResourceId = (int)EnumNoOfHdp.ThreePlusThreeLines } };
        dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchNoOfHdpGetCriteria>()).Returns(Task.FromResult(noOfHdp));
        var expected = new NumberOfLines { CreditFt = 3, CreditFirstHalf = 2, LicenseeFt = 3, LicenseeFirstHalf = 2 };

        var actual = await handler.Handle(new GetNumberOfLinesHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

        Equals(expected, actual);
    }

    [Fact]
    public async Task Handler_MatchWithThreePlusFourLinesNoOfHDP_ReturnData()
    {
        Setup();
        IEnumerable<MatchResource> noOfHdp = new List<MatchResource> { new() { ResourceId = (int)EnumNoOfHdp.ThreePlusFourLines } };
        dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchNoOfHdpGetCriteria>()).Returns(Task.FromResult(noOfHdp));
        var expected = new NumberOfLines { CreditFt = 3, CreditFirstHalf = 2, LicenseeFt = 4, LicenseeFirstHalf = 3 };

        var actual = await handler.Handle(new GetNumberOfLinesHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

        Equals(expected, actual);
    }

    [Fact]
    public async Task Handler_MatchWithFourPlusFourLinesNoOfHDP_ReturnData()
    {
        Setup();
        IEnumerable<MatchResource> noOfHdp = new List<MatchResource> { new() { ResourceId = (int)EnumNoOfHdp.FourPlusFourLines } };
        dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchNoOfHdpGetCriteria>()).Returns(Task.FromResult(noOfHdp));
        var expected = new NumberOfLines { CreditFt = 4, CreditFirstHalf = 3, LicenseeFt = 4, LicenseeFirstHalf = 3 };

        var actual = await handler.Handle(new GetNumberOfLinesHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

        Equals(expected, actual);
    }

    [Fact]
    public async Task Handler_MatchWithNoNoOfHDP_ReturnData()
    {
        Setup();
        IEnumerable<MatchResource> noOfHdp = new List<MatchResource>();
        dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchNoOfHdpGetCriteria>()).Returns(Task.FromResult(noOfHdp));
        var expected = new NumberOfLines { CreditFt = 2, CreditFirstHalf = 2, LicenseeFt = 3, LicenseeFirstHalf = 2 };

        var actual = await handler.Handle(new GetNumberOfLinesHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

        Equals(expected, actual);
    }

    [Fact]
    public async Task Handler_MatchWithFirstHDPNoOfHDP_ReturnData()
    {
        Setup();
        IEnumerable<MatchResource> noOfHdp = new List<MatchResource> { new() { ResourceId = (int)EnumNoOfHdp.FirstHDP } };
        dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchNoOfHdpGetCriteria>()).Returns(Task.FromResult(noOfHdp));
        var expected = new NumberOfLines();

        var actual = await handler.Handle(new GetNumberOfLinesHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

        Equals(expected, actual);
    }

    [Fact]
    public async Task Handler_MatchWithOneLineNoOfHDP_ReturnData()
    {
        Setup();
        IEnumerable<MatchResource> oneLine = new List<MatchResource> { new() { ResourceId = (int)EnumNoOfHdp.OneLine } };
        dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchNoOfHdpGetCriteria>()).Returns(Task.FromResult(oneLine));
        var expected = new NumberOfLines { CreditFt = 1, CreditFirstHalf = 1, LicenseeFt = 0, LicenseeFirstHalf = 0 };

        var actual = await handler.Handle(new GetNumberOfLinesHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

        Equals(expected, actual);
    }

    [Fact]
    public async Task Handler_MatchWithTwoLinesNoOfHDP_ReturnData()
    {
        Setup();
        IEnumerable<MatchResource> twoLines = new List<MatchResource> { new() { ResourceId = (int)EnumNoOfHdp.TwoLines } };
        dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchNoOfHdpGetCriteria>()).Returns(Task.FromResult(twoLines));
        var expected = new NumberOfLines { CreditFt = 2, CreditFirstHalf = 2, LicenseeFt = 0, LicenseeFirstHalf = 0 };

        var actual = await handler.Handle(new GetNumberOfLinesHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

        Equals(expected, actual);
    }

    private void Setup()
    {
        dynamicRepository = Substitute.For<IDynamicRepository>();
        handler = new GetNumberOfLinesHandler.Handler(dynamicRepository);
    }
}