namespace OWP.MaxbetSetting.Models
{
    public class LogDisplayModel
    {
        public int Action { get; set; }
        public MaxBetLogSettingModel OldValue { get; set; }
        public MaxBetLogSettingModel NewValue { get; set; }
        public string ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }

    public class MaxBetLogSettingModel
    {
        public string FT_HDP { get; set; }
        public string FT_OU { get; set; }
        public string FirstHalf_HDP { get; set; }
        public string FirstHalf_OU { get; set; }
        public string NoOfCorner_FT_HDP { get; set; }
        public string NoOfCorner_FT_OU { get; set; }
        public string NoOfCorner_1H_HDP { get; set; }
        public string NoOfCorner_1H_OU { get; set; }
        public string NextCorner_FT_HDP { get; set; }
        public string TotalBooking_FT_HDP { get; set; }
        public string TotalBooking_FT_OU { get; set; }
        public string TotalBooking_1H_HDP { get; set; }
        public string TotalBooking_1H_OU { get; set; }
        public string NextBooking_FT_HDP { get; set; }
        public string SecondAccept_Main { get; set; }
        public string SecondAccept_Corner { get; set; }
        public string SecondAccept_TotalBooking { get; set; }
        public string SecondAccept_Fantasy { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}