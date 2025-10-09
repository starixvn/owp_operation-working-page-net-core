using System.Collections.Immutable;

namespace OWP.Event.EventShared.Helper;


public static class MappingShortName
{
    public static readonly IImmutableDictionary<string,string> ShortNameMapping = new Dictionary<string, string>
    {
        { "KO+15", "KO+15" },
        { "Fast Market", "FM" },
        { "10s Accept", "10s" },
        { "15s Accept", "15s" },
        { "20s Accept", "20s" },
        { "30s Accept", "30s" },
        { "5s Accept", "5s" },
        { "8s Accept", "8s" },
        { "12s Accept", "12s" },
        { "6s Accept", "6s" },
        { "1s Accept", "1s" },
        { "2s Accept", "2s" },
        { "3s Accept", "3s" },
        { "4s Accept", "4s" },
        { "7s Accept", "7s" },
        { "9s Accept", "9s" },
        { "11s Accept", "11s" },
        { "13s Accept", "13s" },
        { "14s Accept", "14s" },
        { "16s Accept", "16s" },
        { "17s Accept", "17s" },
        { "18s Accept", "18s" },
        { "19s Accept", "19s" },
        { "21s Accept", "21s" },
        { "22s Accept", "22s" },
        { "23s Accept", "23s" },
        { "24s Accept", "24s" },
        { "25s Accept", "25s" },
        { "35s Accept", "35s" },
        { "40s Accept", "40s" },
        { "45s Accept", "45s" },
        { "50s Accept", "50s" },
        { "55s Accept", "55s" },
        { "60s Accept", "60s" },
        { "65s Accept", "65s" },
        { "70s Accept", "70s" },
        { "75s Accept", "75s" },
        { "80s Accept", "80s" },
        { "85s Accept", "85s" },
        { "90s Accept", "90s" },

        { "OFF VR", "OVR" },
        { "OFF VR (Whole Time)", "OVR (WT)" },
        { "OFF VR 10 mins", "OVR 10'" },
        { "OFF VR HDP", "OVR HDP" },
        { "OFF VR HDP 10 mins", "OVR HDP 10'" },
        { "If Big HDP > OFF VR", "Big HDP > OVR" },
        { "If Big HDP > OFF VR 10 mins", "Big HDP > OVR 10'" },

        { "Time Machine", "TM" },
        { "1HDP", "1HDP" },
        { "2HDP", "2HDP" },
        { "2+1H", "2+1H" },
        { "3HDP", "3HDP" },
        { "4HDP", "4HDP" },

        { "2+4 lines", "2+4L" },
        { "3+3 lines", "3+3L" },
        { "3+4 lines", "3+4L" },
        { "4+4 lines", "4+4L" },
        { "1 line", "1L" },
        { "2 lines", "2L" },

        { "40BT", "40BT" },
        { "23BT", "23BT" },
        { "19BT", "19BT" },
        { "8BT", "8BT" }
    }.ToImmutableDictionary();


    public static string GetShortName(string fullName)
    {
       return CollectionExtensions.GetValueOrDefault(ShortNameMapping, fullName, fullName);
    }

}
