namespace OWP.WorkingShift.Common.Models
{
    public class TimezoneSettingModel
    {
        public byte SettingId { get; set; }

        public string Name { get; set; }

        public int Offset { get; set; }

        public bool IsSelected { get; set; }
    }
}
