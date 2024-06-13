namespace OWP.WorkingShift.WeeklyWorkingShift.Models
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public class WorkingShiftImportEntity
    {
        [XmlAttribute("EmployeeId")]
        public int EmployeeId { get; set; }

        [XmlAttribute(AttributeName = "ShiftDate", DataType = "date")]
        public DateTime ShiftDate { get; set; }

        [XmlAttribute("ShiftId")]
        public int ShiftId { get; set; }
    }
}