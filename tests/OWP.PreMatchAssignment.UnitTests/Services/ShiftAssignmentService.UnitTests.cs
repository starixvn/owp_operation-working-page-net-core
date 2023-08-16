using AutoMapper;
using Fanex.NAP.Client.Helpers.Web;
using NSubstitute;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.Handlers;
using OWP.PrematchAssignment.Models;
using OWP.PrematchAssignment.Services;
using Xunit;

namespace OWP.PreMatchAssignment.UnitTests.Services;

using Xunit.Asserts.Compare;

public class ShiftAssignmentService
{
    [Fact]
    public async Task QueryDayShift_WithEmployeeFilter_ShouldReturnFilteredPreMatchData()
    {
        //Arrange
        var repository = Substitute.For<Repository>();
        var mapper = Substitute.For<IMapper>();
        repository.QueryDayShiftPreMatchAssignments(Arg.Any<QueryShiftPreMatchCriteria>()).Returns(CreateMockData());
        var mockResult = new List<LinkedMatch>
        {
            new() {InfositeMatchId = 1, OWPMatchId = 100, MatchStatus = "Live"},
            new() {InfositeMatchId = 2, OWPMatchId = 200, MatchStatus = "Live"},
            new() {InfositeMatchId = 3, OWPMatchId = 100, MatchStatus = "Cancel"}
        };
        repository.QueryLinkedOwpMatch(Arg.Any<List<int>>(), Arg.Any<DateTime>())
            .Returns(mockResult);
        var leagues = new List<LeagueAssignment>
        {
            new()
            {
                No = 1,
                PreMatchLeagueId = 1,
                LeagueId = 1,
                LeagueName = "test1",
                MatchCount = 2,
                HDP = "sa",
                OU = "sa",
                OUTakeOver = "starix14"
            },
            new()
            {
                No = 2,
                PreMatchLeagueId = 2,
                LeagueId = 2,
                LeagueName = "test2",
                MatchCount = 1,
                HDP = "starix15",
                OU = "sa",
                OUTakeOver = "starix14"
            }
        };

        var matches = new List<InfoSiteMatch>
        {
            new()
            {
                InfoSiteMatchId = 1,
                PreMatchLeagueId = 1,
                MatchStatus = "LIVE(!)"
            },
            new()
            {
                InfoSiteMatchId = 2,
                PreMatchLeagueId = 1,
                MatchStatus = "LIVE(!)"
            },
            new()
            {
                InfoSiteMatchId = 3,
                PreMatchLeagueId = 2,
                MatchStatus = "CANCEL"
            }
        };

        mapper.Map<IEnumerable<LeagueAssignment>>(Arg.Any<IEnumerable<PreMatchLeague>>()).Returns(leagues);

        var service = new ShiftAssignmentServiceImp(repository, mapper);

        //Act
        var actual = await service.QueryDayShift(new QueryDayShiftPreMatchRequest
        {
            Employee = "sa",
            EventDate = DateTime.Now,
            MatchOffers = new List<string> { "" },
            LeagueName = string.Empty
        });

        //Assert
        DeepAssert.Equal(matches, actual.InfoSiteMatches);
        DeepAssert.Equal(leagues, actual.PreMatchLeagues);
    }

