using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.PreMatchNotification.Assignment.Services
{
    public interface AssignmentValueGenerator
    {
        IEnumerable<ChangedValue> Create(IList<AssignmentSnapshot> oldSnapshot, IList<AssignmentSnapshot> newSnapshot, IList<Assignee> assignees);
    }

    public class AssignmentValueGeneratorImp : AssignmentValueGenerator
    {
        public IEnumerable<ChangedValue> Create(IList<AssignmentSnapshot> oldSnapshot, IList<AssignmentSnapshot> newSnapshot, IList<Assignee> assignees)
        {
            var prematchIdUpdated = newSnapshot.Select(p => p.PreMatchLeagueId);
            foreach (var prematchId in prematchIdUpdated)
            {

            }
        }
    }
}