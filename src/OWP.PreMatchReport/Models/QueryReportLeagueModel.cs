namespace OWP.PreMatchReport.Models
{
    public class ReportLeagueModel
    {
        public string TraderName { get; set; }
        
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
        #endregion

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
        #endregion
    }
}