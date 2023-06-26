using OWP.AssignmentUpload.SyncInfoSite.Models;
using OWP.AssignmentUpload.SyncInfoSite.Services;
using OWP.AssignmentUpload.Validation.Models;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.AssignmentUpload.UnitTests.SyncInfoSite.services;

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
        var hashKey = Guid.NewGuid().ToString();
        var mainMatchOriginModel = new List<MainMatchBackupModel>
        {
            new()
            {
                key= parentKey,
                League="SeaGame",
                Home= "VN",
                Away="Thai Lan"
            }
        };

        var owpMinorMatch = new List<AssignmentModel>
        {
            new()
            {
                HashKey= hashKey,
                ParentHashKey= parentKey,
                League="SeaGame",
                Home="VN Total Booking",
                Away="Thai Lan Total Booking"
            }
        };
        var newMainMatch = new List<AssignmentModel>
        {
            new()
            {
                HashKey=parentKey,
                League="V_league",
                Home= "Ha Noi",
                Away="Hai Phong"
            }
        };

        var expected = new List<AssignmentModel>
        {
            new()
            {
                HashKey= hashKey,
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
            new()
            {
                League= "SeaGame",
                Home="VN",
                Away="TL"
            }
        };

        var infoSiteMatches = new List<InfositeMatch>
        {
            new()
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
            new()
            {
                League= "SeaGame",
                Home="VN",
                Away="TL"
            }
        };

        InfositeMappingServiceImp.MapMainMatchNotExistInfoSiteId(owpMainMatch, infoSiteMatches);
        DeepAssert.Equal(expected, owpMainMatch);
    }

    [Fact]
    public void MapMainMatch_CompareLeagueHomeWayKoTimeWithInfoSite_MapSuccessfully()
    {
        var owpMainMatch = new List<AssignmentModel>
        {
            new()
            {
                League= "SeaGame",
                Home="VN",
                Away="TL",
                IsSaba= true,
                EventDate = new DateTime(2020,01,01),
                KickOffTime = new TimeSpan(10,20,0)
            }
        };

        var infoSiteMatches = new List<InfositeMatch>
        {
            new()
            {
                LeagueName= "SeaGame",
                HomeName="VN",
                AwayName="TL",
                Website1 = "web",
                Agent = "abc",
                EventDateTime = new DateTime(2020,01,01,10,20,0)
            }
        };
        var expected = new List<AssignmentModel>
        {
            new()
            {
                League= "SeaGame",
                Home="VN",
                Away="TL",
                Agent = "abc",
                Website1 = "web",
                EventDate = new DateTime(2020,01,01,10,20,0)
            }
        };

        InfositeMappingServiceImp.MapMainMatchNotExistInfoSiteId(owpMainMatch, infoSiteMatches);
        Assert.Equal(expected[0].Agent, owpMainMatch[0].Agent);
        Assert.Equal(expected[0].Website1, owpMainMatch[0].Website1);
    }

    [Fact]
    public void MapMainMatch_NotSameLeagueHomeWayKoTimeWithInfoSite_NoMap()
    {
        var owpMainMatch = new List<AssignmentModel>
        {
            new()
            {
                League= "SeaGame",
                Home="VN",
                Away="TL",
                IsSaba=true,
                KickOffTime = new TimeSpan(3,20,0)
            }
        };

        var infoSiteMatches = new List<InfositeMatch>
        {
            new()
            {
                LeagueName= "SeaGame",
                HomeName="VN",
                AwayName="TL",
                Agent = "abc",
                Website1 = "web",
                EventDateTime = new DateTime(2020,01,01,10,20,0)
            }
        };
        
        InfositeMappingServiceImp.MapMainMatchNotExistInfoSiteId(owpMainMatch, infoSiteMatches);
        Assert.Empty(owpMainMatch[0].Agent);
    }

    [Fact]
    public void MapMainMatch_OwpInfoSiteIdFilled_CompareLeagueHomeWayWithInfoSite_MapSuccessfully()
    {
        var owpMainMatch = new List<AssignmentModel>
        {
            new()
            {
                League= "SeaGame",
                Home="Ha Noi",
                Away="Hai Phong",
                WB="WB"
            }
        };

        var infoSiteMatches = new List<InfositeMatch>
        {
            new()
            {
                InfoSiteMatchId= 123,
                LeagueName= "SeaGame",
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
            new()
            {
                InfoSiteMatchId= 123,
                League= "SeaGame",
                Home="Ha Noi",
                Away="Hai Phong",
                WB="WB",
                AdminSiteId = 456,
                Website1= "www.w1.com",
                Website2= "www.w2.com",
                Website3= "www.w3.com"
            }
        };
        InfositeMappingServiceImp.MapMainMatchNotExistInfoSiteId(owpMainMatch, infoSiteMatches);
        DeepAssert.Equal(expected, owpMainMatch);
    }

    [Fact]
    public void MapMainMatch_WhenOwpInfoSiteIdNotBelongToInfoSite_MapSuccessfully()
    {
        var owpMainMatch = new List<AssignmentModel>
        {
            new()
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
            new()
            {
                InfoSiteMatchId=345,
                LeagueName="SeaGame",
                HomeName="VN",
                AwayName="TL",
                AdminSiteMatchId=1111
            }
        };

        var expected = new List<AssignmentModel>
        {
            new()
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
            new()
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
            new()
            {
                InfoSiteMatchId=345,
                LeagueName="SeaGame",
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
            new()
            {
                InfoSiteMatchId=345,
                League="SeaGame",
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
            new()
            {
                League= "SeaGame",
                Home="VN",
                Away="TL"
            }
        };

        var infoSiteMatches = new List<InfositeMatch>
        {
            new()
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
            new()
            {
                League= "SeaGame",
                Home="VN",
                Away="TL"
            }
        };

        infositeMappingService.MapInfoSiteToMainMatch(owpMainMatch, infoSiteMatches);
        DeepAssert.Equal(expected, owpMainMatch);
    }
}