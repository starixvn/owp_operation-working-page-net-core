using Fanex.Data.Repository;

namespace OWP.Criteria;

public abstract class CommandCriteria : NonQueryCommand
{
    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }
}