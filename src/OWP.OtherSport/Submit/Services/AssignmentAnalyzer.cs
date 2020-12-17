namespace OWP.OtherSport.Submit.Services
{
    using Models;
    using Contants;
    using System.Collections.Generic;

    public interface AssignmentAnalyzer
    {
        IEnumerable<MatchAssignment> GetAssignmentsOfMatch(AssignmentModel assignment);
    }

    public class AssignmentModel // temp
    {
        public int OSMatchId { get; set; }
        public string MatchOffer { get; set; }
        public string Sport { get; set; }
        public string FT_HDP { get; set; }
        public string OneH_HDP { get; set; }
        public string OneQ_3Q_HDP { get; set; }
        public string FT_OU_OE { get; set; }
        public string OneH_OU_OE { get; set; }
        public string OneQ_3Q_OU_OE { get; set; }
    }

    public class AssignmentAnalyzerImp: AssignmentAnalyzer
    {
        public IEnumerable<MatchAssignment> GetAssignmentsOfMatch(AssignmentModel assignment)
        {
            switch (assignment.Sport)
            {
                case Sport.Basketball:
                    return GetBasketballAssignments(assignment);

                default:
                    return new List<MatchAssignment>();
            }
        }

        public IEnumerable<MatchAssignment> GetBasketballAssignments(AssignmentModel assignment)
        {
            yield return new MatchAssignment
            {
                
            }
        }
    }
}
