using NSubstitute;
using OWP.PreMatchNotification.Constants;
using OWP.PreMatchNotification.InfoSiteMatchInfo.Models;
using OWP.PreMatchNotification.InfoSiteMatchInfo.Services;
using OWP.PreMatchNotification.Models;
using OWP.PreMatchNotification.Services;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.PreMatchNotification.UnitTests.InfoSiteMatchInfo
{
    public class ChangedValueGeneratorUnitTests
    {
        [Fact]
        public void CreateAssigneeMessages_ChangedSnapshot_ReturnChangedValue()
        {
            var service = MockService();
            var expected = GetExpectedAssigneeMessages();

            var actual = service.CreateAssigneeMessages(MockOldSnapshot(), MockNewSnapshot(), MockPreMatchAssignees());

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void CreateLSTMessages_ChangedSnapshot_ReturnChangedValue()
        {
            var service = MockService();
            var expected = GetExpectedLSTMessages();

            var actual = service.CreateLSTMessages(GetExpectedAssigneeMessages(), MockAssigneeLST(), MockLinkedMatches());

            DeepAssert.Equal(expected, actual);
        }

        private ChangedValueGenerator MockService()
        {
            var dateTimeHelper = Substitute.For<DateTimeHelper>();
            dateTimeHelper.GetCurrentDate().Returns(new DateTime(2021, 06, 23));

            return new ChangedValueGeneratorImp(dateTimeHelper);
        }

        private List<ChangedValue> GetExpectedAssigneeMessages()
        {
            return new List<ChangedValue>
            {
                new ChangedValue
                {
                    InfoSiteMatchId = 1,
                    League="Premier League",
                    Home = "MC",
                    Away = "MU",
                    OldValue = "MC" ,
                    NewValue =  "MC changed",
                    ReceivedUserIds = new List<int> { 1,4,5 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateHome
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 2,
                    League="Premier League",
                    Home = "Liv",
                    Away = "MC",
                    OldValue =  new DateTime(2021, 06, 23, 15, 00, 00).ToString("MM/dd/yyyy hh:mm tt"),
                    NewValue =  new DateTime(2022, 09, 03, 15, 00, 00).ToString("MM/dd/yyyy hh:mm tt"),
                    ReceivedUserIds = new List<int> { 1,4,5 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateKOTime
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 2,
                    League="Premier League",
                    Home = "Liv",
                    Away = "MC",
                    OldValue = "Liv",
                    NewValue = "Liv changed",
                    ReceivedUserIds = new List<int> { 1,4,5 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateHome
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 2,
                    League="Premier League",
                    Home = "Liv",
                    Away = "MC",
                    OldValue = "MC",
                    NewValue = "MC changed",
                    ReceivedUserIds = new List<int> { 1,4,5 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateAway
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 4,
                    League="Premier League",
                    Home = "CHES",
                    Away = "TOT",
                    OldValue = "TOT",
                    NewValue = "TOT changed",
                    ReceivedUserIds = new List<int> { 2,3 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateAway
                },
            };
        }

        private IList<InfoSiteMatch> MockOldSnapshot()
        {
            return new List<InfoSiteMatch>
            {
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 1,
                    HomeName = "MC",
                    AwayName = "MU",
                    EventDateTime = new DateTime(2021, 06, 23, 23, 00, 00),
                    OWPLeagueId = 101,
                    LeagueName="Premier League"
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 2,
                    HomeName = "Liv",
                    AwayName = "MC",
                    EventDateTime = new DateTime(2021, 06, 23, 15, 00, 00),
                    OWPLeagueId = 101,
                    LeagueName="Premier League"
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 3,
                    HomeName = "BVB",
                    AwayName = "BAY",
                    EventDateTime = new DateTime(2021, 06, 24, 03, 59, 21),
                    OWPLeagueId = 101,
                    LeagueName="Premier League"
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 4,
                    HomeName = "CHES",
                    AwayName = "TOT",
                    EventDateTime = new DateTime(2021, 06, 24, 02, 00, 00),
                    OWPLeagueId = 102,
                    LeagueName="Premier League"
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 5,
                    HomeName = "BAR",
                    AwayName = "RM",
                    EventDateTime = new DateTime(2021, 06, 24, 04, 00, 01),
                    OWPLeagueId = 102,
                    LeagueName="Premier League"
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 7,
                    HomeName = "THAI",
                    AwayName = "VIET",
                    EventDateTime = new DateTime(2021, 06, 24, 12, 30, 00),
                    OWPLeagueId = 102,
                    LeagueName="AFF"
                }
            };
        }

        private IList<InfoSiteMatch> MockNewSnapshot()
        {
            return new List<InfoSiteMatch>

            {
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 1,
                    HomeName = "MC changed",
                    AwayName = "MU",
                    EventDateTime = new DateTime(2021, 06, 23, 23, 00, 00),
                    OWPLeagueId = 101
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 2,
                    HomeName = "Liv changed",
                    AwayName = "MC changed",
                    EventDateTime = new DateTime(2022, 09, 03, 15, 00, 00),
                    OWPLeagueId = 101,
                    LeagueName="Premier League change"
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 3,
                    HomeName = "BVB",
                    AwayName = "BAY changed",
                    EventDateTime = new DateTime(2021, 06, 24, 03, 59, 21),
                    OWPLeagueId = 101,
                    LeagueName = "Premier League"
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 4,
                    HomeName = "CHES",
                    AwayName = "TOT changed",
                    EventDateTime = new DateTime(2021, 06, 24, 02, 00, 00),
                    OWPLeagueId = 102,
                    LeagueName = "Premier League "
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 5,
                    HomeName = "BAR",
                    AwayName = "RM changed",
                    EventDateTime = new DateTime(2021, 06, 23, 3, 59, 59),
                    OWPLeagueId = 102,
                    LeagueName = "Premier League"
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 6,
                    HomeName = "TOT changed",
                    AwayName = "MU changed",
                    EventDateTime = new DateTime(2021, 06, 23, 07, 30, 00),
                    OWPLeagueId = 102,
                    LeagueName = "Premier League"
                },
            };
        }

        private IList<Assignee> MockPreMatchAssignees()
        {
            return new List<Assignee>
            {
                new Assignee
                {
                    AssigneeId = 1,
                    OWPLeagueId = 101
                },
                new Assignee
                {
                    AssigneeId = 2,
                    OWPLeagueId = 102
                },
                new Assignee
                {
                    AssigneeId = 3,
                    OWPLeagueId = 102
                },
                new Assignee
                {
                    AssigneeId = 4,
                    OWPLeagueId = 101
                },
                new Assignee
                {
                    AssigneeId = 5,
                    OWPLeagueId = 101
                }
            };
        }

        private IList<ChangedValue> GetExpectedLSTMessages()
        {
            return new List<ChangedValue>
            {
                new ChangedValue
                {
                    InfoSiteMatchId = 1,
                    League="Premier League",
                    Home = "MC",
                    Away = "MU",
                    OldValue = "MC" ,
                    NewValue =  "MC changed",
                    ReceivedUserIds = new List<int> { 2, 3, 4, 5, 6 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateHome
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 2,
                    League = "Premier League",
                    Home = "Liv",
                    Away = "MC",
                    OldValue = new DateTime(2021, 06, 23, 15, 00, 00).ToString("MM/dd/yyyy hh:mm tt"),
                    NewValue = new DateTime(2022, 09, 03, 15, 00, 00).ToString("MM/dd/yyyy hh:mm tt"),
                    ReceivedUserIds = new List<int> { 2, 3, 4, 5, 6 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateKOTime
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 2,
                    League = "Premier League",
                    Home = "Liv",
                    Away = "MC",
                    OldValue = "Liv",
                    NewValue = "Liv changed",
                    ReceivedUserIds = new List<int> { 2, 3, 4, 5, 6 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateHome
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 2,
                    League = "Premier League",
                    Home = "Liv",
                    Away = "MC",
                    OldValue = "MC",
                    NewValue = "MC changed",
                    ReceivedUserIds = new List<int> { 2, 3, 4, 5, 6 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateAway
                },
            };
        }

        private IList<Assignee> MockAssigneeLST()
        {
            return new List<Assignee>
            {
                new Assignee
                {
                    AssigneeId = 2
                },
                new Assignee
                {
                    AssigneeId = 3
                },
                new Assignee
                {
                    AssigneeId = 4
                },
                new Assignee
                {
                    AssigneeId = 5
                },
                new Assignee
                {
                    AssigneeId = 6
                },
            };
        }

        private IList<InfoSiteMatch> MockLinkedMatches()
        {
            return new List<InfoSiteMatch>
            {
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 1
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 2
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 3
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 5
                }
            };
        }
    }
}