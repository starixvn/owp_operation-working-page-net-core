using AutoMapper;
using OWP.AssigneeSuggestionList.Models;
using Xunit;

namespace OWP.AssigneeSuggestionList.UnitTests.Services;

using Constant;
using NSubstitute;
using OWP.AssigneeSuggestionList.Services;
using Xunit.Asserts.Compare;

public class SabaMatchesServicesUnitTest
{
    private readonly IMapper mapper;
    private readonly CategorizeSabaMatchServices categorizeSabaMatchServices;

    public SabaMatchesServicesUnitTest()
    {
        categorizeSabaMatchServices = Substitute.For<CategorizeSabaMatchServices>();
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AssignmentModel, AssignmentModel>();

        });
        mapper = configuration.CreateMapper();
    }

    [Fact]
    public void GroupSaba5Mins()
    {
        var mockedSabaAssignment = new List<AssignmentModel>
        {
            new() { OWPMatchId = 1, LeagueName = "A", HomeName = "B", AwayName="C", MatchType=MatchType.Saba5Mins, UserId = 11, EventDateTime = new DateTime(2022, 03, 29, 02, 30, 00) },
            new() { OWPMatchId = 2, LeagueName = "A", HomeName = "B", AwayName="C", MatchType=MatchType.Saba5Mins, UserId = 11, EventDateTime = new DateTime(2022, 03, 29, 02, 00, 00) },
            new() { OWPMatchId = 3, LeagueName = "A", HomeName = "B", AwayName="C", MatchType=MatchType.Saba5Mins, UserId = 11, EventDateTime = new DateTime(2022, 03, 29, 02, 45, 00) },
            new() { OWPMatchId = 4, LeagueName = "ABC", EventDateTime = new DateTime(2022, 03, 29, 15, 30, 00) },
            new() { MatchType=MatchType.Saba16Mins },
            new() { MatchType=MatchType.Saba5Mins, OWPMatchId = 1, LeagueName = "A", EventDateTime = new DateTime(2022, 03, 29, 15, 30, 00) },
            new() { MatchType=MatchType.Saba5Mins, OWPMatchId = 1, LeagueName = "A", EventDateTime = new DateTime(2022, 03, 29, 15, 00, 00) }
        };

        SabaMatchesServicesImp.GroupSaba5Mins(mockedSabaAssignment);

        var expected = new List<AssignmentModel>
        {
            new() { OWPMatchId = 4, LeagueName = "ABC", EventDateTime = new DateTime(2022, 03, 29, 15, 30, 00) },
            new() { MatchType = MatchType.Saba16Mins },
            new() { OWPMatchId = 2, UserId = 11, LeagueName = "A", HomeName = "B", AwayName="C", MatchType=MatchType.Saba5Mins, EventDateTime = new DateTime(2022, 03, 29, 02, 00, 00) },
            new() { MatchType=MatchType.Saba5Mins, OWPMatchId = 1, LeagueName = "A", EventDateTime = new DateTime(2022, 03, 29, 15, 00, 00) }
        };

        DeepAssert.Equal(expected, mockedSabaAssignment);
    }

    [Fact]
    public void CategorizeAndGroupSabaMatches()
    {
        var mockedAssignment = new List<AssignmentModel>
        {
            // Saba 5 Mins
            new()
            {
                OWPMatchId = 1,
                LeagueName = "SABA Virtual PES 20 - 5 Mins Play",
                HomeName = "B",
                AwayName = "C",
                EventDateTime = new DateTime(2022, 03, 29, 02, 30, 00)
            },
            new()
            {
                OWPMatchId = 2,
                LeagueName = "SABA Virtual PES 20 - 5 Mins Play",
                HomeName = "B",
                AwayName = "C",
                EventDateTime = new DateTime(2022, 03, 29, 02, 00, 00)
            },
            new()
            {
                OWPMatchId = 3,
                LeagueName = "SABA Virtual PES 20 - 5 Mins Play",
                HomeName = "B",
                AwayName = "C",
                EventDateTime = new DateTime(2022, 03, 29, 02, 45, 00)
            },
            new()
            {
                OWPMatchId = 4,
                LeagueName = "SABA PES 20 - 5 Mins Play",
                EventDateTime = new DateTime(2022, 03, 29, 02, 30, 00)
            },
            // group multiple link match
            new()
            {
                OWPMatchId = 5,
                LeagueName = "SABA PES 20 - 15 Mins Play",
                HomeName = "B",
                AwayName = "C",
                EventDateTime = new DateTime(2022, 03, 29, 02, 45, 00)
            },
            new()
            {
                OWPMatchId = 6,
                LeagueName = "SABA PES 20 - 16 Mins Play",
                HomeName = "B",
                AwayName = "C",
                EventDateTime = new DateTime(2022, 03, 29, 02, 45, 00)
            },
            new()
            {
                OWPMatchId = 7,
                LeagueName = "SABA - 15 Mins Play",
                HomeName = "B",
                AwayName = "C",
                EventDateTime = new DateTime(2022, 03, 29, 02, 45, 00)
            },
            new()
            {
                OWPMatchId = 8,
                LeagueName = "M88 SABA PES 20 - 20 Mins Play",
                HomeName = "B",
                AwayName = "C",
                EventDateTime = new DateTime(2022, 03, 29, 02, 45, 00)
            },
            // M88 saba
            new()
            {
                OWPMatchId = 20,
                LeagueName = "M88 SABA PES 20 - 16 Mins Play",
                HomeName = "B",
                AwayName = "C",
                EventDateTime = new DateTime(2022, 03, 29, 02, 45, 00)
            },
            // Futsal 6 mins
            new()
            {
                OWPMatchId = 21,
                LeagueName = "SABA PES 20 - 6 Mins Play",
                HomeName = "B",
                AwayName = "C",
                EventDateTime = new DateTime(2022, 03, 29, 02, 45, 00)
            },
            // 16 mins
            new()
            {
                OWPMatchId = 22,
                LeagueName = "SABA PES 20 - 16 Mins Play",
                HomeName = "B",
                AwayName = "C",
                EventDateTime = new DateTime(2022, 03, 29, 1, 0, 0)
            }
        };

        var expected = new List<AssignmentModel>
        {
            // M88
            new()
            {
                OWPMatchId = 20,
                LeagueName = "M88 SABA PES 20 - 16 Mins Play",
                HomeName = "B",
                AwayName = "C",
                EventDateTime = new DateTime(2022, 03, 29, 02, 45, 00),
                MatchType = MatchType.SabaM8816Mins
            },
            // Futsal 6 mins
            new()
            {
                OWPMatchId = 21,
                LeagueName = "SABA PES 20 - 6 Mins Play",
                HomeName = "B",
                AwayName = "C",
                EventDateTime = new DateTime(2022, 03, 29, 02, 45, 00),
                MatchType = MatchType.SabaFutsal6Mins
            },
            // 5 mins
            new()
            {
                OWPMatchId = 2,
                LeagueName = "SABA Virtual PES 20 - 5 Mins Play",
                HomeName = "B",
                AwayName = "C",
                EventDateTime = new DateTime(2022, 03, 29, 02, 00, 00),
                MatchType = MatchType.SabaMix5mins
            },
            // multiple game
            new()
            {
                OWPMatchId = 5,
                LeagueName = "SABA PES 20 - 15 Mins Play",
                HomeName = "B",
                AwayName = "C",
                EventDateTime = new DateTime(2022, 03, 29, 02, 45, 00),
                MatchType = MatchType.SabaLink4G
            },
            // 16 mins
            new()
            {
                OWPMatchId = 22,
                LeagueName = "SABA PES 20 - 16 Mins Play",
                HomeName = "B",
                AwayName = "C",
                EventDateTime = new DateTime(2022, 03, 29, 1, 0, 0),
                MatchType = MatchType.Saba16Mins
            }
        };

        new SabaMatchesServicesImp(mapper, categorizeSabaMatchServices).CategorizeAndGroupSabaMatches(mockedAssignment);

        DeepAssert.Equal(expected, mockedAssignment, "GroupAssignmentModels");
    }

    [Theory]
    [InlineData(1, MatchType.Saba16Mins, MatchType.Saba16Mins)]
    [InlineData(1, MatchType.Saba15Mins, MatchType.SabaLink1G)]
    [InlineData(2, MatchType.Saba16Mins, MatchType.SabaLink2G)]
    [InlineData(3, MatchType.Saba16Mins, MatchType.SabaLink3G)]
    [InlineData(4, MatchType.Saba16Mins, MatchType.SabaLink4G)]
    [InlineData(5, MatchType.Saba16Mins, MatchType.SabaOther)]
    public void GetMultipleMatchType(int count, MatchType type, MatchType expected)
    {
        var actual = SabaMatchesServicesImp.GetMultipleMatchType(count, type);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MapAssignmentToWorkingSchedule()
    {
        var eventDate = new DateTime(2023, 03, 10);
        var workingSchedule = new WorkingSchedule();
        var assignmentModels = new List<AssignmentModel>
        {
            new()
            {
                OWPMatchId = 20,
                LeagueName = "Saba A",
                EventDateTime = new DateTime(2023, 03, 10, 14, 0, 0),
                MatchType = MatchType.SabaM8816Mins
            },
            new()
            {
                LeagueName = "Saba A",
                EventDateTime = new DateTime(2023, 03, 10, 2, 0, 0),
                MatchType = MatchType.Saba5Mins,
                MaxbetHDP = 3333
            },
            new()
            {
                LeagueName = "Saba A",
                EventDateTime = new DateTime(2023, 03, 11, 2, 0, 0),
                MatchType = MatchType.SabaLink2G,
                GroupAssignmentModels = new List<AssignmentModel>
                {
                    new(){ MaxbetHDP = 100000, LeagueName = "Saba A", MatchType = MatchType.Saba15Mins},
                    new(){ MaxbetHDP = 200000, LeagueName = "Saba A" ,MatchType = MatchType.Saba15Mins}
                }
            },
            new()
            {
                LeagueName = "Saba B",
                EventDateTime = new DateTime(2023, 03, 11, 15, 30, 0),
                MaxbetHDP = 3000,
                KOTime = new TimeSpan(15, 30, 0),
                MatchType = MatchType.SabaOther
            }
        };
        var expected = new WorkingSchedule
        {
            Today_14h =new List<AssignmentCell>{ new () { Text = "*V", TextColor = "#444444", Color = "#FCD5B4",KoAssignment = new DateTime(2023, 03, 10, 14, 0, 0), } },
            Yesterday_02h = new List<AssignmentCell> {new () { Text = "*V/5", TextColor = "#FFFFFF", Color = "#7030A0", KoAssignment = new DateTime(2023, 03, 10, 2, 0, 0) } },
            Today_02h = new List<AssignmentCell>{ new ()  { Text = "*V/15*2", TextColor = "#FFFFFF", Color = "#A34262", KoAssignment = new DateTime(2023, 03, 11, 2, 0, 0) } } ,
            Tomorrow_15h = new List<AssignmentCell>{new() { Text = "Error!", TextColor = "#FF0000", Color = "#FFFFFF", KoAssignment = new DateTime(2023, 03, 11, 15, 30, 0)} } 
        };

        SabaMatchesServicesImp.MapAssignmentsToWorkingSchedule(workingSchedule, assignmentModels, eventDate);

        DeepAssert.Equal(expected, workingSchedule);
    }

    [Fact]
    public void MapAssignmentsToWorkingSchedule()
    {
        var eventDate = new DateTime(2023, 03, 10);
        var workingSchedules = new List<WorkingSchedule>
        {
            new()
            {
                UserInfo = new UserInfo
                {
                    UserId = 111,
                    ShiftStartTime = new DateTime(2023,03,10,14, 0, 0),
                    ShiftEndTime = new DateTime(2023,03,11,13, 0, 0)
                }
            }
        };
        var assignmentModels = new List<AssignmentModel>
        {
            new()
            {
                UserId = 111,
                OWPMatchId = 20,
                LeagueName = "M88 SABA - 16 Mins Play",
                EventDateTime = new DateTime(2023, 03, 10, 14, 0, 0),
                EndTime = new DateTime(2023, 03, 10, 15, 0, 0)
            },
            new()
            {
                UserId = 111,
                LeagueName = "Saba PES - 5 Mins Play",
                EventDateTime = new DateTime(2023, 03, 10, 2, 0, 0),
                EndTime = new DateTime(2023, 03, 10, 3, 0, 0),
                MaxbetHDP = 3333
            },
            new()
            {
                UserId = 111,
                OWPMatchId = 11,
                LeagueName = "SABA - 16 Mins Play",
                EventDateTime = new DateTime(2023, 03, 11, 2, 0, 0),
                EndTime = new DateTime(2023, 03, 11, 3, 0, 0),
                MaxbetHDP = 100000,
                BettypeId = 1
            },
            new()
            {
                UserId = 111,
                OWPMatchId = 11,
                LeagueName = "SABA - 16 Mins Play",
                EventDateTime = new DateTime(2023, 03, 11, 2, 0, 0),
                EndTime = new DateTime(2023, 03, 11, 3, 0, 0),
                MaxbetHDP = 100000,
                BettypeId = 3
            },
            new()
            {
                UserId = 111,
                OWPMatchId = 12,
                LeagueName = "SABA - 20 Mins Play",
                EventDateTime = new DateTime(2023, 03, 11, 2, 0, 0),
                EndTime = new DateTime(2023, 03, 11, 3, 0, 0),
                MaxbetHDP = 200000
            },
            new()
            {
                UserId = 111,
                LeagueName = "Saba B",
                EventDateTime = new DateTime(2023, 03, 11, 15, 30, 0),
                EndTime = new DateTime(2023, 03, 11, 17, 30, 0),
                MaxbetHDP = 3000,
                KOTime = new TimeSpan(15, 30, 0)
            }
        };
        var expected = new WorkingSchedule
        {
            Today_14h = new List<AssignmentCell>
            {
                new()
                {
                     Text = "*V", 
                     TextColor = "#444444",
                     Color = "#FCD5B4" ,
                     KoAssignment = new DateTime(2023, 03, 10, 14, 0, 0)
                }

            } ,
            Yesterday_02h = null,
            Today_02h = new List<AssignmentCell>
            {
                new ()
                {
                    Text = "*V/16+20",
                    TextColor = "#FFFFFF",
                    Color = "#A34262", 
                    KoAssignment    = new DateTime(2023, 03, 11, 2, 0, 0)
                }
            } ,
            Tomorrow_15h = null
        };

        new SabaMatchesServicesImp(mapper, categorizeSabaMatchServices).MapAssignmentsToWorkingSchedules(workingSchedules, assignmentModels, eventDate);

        DeepAssert.Equal(expected, workingSchedules[0], "UserInfo", "CategorizedAssignments");
    }
}
