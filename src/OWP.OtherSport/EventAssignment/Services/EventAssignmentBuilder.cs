namespace OWP.OtherSport.EventAssignment.Services
{
    using AutoMapper;
    using Models;
    using Shared.Helper;
    using Submit.Contants;
    using Submit.Models;
    using System.Collections.Generic;
    using System.Linq;

    public interface EventAssignmentBuilder
    {
        List<EventAssignmentModel> Build(AssignmentSnapshot assignmentSnapshot, QueryEventAssignmentRequest request);
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

        public List<EventAssignmentModel> Build(AssignmentSnapshot assignmentSnapshot, QueryEventAssignmentRequest request)
        {
            RemoveInvalidNote(assignmentSnapshot.MatchAssignments);

            var filteredMatchesByEmployee = FilterHelper.FilterMatchesByEmployee(request.Employee, assignmentSnapshot);
            var filteredMatchesByMatchStatus = FilterHelper.FilterMatchesByMatchStatus(request.MatchStatuses, filteredMatchesByEmployee);

            var eventAssignments = mapper.Map<List<EventAssignmentModel>>(filteredMatchesByMatchStatus.Values);

            assignmentBuilder.Build(eventAssignments, assignmentSnapshot.MatchAssignments);
            matchSettingBuilder.Build(eventAssignments, assignmentSnapshot.MatchSettings);

            return eventAssignments;
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