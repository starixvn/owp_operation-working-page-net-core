namespace OWP.LiveDetail.MatchInfo
{
    using Common;
    using OWP.Model;

    public class CommandResult : OWPCommandResult
    {
        public string SessionId { get; set; }

        public ValidateResult ValidateResult { get; set; }
    }
}