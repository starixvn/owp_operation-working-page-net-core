
namespace OWP.TicketTypeRecipient.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class TicketType : IEqualityComparer<TicketType>
    {
        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public bool Equals(TicketType x, TicketType y)
            => x.TypeId == y.TypeId && x.TypeName.Equals(y.TypeName, StringComparison.CurrentCultureIgnoreCase);

        public int GetHashCode([DisallowNull] TicketType obj)
            => TypeId;
    }
}