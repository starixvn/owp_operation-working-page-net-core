using AutoMapper;
using OWP.Extensions;
using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.Submit.Contants;
using OWP.OtherSport.Submit.Models;
using System.Collections.Generic;
using System.Linq;

namespace OWP.OtherSport.EventAssignment.Services
{
    public interface EventAssignmentBuilder
    {
        List<EventAssignmentModel> Build(AssignmentSnapshot assignmentSnapshot, string employee);
    }

    public class EventAssignmentBuilderImp : EventAssignmentBuilder
    {
        private readonly AssignmentBuilder assignmentBuilder;

        private readonly MatchSettingBuilder matchSettingBuilder;

        private readonly IMapper mapper;

        public EventAssignmentBuilderImp(
            AssignmentBuilder assignmentBuilder,
            MatchSettingBuilder matchSettingBuilder,
            IMapper mapper)
        {
            this.assignmentBuilder = assignmentBuilder;
            this.matchSettingBuilder = matchSettingBuilder;
            this.mapper = mapper;
        }

        public List<EventAssignmentModel> Build(AssignmentSnapshot assignmentSnapshot, string employee)
        {
            RemoveInvalidNote(assignmentSnapshot.MatchAssignments);

            var filterdMatches = FilterMatchesByEmployee(employee, assignmentSnapshot);
            var eventAssignments = mapper.Map<List<EventAssignmentModel>>(filterdMatches.Values);

            assignmentBuilder.Build(eventAssignments, assignmentSnapshot.MatchAssignments);
            matchSettingBuilder.Build(eventAssignments, assignmentSnapshot.MatchSettings);

            return eventAssignments;
        }

        public static Dictionary<int, MatchInfo> FilterMatchesByEmployee(string employee, AssignmentSnapshot assignmentSnapshot)
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

        public static void RemoveInvalidNote(Dictionary<int, List<MatchAssignment>> assignments)
        {
            foreach (var key in assignments.Keys)
            {
                var assignmentList = assignments[key];

                assignments[key] = assignmentList.Where(a =>
                    !(string.Equals(a.AssignmentNote, "#<user>") &&
                    a.UserId == 0 &&
                    a.AssignmentType == (int)AssignmentType.OtherAssignment))
                .OrderBy(x => x.Order)
                .ToList();
            }
        }
    }
}