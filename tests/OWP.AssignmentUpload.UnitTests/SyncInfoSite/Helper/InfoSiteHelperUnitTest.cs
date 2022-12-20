using OWP.AssignmentUpload.SyncInfoSite.Helper;
using OWP.AssignmentUpload.SyncInfoSite.Models;
using OWP.AssignmentUpload.Validation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.AssignmentUpload.UnitTests.SyncInfoSite.Helper
{
    public class InfoSiteHelperUnitTest
    {
        [Fact]
        public void Compare_ReturnTrue()
        {
            var owpMatch = new AssignmentModel
            {
                League = "Seagame",
                Home = "VN",
                Away = "Singapore"
            };

            var infoSiteMatch = new InfositeMatch
            {
                LeagueName = "Seagame",
                HomeName = "VN",
                AwayName = "Singapore"
            };

            var result = InfoSiteHelper.IsSameLeagueHomeAway(owpMatch, infoSiteMatch);
            Assert.True(result);
        }

        [Fact]
        public void Compare_ReturnFalse()
        {
            var owpMatch = new AssignmentModel
            {
                League = "V-League",
                Home = "VN",
                Away = "Singapore"
            };

            var infoSiteMatch = new InfositeMatch
            {
                LeagueName = "Seagame",
                HomeName = "VN",
                AwayName = "Singapore"
            };

            var result = InfoSiteHelper.IsSameLeagueHomeAway(owpMatch, infoSiteMatch);
            Assert.False(result);
        }

        [Fact]
        public void GetMainMatch_Correct()
        {
            var parentKey = Guid.NewGuid().ToString();
            var owpMatch = new List<AssignmentModel>{
                new AssignmentModel
                {
                    HashKey=parentKey,
                    League = "V-League",
                    Home = "VN",
                    Away = "Singapore",
                },
                new AssignmentModel
                {
                    League = "V-League",
                    Home = "VN",
                    Away = "Singapore",
                    ParentHashKey=parentKey
                },
             };

            var expected = new List<AssignmentModel>{
                new AssignmentModel
                {
                    HashKey=parentKey,
                    League = "V-League",
                    Home = "VN",
                    Away = "Singapore",
                }
             };
            var result = InfoSiteHelper.GetMainMatch(owpMatch);
            DeepAssert.Equal(expected, result);
        }

        [Fact]
        public void GetMinorMatch_Correct()
        {
            var parentKey = Guid.NewGuid().ToString();
            var owpMatch = new List<AssignmentModel>{
                new AssignmentModel
                {
                    HashKey=parentKey,
                    League = "V-League",
                    Home = "VN",
                    Away = "Singapore",
                },
                new AssignmentModel
                {
                    League = "V-League",
                    Home = "VN",
                    Away = "Singapore",
                    ParentHashKey=parentKey
                },
             };

            var expected = new List<AssignmentModel>{
                new AssignmentModel
                {
                    League = "V-League",
                    Home = "VN",
                    Away = "Singapore",
                    ParentHashKey=parentKey
                }
             };
            var result = InfoSiteHelper.GetMinorMatch(owpMatch);
            DeepAssert.Equal(expected, result);
        }

        [Fact]
        public void GetMainMatch_ExistsInfoSiteId_()
        {
            var parentKey = Guid.NewGuid().ToString();
            var owpMatch = new List<AssignmentModel>{
                new AssignmentModel
                {
                    HashKey=parentKey,
                    League = "V-League",
                    Home = "VN",
                    Away = "Singapore",
                },
                new AssignmentModel
                {
                    League = "V-League",
                    Home = "VN",
                    Away = "Singapore",
                    ParentHashKey=parentKey
                },
             };

            var expected = new List<AssignmentModel>{
                new AssignmentModel
                {
                    League = "V-League",
                    Home = "VN",
                    Away = "Singapore",
                    ParentHashKey=parentKey
                }
             };
            var result = InfoSiteHelper.GetMinorMatch(owpMatch);
            DeepAssert.Equal(expected, result);
        }
    }
}
