using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Data.Repository;

namespace OWP.OSLeagueSetting.Criteria;

public class GetLeagueSettingByLeagueNameCriteria : CriteriaBase
{
    public string LeagueName { get; set; }

    public  int? LeagueSettingId { get; set; }

    public override string GetSettingKey() => "OWP_OSLeagueSetting_GetLeagueSettingByLeagueName";

    public override bool IsValid() => true;
}