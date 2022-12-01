namespace OWP.LiveGameList.Models
{
    public class MatchSettingModel
    {
        public MatchSettingModel()
        {
            Handicap = new MatchSetting();
            Handicap1st = new MatchSetting();
            OverUnder = new MatchSetting();
            OverUnder1st = new MatchSetting();
            OneXTwo = new MatchSetting();
            OneXTwo1st = new MatchSetting();
            Specific15Mins = new MatchSetting();
            Specific10Mins = new MatchSetting();
            FastMarket = new MatchSetting();
        }

        public MatchSetting Handicap { get; set; }

        public MatchSetting Handicap1st { get; set; }

        public MatchSetting OverUnder { get; set; }

        public MatchSetting OverUnder1st { get; set; }

        public MatchSetting OneXTwo { get; set; }

        public MatchSetting OneXTwo1st { get; set; }

        public MatchSetting Specific15Mins { get; set; }

        public MatchSetting Specific10Mins { get; set; }

        public MatchSetting FastMarket { get; set; }
    }
}
