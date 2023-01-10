namespace OWP.ProviderArrangement.Models;

public class LogDisplayModel
{
    public string Action { get; set; }
    public List<string> OldValue { get; set; }
    public List<string> NewValue { get; set; }
    public string ModifyDate { get; set; }
    public string ModifyBy { get; set; }
}