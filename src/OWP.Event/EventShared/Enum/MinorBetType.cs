using System.ComponentModel;

namespace OWP.Event.EventShared.Enum;

public enum MinorBetType
{
    [Description("Specific 15 Mins")]
    Specific15Mins = -1,

    [Description("Specific 10 Mins")]
    Specific10Mins = -2,

    [Description("No.of Corner")]
    NoOfCorner = -3,

    [Description("Specific 15 Mins Number Of Corners")]
    Specific15MinsCorners = -5,

    [Description("Total Booking")]
    TotalBooking = -6,

    [Description("Specific 15 Mins Total Of Bookings")]
    Specific15MinsTotalBookings = -8,

    [Description("Fast Market")]
    FastMarket = -88,

    [Description("All In One")]
    AllInOne = -89,

    [Description("All In One Number Of Corners")]
    AllInOneCorners = -289,

    [Description("All In One Total Of Bookings")]
    AllInOneTotalBookings = -189,

    [Description("Fast Market Number Of Corners")]
    FastMarketCorners = -288,

    [Description("Fast Market Total Of Bookings")]
    FastMarketTotalBookings = -188

}