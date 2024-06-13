using Fanex.Data.Repository;
using System;
using System.Globalization;

public class UpdateMarketOddCompilerCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public string SessionId { get; private set; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public int OWPMatchId { get; set; }

    public string Market { get; set; }

    public override string GetSettingKey() => "OWP_OddsCompiler_UpdateMarket";

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override bool IsValid() => ActorId > 0 && OWPMatchId > 0;
}