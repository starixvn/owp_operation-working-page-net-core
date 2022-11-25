using OWP.LiveGameList.Models;
using System.Globalization;

namespace OWP.LiveGameList.Helper
{
    public static class LiveGameListHelper
    {
        public static string BuildDisplayMaxbetSpread(decimal? maxBet, decimal? spread)
        {
            var maxbetDisplay = maxBet.ToOWPMaxBetFormat();
            var spreadDisplay = spread.ToOWPSpreadFormat();
            if (string.IsNullOrEmpty(maxbetDisplay) && string.IsNullOrEmpty(spreadDisplay))
            {
                return string.Empty;
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1}",
                maxbetDisplay,
                !string.IsNullOrWhiteSpace(spreadDisplay) ? string.Format(CultureInfo.InvariantCulture, "({0}%)", spreadDisplay) : string.Empty);
        }

        public static string ToOWPMaxBetFormat(this decimal? maxbet)
        {
            return maxbet.HasValue ? (maxbet.Value / 1000).ToString("0.####", CultureInfo.InvariantCulture) + "K" : string.Empty;
        }

        public static string ToOWPSpreadFormat(this decimal? spread)
        {
            return spread.HasValue ? (spread.Value * 100).ToString("0.##", CultureInfo.InvariantCulture) : string.Empty;
        }

        public static string BuildIpTv(IPTVModel iPTVModel)
        {
            var ipTv = new List<string>();

            if (!string.IsNullOrWhiteSpace(iPTVModel.TeleVision))
            {
                ipTv.Add(iPTVModel.TeleVision);
            }

            if (!string.IsNullOrWhiteSpace(iPTVModel.IpTv))
            {
                ipTv.Add(iPTVModel.IpTv);
            }

            if (!string.IsNullOrWhiteSpace(iPTVModel.Channel))
            {
                ipTv.Add(iPTVModel.Channel);
            }

            if (!string.IsNullOrWhiteSpace(iPTVModel.Stream))
            {
                ipTv.Add(iPTVModel.Stream);
            }

            return string.Join("/", ipTv);
        }
    }
}
