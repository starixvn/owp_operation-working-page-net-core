using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OWP.Maxbet.Models;
using OWP.Enums;
using OWP.Helpers;

namespace OWP.Maxbet.Builder
{
    public interface SpreadBuilder
    {
        Task<MaxbetSpread> BuildSpread(int owpMatchId);
    }

    public class SpreadBuilderImp : SpreadBuilder
    {
        private const int leagueGroupMJ = 1;

        private readonly MaxbetReposiory syncMaxbetReposiory;

        public SpreadBuilderImp(MaxbetReposiory syncMaxbetReposiory)
        {
            this.syncMaxbetReposiory = syncMaxbetReposiory;
        }

        public async Task<MaxbetSpread> BuildSpread(int owpMatchId)
        {
            var spreadTupple = await syncMaxbetReposiory.GetSpreadByOwpmatchId(owpMatchId);
            var spread = spreadTupple.Item1?.ToList();
            var leagueGroupId = spreadTupple.Item2?.FirstOrDefault();

            if (!spread.Any())
            {
                return new MaxbetSpread();
            }

            if (spread.Count == 4)
            {
                return BuildSpreadSender(spread);
            }

            return leagueGroupId == leagueGroupMJ
                ? BuildSpreadSenderForLeagueGroupMJ(spread)
                : BuildSpreadSenderForLeagueGroupNotAllowMJ(spread);
        }

        public static MaxbetSpread BuildSpreadSenderForLeagueGroupNotAllowMJ(List<SpreadModel> spread)
        {
            var hasExistsFirstHalfHdp = spread.Any(s => s.BettypeId == (int)EnumBettype.FirstHalfHandicap);
            var hasExistsFirstHalfOu = spread.Any(s => s.BettypeId == (int)EnumBettype.FirstHalfOverUnder);

            var handicap = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.Handicap) ?? new SpreadModel();
            var overUnder = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.OverUnder) ?? new SpreadModel();

            if (!hasExistsFirstHalfHdp)
            {
                if (handicap.Spread < (decimal)0.1)
                {
                    spread.Add(new SpreadModel
                    {
                        BettypeId = (int)EnumBettype.FirstHalfHandicap,
                        Spread = (decimal)0.1
                    });
                }
                else
                {
                    spread.Add(new SpreadModel
                    {
                        BettypeId = (int)EnumBettype.FirstHalfHandicap,
                        Spread = handicap.Spread
                    });
                }
            }

            if (!hasExistsFirstHalfOu)
            {
                if (overUnder.Spread < (decimal)0.1)
                {
                    spread.Add(new SpreadModel
                    {
                        BettypeId = (int)EnumBettype.FirstHalfOverUnder,
                        Spread = (decimal)0.1
                    });
                }
                else
                {
                    spread.Add(new SpreadModel
                    {
                        BettypeId = (int)EnumBettype.FirstHalfOverUnder,
                        Spread = overUnder.Spread
                    });
                }
            }

            return BuildSpreadSender(spread);
        }

        public MaxbetSpread BuildSpreadSenderForLeagueGroupMJ(List<SpreadModel> spread)
        {
            var hasExistsFirstHalfHdp = spread.Any(s => s.BettypeId == (int)EnumBettype.FirstHalfHandicap);
            var hasExistsFirstHalfOu = spread.Any(s => s.BettypeId == (int)EnumBettype.FirstHalfOverUnder);

            var handicap = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.Handicap) ?? new SpreadModel();
            var overUnder = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.OverUnder) ?? new SpreadModel();
            if (!hasExistsFirstHalfHdp)
            {
                spread.Add(new SpreadModel
                {
                    BettypeId = (int)EnumBettype.FirstHalfHandicap,
                    Spread = handicap.Spread
                });
            }
            if (!hasExistsFirstHalfOu)
            {
                spread.Add(new SpreadModel
                {
                    BettypeId = (int)EnumBettype.FirstHalfOverUnder,
                    Spread = overUnder.Spread
                });
            }
            return BuildSpreadSender(spread);
        }

        public static MaxbetSpread BuildSpreadSender(List<SpreadModel> spread)
        {
            var handicap = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.Handicap) ?? new SpreadModel();
            var overunder = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.OverUnder) ?? new SpreadModel();
            var firstHalfHandicap = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.FirstHalfHandicap) ?? new SpreadModel();
            var firstHalfOverUnder = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.FirstHalfOverUnder) ?? new SpreadModel();
            return new MaxbetSpread
            {
                Handicap = handicap.Spread,
                OverUnder = overunder.Spread,
                FirstHalfHandicap = firstHalfHandicap.Spread,
                FirstHalfOverUnder = firstHalfOverUnder.Spread
            };
        }
    }
}