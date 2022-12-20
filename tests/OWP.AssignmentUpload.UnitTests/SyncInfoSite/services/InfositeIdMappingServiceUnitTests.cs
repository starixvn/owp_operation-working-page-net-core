namespace OWP.AssignmentUpload.UnitTests.SyncInfoSite.services
{
    using AssignmentUpload.SyncInfoSite.Models;
    using AssignmentUpload.SyncInfoSite.Services;
    using AssignmentUpload.Validation.Models;
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class InfositeIdMappingServiceUnitTests
    {
        private readonly InfositeMappingServiceImp infositeMappingService;

        public InfositeIdMappingServiceUnitTests()
        {
            infositeMappingService = new InfositeMappingServiceImp();
        }

        [Fact]
        public void MapMainInfoToMinorMatch_Successfully()
        {
            var parentKey = Guid.NewGuid().ToString();
            var hashkey = Guid.NewGuid().ToString();
            var mainMatchOriginModel = new List<MainMatchBackupModel>
            {
                new MainMatchBackupModel
                {
                    key= parentKey,
                    League="Seagame",
                    Home= "VN",
                    Away="Thai Lan"
                }
            };

            var owpMinorMatch = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    HashKey= hashkey,
                    ParentHashKey= parentKey,
                    League="Seagame",
                    Home="VN Total Booking",
                    Away="Thai Lan Total Booking"
                }
            };
            var newMainMatch = new List<AssignmentModel>
             {
                 new AssignmentModel
                 {
                    HashKey=parentKey,
                    League="V_league",
                    Home= "Ha Noi",
                    Away="Hai Phong"
                 }
             };

            var expected = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    HashKey= hashkey,
                    ParentHashKey= parentKey,
                    League="V_league",
                    Home="Ha Noi Total Booking",
                    Away="Hai Phong Total Booking"
                }
            };

            infositeMappingService.MapMainInfoToMinor(owpMinorMatch, mainMatchOriginModel, newMainMatch);

            DeepAssert.Equal(expected, owpMinorMatch);
        }

        [Fact]
        public void MapMainMatch_OwpInfoSiteIdDoesNotFill_CompareLeagueHomeWayWithInfoSite_MapSuccessfully()
        {
            var owpMainMatch = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    League= "Seagame",
                    Home="VN",
                    Away="TL"
                }
            };

            var infoSiteMatchs = new List<InfositeMatch>
            {
                new InfositeMatch
                {
                    InfoSiteMatchId= 123,
                    LeagueName= "V-League",
                    HomeName="Ha Noi",
                    AwayName="Hai Phong",
                    AdminSiteMatchId= 456
                }
            };
            var expected = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    League= "Seagame",
                    Home="VN",
                    Away="TL"
                }
            };

            InfositeMappingServiceImp.MapMainMatchNotExistInfoSiteId(owpMainMatch, infoSiteMatchs);
            DeepAssert.Equal(expected, owpMainMatch);
        }

        [Fact]
        public void MapMainMatch_CompareLeagueHomeWayKOtimeWithInfoSite_MapSuccessfully()
        {
            var owpMainMatch = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    League= "Seagame",
                    Home="VN",
                    Away="TL",
                    IsSaba= true,
                    EventDate = new DateTime(2020,01,01),
                    KickOffTime = new TimeSpan(10,20,0)
                }
            };

            var infoSiteMatchs = new List<InfositeMatch>
            {
                new InfositeMatch
                {
                    LeagueName= "Seagame",
                    HomeName="VN",
                    AwayName="TL",
                    Website1 = "web",
                    Agent = "abc",
                    EventDateTime = new DateTime(2020,01,01,10,20,0)
                }
            };
            var expected = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    League= "Seagame",
                    Home="VN",
                    Away="TL",
                    Agent = "abc",
                    Website1 = "web",
                    EventDate = new DateTime(2020,01,01,10,20,0)
                }
            };

            InfositeMappingServiceImp.MapMainMatchNotExistInfoSiteId(owpMainMatch, infoSiteMatchs);
            Assert.Equal(expected[0].Agent, owpMainMatch[0].Agent);
            Assert.Equal(expected[0].Website1, owpMainMatch[0].Website1);
        }

        [Fact]
        public void MapMainMatch_NotSameLeagueHomeWayKOtimeWithInfoSite_NoMap()
        {
            var owpMainMatch = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    League= "Seagame",
                    Home="VN",
                    Away="TL",
                    IsSaba=true,
                    KickOffTime = new TimeSpan(3,20,0)
                }
            };

            var infoSiteMatchs = new List<InfositeMatch>
            {
                new InfositeMatch
                {
                    LeagueName= "Seagame",
                    HomeName="VN",
                    AwayName="TL",
                    Agent = "abc",
                    Website1 = "web",
                    EventDateTime = new DateTime(2020,01,01,10,20,0)
                }
            };
            var expected = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    League= "Seagame",
                    Home="VN",
                    Away="TL",
                    KickOffTime = new TimeSpan(3,20,0)
                }
            };

            InfositeMappingServiceImp.MapMainMatchNotExistInfoSiteId(owpMainMatch, infoSiteMatchs);
            Assert.Empty(owpMainMatch[0].Agent);
        }

        [Fact]
        public void MapMainMatch_OwpInfoSiteIdFilled_CompareLeagueHomeWayWithInfoSite_MapSuccessfully()
        {
            var owpMainMatch = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    League= "Seagame",
                    Home="Ha Noi",
                    Away="Hai Phong",
                    WB="WB"
                }
            };

            var infoSiteMatchs = new List<InfositeMatch>
            {
                new InfositeMatch
                {
                    InfoSiteMatchId= 123,
                    LeagueName= "Seagame",
                    HomeName="Ha Noi",
                    AwayName="Hai Phong",
                    AdminSiteMatchId= 456,
                    WB="WB",
                    Website1= "www.w1.com",
                    Website2= "www.w2.com",
                    Website3= "www.w3.com",
                    Agent=string.Empty,
                     Channel=string.Empty,
                     IPTV=string.Empty,
                     Division=string.Empty,
                     Round=string.Empty,
                     TV=string.Empty,
                     Website4=string.Empty
                }
            };

            var expected = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    InfoSiteMatchId= 123,
                    League= "Seagame",
                    Home="Ha Noi",
                    Away="Hai Phong",
                    WB="WB",
                    AdminSiteId = 456,
                    Website1= "www.w1.com",
                    Website2= "www.w2.com",
                    Website3= "www.w3.com"
                }
            };
            InfositeMappingServiceImp.MapMainMatchNotExistInfoSiteId(owpMainMatch, infoSiteMatchs);
            DeepAssert.Equal(expected, owpMainMatch);
        }

        [Fact]
        public void MapMainMatch_WhenOwpInfoSiteIdNotBelongToInfoSite_MapSuccessfully()
        {
            var owpMainMatch = new List<AssignmentModel>
            {
                 new AssignmentModel
                 {
                     InfoSiteMatchId=123,
                     League="V-League",
                     Home="LA",
                     Away="TPHCM",
                     AdminSiteId=1111
                 }
            };
            var infositeMatches = new List<InfositeMatch>
            {
                new InfositeMatch
                {
                    InfoSiteMatchId=345,
                     LeagueName="Seagame",
                     HomeName="VN",
                     AwayName="TL",
                     AdminSiteMatchId=1111
                }
            };

            var expected = new List<AssignmentModel>
            {
                 new AssignmentModel
                 {
                     InfoSiteMatchId=123,
                     League="V-League",
                     Home="LA",
                     Away="TPHCM",
                     AdminSiteId=1111
                 }
            };
            InfositeMappingServiceImp.MapMainMatchExistInfoSiteId(owpMainMatch, infositeMatches);
            DeepAssert.Equal(expected, owpMainMatch);
        }

        [Fact]
        public void MapMainMatch_WhenOwpInfoSiteIdBelongToInfoSite_MapSuccessfully()
        {
            var owpMainMatch = new List<AssignmentModel>
            {
                 new AssignmentModel
                 {
                     InfoSiteMatchId=345,
                     League="V-League",
                     Home="LA",
                     Away="TPHCM",
                     AdminSiteId=1111
                 }
            };
            var infositeMatches = new List<InfositeMatch>
            {
                new InfositeMatch
                {
                     InfoSiteMatchId=345,
                     LeagueName="Seagame",
                     HomeName="VN",
                     AwayName="TL",
                     AdminSiteMatchId=11113,
                     Agent=string.Empty,
                     Channel=string.Empty,
                     IPTV=string.Empty,
                     Division=string.Empty,
                     Round=string.Empty,
                     TV=string.Empty,
                     WB=string.Empty,
                     Website1=string.Empty,
                     Website2=string.Empty,
                     Website3=string.Empty,
                     Website4=string.Empty
                }
            };

            var expected = new List<AssignmentModel>
            {
                 new AssignmentModel
                 {
                     InfoSiteMatchId=345,
                     League="Seagame",
                     Home="VN",
                     Away="TL",
                     AdminSiteId=11113
                 }
            };

            InfositeMappingServiceImp.MapMainMatchExistInfoSiteId(owpMainMatch, infositeMatches);

            DeepAssert.Equal(expected, owpMainMatch);
        }

        [Fact]
        public void MapInfoSiteToMainMatch_OwpInfoSiteIdDoesNotFill_CompareLeagueHomeWayWithInfoSite_MapSuccessfully()
        {
            var owpMainMatch = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    League= "Seagame",
                    Home="VN",
                    Away="TL"
                }
            };

            var infoSiteMatchs = new List<InfositeMatch>
            {
                new InfositeMatch
                {
                    InfoSiteMatchId= 123,
                    LeagueName= "V-League",
                    HomeName="Ha Noi",
                    AwayName="Hai Phong",
                    AdminSiteMatchId= 456
                }
            };
            var expected = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    League= "Seagame",
                    Home="VN",
                    Away="TL"
                }
            };

            infositeMappingService.MapInfoSiteToMainMatch(owpMainMatch, infoSiteMatchs);
            DeepAssert.Equal(expected, owpMainMatch);
        }
    }
}
