namespace OWP.PreMatchReport.Models;

public class ReportModel
{
    public string TraderName { get; set; }
    public int TotalMatches { get; set; }
    public int TotalLeague { get; set; }

    #region HDP League

    public int MJ_HDPLeague { get; set; }
    public int M1_HDPLeague { get; set; }
    public int M2_HDPLeague { get; set; }
    public int M3_HDPLeague { get; set; }
    public int M4_HDPLeague { get; set; }
    public int Others_HDPLeague { get; set; }
    public int Others1_HDPLeague { get; set; }
    public int Others2_HDPLeague { get; set; }
    public int Others3_HDPLeague { get; set; }
    public int Others4_HDPLeague { get; set; }
    public int Others5_HDPLeague { get; set; }
    public int Unknown_HDPLeague { get; set; }
    public int Total_HDPLeague { get; set; }

    #endregion HDP League

    #region OU League

    public int MJ_OULeague { get; set; }
    public int M1_OULeague { get; set; }
    public int M2_OULeague { get; set; }
    public int M3_OULeague { get; set; }
    public int M4_OULeague { get; set; }
    public int Others_OULeague { get; set; }
    public int Others1_OULeague { get; set; }
    public int Others2_OULeague { get; set; }
    public int Others3_OULeague { get; set; }
    public int Others4_OULeague { get; set; }
    public int Others5_OULeague { get; set; }
    public int Unknown_OULeague { get; set; }
    public int Total_OULeague { get; set; }

    #endregion OU League

    #region HDP Matches

    public int MJ_HDPMatches { get; set; }
    public int M1_HDPMatches { get; set; }
    public int M2_HDPMatches { get; set; }
    public int M3_HDPMatches { get; set; }
    public int M4_HDPMatches { get; set; }
    public int Others_HDPMatches { get; set; }
    public int Others1_HDPMatches { get; set; }
    public int Others2_HDPMatches { get; set; }
    public int Others3_HDPMatches { get; set; }
    public int Others4_HDPMatches { get; set; }
    public int Others5_HDPMatches { get; set; }
    public int Unknown_HDPMatches { get; set; }
    public int Total_HDPMatches { get; set; }

    #endregion HDP Matches

    #region OU Matches

    public int MJ_OUMatches { get; set; }
    public int M1_OUMatches { get; set; }
    public int M2_OUMatches { get; set; }
    public int M3_OUMatches { get; set; }
    public int M4_OUMatches { get; set; }
    public int Others_OUMatches { get; set; }
    public int Others1_OUMatches { get; set; }
    public int Others2_OUMatches { get; set; }
    public int Others3_OUMatches { get; set; }
    public int Others4_OUMatches { get; set; }
    public int Others5_OUMatches { get; set; }
    public int Unknown_OUMatches { get; set; }
    public int Total_OUMatches { get; set; }

    #endregion OU Matches
}