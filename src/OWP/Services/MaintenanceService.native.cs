namespace OWP.Services;

public class MaintenanceService
{
    private bool _isUnderMaintenance;

    public bool IsUnderMaintenance()
    {
        return _isUnderMaintenance;
    }

    public void SetMaintenanceMode(bool isUnderMaintenance)
    {
        _isUnderMaintenance = isUnderMaintenance;
    }
}