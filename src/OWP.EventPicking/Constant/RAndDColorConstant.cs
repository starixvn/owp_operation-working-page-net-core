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
    public static readonly ImmutableDictionary<int, (string,string)> RAndDColorDict = new Dictionary<int,(string, string)>
    {
        {1,(newColor,white)},
        {2,(updateColor,green)},
        {3,(updateColor,green)},
        {4,(updateColor,green)},
        {5,(updateColor,green)},
        {6,(cancelColor,white)},
        {7,(updateColor,green)},
        {8,(cancelColor,white)},       
        {9,(updateColor,green)},
        {10,(cancelColor,white)},
        {11,(deleteColor,white)}
    }.ToImmutableDictionary();

}
