using Fanex.Data.Repository;

namespace OWP.OtherSport.Submit.Criteria;
public class UpdateOfferLiveCriteria : NonQueryCommand
{
    public string Xml { get; set; }

    public override string GetSettingKey() => "OWP_OSLiveGameAssignment_UpdateOfferLive";

    public override bool IsValid() => true;
}
