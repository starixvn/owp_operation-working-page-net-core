namespace OWP.Services;

public class MaintenanceService
{
    private bool _isUnderMaintenance;
    private string _fromTime;
    private string _toTime;

    public bool IsUnderMaintenance()
    {
        return _isUnderMaintenance;
    }

    public string GetFromTime()
    {
        return _fromTime ?? string.Empty;
    }

    public string GetToTime()
    {
        return _toTime ?? string.Empty;
    }

    public void SetMaintenanceMode(bool isUnderMaintenance)
    {
        SetMaintenanceMode(isUnderMaintenance, null, null);
    }

    public void SetMaintenanceMode(bool isUnderMaintenance, string from, string to)
    {
        _isUnderMaintenance = isUnderMaintenance;
        _fromTime = from;
        _toTime = to;
    }
}