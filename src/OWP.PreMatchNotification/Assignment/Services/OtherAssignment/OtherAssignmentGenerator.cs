using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.PreMatchNotification.Assignment.Services.OtherAssignment
{
    public interface OtherAssignmentGenerator
    {
        IEnumerable<ChangedValue> Create(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);
    }

    public class OtherAssignmentGeneratorImp : OtherAssignmentGenerator
    {
        public IEnumerable<ChangedValue> Create(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
        {
            throw new NotImplementedException();
        }
    }
}
