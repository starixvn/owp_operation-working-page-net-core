namespace OWP.PreMatchNotification.UnitTests.InfoSiteMatchInfo
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Models;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Services;
    using Models;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class ChangedValueGeneratorUnitTests
    {
        [Fact]
        public void CreateAssigneeMessages_WithChangedSnapshot_ReturnChangedValue()
        {
            var service = new ChangedValueGeneratorImp();
            var expected = CreateExpectedChangedValueForAssignees();

            var actual = service.CreateAssigneeMessages(MockOldSnapshot(), MockNewSnapshot(), MockPreMatchAssignees());

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void CreateLSTMessages_WithChangedSnapshot_ReturnChangedValue()
        {
            var service = new ChangedValueGeneratorImp();
            var expected = CreateExpectedChangedValueForLST();

            var actual = service.CreateLSTMessages(CreateExpectedChangedValueForAssignees(), MockLST(), MockLinkedLGAMatches());

            DeepAssert.Equal(expected, actual);
        }

        private List<ChangedValue> CreateExpectedChangedValueForAssignees()
            => new List<ChangedValue> {
                new ChangedValue
                {
                    InfoSiteMatchId = 2,
                    League = "league1",
                    Home = "c",
                    Away = "b",
                    OldValue = "b",
                    NewValue = "b-new",
                    ReceivedUserIds = new List<int> { 1 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateAway
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 4,
                    League = "league2",
                    Home = "f",
                    Away = "e",
                    OldValue = new DateTime(2021, 06, 17, 07, 00, 00).ToString("MM/dd/yyyy hh:mm tt"),
                    NewValue = new DateTime(2021, 06, 17, 10, 00, 00).ToString("MM/dd/yyyy hh:mm tt"),
                    ReceivedUserIds = new List<int> { 2, 3 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateKOTime
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 4,
                    League = "league2",
                    Home = "f",
                    Away = "e",
                    OldValue = "f",
                    NewValue = "f-new",
                    ReceivedUserIds = new List<int> { 2, 3 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateHome
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 4,
                    League = "league2",
                    Home = "f",
                    Away = "e",
                    OldValue = "e",
                    NewValue = "e-new",
                    ReceivedUserIds = new List<int> { 2, 3 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateAway
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 6,
                    League = "league3",
                    Home = "d",
                    Away = "a",
                    OldValue = "a",
                    NewValue = "a-new",
                    ReceivedUserIds = new List<int> { 4 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateAway
                }
            };

        private List<ChangedValue> CreateExpectedChangedValueForLST()
            => new List<ChangedValue> {
                new ChangedValue
                {
                    InfoSiteMatchId = 4,
                    League = "league2",
                    Home = "f",
                    Away = "e",
                    OldValue = new DateTime(2021, 06, 17, 07, 00, 00).ToString("MM/dd/yyyy hh:mm tt"),
                    NewValue = new DateTime(2021, 06, 17, 10, 00, 00).ToString("MM/dd/yyyy hh:mm tt"),
                    ReceivedUserIds = new List<int> { 10, 11, 12 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateKOTime
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 4,
                    League = "league2",
                    Home = "f",
                    Away = "e",
                    OldValue = "f",
                    NewValue = "f-new",
                    ReceivedUserIds = new List<int> { 10, 11, 12 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateHome
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 4,
                    League = "league2",
                    Home = "f",
                    Away = "e",
                    OldValue = "e",
                    NewValue = "e-new",
                    ReceivedUserIds = new List<int> { 10, 11, 12 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateAway
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 6,
                    League = "league3",
                    Home = "d",
                    Away = "a",
                    OldValue = "a",
                    NewValue = "a-new",
                    ReceivedUserIds = new List<int> { 10, 11, 12 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateAway
                }
            };

        private List<Assignee> MockLST()
            => new List<Assignee> {
                new Assignee
                {
                    AssigneeId = 10,
                    AssigneeName = "sa"
                },
                new Assignee
                {
                    AssigneeId = 11,
                    AssigneeName = "starix1"
                },
                new Assignee
                {
                    AssigneeId = 12,
                    AssigneeName = "starix2"
                }
            };

        private List<Assignee> MockPreMatchAssignees()
            => new List<Assignee> {
                new Assignee
                {
                    AssigneeId = 1,
                    AssigneeName = "sa",
                    OWPLeagueId = 1
                },
                new Assignee
                {
                    AssigneeId = 2,
                    AssigneeName = "starix1",
                    OWPLeagueId = 2
                },
                new Assignee
                {
                    AssigneeId = 3,
                    AssigneeName = "starix2",
                    OWPLeagueId = 2
                },
                new Assignee
                {
                    AssigneeId = 4,
                    AssigneeName = "tyler",
                    OWPLeagueId = 3
                }
            };

        private List<InfoSiteMatch> MockOldSnapshot()
            => new List<InfoSiteMatch>
            {
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 1,
                    AwayName = "a",
                    HomeName = "b",
                    LeagueName = "league1",
                    EventDateTime = new DateTime(2021, 06, 17, 05, 00, 00),
                    OWPLeagueId = 1
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 2,
                    AwayName = "b",
                    HomeName = "c",
                    LeagueName = "league1",
                    EventDateTime = new DateTime(2021, 06, 17, 05, 00, 00),
                    OWPLeagueId = 1
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 3,
                    AwayName = "d",
                    HomeName = "e",
                    LeagueName = "league2",
                    EventDateTime = new DateTime(2021, 06, 17, 06, 00, 00),
                    OWPLeagueId = 2
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 4,
                    AwayName = "e",
                    HomeName = "f",
                    LeagueName = "league2",
                    EventDateTime = new DateTime(2021, 06, 17, 07, 00, 00),
                    OWPLeagueId = 2
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 5,
                    AwayName = "a",
                    HomeName = "c",
                    LeagueName = "league2",
                    EventDateTime = new DateTime(2021, 06, 17, 07, 30, 00),
                    OWPLeagueId = 2
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 6,
                    AwayName = "a",
                    HomeName = "d",
                    LeagueName = "league3",
                    EventDateTime = new DateTime(2021, 06, 17, 07, 30, 00),
                    OWPLeagueId = 3
                }
            };

        private List<InfoSiteMatch> MockNewSnapshot()
            => new List<InfoSiteMatch>
            {
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 1,
                    AwayName = "a",
                    HomeName = "b",
                    LeagueName = "league1",
                    EventDateTime = new DateTime(2021, 06, 17, 05, 00, 00),
                    OWPLeagueId = 1
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 2,
                    AwayName = "b-new",
                    HomeName = "c",
                    LeagueName = "league1",
                    EventDateTime = new DateTime(2021, 06, 17, 05, 00, 00),
                    OWPLeagueId = 1
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 3,
                    AwayName = "d",
                    HomeName = "e",
                    LeagueName = "league2-new",
                    EventDateTime = new DateTime(2021, 06, 17, 06, 00, 00),
                    OWPLeagueId = 100
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 4,
                    AwayName = "e-new",
                    HomeName = "f-new",
                    LeagueName = "league2",
                    EventDateTime = new DateTime(2021, 06, 17, 10, 00, 00),
                    OWPLeagueId = 2
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 5,
                    AwayName = "a",
                    HomeName = "c",
                    LeagueName = "league2",
                    EventDateTime = new DateTime(2021, 06, 17, 07, 30, 00),
                    OWPLeagueId = 2
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 6,
                    AwayName = "a-new",
                    HomeName = "d",
                    LeagueName = "league3",
                    EventDateTime = new DateTime(2021, 06, 17, 07, 30, 00),
                    OWPLeagueId = 3
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 9,
                    AwayName = "t",
                    HomeName = "y",
                    LeagueName = "league4",
                    EventDateTime = new DateTime(2021, 06, 18, 07, 30, 00),
                    OWPLeagueId = 4
                }
            };

        private List<InfoSiteMatch> MockLinkedLGAMatches()
            => new List<InfoSiteMatch>
            {
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 4
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 6
                }
            };
    }
}