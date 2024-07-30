namespace OWP.WorkingShift.ViewLog.Constants;

using System.ComponentModel;

public enum ActionId
{
    [Description("Apply/Update default shifts")]
    UpdateDefaultShift = 74,

    [Description("Copy shifts for a whole week")]
    CopyWholeShifts = 75,

    [Description("Delete shifts for a whole week")]
    DeleteWholeShifts = 76,

    [Description("Copy a shift")]
    CopySingleShift = 77,

    [Description("Add/Update a shift")]
    UpdateShiftCode = 78,

    [Description("Delete a shift")]
    DeleteShift = 79,

    [Description("Apply the default shifts")]
    ApplyDefaultShift = 84,

    [Description("Upload working shifts")]
    UploadWorkingShift = 85,

    [Description("Copy shifts from the previous week")]
    CopyFromPreviousWeek = 87,

    [Description("Publish shifts")]
    PublishShift = 89
}