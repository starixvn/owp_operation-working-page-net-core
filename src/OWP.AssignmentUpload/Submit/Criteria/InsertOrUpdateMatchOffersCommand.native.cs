using Fanex.Data.Repository;

namespace OWP.AssignmentUpload.Submit.Criteria;

public class InsertOrUpdateMatchOffersCommand : NonQueryCommand
{
    public string MatchOffers { get; set; }

    public string MatchIds { get; set; }

    public override string GetSettingKey() => "OWP_AssignmentUpload_MatchOffersInsertOrUpdate";

    public override bool IsValid() => true;
}