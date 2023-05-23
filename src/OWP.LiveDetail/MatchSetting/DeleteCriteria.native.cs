using System.Globalization;
using Fanex.Data.Repository;

namespace OWP.LiveDetail.MatchSetting;

public class DeleteCriteria : NonQueryCommand
{
    public string SessionId { get; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public int UserId { get; set; }

    public override string GetSettingKey() => "OWP_LiveDetail_MatchSetting_Delete";

    public int? MinorBettypeId { get; set; }

    public int MatchId { get; set; }

    public int BettypeId { get; set; }

    public string Bettype { get; set; }

    public override bool IsValid() => true;
}