using Fanex.Data.Repository;

namespace OWP.OtherSport.Validation.Criteria;

public class ExistsOSMatchCriteria : INonQueryCommand
{
    public string OSMatches { get; set; }

    public string GetSettingKey() => "OWP_OSAssignmentUpload_ExistsOSMatch";

    public bool IsValid() => true;
}