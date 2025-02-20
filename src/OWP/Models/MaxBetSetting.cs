namespace OWP.Models;

public class MaxBetSettingModel
{
    public decimal FT_HDP { get; set; }
    public decimal? FT_OU { get; set; }
    public decimal? FirstHalf_HDP { get; set; }
    public decimal? FirstHalf_OU { get; set; }
    public decimal? NoOfCorner_FT_HDP { get; set; }
    public decimal? NoOfCorner_FT_OU { get; set; }
    public decimal? NoOfCorner_1H_HDP { get; set; }
    public decimal? NoOfCorner_1H_OU { get; set; }
    public decimal? NextCorner_FT_HDP { get; set; }
    public decimal? TotalBooking_FT_HDP { get; set; }
    public decimal? TotalBooking_FT_OU { get; set; }
    public decimal? TotalBooking_1H_HDP { get; set; }
    public decimal? TotalBooking_1H_OU { get; set; }
    public decimal? NextBooking_FT_HDP { get; set; }
    public string SecondAccept_Main { get; set; }
    public string SecondAccept_Corner { get; set; }
    public string SecondAccept_TotalBooking { get; set; }
    public string SecondAccept_Fantasy { get; set; }
    public string LastUpdatedBy { get; set; }

    public void MapsFollowFTHDP()
    {
        FT_OU = MapsFollowFTHDP(FT_OU);
        FirstHalf_HDP = MapsFollowFTHDP(FirstHalf_HDP);
        FirstHalf_OU = MapsFollowFTHDP(FirstHalf_OU);
        NoOfCorner_FT_HDP = MapsFollowFTHDP(NoOfCorner_FT_HDP);
        NoOfCorner_FT_OU = MapsFollowFTHDP(NoOfCorner_FT_OU);
        NoOfCorner_1H_HDP = MapsFollowFTHDP(NoOfCorner_1H_HDP);
        NoOfCorner_1H_OU = MapsFollowFTHDP(NoOfCorner_1H_OU);
        NextCorner_FT_HDP = MapsFollowFTHDP(NextCorner_FT_HDP);
        TotalBooking_FT_HDP = MapsFollowFTHDP(TotalBooking_FT_HDP);
        TotalBooking_FT_OU = MapsFollowFTHDP(TotalBooking_FT_OU);
        TotalBooking_1H_HDP = MapsFollowFTHDP(TotalBooking_1H_HDP);
        TotalBooking_1H_OU = MapsFollowFTHDP(TotalBooking_1H_OU);
        NextBooking_FT_HDP = MapsFollowFTHDP(NextBooking_FT_HDP);
    }

    private decimal? MapsFollowFTHDP(decimal? value)
        => value == -1 ? FT_HDP : value;
}
