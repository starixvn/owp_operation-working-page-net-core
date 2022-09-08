namespace OWP.LiveDetail.ReadyTradeStatus
{
    using Fanex.Data.Repository;

    public class InsertReadyTradeStatusCommand : NonQueryCommand
    {
        public int OWPMatchId { get; set; }

        public int UserId { get; set; }

        public override bool IsValid() => OWPMatchId > 0;

        public override string GetSettingKey() => "OWP_LiveDetail_ReadyTradeStatus_Insert";
    }
}
