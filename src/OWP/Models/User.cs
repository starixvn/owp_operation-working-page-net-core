using Newtonsoft.Json;
using OWP.Utils;

namespace OWP.Models;

public class User
{
    public int UserId { get; set; }

    public string UserName { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ProfilePicture { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? RoleId { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string RoleName { get; set; }

    public string FullName { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Avatar { get; set; }

    public string NickName { get; set; }

    public string EmployeeId { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Skype { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? DepartmentId { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string DepartmentName { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? SectionId { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string SectionName { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? CategoryId { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string CategoryName { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Extension { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Phone { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Email { get; set; }

    public string LocationCode { get; set; }

    public string Pickup { get; set; }

    public string Address { get; set; }

    public int? Level { get; set; }

    public string LevelName { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? GroupId { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string GroupName { get; set; }

    public string IsSecondAccount { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string FlagCode => UserHelper.GetFlagCodeFromLocationCode(LocationCode);

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string CompanyName => LocationCode;

    public string LanguageId { get; set; }

    public string Languages { get; set; }
}