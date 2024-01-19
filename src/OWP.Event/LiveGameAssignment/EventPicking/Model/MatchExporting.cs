using Epplus.Mapper.Annotations;
using Olympus.InfoSiteClient.Models;

namespace OWP.Event.LiveGameAssignment.EventPicking.Model;

public class MatchExporting
{
    private static readonly DateTime MinDate = new DateTime(2000, 1, 1);

    public MatchExporting(CommonMatchEx infositeMatch)
    {
        var extInfo = infositeMatch.ext_info ?? new SoccerEventSchedule();
        InfoSiteMatchKey = $"{infositeMatch.match_id}-{infositeMatch.website_id}-{infositeMatch.is_import}";
        InfoSiteMatchId = infositeMatch.match_id;
        InfoSiteMergeMatchKey = GetInfoSiteMergeMatchKey(infositeMatch);
        MatchId = GetMatchId(infositeMatch);
        LeagueName = infositeMatch.league_name;
        HomeName = GetHomeName(infositeMatch, extInfo);
        AwayName = GetAwayName(infositeMatch, extInfo);
        WatchBet = ConvertNullToEmpty(extInfo.wb);
        Round = ConvertNullToEmpty(extInfo.round);
        OutSide = ConvertNullToEmpty(extInfo.code);
        Agent = ConvertNullToEmpty(extInfo.agent);
        ExtraTimeRemark = ConvertNullToEmpty(extInfo.et_pk);
        Venue = ConvertNullToEmpty(extInfo.venue);
        LocalTV = ConvertNullToEmpty(extInfo.local_tv);
        LiveTV = ConvertNullToEmpty(extInfo.tv_live);
        Website = ConvertNullToEmpty(extInfo.website1);
        Note = GetNote(extInfo);
        Provider = GetProvider(infositeMatch.ext_info?.provider);
        UpdatedTime = GetUpdateTime(infositeMatch, extInfo);

        IsChecked = extInfo.rd_checked.HasValue && extInfo.rd_checked.Value;
        GameDate = GetGameDate(infositeMatch);
        Agent = extInfo.agent;
        parent_create_match_id = extInfo.parent_create_match_id.ToString();
        home_remark = extInfo.home_remark;
        away_remark = extInfo.away_remark;
        game_date = extInfo.game_date.ToString();
        op = extInfo.op;
        lt_value = extInfo.lt_value;
        et_pk = extInfo.et_pk;
        website1 = extInfo.website1;
        website2 = extInfo.website2;
        website3 = extInfo.website3;
        website4 = extInfo.website4;
        provider = extInfo.provider;
        provider_use = extInfo.provider_use;
        create_time = extInfo.create_time.ToString();
        create_by = extInfo.create_by.ToString();
        update_time = extInfo.update_time.ToString();
        remark = extInfo.remark;
        remark2 = extInfo.remark2;
        round_total = extInfo.round_total;
        create_streaming_src = extInfo.create_streaming_src.ToString();
        create_streaming_id = extInfo.create_streaming_id;
        create_is_live = extInfo.create_is_live.HasValue && extInfo.create_is_live.Value
            ? "Live"
            : string.Empty;
        create_match_code = extInfo.create_match_code;
    }

