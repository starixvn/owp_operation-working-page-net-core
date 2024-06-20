using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.Models;
public class NotificationEventArgs
{
    public int UserId { get; set; }

    public IList<Message> Messages { get; set; }
}