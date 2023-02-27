using System.Diagnostics.CodeAnalysis;

namespace OWP.TicketTypeRecipient.Models;

public class TicketType : IEqualityComparer<TicketType>
{
    public int TypeId { get; set; }

    public string TypeName { get; set; }

    public bool Equals(TicketType x, TicketType y)
        => x.TypeId == y.TypeId && x.TypeName.Equals(y.TypeName, StringComparison.CurrentCultureIgnoreCase);

    public int GetHashCode([DisallowNull] TicketType obj)
        => TypeId;
}