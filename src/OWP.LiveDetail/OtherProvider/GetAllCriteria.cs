namespace OWP.LiveDetail.OtherProvider
{
    using Fanex.Data.Repository;

    public class GetAllCriteria : CriteriaBase
    {
        public override bool IsValid() => true;

        public override string GetSettingKey() => "OWP_OtherProvider_GetAll";
    }
}