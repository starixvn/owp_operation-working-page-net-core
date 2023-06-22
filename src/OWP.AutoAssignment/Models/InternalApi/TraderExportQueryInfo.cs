using OWP.Extensions;

namespace OWP.AutoAssignment.Models.InternalApi;

[XMLName("Row")]
public record TraderExportQueryInfo
{
    public int InfoSiteMatchId { get; init; }

    public int? FTHDPAssigneeId { get; init; }

    public int? FIRSTHHDPAssigneeId { get; init; }

    public int? FTOUAssigneeId { get; init; }

    public int? FIRSTHOUAssigneeId { get; init; }

    public int Bettype { get; init; }
}