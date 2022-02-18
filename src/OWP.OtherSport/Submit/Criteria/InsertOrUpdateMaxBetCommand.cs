namespace OWP.OtherSport.Submit.Criteria
{
    using Fanex.Data.Repository;

    public class InsertOrUpdateMaxBetCommand : NonQueryCommand
    {
        public string MaxBets { get; set; }

        public string MatchIds { get; set; }

        public override string GetSettingKey() => "OWP_OSAssignmentUpload_MaxBetInsertOrUpdate";

        public override bool IsValid() => true;
    }
}