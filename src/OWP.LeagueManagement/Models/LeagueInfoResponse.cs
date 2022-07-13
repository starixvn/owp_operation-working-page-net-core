namespace OWP.LeagueManagement.Models
{
    using Newtonsoft.Json;
    using System;
    public class LeagueInfoResponse
    {
        [JsonProperty("HasLinkLeagueAndTeam")]
        public bool HasLinkLeagueAndTeam { get; set; }

        [JsonProperty("OWPLeagueId")]
        public int? OWPLeagueId { get; set; }

        [JsonProperty("GroupValue")]
        public int? GroupValue { get; set; }

        [JsonProperty("GroupLabel")]
        public string GroupLabel { get; set; }

        [JsonProperty("LeagueName")]
        public string LeagueName { get; set; }

        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [JsonProperty("IsBlackList")]
        public bool IsBlackList { get; set; }

        [JsonProperty("MinSpread")]
        public decimal? MinSpread { get; set; }

        [JsonProperty("MaxSpread")]
        public decimal? MaxSpread { get; set; }

        [JsonProperty("MinMaxbetWD")]
        public decimal? MinMaxbetWD { get; set; }

        [JsonProperty("MaxMaxbetWD")]
        public decimal? MaxMaxbetWD { get; set; }

        [JsonProperty("MinMaxbetWK")]
        public decimal? MinMaxbetWK { get; set; }

        [JsonProperty("MaxMaxbetWK")]
        public decimal? MaxMaxbetWK { get; set; }

        [JsonProperty("MinFT1X2")]
        public decimal? MinFT1X2 { get; set; }

        [JsonProperty("MaxFT1X2")]
        public decimal? MaxFT1X2 { get; set; }

        [JsonProperty("CN88MinSpread")]
        public decimal? CN88MinSpread { get; set; }

        [JsonProperty("CN88MaxSpread")]
        public decimal? CN88MaxSpread { get; set; }

        [JsonProperty("CN88MinMaxbetWD")]
        public decimal? CN88MinMaxbetWD { get; set; }

        [JsonProperty("CN88MaxMaxbetWD")]
        public decimal? CN88MaxMaxbetWD { get; set; }

        [JsonProperty("CN88MinMaxbetWK")]
        public decimal? CN88MinMaxbetWK { get; set; }

        [JsonProperty("CN88MaxMaxbetWK")]
        public decimal? CN88MaxMaxbetWK { get; set; }

        [JsonProperty("CN88MinFT1X2")]
        public decimal? CN88MinFT1X2 { get; set; }

        [JsonProperty("CN88MaxFT1X2")]
        public decimal? CN88MaxFT1X2 { get; set; }

        [JsonProperty("LastUse")]
        public DateTime? LastUse { get; set; }
    }
}