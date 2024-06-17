using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.Application.QuickSearch;
public class QueryModel
{
    public int UserId { get; set; }

    public string SearchKeywords { get; set; }

    public int SelectTop { get; set; }

    public int? LastOWPMatchId { get; set; }

    public bool IsNumericKeywords { get; set; }

    public bool QueryFullTime { get; set; }

    public bool QueryTimeRange { get; set; }
}
