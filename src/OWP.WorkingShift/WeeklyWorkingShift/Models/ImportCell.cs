namespace OWP.WorkingShift.WeeklyWorkingShift.Models;

public class ImportCell
{
    protected ImportCell(string address)
    {
        Address = address;
    }

    public string Address { get; }
}
