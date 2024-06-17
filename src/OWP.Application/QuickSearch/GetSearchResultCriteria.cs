using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.Application.QuickSearch;
public class GetSearchResultCriteria : CriteriaBase
{
    public int UserId { get; set; }

    public string SearchKeywords { get; set; }

    public bool IsNumericKeywords { get; set; }

    public bool IsFullTimeView { get; set; }

    public bool IsTimeRange { get; set; }

    public override bool IsValid() => !string.IsNullOrWhiteSpace(SearchKeywords) && UserId > 0;

    public override string GetSettingKey() => "OWP_QuickSearchMatchGet";
}