using System.Collections.Immutable;

namespace OWP.EventPicking.Constant;

public static class RAndDColorConstant
{
    private const string white = "#FFFFFF";
    private const string green = "#003A13";
    private const string newColor = "#0088FF";
    private const string deleteColor = "#FAC51F";
    private const string updateColor = "#B1D9BE";
    private const string cancelColor = "#AB383B";
    public static readonly ImmutableDictionary<string, (string,string)> RAndDColorDict = new Dictionary<string,(string, string)>
    {
        {"New match",(newColor,white)},
        {"Delete match",(deleteColor,white)},
        {"KO time update",(updateColor,green)},
        {"League name update",(updateColor,green)},
        {"Team name update",(updateColor,green)},
        {"TV update",(updateColor,green)},
        {"TV cancel",(cancelColor,white)},
        {"IPTV update",(updateColor,green)},
        {"IPTV cancel",(cancelColor,white)},       
        {"Provider update",(updateColor,green)},
        {"Provider cancel",(cancelColor,white)},
    }.ToImmutableDictionary();

}
