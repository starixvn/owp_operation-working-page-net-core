using AutoMapper;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using NSubstitute;
using OWP.PrematchAssignment.Models;
using OWP.PreMatchAssignment.Criteria;
using OWP.PreMatchAssignment.Models;
using OWP.PreMatchAssignment.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.PreMatchAssignment.UnitTests.Services;

public class ShiftAssignmentService
{
    [Fact]
    public async Task QueryDayShift_WithEmployeeFilter_ShouldReturnFilteredPreMatchData()
    {
        //Arrange
        var dynamicRepo = Substitute.For<IDynamicRepository>();
        var mapper = Substitute.For<IMapper>();
        dynamicRepo.FetchMultipleAsync<PreMatchLeague, InfoSiteMatch>(Arg.Compat.Any<QueryShiftPreMatchCriteria>()).Returns(Task.FromResult(CreateMockData()));
        var leagues = new List<LeagueAssignment>
        {
            new LeagueAssignment
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
            new LeagueAssignment
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
            new InfoSiteMatch
            {
                InfoSiteMatchId = 1,
                PreMatchLeagueId = 1
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 2,
                PreMatchLeagueId = 1
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 3,
                PreMatchLeagueId = 2
            }
        };

        mapper.Map<IList<LeagueAssignment>>(Arg.Compat.Any<IList<PreMatchLeague>>()).Returns(leagues);
        var expected = new QueryPreMatchLeagueViewModel { InfoSiteMatches = matches, PreMatchLeagues = leagues };
        var service = new ShiftAssignmentServiceImp(dynamicRepo, mapper);

        //Act
        var actual = await service.QueryDayShift(new Handlers.QueryDayShiftPreMatchRequest
        {
            Employee = "sa",
            EventDate = DateTime.Now,
            MatchOffers = new List<string> { "" },
            LeagueName = string.Empty
        });

        //Assert
        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public async Task QueryDayShift_WithoutFilter_ShouldReturnAllPreMatchData()
    {
        //Arrange
        var dynamicRepo = Substitute.For<IDynamicRepository>();
        var currentSession = Substitute.For<INapSession>();
        var mapper = Substitute.For<IMapper>();
        currentSession.UserId = 8;
        var leagues = new List<LeagueAssignment>
        {
            new LeagueAssignment
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
            new LeagueAssignment
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
            new LeagueAssignment
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
            new InfoSiteMatch
            {
                InfoSiteMatchId = 1,
                PreMatchLeagueId = 1
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 2,
                PreMatchLeagueId = 1
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 3,
                PreMatchLeagueId = 2
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 4,
                PreMatchLeagueId = 3
            }
        };
        var expected = new QueryPreMatchLeagueViewModel { PreMatchLeagues = leagues, InfoSiteMatches = matches };
        dynamicRepo.FetchMultipleAsync<PreMatchLeague, InfoSiteMatch>(Arg.Compat.Any<QueryShiftPreMatchCriteria>()).Returns(Task.FromResult(CreateMockData()));
        mapper.Map<IList<LeagueAssignment>>(Arg.Compat.Any<IList<PreMatchLeague>>()).Returns(leagues);
        var service = new ShiftAssignmentServiceImp(dynamicRepo, mapper);

        //Act
        var actual = await service.QueryDayShift(new Handlers.QueryDayShiftPreMatchRequest
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
        var dynamicRepo = Substitute.For<IDynamicRepository>();
        var mapper = Substitute.For<IMapper>();
        dynamicRepo.FetchAsync<TimeSpan>(Arg.Compat.Any<GetStartTimeCriteria>()).Returns(Task.FromResult(MockStartTime(false)));
        dynamicRepo.FetchMultipleAsync<PreMatchLeague, InfoSiteMatch>(Arg.Compat.Is<QueryShiftPreMatchCriteria>(x => x.IsDay)).Returns(Task.FromResult(CreateMockData()));
        dynamicRepo.FetchMultipleAsync<PreMatchLeague, InfoSiteMatch>(Arg.Compat.Is<QueryShiftPreMatchCriteria>(x => !x.IsDay)).Returns(Task.FromResult(CreateNightShiftMockData()));

        var leagues = new List<LeagueAssignment>
        {
            new LeagueAssignment
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
            new LeagueAssignment
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
            new LeagueAssignment
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
            new InfoSiteMatch
            {
                InfoSiteMatchId = 1,
                PreMatchLeagueId = 4
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 2,
                PreMatchLeagueId = 4
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 3,
                PreMatchLeagueId = 5
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 4,
                PreMatchLeagueId = 6
            }
        };

        mapper.Map<IList<LeagueAssignment>>(Arg.Compat.Any<IList<PreMatchLeague>>()).Returns(leagues);
        var expected = new QueryPreMatchLeagueViewModel { InfoSiteMatches = matches, PreMatchLeagues = leagues };
        var service = new ShiftAssignmentServiceImp(dynamicRepo, mapper);

        //Act
        var actual = await service.QueryNightShift(new Handlers.QueryNightShiftPreMatchRequest
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
        var dynamicRepo = Substitute.For<IDynamicRepository>();
        var mapper = Substitute.For<IMapper>();
        dynamicRepo.FetchAsync<TimeSpan>(Arg.Compat.Any<GetStartTimeCriteria>()).Returns(Task.FromResult(MockStartTime(true)));
        dynamicRepo.FetchMultipleAsync<PreMatchLeague, InfoSiteMatch>(Arg.Compat.Is<QueryShiftPreMatchCriteria>(x => x.IsDay)).Returns(Task.FromResult(CreateMockData()));
        dynamicRepo.FetchMultipleAsync<PreMatchLeague, InfoSiteMatch>(Arg.Compat.Is<QueryShiftPreMatchCriteria>(x => !x.IsDay)).Returns(Task.FromResult(CreateNightShiftMockData()));

        var leagues = new List<LeagueAssignment>
        {
            new LeagueAssignment
            {
                No = 1,
                PreMatchLeagueId = 4,
                LeagueId = 1,
                LeagueName = "test1",
                MatchCount = 2
            },
            new LeagueAssignment
            {
                No = 2,
                PreMatchLeagueId = 5,
                LeagueId = 2,
                LeagueName = "test2"
            },
            new LeagueAssignment
            {
                No = 3,
                PreMatchLeagueId = 6,
                LeagueId = 3,
                LeagueName = "test3",
                MatchCount = 1,
            }
        };

        var matches = new List<InfoSiteMatch>
        {
            new InfoSiteMatch
            {
                InfoSiteMatchId = 1,
                PreMatchLeagueId = 4
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 2,
                PreMatchLeagueId = 4
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 3,
                PreMatchLeagueId = 5
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 4,
                PreMatchLeagueId = 6
            }
        };

        mapper.Map<IList<LeagueAssignment>>(Arg.Compat.Any<IList<PreMatchLeague>>()).Returns(leagues);
        var expected = new QueryPreMatchLeagueViewModel { InfoSiteMatches = matches, PreMatchLeagues = leagues };
        var service = new ShiftAssignmentServiceImp(dynamicRepo, mapper);

        //Act
        var actual = await service.QueryNightShift(new Handlers.QueryNightShiftPreMatchRequest
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
    public async Task QueryNightShift_WithDBStartTimeGreaterThanRequestStartTime_ShouldReturnDataWithNightAndDayShiftAssignment()
    {
        //Arrange
        var dynamicRepo = Substitute.For<IDynamicRepository>();
        var mapper = Substitute.For<IMapper>();
        var requestStartTime = new TimeSpan(13, 00, 00);
        var eventToRequestStartTime = DateTime.Now.Date.Add(new TimeSpan(15, 00, 00));

        dynamicRepo.FetchAsync<TimeSpan>(Arg.Compat.Any<GetStartTimeCriteria>()).Returns(Task.FromResult(MockStartTime(true)));
        dynamicRepo.FetchMultipleAsync<PreMatchLeague, InfoSiteMatch>(Arg.Compat.Is<QueryShiftPreMatchCriteria>(x => x.IsDay))
            .Returns(Task.FromResult(CreateMockData()));
        dynamicRepo.FetchMultipleAsync<PreMatchLeague, InfoSiteMatch>(Arg.Compat.Is<QueryShiftPreMatchCriteria>(x => !x.IsDay))
            .Returns(Task.FromResult(CreateNightShiftMockData()));
        dynamicRepo.FetchMultipleAsync<PreMatchLeague, InfoSiteMatch>
            (Arg.Compat.Is<QueryShiftPreMatchCriteria>(x => !x.IsDay && x.EventFromDateTime == eventToRequestStartTime))
            .Returns(Task.FromResult(CreateNightShiftWithDBStartTimeMockData()));

        var leagues = new List<LeagueAssignment>
        {
            new LeagueAssignment
            {
                No = 1,
                PreMatchLeagueId = 4,
                LeagueId = 1,
                LeagueName = "test1",
                MatchCount = 2,
                HDP = "starix15",
                OUTakeOver = "starix14"
            },
            new LeagueAssignment
            {
                No = 2,
                PreMatchLeagueId = 5,
                LeagueId = 2,
                LeagueName = "test2",
                HDP = "starix16",
                OUTakeOver = "sa"
            },
            new LeagueAssignment
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
            new InfoSiteMatch
            {
                InfoSiteMatchId = 1,
                PreMatchLeagueId = 4
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 2,
                PreMatchLeagueId = 4
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 3,
                PreMatchLeagueId = 5
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 4,
                PreMatchLeagueId = 6
            }
        };

        mapper.Map<IList<LeagueAssignment>>(Arg.Compat.Any<IList<PreMatchLeague>>()).Returns(leagues);
        var expected = new QueryPreMatchLeagueViewModel { InfoSiteMatches = matches, PreMatchLeagues = leagues };
        var service = new ShiftAssignmentServiceImp(dynamicRepo, mapper);

        //Act
        var actual = await service.QueryNightShift(new Handlers.QueryNightShiftPreMatchRequest
        {
            Employee = string.Empty,
            EventDate = DateTime.Now,
            LeagueName = string.Empty,
            MatchOffers = new List<string> { "" },
            StartTime = requestStartTime
        });

        //Assert
        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public async Task MapMatchStatusWhichLinkedOWPA()
    {
        //Arrange
        var dynamicRepo = Substitute.For<IDynamicRepository>();
        var mapper = Substitute.For<IMapper>();
        var matches = new List<InfoSiteMatch>
        {
            new InfoSiteMatch {InfoSiteMatchId = 111, IsLive = false},
            new InfoSiteMatch {InfoSiteMatchId = 222, IsLive = true},
            new InfoSiteMatch {InfoSiteMatchId = 333, IsLive = true},
            new InfoSiteMatch {InfoSiteMatchId = 444, IsLive = true}
        };
        var mockResult = new List<LinkedMatch>
        {
            new LinkedMatch {InfositeMatchId = 111, OWPMatchId = 100, MatchStatus = "Live"},
            new LinkedMatch {InfositeMatchId = 222, OWPMatchId = 200, MatchStatus = "Live"},
            new LinkedMatch {InfositeMatchId = 333, OWPMatchId = 100, MatchStatus = "Cancel"},
        };
        dynamicRepo.FetchAsync<LinkedMatch>(Arg.Compat.Any<QueryLinkedOWPMatchCriteria>())
            .Returns(mockResult);

        var service = new ShiftAssignmentServiceImp(dynamicRepo, mapper);

        await service.MapMatchStatusWhichLinkedOWP(matches, new DateTime());

        Assert.Equal("LIVE(!)", matches[0].MatchStatus);
        Assert.Equal("LIVE", matches[1].MatchStatus);
        Assert.Equal("CANCEL", matches[2].MatchStatus);
        Assert.Empty(matches[3].MatchStatus);
    }

    private IEnumerable<TimeSpan> MockStartTime(bool isExisted)
    {
        if (isExisted)
        {
            return new List<TimeSpan>
                {
                    new TimeSpan(15,00,00)
                };
        }

        return new List<TimeSpan>();
    }

    private ValueTuple<IEnumerable<PreMatchLeague>, IEnumerable<InfoSiteMatch>> CreateMockData()
    {
        var leagues = new List<PreMatchLeague>
        {
            new PreMatchLeague
            {
                PreMatchLeagueId = 1,
                LeagueId = 1,
                LeagueName = "test1",
                MatchCount = 2,
                HDP = "sa",
                OU = "sa",
                OUTakeOver = "starix14"
            },
            new PreMatchLeague
            {
                PreMatchLeagueId = 2,
                LeagueId = 2,
                LeagueName = "test2",
                MatchCount = 1,
                HDP = "starix15",
                OU = "sa",
                OUTakeOver = "starix14"
            },
            new PreMatchLeague
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
            new InfoSiteMatch
            {
                InfoSiteMatchId = 1,
                PreMatchLeagueId = 1
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 2,
                PreMatchLeagueId = 1
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 3,
                PreMatchLeagueId = 2
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 4,
                PreMatchLeagueId = 3
            }
        };

        return new ValueTuple<IEnumerable<PreMatchLeague>, IEnumerable<InfoSiteMatch>>(leagues, matches);
    }

    private ValueTuple<IEnumerable<PreMatchLeague>, IEnumerable<InfoSiteMatch>> CreateNightShiftMockData()
    {
        var leagues = new List<PreMatchLeague>
        {
            new PreMatchLeague
            {
                PreMatchLeagueId = 4,
                LeagueId = 1,
                LeagueName = "test1",
                MatchCount = 2
            },
            new PreMatchLeague
            {
                PreMatchLeagueId = 5,
                LeagueId = 2,
                LeagueName = "test2",
                MatchCount = 1
            },
            new PreMatchLeague
            {
                PreMatchLeagueId = 6,
                LeagueId = 3,
                LeagueName = "test3",
                MatchCount = 1,
            }
        };

        var matches = new List<InfoSiteMatch>
        {
            new InfoSiteMatch
            {
                InfoSiteMatchId = 1,
                PreMatchLeagueId = 4
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 2,
                PreMatchLeagueId = 4
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 3,
                PreMatchLeagueId = 5
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 4,
                PreMatchLeagueId = 6
            }
        };

        return new ValueTuple<IEnumerable<PreMatchLeague>, IEnumerable<InfoSiteMatch>>(leagues, matches);
    }

    private ValueTuple<IEnumerable<PreMatchLeague>, IEnumerable<InfoSiteMatch>> CreateNightShiftWithDBStartTimeMockData()
    {
        var leagues = new List<PreMatchLeague>
        {
            new PreMatchLeague
            {
                PreMatchLeagueId = 4,
                LeagueId = 1,
                LeagueName = "test1",
                MatchCount = 2,
                HDP = "starix15",
                OUTakeOver = "starix14"
            },
            new PreMatchLeague
            {
                PreMatchLeagueId = 5,
                LeagueId = 2,
                LeagueName = "test2",
                MatchCount = 1,
                HDP = "starix16",
                OUTakeOver = "sa"
            }
        };

        var matches = new List<InfoSiteMatch>
        {
            new InfoSiteMatch
            {
                InfoSiteMatchId = 1,
                PreMatchLeagueId = 4
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 2,
                PreMatchLeagueId = 4
            },
            new InfoSiteMatch
            {
                InfoSiteMatchId = 3,
                PreMatchLeagueId = 5
            }
        };

        return new ValueTuple<IEnumerable<PreMatchLeague>, IEnumerable<InfoSiteMatch>>(leagues, matches);
    }

    [Fact]
    public async Task AddOddStatusNo()
    {
        var dynamicRepo = Substitute.For<IDynamicRepository>();
        var mapper = Substitute.For<IMapper>();
        var service = new ShiftAssignmentServiceImp(dynamicRepo, mapper);
        var infositeMatches = new List<InfoSiteMatch>
        {
            new InfoSiteMatch { AdminSiteMatchId = 111 },
            new InfoSiteMatch { AdminSiteMatchId = 222 },
            new InfoSiteMatch { AdminSiteMatchId = 333 },
            new InfoSiteMatch { AdminSiteMatchId = 444 },
        };
        dynamicRepo.FetchAsync<OddStatus>(Arg.Any<QueryOddStatusFromMainDBCriteria>()).Returns(
            new List<OddStatus>
            {
                new OddStatus { AdminSiteMatchId = 111, Bettype = 1, OddsStatus = "running" },
                new OddStatus { AdminSiteMatchId = 111, Bettype = 1, OddsStatus = "closed" },
                new OddStatus { AdminSiteMatchId = 222, Bettype = 1, OddsStatus = "running" },
                new OddStatus { AdminSiteMatchId = 222, Bettype = 3, OddsStatus = "running" },
                new OddStatus { AdminSiteMatchId = 333, Bettype = 3, OddsStatus = "Running" },
            });

        await service.AddOddStatusNo(infositeMatches);

        Assert.Equal("OU (?)", infositeMatches[0].MatchOffer);
        Assert.Equal("Opened", infositeMatches[1].MatchOffer);
        Assert.Equal("HDP (?)", infositeMatches[2].MatchOffer);
        Assert.Equal("", infositeMatches[3].MatchOffer);
    }
}