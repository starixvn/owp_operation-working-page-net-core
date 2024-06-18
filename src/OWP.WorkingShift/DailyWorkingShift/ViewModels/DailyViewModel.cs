namespace OWP.WorkingShift.DailyWorkingShift.ViewModels;


public class DailyViewModel
{
    public DateTime Date { get; set; }

    public IEnumerable<ReferenceResource> Sections { get; set; }

    public IEnumerable<ReferenceResource> Departments { get; set; }

    public IEnumerable<UserLocation> UserLocations { get; set; }

    public DailyWorkingShiftPermission Permission { get; set; }

    public string Username { get; set; }
}