using Fanex.Data.Repository;
using System;
using System.Globalization;

namespace OWP.OddsCompiler.Criteria;
public class SendMarketNotificationCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public string SessionId { get; private set; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public int OWPMatchId { get; set; }

    public string OldValue { get; set; }

    public string NewValue { get; set; }

    public string TemplateCode { get; private set; } = "EditMarket";

    public string AssigneeIds { get; set; }

    public string WatcherIds { get; set; }

    public override string GetSettingKey() => "OWP_MatchInfo_SendNotification";

    public override bool IsValid() => OWPMatchId > 0;
}