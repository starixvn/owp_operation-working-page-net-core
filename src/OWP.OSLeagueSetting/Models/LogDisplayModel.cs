namespace OWP.OSLeagueSetting.Models
{
    public class LogDisplayModel
    {
        public string Action { get; set; }
        public OldDataLeagueSetting OldValue { get; set; }
        public NewDataLeagueSetting NewValue { get; set; }
        public string ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }
}