
namespace OWP.NewLeagueSetting.Models;

using System.ComponentModel.DataAnnotations;
public class LeagueSettingExcelInfo
{
    [Display(Name = "League Group")]
    public string LeagueGroup { get; set; } = default!;

    [Display(Name = "Short League")]
    public string ShortLeague { get; set; } = default!;

    public string League { get; set; } = default!;

    [Display(Name = "Local TV")]
    public string LocalTv { get; set; } = default!;

    public string Round { get; set; } = default!;

    [Display(Name = "League Division")]
    public string LeagueDivision { get; set; } = default!;

    public string Remark { get; set; } = default!;

    [Display(Name = "FT Spread")]
    public string FTSpread { get; set; } = default!;

    [Display(Name = "WD HDP")]
    public string WD_HDP { get; set; } = default!;

    [Display(Name = "WD NO.HDP")]
    public string WD_NO_HDP { get; set; } = default!;

    [Display(Name = "WK HDP")]
    public string WK_HDP { get; set; } = default!;

    [Display(Name = "WK NO.HDP")]
    public string WK_NO_HDP { get; set; } = default!;

    [Display(Name = "LST Note")]
    public string LstNote { get; set; } = default!;
    [Display(Name = "Remark 2")]
    public string Remark2 { get; set; } = default!;

}