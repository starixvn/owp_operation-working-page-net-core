using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.ViewModels;

public class TicketDetailBaseViewModel
{
    public int TicketId { get; set; }

    public int TicketType { get; set; }

    public string TicketStatus { get; set; }

    public string TypeName { get; set; }

    public int? TicketSubTypeId { get; set; }

    public string TicketSubTypeName { get; set; }

    public IList<RecipientResponseModel> Recipients { get; set; }

    public IList<MessageDetailViewModel> Messages { get; set; }

    public DateTime? CommentLatestDate => Messages.FirstOrDefault()?.CreatedDate;

    public IssueCreatorModel CreatorInfo { get; set; }

    public bool IsRead { get; set; }

    public bool IsSaved { get; set; }

    public bool IsPinned { get; set; }

    public int EventType { get; set; }
}