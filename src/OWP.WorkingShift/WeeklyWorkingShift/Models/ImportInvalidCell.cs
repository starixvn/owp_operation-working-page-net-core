namespace OWP.WorkingShift.WeeklyWorkingShift.Models
{
    public class ImportInvalidCell : ImportCell
    {
        public ImportInvalidCell(string address, string type, string message) : base(address)
        {
            Type = type;
            Message = message;
        }

        public string Type { get; set; }

        public string Message { get; set; }
    }
}
