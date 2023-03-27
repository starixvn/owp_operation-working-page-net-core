
namespace OWP.AssigneeSuggestionList.Models
{
    public record AssignmentModel
    {
        public int OWPMatchId { get; set; }

        public DateTime KOTime { get; set; }

        public DateTime EventDateTime { get; set; }

        public int BettypeId { get; set; }

        public string Remark { get; set; }

        public string LeagueName { get; set; }

        public int UserId { get; set; }

        public bool IsMajorLeague { get; set; }

        public bool IsMainMatch { get; set; }

        public bool IsFantasyMatch { get; set; }

        public bool IsCornerMatch { get; set; }

        public bool Is15Mins { get; set; }

        public int? ParentMatchId { get; set; }

        public string ColorCode { get; set; }
    }

    public class AssignmentComparer : IEqualityComparer<AssignmentModel>
    {
        public bool Equals(AssignmentModel source, AssignmentModel dest)
        {
            if (object.ReferenceEquals(source, dest))
            {
                return true;
            }

            if (object.ReferenceEquals(source, null) || object.ReferenceEquals(dest, null))
            {
                return false;
            }

            return source.OWPMatchId == dest.OWPMatchId && source.LeagueName == dest.LeagueName;
        }

        public int GetHashCode(AssignmentModel obj)
        {
            if (obj is null)
            {
                return 0;
            }

            int idHashCode = obj.OWPMatchId.GetHashCode();

            int leagueNameHashCode = obj.LeagueName is null ? 0 : obj.LeagueName.GetHashCode();

            return idHashCode ^ leagueNameHashCode;
        }
    }
}