using System.Collections.Immutable;

namespace OWP.EventPicking.Constant;

public static class RAndDColorConstant
{
    public static readonly ImmutableDictionary<string, (string,string)> RAndDColorDict = new Dictionary<string,(string, string)>
    {
        {"New match",("#0088FF","#FFFFFF")},
        {"Delete match",("#FAC51F","#FFFFFF")},
        {"KO time update",("#B1D9BE","#003A13")},
        {"League name update",("#B1D9BE","#003A13")},
        {"Team name update",("#B1D9BE","#003A13")},
        {"TV update",("#B1D9BE","#003A13")},
        {"TV cancel",("#AB383B","#FFFFFF")},
        {"IPTV update",("#B1D9BE","#003A13")},
        {"IPTV cancel",("#AB383B","#FFFFFF")},       
        {"Provider update",("#B1D9BE","#003A13")},
        {"Provider cancel",("#AB383B","#FFFFFF")},
    }.ToImmutableDictionary();

}
