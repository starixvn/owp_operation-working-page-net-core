namespace OWP.OtherSport.EventAssignment.Services
{
    using Fanex.Data.Repository;
    using OWP.OtherSport.EventAssignment.Criteria;
    using OWP.OtherSport.EventAssignment.Models;
    using OWP.OtherSport.Submit.Contants;
    using OWP.OtherSport.Submit.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public interface CommandRepository
    {
        Task<IEnumerable<EventAssignmentModel>> Query(QueryEventAssignmentRequest request);        
    }

    public class CommandRepositoryImp : CommandRepository
    {
        private readonly IDynamicRepository dynamicRepository;

        public CommandRepositoryImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<EventAssignmentModel>> Query(QueryEventAssignmentRequest request)
        {
            var criteria = new GetEventAssignmentCriteria
            {
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };

            var (matches, assignments, maxbets) = await dynamicRepository.FetchMultipleAsync<MatchInfo, MatchAssignment, MatchMaxBet>(criteria);          

            var events = MappingInfoToEventAssignmentModel(matches, assignments, maxbets);

            return events;
        }

        private static IEnumerable<EventAssignmentModel> MappingInfoToEventAssignmentModel(
          IEnumerable<MatchInfo> matches,
          IEnumerable<MatchAssignment> assignments,
          IEnumerable<MatchMaxBet> maxbets)
        {
            var events = new List<EventAssignmentModel>();
            foreach (var match in matches)
            {
                var eventInstance = new EventAssignmentModel
                {
                    OSMatchId = match.OSMatchId,
                    MatchStatus = match.MatchStatus,
                    Sport = match.Sport,
                    DateTime = match.DateTime,
                    Home = match.Home,
                    Away = match.Away,
                    League = match.League,
                    Category = match.Category,
                    NoHDP = match.NoHDP,
                    MatchOffer = match.MatchOffer,
                    AutoLink = match.AutoLink,
                    IPTV = match.IPTV,
                    Service = match.Service,
                    OtherProvider = match.OtherProvider,
                    WB = match.WB,
                    Provider = match.Provider,
                    Remark = match.Remark,
                    //TraderMaxbetModels = GetTraderMaxbet(match.OSMatchId, assignments, maxbets)
                };    
            
                events.Add(eventInstance);
            }

            return events;
        }

        //public static IDictionary<string, TraderMaxBetModel> GetTraderMaxbet(int osMatchId, IEnumerable<MatchAssignment> assignments,
        //  IEnumerable<MatchMaxBet> maxbets)
        //{
        //    var maxbetList = maxbets.Where(x => x.OSMatchId == osMatchId).ToDictionary(m=>m.BetTypeName);
        //    var assignmentList = assignments.Where(x => x.OSMatchId == osMatchId).GroupBy(a => a.BetTypeName).ToDictionary(a=>a.Key, a=>a.ToList());
        //    var traderMaxBetModels = MapTraderMaxBetModels(assignmentList, maxbetList).ToDictionary(m=>m.BettypeName);
        //    return traderMaxBetModels;
        //}

        public static List<TraderMaxBetModel> MapTraderMaxBetModels(IDictionary<string, List<MatchAssignment>> assignments,
          Dictionary<string, MatchMaxBet> maxbets)
        {        
            var traderMaxBetList = new List<TraderMaxBetModel>();
            var traderMaxBet = new TraderMaxBetModel();
          
            //foreach (var key in assignments.Keys)
            //{
            //    foreach(var assignment in assignments[key])
            //    {
            //        traderMaxBet.Assignment = assignment.Username;
            //        if (Equals(assignment.AssignmentType, (int)AssignmentType.OtherAssignment))
            //        {
            //            traderMaxBet.OtherAssignment = DetectOtherAssignment(assignment);
            //        }
            //    }
            //    traderMaxBet.BettypeName = key;
            //    traderMaxBetList.Add(traderMaxBet);
            //}

            //foreach (var maxbet in maxbets.Values)
            //{
            //    var newTraderMaxbet = new TraderMaxBetModel();
            //    foreach (var a in traderMaxBetList)
            //    {
            //        if (Equals(a.BettypeName, maxbet.BetTypeName))
            //        {
            //            a.MaxBet = maxbet.MaxBet;
            //        }
            //        else
            //        {
            //            newTraderMaxbet.MaxBet = maxbet.MaxBet;
            //            newTraderMaxbet.BettypeName = maxbet.BetTypeName;
            //            traderMaxBetList.Add(newTraderMaxbet);
            //        }
            //    }
            //}         

            return traderMaxBetList;
        }
    
        public static string DetectOtherAssignment(MatchAssignment assignment)
        {
            if(assignment.AssignmentNote != null)
            {
                return assignment.AssignmentNote.Contains("#<user>") 
                    ? assignment.AssignmentNote.Replace("#<user>", $"#{assignment.Username}") 
                    : assignment.AssignmentNote;
            }

            return string.Empty;
        }
    }
}
