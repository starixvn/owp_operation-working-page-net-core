namespace OWP.PreMatchNotification.UnitTests.InfoSiteMatchInfo
{
    using System;
    using System.Collections.Generic;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Models;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Services;
    using OWP.PreMatchNotification.Models;
    using Xunit;

    public class ChangedValueGeneratorUnitTests
    {
        [Fact]
        public void CreateAssigneeMessages_WithChangedSnapshot_ReturnChangedValue()
        {
            var service = new ChangedValueGeneratorImp();
            var expected = CreateExpected();

        }

        private List<ChangedValue> CreateExpected()
            => new List<ChangedValue> {
                new ChangedValue
                {
                    
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
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 7,
                    AwayName = "a",
                    HomeName = "e",
                    LeagueName = "league3",
                    EventDateTime = new DateTime(2021, 06, 18, 07, 30, 00),
                    OWPLeagueId = 3
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 8,
                    AwayName = "b",
                    HomeName = "c",
                    LeagueName = "league4",
                    EventDateTime = new DateTime(2021, 06, 18, 07, 30, 00),
                    OWPLeagueId = 4
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
                    AwayName = "a",
                    HomeName = "d",
                    LeagueName = "league3",
                    EventDateTime = new DateTime(2021, 06, 17, 07, 30, 00),
                    OWPLeagueId = 3
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 7,
                    AwayName = "a",
                    HomeName = "e-new",
                    LeagueName = "league3",
                    EventDateTime = new DateTime(2021, 06, 18, 07, 30, 00),
                    OWPLeagueId = 3
                },
                new InfoSiteMatch
                {
                    InfoSiteMatchId = 8,
                    AwayName = "b",
                    HomeName = "c-new",
                    LeagueName = "league4",
                    EventDateTime = new DateTime(2021, 06, 18, 07, 30, 00),
                    OWPLeagueId = 4
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
    }
}
