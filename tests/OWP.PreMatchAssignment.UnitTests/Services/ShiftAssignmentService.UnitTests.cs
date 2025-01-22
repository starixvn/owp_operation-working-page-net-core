using AutoMapper;
using Fanex.NAP.Client;
using NSubstitute;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.Handlers;
using OWP.PrematchAssignment.Models;
using OWP.PrematchAssignment.Services;
using Xunit;

namespace OWP.PreMatchAssignment.UnitTests.Services;

using Xunit.Asserts.Compare;

public class ShiftAssignmentServiceTests
{
    private readonly Repository repository;
    private readonly IMapper mapper;
    private readonly ShiftAssignmentService shiftAssignmentService;

    public ShiftAssignmentServiceTests()
    {
        repository = Substitute.For<Repository>();
        mapper = Substitute.For<IMapper>();
        shiftAssignmentService = new ShiftAssignmentServiceImp(repository, mapper);
    }

    [Fact]
    public async Task QueryDayShift_WithEmployeeFilter_ShouldReturnFilteredPreMatchData()
    {
        //Arrange
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
            MatchOffers = [""],
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
        var currentSession = Substitute.For<INapSession>();
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
            MatchOffers = [""],
            LeagueName = string.Empty
        });

        //Assert
        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public async Task QueryNightShift_WithoutDBStartTime_ShouldReturnDataWithDayShiftAssignment()
    {
        //Arrange
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
            MatchOffers = [""],
            LeagueName = string.Empty
        });

        //Assert
        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public async Task QueryNightShift_WithDBStartTimeLessThanRequestStartTime_ShouldReturnDataWithNightShiftAssignment()
    {
        //Arrange
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
            MatchOffers = [""],
            StartTime = new TimeSpan(11, 00, 00)
        });

        //Assert
        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public async Task MapMatchStatusWhichLinkedOWPA()
    {
        //Arrange
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

    private static TimeSpan MockStartTime(bool isExisted) => isExisted ? new TimeSpan(15, 00, 00) : TimeSpan.Zero;

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

    [Fact]
    public async Task MapTicketLists_ShouldMapTicketsCorrectly()
    {
        // Arrange
        var actorId = 1;

        var preMatchLeagues = new List<LeagueAssignment>
            {
                new LeagueAssignment { LeagueId = 1 },
                new LeagueAssignment { LeagueId = 2 }
            };

        var infoSiteMatches = new List<InfoSiteMatch>
            {
                new InfoSiteMatch { InfoSiteMatchId = 1 },
                new InfoSiteMatch { InfoSiteMatchId = 2 }
            };

        var leagueTickets = new Dictionary<int, List<TicketLeagueBasicInfo>>
            {
                { 1, new List<TicketLeagueBasicInfo> { new TicketLeagueBasicInfo { LatestCreatedDate = DateTime.Now.AddDays(-1) }, new TicketLeagueBasicInfo { LatestCreatedDate = DateTime.Now } } },
                {
                    2,
                    new List<TicketLeagueBasicInfo>
                    {
                        new TicketLeagueBasicInfo { LatestCreatedDate = DateTime.Now.AddDays(-2) },
                        new TicketLeagueBasicInfo { LatestCreatedDate = DateTime.Now.AddDays(-1) }
                    }
                }
            };

        var matchTickets = new Dictionary<int, List<TicketMatchBasicInfo>>
            {
                {
                    1,
                    new List<TicketMatchBasicInfo>
                    {
                        new TicketMatchBasicInfo { LatestCreatedDate = DateTime.Now.AddDays(-3) },
                        new TicketMatchBasicInfo { LatestCreatedDate = DateTime.Now.AddDays(-1) }
                    }
                },
                {
                    2,
                    new List<TicketMatchBasicInfo>
                    {
                        new TicketMatchBasicInfo { LatestCreatedDate = DateTime.Now.AddDays(-4) },
                        new TicketMatchBasicInfo { LatestCreatedDate = DateTime.Now.AddDays(-2) }
                    }
                }
            };

        repository.QueryAllLeaguesTicket(Arg.Any<IEnumerable<int>>(), actorId).Returns(leagueTickets);
        repository.QueryAllMatchesTicket(Arg.Any<IEnumerable<int>>(), actorId).Returns(matchTickets);

        // Act
        await shiftAssignmentService.MapTicketLists(preMatchLeagues, infoSiteMatches, actorId);

        // Assert
        Assert.Equal(2, preMatchLeagues[0].TicketList.Count);
        Assert.Equal(2, preMatchLeagues[1].TicketList.Count);
        Assert.Equal(2, infoSiteMatches[0].TicketList.Count);
        Assert.Equal(2, infoSiteMatches[1].TicketList.Count);

        Assert.Equal(leagueTickets[1].OrderByDescending(t => t.LatestCreatedDate), preMatchLeagues[0].TicketList);
        Assert.Equal(leagueTickets[2].OrderByDescending(t => t.LatestCreatedDate), preMatchLeagues[1].TicketList);
        Assert.Equal(matchTickets[1].OrderByDescending(t => t.LatestCreatedDate), infoSiteMatches[0].TicketList);
        Assert.Equal(matchTickets[2].OrderByDescending(t => t.LatestCreatedDate), infoSiteMatches[1].TicketList);
    }

    [Fact]
    public void UpdateAssignmentHintNightShift_ShouldUpdateNightShiftLeaguesHints()
    {
        // Arrange
        var dayShiftLeagues = new Dictionary<int, PreMatchLeague>
        {
            {
                1, new PreMatchLeague
                {
                    LeagueId = 1,
                    HDP = "DayHDP1",
                    OtherHDP = "DayOtherHDP1",
                    OU = "DayOU1",
                    OtherOU = "DayOtherOU1",
                    HDPTakeOver = "DayHDPTakeOver1",
                    OtherHDPTakeOver = "DayOtherHDPTakeOver1",
                    OUTakeOver = "DayOUTakeOver1",
                    OtherOUTakeOver = "DayOtherOUTakeOver1"
                }
            }
        };

        var nightShiftLeagues = new Dictionary<int, PreMatchLeague>
        {
            {
                1, new PreMatchLeague
                {
                    LeagueId = 1,
                    HDPHint = null,
                    OtherHDPHint = null,
                    OUHint = null,
                    OtherOUHint = null,
                    HDPTakeOverHint = null,
                    OtherHDPTakeOverHint = null,
                    OUTakeOverHint = null,
                    OtherOUTakeOverHint = null
                }
            }
        };

        // Act
        ShiftAssignmentServiceImp.UpdateAssignmentHintNightShift(nightShiftLeagues, dayShiftLeagues);

        // Assert
        var nightShiftLeague = nightShiftLeagues[1];
        Assert.Equal("DayHDP1", nightShiftLeague.HDPHint);
        Assert.Equal("DayOtherHDP1", nightShiftLeague.OtherHDPHint);
        Assert.Equal("DayOU1", nightShiftLeague.OUHint);
        Assert.Equal("DayOtherOU1", nightShiftLeague.OtherOUHint);
        Assert.Equal("DayHDPTakeOver1", nightShiftLeague.HDPTakeOverHint);
        Assert.Equal("DayOtherHDPTakeOver1", nightShiftLeague.OtherHDPTakeOverHint);
        Assert.Equal("DayOUTakeOver1", nightShiftLeague.OUTakeOverHint);
        Assert.Equal("DayOtherOUTakeOver1", nightShiftLeague.OtherOUTakeOverHint);
    }
}