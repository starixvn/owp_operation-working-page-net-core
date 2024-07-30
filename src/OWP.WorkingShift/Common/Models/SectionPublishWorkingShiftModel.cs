namespace OWP.WorkingShift.Common.Models;

using System;
using Resources;

public class SectionPublishWorkingShiftModel
{
    public int SectionId { get; set; }

    public string Name { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public bool? IsPublished { get; set; }

    public string LastPublishedBy { get; set; }

    public DateTime? LastPublishedDate { get; set; }

    public string DisplayTooltip
    {
        get
        {
            if (!IsPublished.HasValue)
            {
                return string.Empty;
            }

            var getPublishedBy = string.IsNullOrEmpty(LastPublishedBy) ? "SYSTEM" : LastPublishedBy;
            var getTooltipText = IsPublished == true ? ViewLang.WorkingShiftPublishSection_Tooltip :
                ViewLang.WorkingShiftUnPublishSection_Tooltip;

            return string.Format(getTooltipText, getPublishedBy, LastPublishedDate.Value.ToString("MMM dd, yyyy"));
        }
    }
}