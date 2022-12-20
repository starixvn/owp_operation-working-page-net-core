using OWP.AssignmentUpload.SyncInfoSite.Models;
using OWP.AssignmentUpload.Validation.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OWP.AssignmentUpload.SyncInfoSite.Helper
{
    public static class InfoSiteHelper
    {
        public static bool IsSameLeagueHomeAway(AssignmentModel owpMatch, InfositeMatch infoSiteMatch)
            => string.Equals(infoSiteMatch.LeagueName, owpMatch.League, StringComparison.CurrentCultureIgnoreCase) &&
               string.Equals(infoSiteMatch.HomeName, owpMatch.Home, StringComparison.CurrentCultureIgnoreCase) &&
               string.Equals(infoSiteMatch.AwayName, owpMatch.Away, StringComparison.CurrentCultureIgnoreCase);

        public static bool IsSameLeagueHomeAwayKOtime(AssignmentModel owpMatch, InfositeMatch infoSiteMatch)
        => IsSameLeagueHomeAway(owpMatch, infoSiteMatch) &&
           owpMatch.EventDate.Add(owpMatch.KickOffTime) == infoSiteMatch.EventDateTime;

        public static List<AssignmentModel> GetMainMatch(List<AssignmentModel> owpMatch)
          => owpMatch.Where(a => string.IsNullOrWhiteSpace(a.ParentHashKey)).ToList();

        public static List<AssignmentModel> GetMinorMatch(List<AssignmentModel> owpmatch)
            => owpmatch.Where(a => !string.IsNullOrWhiteSpace(a.ParentHashKey)).ToList();

        public static List<AssignmentModel> GetMainMatchNotExistsInfoSiteId(List<AssignmentModel> owpMatch)
           => owpMatch.Where(a => a.InfoSiteMatchId <= 0).ToList();

        public static List<AssignmentModel> GetMainMatchExistsInfoSiteId(List<AssignmentModel> owpMatch)
            => owpMatch.Where(a => a.InfoSiteMatchId > 0).ToList();
    }
}