    public MatchExporting(ChildMatch childMatch)
    {
        InfoSiteMatchKey = string.Empty;
        InfoSiteMatchId = childMatch.match_id;
        InfoSiteMergeMatchKey = string.Empty;
        MatchId = unchecked((int)childMatch.create_match_id.GetValueOrDefault(0));
        LeagueName = childMatch.league_name;
        HomeName = childMatch.home_name;
        AwayName = childMatch.away_name;
        WatchBet = string.Empty;
        Round = string.Empty;
        OutSide = string.Empty;
        Agent = string.Empty;
        ExtraTimeRemark = string.Empty;
        Venue = string.Empty;
        LocalTV = string.Empty;
        LiveTV = string.Empty;
        Website = string.Empty;
        Note = string.Empty;
        Provider = string.Empty;
        UpdatedTime = MinDate;

        IsChecked = true;
        GameDate = ConvertTimeToUTC8(childMatch.game_date.GetValueOrDefault(MinDate));
        Agent = string.Empty;
        parent_create_match_id = childMatch.ibc_match_id.ToString();
        home_remark = string.Empty;
        away_remark = string.Empty;
        game_date = GameDate.ToString();
        op = string.Empty;
        lt_value = string.Empty;
        et_pk = string.Empty;
        website1 = string.Empty;
        website2 = string.Empty;
        website3 = string.Empty;
        website4 = string.Empty;
        provider = string.Empty;
        provider_use = string.Empty;
        create_time = string.Empty;
        create_by = string.Empty;
        update_time = string.Empty;
        remark = string.Empty;
        remark2 = string.Empty;
        round_total = string.Empty;
        create_streaming_src = string.Empty;
        create_streaming_id = string.Empty;
        create_is_live = string.Empty;
        create_match_code = childMatch.create_match_code;

        // 0: live, 1: live+deadball, 2: hedge
        create_option = childMatch.create_option;

        // 0: closed.
        // 1: running.
        // 2: postponed.
        // 3: internal.
        // 4: completed.
        create_event_status = childMatch.create_event_status <= 4 ? new[] { "closed", "running", "postponed", "internal", "completed" }[childMatch.create_event_status.Value] : string.Empty;
        // Is Live message.
        create_is_live_msg = childMatch.create_is_live_msg;

        // Is link parent to child. if set true, this use for "child attribute follow parent when parent changed". BUT this function no implement due to requirement no need
        is_link = childMatch.is_link.ToString();
        // 0:Child League, 1:ExtraTime, 2:Penalty Handicap 3:Penalty Over-Under
        league_type = childMatch.league_type.HasValue && childMatch.league_type.Value <= 3 ? new[] { "Child League", "ExtraTime", "Penalty Handicap", "Penalty Over-Under" }[childMatch.league_type.Value] : string.Empty;
        // Is delete.
        is_delete = childMatch.is_delete.ToString();
    }

    public DateTime GameDate { get; set; }

    public bool IsChecked { get; set; }

    [Cell("A2")]
    public string EventDate => GameDate.Date.ToString();

    [Cell("B2")]
    public string KickOffTime => GameDate.ToString("hh:mm tt");

    public string InfoSiteMatchKey { get; set; }

    public string InfoSiteMergeMatchKey { get; set; }

    [Cell("C2")]
    public int? MatchId { get; set; }

    [Cell("D2")]
    public string LeagueName { get; set; }

    [Cell("E2")]
    public string HomeName { get; set; }

    [Cell("F2")]
    public string AwayName { get; set; }

    [Cell("G2")]
    public string MatchOffer { get; set; }

    [Cell("H2")]
    public string Maxbet_HDP { get; set; }

    [Cell("I2")]
    public decimal? Spread_HDP { get; set; }

    [Cell("J2")]
    public string Maxbet_OU { get; set; }

    [Cell("K2")]
    public decimal? Spread_OU { get; set; }

    [Cell("L2")]
    public string NoOfHDP { get; set; }

    [Cell("M2")]
    public string LeagueRemark { get; set; }

    [Cell("N2")]
    public string LSTNote { get; set; }

    [Cell("O2")]
    public string Note { get; set; }

    [Cell("P2")]
    public string OutSide { get; set; }

    [Cell("Q2")]
    public string Round { get; set; }

    [Cell("R2")]
    public string Venue { get; set; }

    [Cell("S2")]
    public string LocalTV { get; set; }

    [Cell("T2")]
    public string LiveTV { get; set; }

    public string ExtraTimeRemark { get; set; }

    public string Website { get; set; }

    public string Provider { get; set; }

    public DateTime UpdatedTime { get; set; }

    [Cell("U2")]
    public string parent_create_match_id { get; set; }

    [Cell("V2")]
    public string home_remark { get; set; }

    [Cell("W2")]
    public string away_remark { get; set; }

    [Cell("X2")]
    public string game_date { get; set; }

    [Cell("Y2")]
    public string Agent { get; set; }

    [Cell("Z2")]
    public string op { get; set; }

    [Cell("AA2")]
    public string WatchBet { get; set; }

    [Cell("AB2")]
    public string lt_value { get; set; }

    [Cell("AC2")]
    public string et_pk { get; set; }

    [Cell("AD2")]
    public string website1 { get; set; }

    [Cell("AE2")]
    public string website2 { get; set; }

    [Cell("AF2")]
    public string website3 { get; set; }

    [Cell("AG2")]
    public string website4 { get; set; }

    [Cell("AH2")]
    public string provider { get; set; }

    [Cell("AI2")]
    public string provider_use { get; set; }

    [Cell("AJ2")]
    public string create_time { get; set; }

    [Cell("AK2")]
    public string create_by { get; set; }

    [Cell("AL2")]
    public string update_time { get; set; }

