namespace OWP.TicketCenter.ViewModels;

public class UserViewModel
{
    public string NickName
    { get; set; }

    public int? RecipientId
    { get; set; }

    public int RecipientType => (int)Enum.RecipientType.Individual;

    public string UserName
    { get; set; }
}