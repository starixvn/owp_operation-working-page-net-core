namespace OWP.WorkingShift.WeeklyWorkingShift.Models;

using System.Xml.Serialization;

[XmlRoot("Data")]
public class WorkingShiftUploadData
{
    [XmlElement("Row")]
    public WorkingShiftImportEntity[] WorkingShifts { get; set; }
}
