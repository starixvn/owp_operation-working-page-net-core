namespace OWP.RecipientGroup.Criteria
{
    using Fanex.Data.Repository;

    public class SaveRecipientGroupCommand : NonQueryCommand
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string Recipients { get; set; }

        public override string GetSettingKey() => "OWP_RecipientGroup_SaveRecipientGroup";

        public override bool IsValid() => true;
    }
}
