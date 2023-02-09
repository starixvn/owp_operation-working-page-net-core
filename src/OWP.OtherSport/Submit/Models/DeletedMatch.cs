using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.OtherSport.Submit.Models;

public class DeletedMatch
{
    public int OSMatchId { get; set; }

    public int? AdminSiteMatchId { get; set; }

    public string Sport { get; set; }

    public DateTime EventDate { get; set; }
}