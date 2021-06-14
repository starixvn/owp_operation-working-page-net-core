namespace OWP.PreMatchNotification.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Constants;
    using Models;

    public interface ChangedValueAggregator
    {
        List<ChangedValue> Aggregate(List<ChangedValue> changedValues);

        IEnumerable<ChangedValue> GetAggreatedValueForIndirectOtherAssignments(IEnumerable<ChangedValue> changedValues);

        ChangedValue GetAggregatedValueForDoubleAssignment(ChangedValue deletingAssignment, ChangedValue insertingChangedValue);

        IEnumerable<ChangedValue> GetAggregatedValueForIndirectMainAssignments(IEnumerable<ChangedValue> changedValues);

        IEnumerable<ChangedValue> GetIndirectMainAssignmentChangedValues(List<ChangedValue> changedValues);

        IEnumerable<ChangedValue> GetIndirectOtherAssignmentChangedValues(List<ChangedValue> changedValues);

        IEnumerable<ChangedValue> GetNonAggregatedChangedValues(List<ChangedValue> changedValues);
    }

    public class ChangedValueAggregatorImp : ChangedValueAggregator
    {
        internal class AssignmentGroup
        {
            public int InfoSiteMatchId { get; set; }

            public int BetType { get; set; }

            public string BetTypeName { get; set; }

            public int AssigneeId { get; set; }

            public bool IsIndirectChanged { get; set; }

            public bool IsWatcher { get; set; }

            public override bool Equals(object obj)
            {
                var other = obj as AssignmentGroup;

                var condition1 = InfoSiteMatchId == other.InfoSiteMatchId && BetType == other.BetType && AssigneeId == other.AssigneeId;
                var condition2 = IsIndirectChanged == other.IsIndirectChanged;

                return condition1 && condition2;
            }

            public override int GetHashCode()
                => InfoSiteMatchId * 17
                    + BetType.GetHashCode() * 19
                    + AssigneeId * 29
                    + (IsIndirectChanged ? 37 : 0);
        }

        private readonly IMapper mapper;

        public ChangedValueAggregatorImp(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public List<ChangedValue> Aggregate(List<ChangedValue> changedValues)
        {
            var nonAggregatedChangedValues = GetNonAggregatedChangedValues(changedValues);
            var indirectMainAssignmentChangedValues = GetIndirectMainAssignmentChangedValues(changedValues);
            var indirectOtherAssignmentChangedValues = GetIndirectOtherAssignmentChangedValues(changedValues);

            var aggregatedValueForIndirectMainAssignments = GetAggregatedValueForIndirectMainAssignments(indirectMainAssignmentChangedValues);
            var aggreatedValueForIndirectOtherAssignments = GetAggreatedValueForIndirectOtherAssignments(indirectOtherAssignmentChangedValues);

            return nonAggregatedChangedValues.Union(aggregatedValueForIndirectMainAssignments)
                                            .Union(aggreatedValueForIndirectOtherAssignments)
                                            .Distinct(new AssignmentChangedValueComparer())
                                            .ToList();
        }

        public IEnumerable<ChangedValue> GetNonAggregatedChangedValues(List<ChangedValue> changedValues)
        {
            return changedValues.Where(m =>
            {
                var isMainAssignment = m.MessageTypeId == (int)MsgTemplateCode.AssignmentDelete || m.MessageTypeId == (int)MsgTemplateCode.AssignmentInsert;
                var isOtherAssignment = m.MessageTypeId == (int)MsgTemplateCode.OtherAssignmentDelete || m.MessageTypeId == (int)MsgTemplateCode.OtherAssignmentInsert;

                return !m.IsIndirectChangedAssignment || !isMainAssignment && !isOtherAssignment;
            });
        }

        public IEnumerable<ChangedValue> GetIndirectOtherAssignmentChangedValues(List<ChangedValue> changedValues)
        {
            return changedValues.Where(m =>
            {
                var isOtherAssignment = m.MessageTypeId == (int)MsgTemplateCode.OtherAssignmentDelete ||
                                        m.MessageTypeId == (int)MsgTemplateCode.OtherAssignmentInsert;

                return m.IsIndirectChangedAssignment && isOtherAssignment;
            });
        }

        public IEnumerable<ChangedValue> GetIndirectMainAssignmentChangedValues(List<ChangedValue> changedValues)
        {
            return changedValues.Where(m =>
            {
                var isMainAssignment = m.MessageTypeId == (int)MsgTemplateCode.AssignmentDelete || m.MessageTypeId == (int)MsgTemplateCode.AssignmentInsert;

                return m.IsIndirectChangedAssignment && isMainAssignment;
            });
        }

        public IEnumerable<ChangedValue> GetAggregatedValueForIndirectMainAssignments(IEnumerable<ChangedValue> changedValues)
        {
            var changedValuesGroupBy = changedValues.GroupBy(v => new AssignmentGroup
            {
                InfoSiteMatchId = v.InfoSiteMatchId,
                BetTypeName = v.BetTypeName,
                AssigneeId = v.UserId,
                IsIndirectChanged = v.IsIndirectChangedAssignment
            });

            foreach (var item in changedValuesGroupBy)
            {
                var deletingAssignment = item.FirstOrDefault(m => m.MessageTypeId == (int)MsgTemplateCode.AssignmentDelete);
                var insertingAssignment = item.FirstOrDefault(m => m.MessageTypeId == (int)MsgTemplateCode.AssignmentInsert);

                if (deletingAssignment != null && insertingAssignment != null)
                {
                    yield return GetAggregatedValueForDoubleAssignment(deletingAssignment, insertingAssignment);
                }

                if (deletingAssignment != null && insertingAssignment == null)
                {
                    yield return GetAggregatedValueForSingleAssignment(deletingAssignment);
                }

                if (insertingAssignment != null && deletingAssignment == null)
                {
                    yield return GetAggregatedValueForSingleAssignment(insertingAssignment);
                }
            }
        }

        public ChangedValue GetAggregatedValueForDoubleAssignment(ChangedValue deletingAssignment, ChangedValue insertingChangedValue)
        {
            var aggregatedChangedValue = mapper.Map<ChangedValue>(deletingAssignment);

            aggregatedChangedValue.MessageTypeId = (int)MsgTemplateCode.AssignmentUpdate;
            aggregatedChangedValue.OldValue = deletingAssignment.OldValue;
            aggregatedChangedValue.NewValue = insertingChangedValue.NewValue;

            return aggregatedChangedValue;
        }

        private ChangedValue GetAggregatedValueForSingleAssignment(ChangedValue changedValue)
        {
            var aggregatedChangedValue = mapper.Map<ChangedValue>(changedValue);
            aggregatedChangedValue.MessageTypeId = (int)MsgTemplateCode.AssignmentUpdate;
            return aggregatedChangedValue;
        }

        public IEnumerable<ChangedValue> GetAggreatedValueForIndirectOtherAssignments(IEnumerable<ChangedValue> changedValues)
        {
            foreach (var value in changedValues)
            {
                value.MessageTypeId = (int)MsgTemplateCode.AssignmentUpdate;
            }

            return changedValues;
        }
    }
}