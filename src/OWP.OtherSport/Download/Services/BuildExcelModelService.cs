namespace OWP.OtherSport.Download.Services
{
    using AutoMapper;
    using OWP.Extensions;
    using OWP.OtherSport.Download.Constants;
    using OWP.OtherSport.Download.Models;
    using OWP.OtherSport.Submit.Contants;
    using OWP.OtherSport.Submit.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface BuildExcelModelService
    {
        Task<ExcelModel> BuildExcelModel(DownloadAssignmentRequest request);
    }

    public class BuildExcelModelServiceImp : BuildExcelModelService
    {
        private readonly CommandRepository commandRepository;

        private readonly IMapper mapper;

        public BuildExcelModelServiceImp(
            CommandRepository commandRepository,
            IMapper mapper)
        {
            this.commandRepository = commandRepository;
            this.mapper = mapper;
        }

        public async Task<ExcelModel> BuildExcelModel(DownloadAssignmentRequest request)
        {
            var excelModel = new ExcelModel();

            excelModel.HeaderInfo = BuildExcelHeader(request);
            excelModel.ExcelAssignmentModels = await BuildExcelAssignments(request);

            return excelModel;
        }

        private static ExcelHeaderInfoModel BuildExcelHeader(DownloadAssignmentRequest request)
        {
            return new ExcelHeaderInfoModel
            {
                FromDate = request.From.ToString("MMM dd, yyyy"),
                ToDate = request.To.ToString("MMM dd, yyyy"),
                Employee = string.IsNullOrEmpty(request.Employee) ? "All" : request.Employee,
                Sports = request.Sports,
                MatchStatus = request.MatchStatuses
            };
        }

        public async Task<ExcelAssignmentModel> BuildExcelAssignments(DownloadAssignmentRequest request)
        {
            var assignmentSnapshot = await commandRepository.Query(request);
            assignmentSnapshot.Matches = FilterMatchesByEmployee(request.Employee, assignmentSnapshot);

            var matchAssignments = BuildTraders(assignmentSnapshot);
            var matchMaxbets = BuildMaxbets(assignmentSnapshot);

            return new ExcelAssignmentModel
            {
                Matches = assignmentSnapshot.Matches.Values.OrderBy(m=>m.MatchNo).ToList(),
                MatchAssignments = matchAssignments,
                MaxBet = matchMaxbets
            };
        }

        private static Dictionary<int, MatchInfo> FilterMatchesByEmployee(string employee, AssignmentSnapshot assignmentSnapshot)
        {
            if (string.IsNullOrEmpty(employee))
            {
                return assignmentSnapshot.Matches;
            }

            var matches = new Dictionary<int, MatchInfo>();
            foreach (var key in assignmentSnapshot.Matches.Keys)
            {
                var assignments = assignmentSnapshot.MatchAssignments.GetValue(key, new List<MatchAssignment>());
                var existsAssignment = assignments.Any(match =>
                    match.Username.IsContains(employee) ||
                    match.Nickname.IsContains(employee));
                if (existsAssignment)
                {
                    matches.Add(key, assignmentSnapshot.Matches[key]);
                }
            }

            return matches;
        }

        public Dictionary<int, List<ExcelMatchAssignment>> BuildTraders(AssignmentSnapshot assignmentSnapshot)
        {
            var matchAssignments = new Dictionary<int, List<ExcelMatchAssignment>>();

            foreach (var key in assignmentSnapshot.Matches.Keys)
            {
                var sport = assignmentSnapshot.Matches[key].Sport;
                var assignments = assignmentSnapshot.MatchAssignments.GetValue(key, new List<MatchAssignment>());
                var excelAssignments = BuildTrader(sport, assignments);

                matchAssignments.Add(key, excelAssignments.ToList());
            }

            return matchAssignments;
        }

        private IEnumerable<ExcelMatchAssignment> BuildTrader(string sport, List<MatchAssignment> assignments)
        {
            foreach (var assignment in assignments)
            {
                var excelAssignment = mapper.Map<ExcelMatchAssignment>(assignment);
                excelAssignment.ColumnIndex = GetTraderColumnIndex(assignment, sport);
                excelAssignment.Value = GetTraderValue(assignment);

                yield return excelAssignment;
            }
        }

        private static int GetTraderColumnIndex(MatchAssignment assignment, string sport)
            => assignment.AssignmentType == (int)AssignmentType.Assignment
                ? ExcelConst.TraderColumn.GetValue(new Tuple<string, string>(sport, assignment.BetTypeName), 1)
                : ExcelConst.OtherTraderColumn.GetValue(new Tuple<string, string>(sport, assignment.BetTypeName), 2);

        private static string GetTraderValue(MatchAssignment assignment)
            => assignment.AssignmentType == (int)AssignmentType.Assignment
                ? assignment.Username
                : assignment.AssignmentNote.Replace("<user>", assignment.Username);

        public Dictionary<int, List<ExcelMatchMaxBet>> BuildMaxbets(AssignmentSnapshot assignmentSnapshot)
        {
            var matchMaxbets = new Dictionary<int, List<ExcelMatchMaxBet>>();

            foreach (var key in assignmentSnapshot.Matches.Keys)
            {
                var sport = assignmentSnapshot.Matches[key].Sport;
                var maxbets = assignmentSnapshot.MaxBet.GetValue(key, new List<MatchMaxBet>());
                var excelAssignments = BuildMaxbet(sport, maxbets);

                matchMaxbets.Add(key, excelAssignments.ToList());
            }

            return matchMaxbets;
        }

        private IEnumerable<ExcelMatchMaxBet> BuildMaxbet(string sport, List<MatchMaxBet> maxbets)
        {
            foreach (var maxbet in maxbets)
            {
                var excelMaxbets = mapper.Map<ExcelMatchMaxBet>(maxbet);
                excelMaxbets.ColumnIndex = GetMaxBetColumnIndex(maxbet, sport);
                excelMaxbets.Value = GetMaxbetValue(maxbet);

                yield return excelMaxbets;
            }
        }

        private static int GetMaxBetColumnIndex(MatchMaxBet maxbet, string sport)
            => ExcelConst.MaxbetColumn.GetValue(new Tuple<string, string>(sport, maxbet.BetTypeName), 3);

        private static string GetMaxbetValue(MatchMaxBet maxbet)
            => maxbet.MaxBet.ToString().TrimEnd('0').TrimEnd('.');
    }
}
