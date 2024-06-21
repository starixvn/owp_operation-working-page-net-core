using System;
using System.Collections.Generic;


namespace OWP.OddsCompiler.Models;
public class QueryModel
{
    public DateTime Date { get; set; }

    public string League { get; set; }

    public List<string> MatchStatusIds { get; set; } = new();

    public bool IsExcludeSpecialProducts { get; set; }
}
