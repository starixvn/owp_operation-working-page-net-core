using OWP.OtherSport.Submit.Models;
using OWP.OtherSport.Validation.Models;
using System.Collections.Generic;

namespace OWP.OtherSport.Download.Models
{
    public class ExcelModel
    {
        public ExcelHeaderInfoModel HeaderInfo { get; set; }

        public ExcelAssignmentModel ExcelAssignmentModels { get; set; }
    }

    public class ExcelAssignmentModel
    {
        public List<MatchInfo> Matches { get; set; }

        public Dictionary<int, List<ExcelMatchMaxBet>> MaxBet { get; set; }

        public Dictionary<int, List<ExcelMatchAssignment>> MatchAssignments { get; set; }

        public ExcelAssignmentModel()
        {
            Matches = new List<MatchInfo>();
            MaxBet = new Dictionary<int, List<ExcelMatchMaxBet>>();
            MatchAssignments = new Dictionary<int, List<ExcelMatchAssignment>>();
        }
    }

    public class ExcelMatchMaxBet: MatchSetting
    {
        public int ColumnIndex { get; set; }

        public string Value { get; set; }
    }

    public class ExcelMatchAssignment : MatchAssignment
    {
        public int ColumnIndex { get; set; }

        public string Value { get; set; }
    }
}
