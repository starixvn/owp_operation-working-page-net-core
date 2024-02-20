using Newtonsoft.Json;
using OWP.Extensions;

namespace OWP.Event.LiveGameAssignment.EventPicking.Model;

public class LeagueSettingNewFormat
{
    public int LeagueSettingId { get; set; }

    public string LeagueName { get; set; }

    public string Remark { get; set; }

    public string FormatedRemark
    => StringExtension.ConvertNullToEmpty(Remark);

    public string LSTNote { get; set; }

    public string FormatedLSTNote
        => StringExtension.ConvertNullToEmpty(LSTNote);

    public bool HasTV { get; set; }

    public string FullConditionDescription { get; set; }

    public decimal? Spread_HDP { get; set; }

    public decimal? Spread_OU { get; set; }

    //WD
    public string MaxbetWD_MatchOffer { get; set; }

    public decimal? MaxbetWD_HDP { get; set; }

    public decimal? MaxbetWD_OU { get; set; }

    public string MaxbetWD_NoOfHDP { get; set; }

    //WK
    public string MaxbetWK_MatchOffer { get; set; }

    public decimal? MaxbetWK_HDP { get; set; }

    public decimal? MaxbetWK_OU { get; set; }

    public string MaxbetWK_NoOfHDP { get; set; }

    public LeagueSettingCondition GetFullConditionDes()
        => string.IsNullOrEmpty(FullConditionDescription) ? new LeagueSettingCondition() : JsonConvert.DeserializeObject<LeagueSettingCondition>(FullConditionDescription);
}