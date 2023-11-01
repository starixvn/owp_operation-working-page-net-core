using OWP.AssignmentUpload.SyncInfoSite.Helper;
using OWP.AssignmentUpload.SyncInfoSite.Models;
using OWP.AssignmentUpload.Validation.Models;
using OWP.AssignmentUpload.Validation.Services;

namespace OWP.AssignmentUpload.SyncInfoSite.Services;

public interface InfositeMappingService
{
    void MapInfoSiteToMainMatch(List<AssignmentModel> owpMainMatch, List<InfositeMatch> infositeMatches);

    void MapMainInfoToMinor(
        List<AssignmentModel> owpMinorMatch,
        List<MainMatchBackupModel> mainMatchOriginModel,
        List<AssignmentModel> newMainMatch);
}

public class InfositeMappingServiceImp : InfositeMappingService
{
    public void MapInfoSiteToMainMatch(List<AssignmentModel> owpMainMatch, List<InfositeMatch> infositeMatches)
    {
        MapMainMatchNotExistInfoSiteId(owpMainMatch, infositeMatches);
        MapMainMatchExistInfoSiteId(owpMainMatch, infositeMatches);
    }

    public void MapMainInfoToMinor(
        List<AssignmentModel> owpMinorMatch,
        List<MainMatchBackupModel> mainMatchOriginModel,
        List<AssignmentModel> newMainMatch)
    {
        foreach (var minorMatch in owpMinorMatch)
        {
            var mainMatch = newMainMatch.First(a => string.Equals(a.HashKey, minorMatch.ParentHashKey));
            var minorLeague = minorMatch.League;
            var minorHome = minorMatch.Home;
            var minorAway = minorMatch.Away;
            var mainMatchOrigin = mainMatchOriginModel.First(m => string.Equals(m.key, minorMatch.ParentHashKey));
            minorMatch.League = minorLeague.Replace(mainMatchOrigin.League, mainMatch.League);
            minorMatch.Home = minorHome.Replace(mainMatchOrigin.Home, mainMatch.Home);
            minorMatch.Away = minorAway.Replace(mainMatchOrigin.Away, mainMatch.Away);
            minorMatch.IsSaba = mainMatch.IsSaba;
        }
    }

    public static void MapMainMatchExistInfoSiteId(List<AssignmentModel> owpMainMatch, List<InfositeMatch> infositeMatches)
    {
        var owpMatch = InfoSiteHelper.GetMainMatchExistsInfoSiteId(owpMainMatch);
        foreach (var match in owpMatch)
        {
            var infoSiteMatch = infositeMatches.Find(i => i.InfoSiteMatchId == match.InfoSiteMatchId);
            if (infoSiteMatch != null)
            {
                MapInfo(match, infoSiteMatch);
            }
        }
    }

    public static void MapMainMatchNotExistInfoSiteId(List<AssignmentModel> owpMainMatch, List<InfositeMatch> infositeMatches)
    {
        var owpMatch = InfoSiteHelper.GetMainMatchNotExistsInfoSiteId(owpMainMatch);
        foreach (var match in owpMatch)
        {
            var infoSiteMatch = match.IsSaba
                ? infositeMatches.Find(infoSite => InfoSiteHelper.IsSameLeagueHomeAwayKoTime(match, infoSite))
                : infositeMatches.Find(infoSite => InfoSiteHelper.IsSameLeagueHomeAway(match, infoSite));

            if (infoSiteMatch != null)
            {
                MapInfo(match, infoSiteMatch);
            }
        }
    }

    public static bool IsSabaMatch(string league)
        => AssignmentMapperImp.IsSabaMatch(league);

    private static void MapInfo(AssignmentModel owpMatch, InfositeMatch infositeMatch)
    {
        owpMatch.InfoSiteMatchId = infositeMatch.InfoSiteMatchId;
        owpMatch.League = infositeMatch.LeagueName;
        owpMatch.Home = infositeMatch.HomeName;
        owpMatch.Away = infositeMatch.AwayName;
        owpMatch.Round = infositeMatch.Round;
        owpMatch.Division = infositeMatch.Division;
        owpMatch.Agent = infositeMatch.Agent;
        owpMatch.TV = infositeMatch.TV;
        owpMatch.Channel = infositeMatch.Channel;
        owpMatch.WB = infositeMatch.WB;
        owpMatch.Website1 = infositeMatch.Website1;
        owpMatch.Website2 = infositeMatch.Website2;
        owpMatch.Website3 = infositeMatch.Website3;
        owpMatch.Website4 = infositeMatch.Website4;
        owpMatch.InfoSiteMatchId = infositeMatch.InfoSiteMatchId;
        owpMatch.IPTV = infositeMatch.IPTV;
        owpMatch.AdminSiteId = infositeMatch.AdminSiteMatchId;
        owpMatch.IsNeutral = infositeMatch.IsNeutral;
        owpMatch.EtPk = infositeMatch.EtPk;
        owpMatch.IsSaba = IsSabaMatch(infositeMatch.LeagueName);
    }
}