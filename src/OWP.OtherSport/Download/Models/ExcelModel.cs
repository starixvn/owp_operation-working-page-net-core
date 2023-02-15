namespace OWP.OtherSport.Download.Models;

using Submit.Models;

public class ExcelModel
{
    public ExcelHeaderInfoModel HeaderInfo { get; set; }

    public ExcelAssignmentModel ExcelAssignmentModels { get; set; }
}

public class ExcelAssignmentModel
{
    public List<MatchInfo> Matches { get; set; }

    public Dictionary<int, List<ExcelMatchMaxBetSpread>> MaxBetSpread { get; set; }

    public Dictionary<int, List<ExcelMatchAssignment>> MatchAssignments { get; set; }

    public ExcelAssignmentModel()
    {
        Matches = new List<MatchInfo>();
        MaxBetSpread = new Dictionary<int, List<ExcelMatchMaxBetSpread>>();
        MatchAssignments = new Dictionary<int, List<ExcelMatchAssignment>>();
    }
}

public class ExcelMatchMaxBetSpread : MatchSetting
{
    public int ColumnIndexMaxbet { get; set; }

    public string ValueMaxbet { get; set; }

    public int ColumnIndexSpread { get; set; }

    public string ValueSpread { get; set; }
}

public class ExcelMatchAssignment : MatchAssignment
{
    public int ColumnIndex { get; set; }

    public string Value { get; set; }
}