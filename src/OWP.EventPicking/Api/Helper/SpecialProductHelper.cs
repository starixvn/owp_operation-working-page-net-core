using OWP.Constants;
using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Helper;
internal static class SpecialProductHelper
{
    public static void BuildSpecialProduct(IList<EventPickingModel> events)
    {
        foreach (var @event in events)
        {
            var (leaguePostfix, teamPostfix) = BuildPostFixSpecialProduct(@event.MinorBettypeId);
            @event.LeagueName = $"{@event.LeagueName} {leaguePostfix}";
            @event.HomeName = $"{@event.HomeName} {teamPostfix}";
            @event.AwayName = $"{@event.AwayName} {teamPostfix}";
        }
    }

    public static (string leaguePostfix, string teamPostfix) BuildPostFixSpecialProduct(int? minorBettypeId)
    {
        return minorBettypeId switch
        {
            Bettype.NoOfCorner => ("- CORNERS", "- No.of Corners"),
            Bettype.NextCorner => ("- CORNERS", "- Next Corners"),
            Bettype.TotalBooking => ("- BOOKING", "- Total Bookings"),
            Bettype.NextBooking => ("- BOOKING", "- Next Booking"),
            _ => (string.Empty, string.Empty)
        };
    }
}