    [Fact]
    public async Task QueryDayShift_WithoutFilter_ShouldReturnAllPreMatchData()
    {
        //Arrange
        var repository = Substitute.For<Repository>();
        var currentSession = Substitute.For<INapSession>();
        var mapper = Substitute.For<IMapper>();
        currentSession.UserId = 8;
        var mockResult = new List<LinkedMatch>
        {
            new() {InfositeMatchId = 1, OWPMatchId = 100},
            new() {InfositeMatchId = 2, OWPMatchId = 200},
            new() {InfositeMatchId = 3, OWPMatchId = 100}
        };
        repository.QueryLinkedOwpMatch(Arg.Any<List<int>>(), Arg.Any<DateTime>()).Returns(mockResult);
        var leagues = new List<LeagueAssignment>
        {
            new()
            {
                No = 1,
                PreMatchLeagueId = 1,
                LeagueId = 1,
                LeagueName = "test1",
                MatchCount = 2,
                HDP = "sa",
                OU = "sa",
                OUTakeOver = "starix14"
            },
            new()
            {
                No = 2,
                PreMatchLeagueId = 2,
                LeagueId = 2,
                LeagueName = "test2",
                MatchCount = 1,
                HDP = "starix15",
                OU = "sa",
                OUTakeOver = "starix14"
            },
            new()
            {
                PreMatchLeagueId = 3,
                LeagueId = 3,
                LeagueName = "test3",
                MatchCount = 1,
                OU = "NewOU"
            }
        };

        var matches = new List<InfoSiteMatch>
        {
            new()
            {
                InfoSiteMatchId = 1,
                PreMatchLeagueId = 1
            },
            new()
            {
                InfoSiteMatchId = 2,
                PreMatchLeagueId = 1
            },
            new()
            {
                InfoSiteMatchId = 3,
                PreMatchLeagueId = 2
            },
            new()
            {
                InfoSiteMatchId = 4,
                PreMatchLeagueId = 3
            }
        };
        var expected = new QueryPreMatchLeagueViewModel { PreMatchLeagues = leagues, InfoSiteMatches = matches };
        repository.QueryDayShiftPreMatchAssignments(Arg.Any<QueryShiftPreMatchCriteria>()).Returns(CreateMockData());
        mapper.Map<IEnumerable<LeagueAssignment>>(Arg.Any<IEnumerable<PreMatchLeague>>()).Returns(leagues);
        var service = new ShiftAssignmentServiceImp(repository, mapper);

        //Act
        var actual = await service.QueryDayShift(new QueryDayShiftPreMatchRequest
        {
            Employee = string.Empty,
            EventDate = DateTime.Now,
            MatchOffers = new List<string> { "" },
            LeagueName = string.Empty
        });

        //Assert
        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public async Task QueryNightShift_WithoutDBStartTime_ShouldReturnDataWithDayShiftAssignment()
    {
        //Arrange
        var repository = Substitute.For<Repository>();
        var mapper = Substitute.For<IMapper>();
        repository.QueryExistedStartTime(Arg.Any<DateTime>()).Returns(MockStartTime(false));
        repository.QueryDayShiftPreMatchAssignments(Arg.Is<QueryShiftPreMatchCriteria>(x => x.IsDay)).Returns(CreateMockData());
        repository.QueryDayShiftPreMatchAssignments(Arg.Is<QueryShiftPreMatchCriteria>(x => !x.IsDay)).Returns(CreateNightShiftMockData());
        var mockResult = new List<LinkedMatch>
        {
            new() {InfositeMatchId = 1, OWPMatchId = 100},
            new() {InfositeMatchId = 2, OWPMatchId = 200},
            new() {InfositeMatchId = 3, OWPMatchId = 100}
        };
        repository.QueryLinkedOwpMatch(Arg.Any<List<int>>(), Arg.Any<DateTime>()).Returns(mockResult);
        var leagues = new List<LeagueAssignment>
        {
            new()
            {
                No = 1,
                PreMatchLeagueId = 4,
                LeagueId = 1,
                LeagueName = "test1",
                MatchCount = 2,
                HDP = "sa",
                OU = "sa",
                OUTakeOver = "starix14"
            },
            new()
            {
                No = 2,
                PreMatchLeagueId = 5,
                LeagueId = 2,
                LeagueName = "test2",
                MatchCount = 1,
                HDP = "starix15",
                OU = "sa",
                OUTakeOver = "starix14"
            },
            new()
            {
                No = 3,
                PreMatchLeagueId = 6,
                LeagueId = 3,
                LeagueName = "test3",
                MatchCount = 1,
                OU = "NewOU"
            }
        };

        var matches = new List<InfoSiteMatch>
        {
            new()
            {
                InfoSiteMatchId = 1,
                PreMatchLeagueId = 4
            },
            new()
            {
                InfoSiteMatchId = 2,
                PreMatchLeagueId = 4
            },
            new()
            {
                InfoSiteMatchId = 3,
                PreMatchLeagueId = 5
            },
            new()
            {
                InfoSiteMatchId = 4,
                PreMatchLeagueId = 6
            }
        };

        mapper.Map<IEnumerable<LeagueAssignment>>(Arg.Any<IEnumerable<PreMatchLeague>>()).Returns(leagues);
        var expected = new QueryPreMatchLeagueViewModel { InfoSiteMatches = matches, PreMatchLeagues = leagues };
        var service = new ShiftAssignmentServiceImp(repository, mapper);

        //Act
        var actual = await service.QueryNightShift(new QueryNightShiftPreMatchRequest
        {
            Employee = string.Empty,
            EventDate = DateTime.Now,
            MatchOffers = new List<string> { "" },
            LeagueName = string.Empty
        });

        //Assert
        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public async Task QueryNightShift_WithDBStartTimeLessThanRequestStartTime_ShouldReturnDataWithNightShiftAssignment()
    {
        //Arrange
        var repository = Substitute.For<Repository>();
        var mapper = Substitute.For<IMapper>();
        repository.QueryExistedStartTime(Arg.Any<DateTime>()).Returns(MockStartTime(true));
        repository.QueryDayShiftPreMatchAssignments(Arg.Is<QueryShiftPreMatchCriteria>(x => x.IsDay)).Returns(CreateMockData());
        repository.QueryDayShiftPreMatchAssignments(Arg.Is<QueryShiftPreMatchCriteria>(x => !x.IsDay)).Returns(CreateNightShiftMockData());
        var mockResult = new List<LinkedMatch>
        {
            new() {InfositeMatchId = 1, OWPMatchId = 100},
            new() {InfositeMatchId = 2, OWPMatchId = 200},
            new() {InfositeMatchId = 3, OWPMatchId = 100}
        };
        repository.QueryLinkedOwpMatch(Arg.Any<List<int>>(), Arg.Any<DateTime>()).Returns(mockResult);

        var leagues = new List<LeagueAssignment>
        {
            new ()
            {
                No = 1,
                PreMatchLeagueId = 4,
                LeagueId = 1,
                LeagueName = "test1",
                MatchCount = 2
            },
            new ()
            {
                No = 2,
                PreMatchLeagueId = 5,
                LeagueId = 2,
                LeagueName = "test2"
            },
            new ()
            {
                No = 3,
                PreMatchLeagueId = 6,
                LeagueId = 3,
                LeagueName = "test3",
                MatchCount = 1
            }
        };

        var matches = new List<InfoSiteMatch>
        {
            new()
            {
                InfoSiteMatchId = 1,
                PreMatchLeagueId = 4
            },
            new()
            {
                InfoSiteMatchId = 2,
                PreMatchLeagueId = 4
            },
            new()
            {
                InfoSiteMatchId = 3,
                PreMatchLeagueId = 5
            },
            new()
            {
                InfoSiteMatchId = 4,
                PreMatchLeagueId = 6
            }
        };

        mapper.Map<IEnumerable<LeagueAssignment>>(Arg.Any<IEnumerable<PreMatchLeague>>()).Returns(leagues);
        var expected = new QueryPreMatchLeagueViewModel { InfoSiteMatches = matches, PreMatchLeagues = leagues };
        var service = new ShiftAssignmentServiceImp(repository, mapper);

        //Act
        var actual = await service.QueryNightShift(new QueryNightShiftPreMatchRequest
        {
            Employee = string.Empty,
            EventDate = DateTime.Now,
            LeagueName = string.Empty,
            MatchOffers = new List<string> { "" },
            StartTime = new TimeSpan(11, 00, 00)
        });

        //Assert
        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public async Task MapMatchStatusWhichLinkedOWPA()
    {
        //Arrange
        var repository = Substitute.For<Repository>();
        var mapper = Substitute.For<IMapper>();
        var matches = new List<InfoSiteMatch>
        {
            new() {InfoSiteMatchId = 111, IsLive = false},
            new() {InfoSiteMatchId = 222, IsLive = true},
            new() {InfoSiteMatchId = 333, IsLive = true},
            new() {InfoSiteMatchId = 444, IsLive = true}
        };
        var mockResult = new List<LinkedMatch>
        {
            new() {InfositeMatchId = 111, OWPMatchId = 100, MatchStatus = "Live"},
            new() {InfositeMatchId = 222, OWPMatchId = 200, MatchStatus = "Live"},
            new() {InfositeMatchId = 333, OWPMatchId = 100, MatchStatus = "Cancel"}
        };
        repository.QueryLinkedOwpMatch(Arg.Any<List<int>>(), Arg.Any<DateTime>())
            .Returns(mockResult);

        var service = new ShiftAssignmentServiceImp(repository, mapper);

        await service.MapMatchStatusWhichLinkedOwp(matches, new DateTime());

        Assert.Equal("LIVE(!)", matches[0].MatchStatus);
        Assert.Equal("LIVE", matches[1].MatchStatus);
        Assert.Equal("CANCEL", matches[2].MatchStatus);
        Assert.Empty(matches[3].MatchStatus);
    }

    private static TimeSpan MockStartTime(bool isExisted) => isExisted ? new TimeSpan(15, 00, 00) : new TimeSpan();

    private static (List<PreMatchLeague>, List<InfoSiteMatch>) CreateMockData()
    {
        var leagues = new List<PreMatchLeague>
        {
            new()
            {
                PreMatchLeagueId = 1,
                LeagueId = 1,
                LeagueName = "test1",
                MatchCount = 2,
                HDP = "sa",
                OU = "sa",
                OUTakeOver = "starix14"
            },
            new()
            {
                PreMatchLeagueId = 2,
                LeagueId = 2,
                LeagueName = "test2",
                MatchCount = 1,
                HDP = "starix15",
                OU = "sa",
                OUTakeOver = "starix14"
            },
            new()
            {
                PreMatchLeagueId = 3,
                LeagueId = 3,
                LeagueName = "test3",
                MatchCount = 1,
                OU = "NewOU"
            }
        };

        var matches = new List<InfoSiteMatch>
        {
            new()
            {
                InfoSiteMatchId = 1,
                PreMatchLeagueId = 1
            },
            new()
            {
                InfoSiteMatchId = 2,
                PreMatchLeagueId = 1
            },
            new()
            {
                InfoSiteMatchId = 3,
                PreMatchLeagueId = 2
            },
            new()
            {
                InfoSiteMatchId = 4,
                PreMatchLeagueId = 3
            }
        };

        return (leagues, matches);
    }

    private static (List<PreMatchLeague>, List<InfoSiteMatch>) CreateNightShiftMockData()
    {
        var leagues = new List<PreMatchLeague>
        {
            new()
            {
                PreMatchLeagueId = 4,
                LeagueId = 1,
                LeagueName = "test1",
                MatchCount = 2
            },
            new()
            {
                PreMatchLeagueId = 5,
                LeagueId = 2,
                LeagueName = "test2",
                MatchCount = 1
            },
            new()
            {
                PreMatchLeagueId = 6,
                LeagueId = 3,
                LeagueName = "test3",
                MatchCount = 1
            }
        };

        var matches = new List<InfoSiteMatch>
        {
            new()
            {
                InfoSiteMatchId = 1,
                PreMatchLeagueId = 4
            },
            new()
            {
                InfoSiteMatchId = 2,
                PreMatchLeagueId = 4
            },
            new()
            {
                InfoSiteMatchId = 3,
                PreMatchLeagueId = 5
            },
            new()
            {
                InfoSiteMatchId = 4,
                PreMatchLeagueId = 6
            }
        };

        return (leagues, matches);
    }

    [Fact]
    public async Task AddOddStatusNo()
    {
        var repository = Substitute.For<Repository>();
        var mapper = Substitute.For<IMapper>();
        var service = new ShiftAssignmentServiceImp(repository, mapper);
        var infositeMatches = new List<InfoSiteMatch>
        {
            new() { AdminSiteMatchId = 111 },
            new() { AdminSiteMatchId = 222 },
            new() { AdminSiteMatchId = 333 },
            new() { AdminSiteMatchId = 444 }
        };
        repository.QueryOddStatusFromMainDb(Arg.Any<IEnumerable<InfoSiteMatch>>()).Returns(
            new List<OddStatus>
            {
                new() { AdminSiteMatchId = 111, Bettype = 1, OddsStatus = "running" },
                new() { AdminSiteMatchId = 111, Bettype = 1, OddsStatus = "closed" },
                new() { AdminSiteMatchId = 222, Bettype = 1, OddsStatus = "running" },
                new() { AdminSiteMatchId = 222, Bettype = 3, OddsStatus = "running" },
                new() { AdminSiteMatchId = 333, Bettype = 3, OddsStatus = "Running" }
            });

        await service.AddOddStatusNo(infositeMatches);

        Assert.Equal("OU (?)", infositeMatches[0].MatchOffer);
        Assert.Equal("Opened", infositeMatches[1].MatchOffer);
        Assert.Equal("HDP (?)", infositeMatches[2].MatchOffer);
        Assert.Equal("", infositeMatches[3].MatchOffer);
    }
}