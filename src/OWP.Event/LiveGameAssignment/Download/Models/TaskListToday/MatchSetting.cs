namespace Fanex.NAP.OWP.TaskScheduling.Models.TaskListToday
{
    using System;
    using Core.Helpers;
    using Newtonsoft.Json;

    [Serializable]
    public class MatchSetting
    {
        [JsonIgnore]
        public int OWPMatchId { get; set; }

        [JsonIgnore]
        public int ParentMatchId { get; set; }

        [JsonIgnore]
        public int? MinorBettypeId { get; set; }

        public int BettypeId { get; set; }

        public decimal? Maxbet { get; set; }

        public decimal? Spread { get; set; }

        public string MaxbetSpreadToDisplay => HelperService.BuildDisplayMaxbetSpread(Maxbet, Spread);

        [JsonIgnore]
        public bool IsHasMaxbetSpread
        {
            get
            {
                return Maxbet.HasValue || Spread.HasValue;
            }
        }
    }
}