    [Cell("AM2")]
    public string remark { get; set; }

    [Cell("AN2")]
    public string remark2 { get; set; }

    [Cell("AO2")]
    public string round_total { get; set; }

    [Cell("AP2")]
    public string create_streaming_src { get; set; }

    [Cell("AQ2")]
    public string create_streaming_id { get; set; }

    [Cell("AR2")]
    public string create_is_live { get; set; }

    [Cell("AS2")]
    public long InfoSiteMatchId { get; set; }

    #region Child match

    [Cell("AT2")]
    public string create_match_code { get; set; }

    // 0: live, 1: live+deadball, 2: hedge
    [Cell("AU2")]
    public string create_option { get; set; }

    // 0: closed.
    // 1: running.
    // 2: postponed.
    // 3: internal.
    // 4: completed.
    [Cell("AV2")]
    public string create_event_status { get; set; }

    // Is Live message.
    [Cell("AW2")]
    public string create_is_live_msg { get; set; }

    // Is link parent to child. if set true, this use for "child attribute follow parent when parent changed". BUT this function no implement due to requirement no need

    [Cell("AX2")]
    public string is_link { get; set; }

    // 0:Child League, 1:ExtraTime, 2:Penalty Handicap 3:Penalty Over-Under
    [Cell("AY2")]
    public string league_type { get; set; }

    // Is delete.
    [Cell("AZ2")]
    public string is_delete { get; set; }

    #endregion Child match

    private static string GetHomeName(CommonMatchEx infositeMatch, SoccerEventSchedule extInfo)
    {
        var isNeutral = extInfo.is_neutral.HasValue && extInfo.is_neutral.Value;
        var isReverse = extInfo.is_team_reverse.HasValue && extInfo.is_team_reverse.Value;
        var homename = isReverse ? infositeMatch.away_name : infositeMatch.home_name;
        return isNeutral ? homename + " (N)" : homename;
    }

    private static string GetAwayName(CommonMatchEx infositeMatch, SoccerEventSchedule extInfo)
    {
        var isReverse = extInfo.is_team_reverse.HasValue && extInfo.is_team_reverse.Value;
        return isReverse ? infositeMatch.home_name : infositeMatch.away_name;
    }

    private static string ConvertNullToEmpty(string value)
    {
        return string.IsNullOrEmpty(value)
            ? string.Empty
            : value.Trim();
    }

    private static string GetInfoSiteMergeMatchKey(CommonMatchEx infositeMatch)
    {
        var matchKey = $"{infositeMatch.match_id}-{infositeMatch.website_id}-{infositeMatch.is_import}";
        var mergeMatchKey = string.IsNullOrEmpty(infositeMatch.merge_match) ? string.Empty : infositeMatch.merge_match;

        return mergeMatchKey.Replace(matchKey, string.Empty).Trim(';');
    }

    private static string GetNote(SoccerEventSchedule extInfo)
    {
        if (extInfo == null)
        {
            return string.Empty;
        }

        var remark2Info = string.IsNullOrEmpty(extInfo.remark2) ? string.Empty : "@#" + extInfo.remark2;
        return extInfo.remark + remark2Info;
    }

    private static DateTime GetGameDate(CommonMatchEx infositeMatch)
    {
        var exInfoGameDate = infositeMatch.ext_info?.game_date != null && infositeMatch.ext_info.game_date.HasValue
            ? infositeMatch.ext_info.game_date.Value
            : MinDate;

        return ConvertTimeToUTC8(exInfoGameDate);
    }

    private static DateTime GetUpdateTime(CommonMatchEx infositeMatch, SoccerEventSchedule extInfo)
    {
        var exInfoTime = extInfo.update_time.HasValue ? extInfo.update_time.Value : MinDate;
        var matchTime = infositeMatch.update_timestamp.HasValue ? infositeMatch.update_timestamp.Value : MinDate;

        return exInfoTime > matchTime ? exInfoTime : matchTime;
    }

    private static DateTime ConvertTimeToUTC8(DateTime date)
    {
        return date.AddHours(8);
    }

    private static int? GetMatchId(CommonMatchEx infositeMatch)
    {
        int? matchId = null;
        if (infositeMatch.ext_info != null && infositeMatch.ext_info.create_match_id != 0)
        {
            matchId = infositeMatch.ext_info.create_match_id;
        }

        return matchId;
    }

    private static string GetProvider(string providers)
    {
        return string.IsNullOrWhiteSpace(providers) ? string.Empty : providers.Replace("/", string.Empty);
    }
}