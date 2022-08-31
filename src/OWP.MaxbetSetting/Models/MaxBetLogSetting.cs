namespace OWP.MaxbetSetting.Models
{
    public class MaxBetLogSetting
    {
        public int Action { get; set; }
        public MaxBetSettingModel OldValue { get; set; }
        public MaxBetSettingModel NewValue { get; set; }
        public string ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }
}