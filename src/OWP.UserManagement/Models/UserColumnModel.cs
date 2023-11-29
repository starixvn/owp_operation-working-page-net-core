namespace OWP.UserManagement.Models
{
    public class UserColumnModel
    {
        public UserColumnModel(UserManagementPermission permissions, string section)
        {
            Permission = permissions;
            Section = section;
        }

        public string HeaderText { get; set; }

        public string Name { get; set; }

        public int Width { get; set; } = 100;

        public string HeaderAlign { get; set; } = "left";

        public string CellAlign { get; set; } = "left";

        public UserManagementPermission Permission { get; set; }

        public string Section { get; set; }

    }
}