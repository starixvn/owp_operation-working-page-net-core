
namespace OWP.OtherSport.Validation.Services
{
    using AutoMapper;
    using OWP.OtherSport.Validation.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public interface AssignmentMapper
    {
        List<Trader> Traders { get; set; }

        int ExtractTrader(string text);

        string ExtractNote(string text);

        string ParseOtherTrader(string otherTraders);

        int GetTraderId(string name);

        List<AssignmentModel> Map(List<AssignmentModelInExcel> excelModels);

        AssignmentModel Map(AssignmentModelInExcel excelModel);

        void MapOtherTraders(AssignmentModelInExcel excelModel, AssignmentModel model);

        void MapTraders(AssignmentModelInExcel excelModel, AssignmentModel model);
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

        public List<AssignmentModel> Map(List<AssignmentModelInExcel> excelModels)
        {
            return excelModels.Select(m => Map(m)).ToList();
        }

        public AssignmentModel Map(AssignmentModelInExcel excelModel)
        {
            var model = mapper.Map<AssignmentModel>(excelModel);
            MapTraders(excelModel, model);
            MapOtherTraders(excelModel, model);
            return model;
        }           
      
        public void MapOtherTraders(AssignmentModelInExcel excelModel, AssignmentModel model)
        {
            //basket ball
            model.OtherTraderBasketball1HHDP = ParseOtherTrader(excelModel.OtherTraderBasketball1HHDP);
            model.OtherTraderBasketball1HOUOE = ParseOtherTrader(excelModel.OtherTraderBasketball1HOUOE);
            model.OtherTraderBasketball1Q3QHDP = ParseOtherTrader(excelModel.OtherTraderBasketball1Q3QHDP);
            model.OtherTraderBasketball1Q3QOUOE = ParseOtherTrader(excelModel.OtherTraderBasketball1Q3QOUOE);
            model.OtherTraderBasketballFTHDP = ParseOtherTrader(excelModel.OtherTraderBasketballFTHDP);
            model.OtherTraderBasketballFTOUOE = ParseOtherTrader(excelModel.OtherTraderBasketballFTOUOE);

            //foot ball
            model.OtherTraderFootballFTHDP = ParseOtherTrader(excelModel.OtherTraderFootballFTHDP);
            model.OtherTraderFootball1HHDP = ParseOtherTrader(excelModel.OtherTraderFootball1HHDP);
            model.OtherTraderFootball1Q3QHDP = ParseOtherTrader(excelModel.OtherTraderFootball1Q3QHDP);
            model.OtherTraderFootballFTOU = ParseOtherTrader(excelModel.OtherTraderFootballFTOU);
            model.OtherTraderFootball1HOU = ParseOtherTrader(excelModel.OtherTraderFootball1HOU);
            model.OtherTraderFootball1Q3QOU = ParseOtherTrader(excelModel.OtherTraderFootball1Q3QOU);

            //FieldHockey
            model.OtherTraderFieldHockeyFTHDP = ParseOtherTrader(excelModel.OtherTraderFieldHockeyFTHDP);
            model.OtherTraderFieldHockey1HHDP = ParseOtherTrader(excelModel.OtherTraderFieldHockey1HHDP);
            model.OtherTraderFieldHockey1QHDP = ParseOtherTrader(excelModel.OtherTraderFieldHockey1QHDP);
            model.OtherTraderFieldHockeyFTOU = ParseOtherTrader(excelModel.OtherTraderFieldHockeyFTOU);
            model.OtherTraderFieldHockey1HOU = ParseOtherTrader(excelModel.OtherTraderFieldHockey1HOU);
            model.OtherTraderFieldHockey1QOU = ParseOtherTrader(excelModel.OtherTraderFieldHockey1QOU);

            //IceHockey
            model.OtherTraderIceHockeyFTHDP = ParseOtherTrader(excelModel.OtherTraderIceHockeyFTHDP);
            model.OtherTraderIceHockey1st2ndPeriodsHDP = ParseOtherTrader(excelModel.OtherTraderIceHockey1st2ndPeriodsHDP);
            model.OtherTraderIceHockeyFTOU = ParseOtherTrader(excelModel.OtherTraderIceHockeyFTOU);
            model.OtherTraderIceHockey1st2ndPeriodsOU = ParseOtherTrader(excelModel.OtherTraderIceHockey1st2ndPeriodsOU);

            //DartsLegs
            model.OtherTraderDartsLegsHDP = ParseOtherTrader(excelModel.OtherTraderDartsLegsHDP);
            model.OtherTraderDartsMost180sHDP = ParseOtherTrader(excelModel.OtherTraderDartsMost180sHDP);

            //Pool
            model.OtherTraderPoolMatchWinnerHDP = ParseOtherTrader(excelModel.OtherTraderPoolMatchWinnerHDP);
            model.OtherTraderPoolRacksWinnerHDP = ParseOtherTrader(excelModel.OtherTraderPoolRacksWinnerHDP);

            //TableTennis
            model.OtherTraderTableTennisSetWinnerHDP = ParseOtherTrader(excelModel.OtherTraderTableTennisSetWinnerHDP);
            model.OtherTraderTableTennisSetXWinnerHDP = ParseOtherTrader(excelModel.OtherTraderTableTennisSetXWinnerHDP);

            //Volleyball
            model.OtherTraderVolleyballSetWinnerHDPOU = ParseOtherTrader(excelModel.OtherTraderVolleyballSetWinnerHDPOU);
            model.OtherTraderVolleyballSetsXPointWinnerHDP = ParseOtherTrader(excelModel.OtherTraderVolleyballSetsXPointWinnerHDP);

            //Handball
            model.OtherTraderHandballFTHDP = ParseOtherTrader(excelModel.OtherTraderHandballFTHDP);
            model.OtherTraderHandball1HHDP = ParseOtherTrader(excelModel.OtherTraderHandball1HHDP);
            model.OtherTraderHandballFTOU = ParseOtherTrader(excelModel.OtherTraderHandballFTOU);
            model.OtherTraderHandball1HOU = ParseOtherTrader(excelModel.OtherTraderHandball1HOU);

            //netball
            model.OtherTraderNetballFTHDP = ParseOtherTrader(excelModel.OtherTraderNetballFTHDP);
            model.OtherTraderNetball1HHDP = ParseOtherTrader(excelModel.OtherTraderNetball1HHDP);
            model.OtherTraderNetball1Q3QHDP = ParseOtherTrader(excelModel.OtherTraderNetball1Q3QHDP);
            model.OtherTraderNetballFTOUOE = ParseOtherTrader(excelModel.OtherTraderNetballFTOUOE);
            model.OtherTraderNetball1HOUOE = ParseOtherTrader(excelModel.OtherTraderNetball1HOUOE);
            model.OtherTraderNetball1Q3QOUOE = ParseOtherTrader(excelModel.OtherTraderNetball1Q3QOUOE);

            //Cricket
            model.OtherTraderCricketMatchWinnerInningsOU = ParseOtherTrader(excelModel.OtherTraderCricketMatchWinnerInningsOU);
            model.OtherTraderCricketNextOverOUPlayerOU = ParseOtherTrader(excelModel.OtherTraderCricketNextOverOUPlayerOU);
            model.OtherTraderCricketFT1X2 = ParseOtherTrader(excelModel.OtherTraderCricketFT1X2);

            //Snooker
            model.OtherTraderSnookerMatchWinnerHDPOU = ParseOtherTrader(excelModel.OtherTraderSnookerMatchWinnerHDPOU);
            model.OtherTraderSnookerFramesWinnerHDPOUOE = ParseOtherTrader(excelModel.OtherTraderSnookerFramesWinnerHDPOUOE);
            model.OtherTraderSnookerFirst5FramesHDP = ParseOtherTrader(excelModel.OtherTraderSnookerFirst5FramesHDP);
            model.OtherTraderSnookerFirst8FramesHDP = ParseOtherTrader(excelModel.OtherTraderSnookerFirst8FramesHDP);
            model.OtherTraderSnookerFirst9FramesHDP = ParseOtherTrader(excelModel.OtherTraderSnookerFirst9FramesHDP);

            //baseball
            model.OtherTraderBaseballFTRunLineHDP = ParseOtherTrader(excelModel.OtherTraderBaseballFTRunLineHDP);
            model.OtherTraderBaseball1H1st5inningsHDP = ParseOtherTrader(excelModel.OtherTraderBaseball1H1st5inningsHDP);
            model.OtherTraderBaseballFTTotalRunOU = ParseOtherTrader(excelModel.OtherTraderBaseballFTTotalRunOU);
            model.OtherTraderBaseball1H1st5inningsOU = ParseOtherTrader(excelModel.OtherTraderBaseball1H1st5inningsOU);
            model.OtherTraderBaseballInningHitsRuns = ParseOtherTrader(excelModel.OtherTraderBaseballInningHitsRuns);

            //badminton
            model.OtherTraderBadmintonAllProductPointWinner = ParseOtherTrader(excelModel.OtherTraderBadmintonAllProductPointWinner);
            model.OtherTraderBadmintonAllProductSetWinner = ParseOtherTrader(excelModel.OtherTraderBadmintonAllProductSetWinner);
            model.OtherTraderBadmintonMatchHandicap = ParseOtherTrader(excelModel.OtherTraderBadmintonMatchHandicap);
            model.OtherTraderBadmintonMatchHandicap = ParseOtherTrader(excelModel.OtherTraderBadmintonMatchHandicap);
            model.OtherTraderBadmintonRacetoPoints7911 = ParseOtherTrader(excelModel.OtherTraderBadmintonRacetoPoints7911);

            //tennis
            model.OtherTraderTennisAllProductLS = ParseOtherTrader(excelModel.OtherTraderTennisAllProductLS);
            model.OtherTraderTennisNextGame = ParseOtherTrader(excelModel.OtherTraderTennisNextGame);
            model.OtherTraderTennisMostAces = ParseOtherTrader(excelModel.OtherTraderTennisMostAces);
            model.OtherTraderTennisMostDoubleFaults = ParseOtherTrader(excelModel.OtherTraderTennisMostDoubleFaults);
        }
    
        public void MapTraders(AssignmentModelInExcel excelModel, AssignmentModel model)
        {
            //basketball
            model.TraderBasketball1HHDP = GetTraderId(excelModel.TraderBasketball1HHDP);
            model.TraderBasketball1HOUOE = GetTraderId(excelModel.TraderBasketball1HOUOE);
            model.TraderBasketball1Q3QHDP = GetTraderId(excelModel.TraderBasketball1Q3QHDP);
            model.TraderBasketball1Q3QOUOE = GetTraderId(excelModel.TraderBasketball1Q3QOUOE);
            model.TraderBasketballFTHDP = GetTraderId(excelModel.TraderBasketballFTHDP);
            model.TraderBasketballFTOUOE = GetTraderId(excelModel.TraderBasketballFTOUOE);

            //football
            model.TraderFootballFTHDP = GetTraderId(excelModel.TraderFootballFTHDP);
            model.TraderFootball1HHDP = GetTraderId(excelModel.TraderFootball1HHDP);
            model.TraderFootball1Q3QHDP = GetTraderId(excelModel.TraderFootball1Q3QHDP);
            model.TraderFootballFTOU = GetTraderId(excelModel.TraderFootballFTOU);
            model.TraderFootball1HOU = GetTraderId(excelModel.TraderFootball1HOU);
            model.TraderFootball1Q3QOU = GetTraderId(excelModel.TraderFootball1Q3QOU);

            //field hockey
            model.TraderFieldHockeyFTHDP = GetTraderId(excelModel.TraderFieldHockeyFTHDP);
            model.TraderFieldHockey1HHDP = GetTraderId(excelModel.TraderFieldHockey1HHDP);
            model.TraderFieldHockey1QHDP = GetTraderId(excelModel.TraderFieldHockey1QHDP);
            model.TraderFieldHockeyFTOU = GetTraderId(excelModel.TraderFieldHockeyFTOU);
            model.TraderFieldHockey1HOU = GetTraderId(excelModel.TraderFieldHockey1HOU);
            model.TraderFieldHockey1QOU = GetTraderId(excelModel.TraderFieldHockey1QOU);

            //ice hockey
            model.TraderIceHockeyFTHDP = GetTraderId(excelModel.TraderIceHockeyFTHDP);
            model.TraderIceHockey1st2ndHDP = GetTraderId(excelModel.TraderIceHockey1st2ndHDP);
            model.TraderIceHockeyFTOU = GetTraderId(excelModel.TraderIceHockeyFTOU);
            model.TraderIceHockey1st2ndOU = GetTraderId(excelModel.TraderIceHockey1st2ndOU);

            //other sport
            model.TraderOtherSportsMatchWinner = GetTraderId(excelModel.TraderOtherSportsMatchWinner);

            //darts legs
            model.TraderDartsLegsHDP = GetTraderId(excelModel.TraderDartsLegsHDP);
            model.TraderDartsMost180sHDP = GetTraderId(excelModel.TraderDartsMost180sHDP);

            //pool match
            model.TraderPoolMatchWinnerHDP = GetTraderId(excelModel.TraderPoolMatchWinnerHDP);
            model.TraderPoolRacksWinnerHDP = GetTraderId(excelModel.TraderPoolRacksWinnerHDP);

            //tennis 
            model.TraderTableTennisSetWinnerHDP = GetTraderId(excelModel.TraderTableTennisSetWinnerHDP);
            model.TraderTableTennisSetXWinnerHDP = GetTraderId(excelModel.TraderTableTennisSetXWinnerHDP);

            //Volleyball 
            model.TraderVolleyballSetWinnerHDPOE = GetTraderId(excelModel.TraderVolleyballSetWinnerHDPOE);
            model.TraderVolleyballSetsXPointWinnerHDP = GetTraderId(excelModel.TraderVolleyballSetsXPointWinnerHDP);

            //handball
            model.TraderHandballFTHDP = GetTraderId(excelModel.TraderHandballFTHDP);
            model.TraderHandball1HHDP = GetTraderId(excelModel.TraderHandball1HHDP);
            model.TraderHandballFTOU = GetTraderId(excelModel.TraderHandballFTOU);
            model.TraderHandball1HOU = GetTraderId(excelModel.TraderHandball1HOU);

            //netball
            model.TraderNetballFTHDP = GetTraderId(excelModel.TraderNetballFTHDP);
            model.TraderNetball1HHDP = GetTraderId(excelModel.TraderNetball1HHDP);
            model.TraderNetball1Q3QHDP = GetTraderId(excelModel.TraderNetball1Q3QHDP);
            model.TraderNetballFTOUOE = GetTraderId(excelModel.TraderNetballFTOUOE);
            model.TraderNetball1HOUOE = GetTraderId(excelModel.TraderNetball1HOUOE);
            model.TraderNetball1Q3QOUOE = GetTraderId(excelModel.TraderNetball1Q3QOUOE);

            //cricket
            model.TraderCricketMatchWinnerInningsOU = GetTraderId(excelModel.TraderCricketMatchWinnerInningsOU);
            model.TraderCricketNextOverOUPlayerOU = GetTraderId(excelModel.TraderCricketNextOverOUPlayerOU);
            model.TraderCricketFT1X2 = GetTraderId(excelModel.TraderCricketFT1X2);

            //snooker
            model.TraderSnookerMatchWinnerHDPOU = GetTraderId(excelModel.TraderSnookerMatchWinnerHDPOU);
            model.TraderSnookerFramesWinnerHDPOUOE = GetTraderId(excelModel.TraderSnookerFramesWinnerHDPOUOE);
            model.TraderSnookerFirst5FramesHDP = GetTraderId(excelModel.TraderSnookerFirst5FramesHDP);
            model.TraderSnookerFirst8FramesHDP = GetTraderId(excelModel.TraderSnookerFirst8FramesHDP);
            model.TraderSnookerFirst9FramesHDP = GetTraderId(excelModel.TraderSnookerFirst9FramesHDP);

            //baseball
            model.TraderBaseballFTRunLineHDP = GetTraderId(excelModel.TraderBaseballFTRunLineHDP);
            model.TraderBaseball1H1st5InningsHDP = GetTraderId(excelModel.TraderBaseball1H1st5InningsHDP);
            model.TraderBaseballFTTotalRunOU = GetTraderId(excelModel.TraderBaseballFTTotalRunOU);
            model.TraderBaseball1H1st5InningsOU = GetTraderId(excelModel.TraderBaseball1H1st5InningsOU);
            model.TraderBaseballInningHitsRuns = GetTraderId(excelModel.TraderBaseballInningHitsRuns);

            //badminton
            model.TraderBadmintonAllProductSetWinner = GetTraderId(excelModel.TraderBadmintonAllProductSetWinner);
            model.TraderBadmintonAllProductPointWinner = GetTraderId(excelModel.TraderBadmintonAllProductPointWinner);
            model.TraderBadmintonRacetoPoints7911 = GetTraderId(excelModel.TraderBadmintonRacetoPoints7911);
            model.TraderBadmintonNextPointWinner = GetTraderId(excelModel.TraderBadmintonNextPointWinner);
            model.TraderBadmintonMatchHandicap = GetTraderId(excelModel.TraderBadmintonMatchHandicap);

            //tennis
            model.TraderTennisAllProductLS = GetTraderId(excelModel.TraderTennisAllProductLS);
            model.TraderTennisNextGame = GetTraderId(excelModel.TraderTennisNextGame);
            model.TraderTennisMostAces = GetTraderId(excelModel.TraderTennisMostAces);
            model.TraderTennisMostDoubleFaults = GetTraderId(excelModel.TraderTennisMostDoubleFaults);
        }

        public void MapTraders1(AssignmentModel model)
        {
            var properties = typeof(AssignmentModel).GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name.StartsWith("Trader", StringComparison.CurrentCultureIgnoreCase))
                {
                    var property = GetProperty(model, prop.Name);
                    var traderValueInExcel = prop.GetValue(prop.Name).ToString();
                    var excelTrader = GetTraderId(traderValueInExcel);
                    property.SetValue(prop.Name, excelTrader);
                }
            }           
        }

        private static PropertyInfo GetProperty(AssignmentModel model, string propertyName)
        {
            return model.GetType().GetProperties().Single(p => p.Name == propertyName);
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

        public int ExtractTrader(string text)
        {
            var matches = otherTraderRegex.Matches(text);
            var name = matches.Cast<Match>().Select(s => s.Value.Remove(0, 1)).FirstOrDefault();

            return string.IsNullOrEmpty(name) ? 0 : GetTraderId(name);
        }

        public int GetTraderId(string name)
        {
            var trader = Traders.FirstOrDefault(t => t.Username.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            return trader?.UserId ?? 0;
        }

        public string ParseOtherTrader(string otherTraders)
        {                  
            var trader = ExtractTrader(otherTraders);
            var note = ExtractNote(otherTraders);
            return $"{trader},{note}";                
        }
    }
}
