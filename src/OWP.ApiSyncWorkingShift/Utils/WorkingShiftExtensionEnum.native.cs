namespace OWP.ApiSyncWorkingShift.Utils
{
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Reviewed.")]
    public enum WorkingShiftExtension
    {
        [Description("(REAL)")]
        Real = 1,

        [Description("(SPECIAL)")]
        Special = 2
    }
}