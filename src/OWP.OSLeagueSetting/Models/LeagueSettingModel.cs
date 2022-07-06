using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.OSLeagueSetting.Models;

public class LeagueSettingModel
{
    public int LeagueSettingId { get; set; }

    public string CategoryName { get; set; }

    public string OSLeagueName { get; set; }

    public bool IsActive { get; set; }

    public double MaxBetHDP { get; set; }

    public double MaxBetOU { get; set; }

    public double SpreadHDP { get; set; }

    public double SpreadOU { get; set; }

    public string RoundName { get; set; }

    public string SummaryInfo { get; set; }

    public string SummaryRemark { get; set; }
}