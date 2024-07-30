using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace OWP.AssignmentUpload.Validation.Constants;

[SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Reviewed.")]
public enum MinorBettype
{
    [Description("Fast Market")]
    FastMarket = -88,

    [Description("All In One")]
    AllInOne = -89,

    [Description("Specific 15 Mins")]
    Specific15Mins = -1,

    [Description("Specific 10 Mins")]
    Specific10Mins = -2,

    [Description("Specific 15 Mins Total Of Bookings")]
    Specific15MinsTotalBookings = -8,

    [Description("Specific 15 Mins Number Of Corners")]
    Specific15MinsCorners = -5,


    [Description("Total Booking")]
    TotalBooking = -6,

    [Description("No.of Corner")]
    NoOfCorner = -3,

    [Description("All In One Total Of Bookings")]
    AllInOneTotalBookings = -189,

    [Description("All In One Number Of Corners")]
    AllInOneCorners = -289,

    [Description("Fast Market Total Of Bookings")]
    FastMarketTotalBookings = -188,

    [Description("Fast Market Number Of Corners")]
    FastMarketCorners = -288
}

public static class MinorBettypeConst
{
    public static readonly ImmutableList<int> MinorBettypeIdGenerateBySystem = ImmutableList<int>.Empty.AddRange(new List<int>
    {
        -88, -89, -1, -2, -8, -5, -188, -288, -189, -289
    });
}

