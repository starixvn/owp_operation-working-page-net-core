namespace OWP.Criteria;

using Fanex.Data.Repository;

public abstract class CommandCriteria : NonQueryCommand
{
    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }
}