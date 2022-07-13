namespace OWP.ApiSyncWorkingShift.Utils
{
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    public enum WorkingShiftExtension
    {
        [Description("(REAL)")]
        Real = 1,

        [Description("(SPECIAL)")]
        Special = 2
    }
}