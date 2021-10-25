using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public abstract class TicketDetailCriteriaBase: CriteriaBase
    {
        public int TicketId { get; set; }

        public override bool IsValid()
            => TicketId > 0;
    }
}
