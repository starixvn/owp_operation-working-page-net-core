namespace Fanex.NAP.OWP.TaskScheduling.Models.TaskListToday
{
    using System.Collections.Generic;

    public class OUSpecificBetTypeGroupListModel
    {
        public bool IsMajorBetType { get; set; }

        public string BetTypeName { get; set; }

        public string MaxbetSpread { get; set; }

        public bool IsHasMaxbetSpread { get; set; }

        public IEnumerable<Assignment> MainAssignment { get; set; }

        public IEnumerable<Assignment> OthersAssignment { get; set; }
    }
}