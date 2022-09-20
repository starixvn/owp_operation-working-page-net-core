namespace OWP.LiveDetail.MatchSetting
{
    using System;
    using System.Globalization;
    using Fanex.Data.Repository;

    public class CreateCriteria : NonQueryCommand
    {
        public string SessionId { get; private set; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

        public int UserId { get; set; }

        public int? MinorBettypeId { get; set; }

        public int MatchId { get; set; }

        public int BettypeId { get; set; }

        public string Bettype { get; set; }

        public decimal? Maxbet { get; set; }

        public decimal? Spread { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => "OWP_LiveDetail_MatchSetting_Create";
    }
}