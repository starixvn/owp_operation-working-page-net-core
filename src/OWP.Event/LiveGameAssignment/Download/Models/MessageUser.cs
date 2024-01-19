namespace OWP.Event.LiveGameAssignment.Download.Models;

using System.Collections.Generic;
using OWP.Models;

public class MessageUser
{
    public int UserId { get; set; }

    public string UserName { get; set; }

    public IList<Message> Messages { get; set; }
}