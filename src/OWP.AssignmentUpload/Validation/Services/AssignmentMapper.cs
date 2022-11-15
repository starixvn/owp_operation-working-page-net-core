namespace OWP.AssignmentUpload.Validation.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AutoMapper;
    using Models;

    public interface AssignmentMapper
    {
        List<Trader> Traders { get; set; }

        int ExtractTrader(string text);

        string ExtractNote(string text);

        string ParseOtherTraders(string otherTraders);

        int GetTraderId(string name);

        List<AssignmentModel> Map(ValidationRequest request, List<AssignmentModelInExcel> excelModels);

        AssignmentModel Map(ValidationRequest request, AssignmentModelInExcel excelModel);

        void MapHashKey(AssignmentModel model);

        void MapEventDate(DateTime eventDate, AssignmentModelInExcel excelModel, AssignmentModel model);

        void MapTime(AssignmentModelInExcel excelModel, AssignmentModel model);

        void MapOtherTraders(AssignmentModelInExcel excelModel, AssignmentModel model);

        void MapTraders(AssignmentModelInExcel excelModel, AssignmentModel model);


        void MapMatchStatus(AssignmentModelInExcel excelModel, AssignmentModel model);

        string GetMatchStatus(string matchStatus);
    }

    public class AssignmentMapperImp : AssignmentMapper
    {
        private readonly IMapper mapper;

        private static readonly Regex otherTraderRegex = new Regex(@"#[^\s#]+", RegexOptions.IgnoreCase);

        public AssignmentMapperImp(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public List<Trader> Traders { get; set; }

        public List<AssignmentModel> Map(ValidationRequest request, List<AssignmentModelInExcel> excelModels)
        {
            return excelModels.Select(m => Map(request, m)).ToList();
        }

        public AssignmentModel Map(ValidationRequest request, AssignmentModelInExcel excelModel)
        {
            var model = mapper.Map<AssignmentModel>(excelModel);

            MapSessionId(request.SessionId, model);
            MapHashKey(model);
            MapEventDate(request.EventDate, excelModel, model);
            MapTime(excelModel, model);
            MapTraders(excelModel, model);
            MapOtherTraders(excelModel, model);
            MapMatchStatus(excelModel, model);
            MapIsNeutral(excelModel, model);
            MapTeamName(model);
            MapIsSabaMatch(model);

            return model;
        }

        private static void MapSessionId(string sessionId, AssignmentModel model)
        {
            model.SessionId = sessionId;
        }

        public void MapHashKey(AssignmentModel model)
        {
            model.HashKey = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Everyday, user uploads a file for assignments from 12:00 PM of that day to 12:00 PM of next day
        /// For an example, in case he/she uploads a file for the day 20-03-2020
        /// He/she has a special mental mapping:
        ///     - If the kick off time is from 12:00 to 23:59, the event day is 20-03-2020
        ///     - If the kick off time is from 00:00 to 11:59, the event day is the next day 21-03-2020
        /// </summary>
        /// <param name="eventDate"></param>
        /// <param name="excelModel"></param>
        /// <param name="model"></param>
        public void MapEventDate(DateTime eventDate, AssignmentModelInExcel excelModel, AssignmentModel model)
        {
            const int twelfthHour = 12;
            DateTime time;
            if (DateTime.TryParse(excelModel.Time, CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
            {
                var tempTime = time;
                model.EventDate = tempTime.Hour < twelfthHour ? eventDate.AddDays(1) : eventDate;
            }
        }

        public void MapTime(AssignmentModelInExcel excelModel, AssignmentModel model)
        {
            DateTime time;
            DateTime otherTime;

            if (DateTime.TryParse(excelModel.Time, CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
            {
                model.KickOffTime = time.TimeOfDay;
            }
            if (!string.IsNullOrWhiteSpace(excelModel.OtherOfTime) &&
                DateTime.TryParse(excelModel.OtherOfTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out otherTime))
            {
                model.OtherTime = otherTime.TimeOfDay;
            }
            else
            {
                model.OtherTime = null;
            }
        }

        public void MapTraders(AssignmentModelInExcel excelModel, AssignmentModel model)
        {
            model.Trader10Mins = GetTraderId(excelModel.Trader10MinsName);
            model.Trader15Mins = GetTraderId(excelModel.Trader15MinsName);
            model.Trader1H1X2 = GetTraderId(excelModel.Trader1H1X2Name);
            model.Trader1HHDP = GetTraderId(excelModel.Trader1HHDPName);
            model.Trader1HOU = GetTraderId(excelModel.Trader1HOUName);
            model.Trader1X2 = GetTraderId(excelModel.Trader1X2Name);
            model.TraderAIO = GetTraderId(excelModel.TraderAIOName);
            model.TraderFM = GetTraderId(excelModel.TraderFMName);
            model.TraderHDP = GetTraderId(excelModel.TraderHDPName);
            model.TraderLA = GetTraderId(excelModel.TraderLAName);
            model.TraderOU = GetTraderId(excelModel.TraderOUName);
        }

        public void MapOtherTraders(AssignmentModelInExcel excelModel, AssignmentModel model)
        {
            model.OtherTraders10Mins = ParseOtherTraders(excelModel.OtherTraders10Mins);
            model.OtherTraders15Mins = ParseOtherTraders(excelModel.OtherTraders15Mins);
            model.OtherTraders1H1X2 = ParseOtherTraders(excelModel.OtherTraders1H1X2);
            model.OtherTraders1HHDP = ParseOtherTraders(excelModel.OtherTraders1HHDP);
            model.OtherTraders1HOU = ParseOtherTraders(excelModel.OtherTraders1HOU);
            model.OtherTraders1X2 = ParseOtherTraders(excelModel.OtherTraders1X2);
            model.OtherTradersAIO = ParseOtherTraders(excelModel.OtherTradersAIO);
            model.OtherTradersFM = ParseOtherTraders(excelModel.OtherTradersFM);
            model.OtherTradersHDP = ParseOtherTraders(excelModel.OtherTradersHDP);
            model.OtherTradersLA = ParseOtherTraders(excelModel.OtherTradersLA);
            model.OtherTradersOU = ParseOtherTraders(excelModel.OtherTradersOU);
        }

        public int GetTraderId(string name)
        {
            var trader = Traders.FirstOrDefault(t => t.Username.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            return trader?.UserId ?? 0;
        }

        public string ParseOtherTraders(string otherTraders)
        {
            var parts = otherTraders.Split(',')
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrWhiteSpace(p));

            var assignments = parts.Select(p =>
            {
                var trader = ExtractTrader(p);
                var note = ExtractNote(p);
                return $"{trader},{note}";
            });

            return $"{string.Join("<o>", assignments)}";
        }

        public int ExtractTrader(string text)
        {
            var matches = otherTraderRegex.Matches(text);
            var name = matches.Cast<Match>().Select(s => s.Value.Remove(0, 1)).FirstOrDefault();

            return string.IsNullOrEmpty(name) ? 0 : GetTraderId(name);
        }

        public string ExtractNote(string text)
        {
            var startIndex = text.IndexOf('#');
            if (startIndex == -1)
            {
                return text;
            }
            var endIndex = text.IndexOf(' ', startIndex);
            if (endIndex == -1)
            {
                endIndex = text.Length;
            }

            var note = text.Substring(0, startIndex).Trim() + " #<user> " + text.Substring(endIndex).Trim();
            return $"{note}";
        }

        public void MapMatchStatus(AssignmentModelInExcel excelModel, AssignmentModel model)
        {
            model.MatchStatus = GetMatchStatus(excelModel.MatchStatus);
        }

        public static void MapIsNeutral(AssignmentModelInExcel excelModel, AssignmentModel model)
        {
            if (excelModel.Home.EndsWith("(N)", StringComparison.Ordinal))
            {
                model.IsNeutral = true;
                model.Home = excelModel.Home.Remove(excelModel.Home.Length - 3).TrimEnd();
            }
        }

        public string GetMatchStatus(string matchStatus)
        {
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            var lowerCase = textInfo.ToLower(matchStatus);
            var titleCase = textInfo.ToTitleCase(lowerCase);
            switch (titleCase)
            {
                case "":
                case "Completed":
                case "In-Play":
                    return "Live";
                default:
                    return titleCase;
            }
        }

        public static void MapTeamName(AssignmentModel model)
        {
            model.Home = RemoveNeutralSign(model.Home);
            model.Away = RemoveNeutralSign(model.Away);
        }

        public static string RemoveNeutralSign(string name)
        {
            return Regex.Replace(name, @"\s*\(n\)\s*", " ", RegexOptions.IgnoreCase).Trim();
        }

        public static void MapIsSabaMatch(AssignmentModel model)
        {
            model.IsSaba = IsSabaMatch(model.League);
        }

        public static bool IsSabaMatch(string league)
        {
            return league.IndexOf("SABA ", StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }
}
