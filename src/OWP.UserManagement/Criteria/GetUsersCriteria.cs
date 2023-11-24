namespace OWP.UserManagement.Criteria
{
    using Fanex.Data.Repository;

    public class GetUsersCriteria : CriteriaBase
    {
        public string User { get; set; }

        public int? RoleId { get; set; }

        public int? SectionId { get; set; }

        public int? DepartmentId { get; set; }

        public int? CategoryId { get; set; }

        public string LocationCode { get; set; }

        public int TotalRecords { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int SiteId { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => "OWP_UserListGet";
    }
}