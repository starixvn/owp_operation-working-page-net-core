namespace OWP.UserManagement.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using OWP.Models;

    public static class RefResourceHelper
    {
        private static readonly IList<string> SortedGroups = new List<string>
        {
            "CLA-SPV",
            "CLA-SPV/WFH",
            "CLA",
            "CLA/WFH",
            "SLA-SPV",
            "SLA-SPV/3F",
            "(D)-SLA-SPV/3F",
            "SLA-SPV/3AF",
            "(D)-SLA-SPV/3AF",
            "SLA-SPV/WFH",
            "SLA/3F",
            "(D)-SLA/3F",
            "SLA/3AF",
            "(D)-SLA/3AF",
            "SLA/WFH",
            "C_LG",
            "C_LG(Special Team)",
            "C_LG(Support Team)",
            "S_LG",
            "S_LG(Special Team)",
            "SUPPORT TEAM"
        };

        public static IList<ReferenceResource> SortUserGroups(IList<ReferenceResource> groups)
            => groups.OrderBy(x => SortedGroups.IndexOf(x.Value) >= 0 ? SortedGroups.IndexOf(x.Value) : groups.Count).ToList();
    }
}