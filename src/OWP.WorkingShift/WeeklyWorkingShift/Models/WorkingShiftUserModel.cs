namespace OWP.WorkingShift.WeeklyWorkingShift.Models;

using System;

public class WorkingShiftUserModel
{
    public int UserId { get; set; }

    public string EmployeeId { get; set; }

    public string Address { get; set; }

    public string Username { get; set; }

    public string Nickname { get; set; }

    public int? OrderId { get; set; }

    public int SectionId { get; set; }

    public string SectionName { get; set; }

    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; }

    public string CategoryId { get; set; }

    public string Phone { get; set; }

    public string Skype { get; set; }

    public string Extension { get; set; }

    public string Email { get; set; }

    public string Remark { get; set; }

    public bool Pickup { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public string DisplayName => !string.IsNullOrEmpty(Nickname) ? Nickname : Username;

    public Common.Models.WorkingShiftModel[] WorkingShifts { get; set; }

    public int?[] DefaultShiftIds { get; set; }
}