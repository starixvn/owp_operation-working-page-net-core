namespace OWP.AssigneeSuggestionList.Models;

public class ETPKUpdateSchedule
{
    public ETPKUpdateSchedule() { }

    public ETPKUpdateSchedule(string etpk)
    {
        EtPkValue = etpk;
    }

    public bool IsHasEtPk => !string.IsNullOrEmpty(EtPkValue);

    public string EtPkValue { get; set; }
}