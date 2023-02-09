using Fanex.Data.Repository;

namespace OWP.AutoAssignment.Criteria.InternalApi;

public abstract class ExportInfoCriteriaBase : CriteriaBase
{
    public string Xml { get; set; }
}