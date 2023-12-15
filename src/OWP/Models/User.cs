using OWP.Utils;

namespace OWP.Models;

public class User
{
    public int UserId { get; set; }

    public string UserName { get; set; }

    public string ProfilePicture { get; set; }

    public int? RoleId { get; set; }

    public string RoleName { get; set; }

    public string FullName { get; set; }

    public string Avatar { get; set; }

    public string NickName { get; set; }

    public string EmployeeId { get; set; }

    public string Skype { get; set; }

    public int? DepartmentId { get; set; }

    public string DepartmentName { get; set; }

    public int? SectionId { get; set; }

    public string SectionName { get; set; }

    public int? CategoryId { get; set; }

    public string CategoryName { get; set; }

    public string Extension { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string LocationCode { get; set; }

    public string Pickup { get; set; }

    public string Address { get; set; }

    public int? Level { get; set; }

    public string LevelName { get; set; }

    public int? GroupId { get; set; }

    public string GroupName { get; set; }

    public string IsSecondAccount { get; set; }

    public string FlagCode => UserHelper.GetFlagCodeFromLocationCode(LocationCode);

    public string CompanyName => LocationCode;

    public string LanguageId { get; set; }

    public string Languages { get; set; }
}