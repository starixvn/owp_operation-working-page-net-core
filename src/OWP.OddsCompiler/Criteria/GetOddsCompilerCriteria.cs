using Fanex.Data.Repository;
using System;


namespace OWP.OddsCompiler.Criteria;
public class GetOddsCompilerCriteria : CriteriaBase
{
    public string League { get; set; }

    public DateTime Date { get; set; }

    public string MatchStatusIds { get; set; }

    public bool IsExcludeSpecialProducts { get; set; }

    public override string GetSettingKey() => "OWP_OddsCompiler_Get";

    public override bool IsValid() => Date > DateTime.MinValue;
}