namespace OWP.WorkingShift.WeeklyWorkingShift.Models;

using System;
using System.Collections.Generic;
using OWP.Models;

public class FilterViewModel
{
    public bool ExcludeClosedAccount { get; set; } = true;

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public WorkingShiftPermission Permission { get; set; }

    public bool IsFrame { get; set; }

    public int? TimeZoneOffset { get; set; }

    public int CurrentUserId { get; set; }

    public int CurrentSectionId { get; set; }

    public IList<ReferenceResource> Sections { get; set; }

    public IList<ReferenceResource> Departments { get; set; }

    public IList<ReferenceResource> UserCategories { get; set; }

    public IList<UserLocation> UserLocations { get; set; }

    public IList<TimezoneSettingModel> TimezoneSettings { get; set; }
}