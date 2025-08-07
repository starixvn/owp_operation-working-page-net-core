
using Epplus.Mapper.Annotations;
using OWP.Utils;

namespace OWP.ProviderVerification.Models;

public class EventModel
{
    private string _time;

    public int OWPMatchId { get; set; }

    [Cell("A3")]
    public string MatchNo { get; set; }

    [Cell("B3")]
    public string MatchStatus { get; set; }

    public DateTime EventDateTime { get; set; }

    [Cell("C3")]
    public string EventDate => EventDateTime.Date.ToString(CultureInfo.InvariantCulture);

    [Cell("D3")]
    public string KoTime
    {
        get => _time;
        set =>
            _time = DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time)
                ? time.ToString("hh:mm tt", CultureInfo.InvariantCulture) : value;
    }

    [Cell("E3")]
    public string League { get; set; }

    [Cell("F3")]
    public string Home { get; set; }

    [Cell("G3")]
    public string Away { get; set; }

    [Cell("K3")]
    public string Remark { get; set; }

    [Cell("L3")]
    public string RemarkBy { get; set; }

    [Cell("H3")]
    public string LstBookProvider { get; set; }

    [Cell("I3")]
    public string InHouseProviderId { get; set; }

    public string ProviderMatchId { get; set; }

    public string Provider { get; set; }

    public string OtherProvider { get; set; }

    public decimal? MaxBet { get; set; }

    private string _maxBetToDisplay;
    public string MaxBetToDisplay
    {
        get => _maxBetToDisplay ??= MaxBet.ToOWPMaxBetFormat();
        set => _maxBetToDisplay = value;
    }
}

public class ProviderVerificationModel : EventModel
{
    public List<AgentConfirmOption> AgentConfirm { get; set; }

    [Cell("J3")]
    public string AgentConfirmList => AgentConfirm != null
        ? string.Join(", ", AgentConfirm.Where(x => !string.IsNullOrEmpty(x.Label)).Select(x => x.Label))
        : string.Empty;

    public List<ProviderMatchModel> ProviderMatch { get; set; }

    public string RowColor => MatchStatus switch
    {
        "Cancel" => "#FDFBDE",
        "Postponed" or "Suspended" => "#DDEFDA",
        _ => "#FFF"
    };

    public bool? HasTicket { get; set; }

    public bool? IsRead { get; set; }

}
