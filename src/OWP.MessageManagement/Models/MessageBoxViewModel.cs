
using System.Collections.Generic;

namespace OWP.MessageManagement.Models;
public class MessageBoxViewModel
{
    public IList<MessageModel> Messages { get; set; }

    public int TotalRecords { get; set; }
}
