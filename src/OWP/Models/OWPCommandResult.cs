namespace OWP.Model;

public class OWPCommandResult
{
    public int Code { get; set; } = 1;


    public string Message { get; set; }


    public virtual bool IsSuccessful => true;

}