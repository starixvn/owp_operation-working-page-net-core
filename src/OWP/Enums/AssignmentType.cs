using System.ComponentModel;

namespace OWP.Enums;

public enum AssignmentType
{
    [Description("Main Trader")] Main = 1,

    [Description("Others Trader")] Others = 2
}