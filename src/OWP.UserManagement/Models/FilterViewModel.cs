namespace OWP.UserManagement.Models
{
    using System.Collections.Generic;
    using OWP.Models;

    public class FilterViewModel
    {
        public string User { get; set; }

        public int RoleId { get; set; }

        public int SectionId { get; set; }

        public int TotalRecords { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<ReferenceResource> Roles { get; set; }

        public IEnumerable<ReferenceResource> Sections { get; set; }

        public IEnumerable<ReferenceResource> Departments { get; set; }

        public IEnumerable<ReferenceResource> Categories { get; set; }

        public IEnumerable<ReferenceResource> LocationCodes { get; set; }

        public IEnumerable<ReferenceResource> Levels { get; set; }

        public IList<ReferenceResource> UserGroups { get; set; }

        public IEnumerable<ReferenceResource> Languages { get; set; }

        public UserManagementPermission Permission { get; set; }

    }
}