"use strict";

var workingShiftService = function ($) {
    return {
        ValidateWorkingShift: function (file, cb) {
            return workingShiftFactory.ValidateWorkingShift(file).success(cb);
        },
        ImportWorkingShift: function (shift, cb) {
            return workingShiftFactory.ImportWorkingShift(shift).success(cb);
        },
        UpdateShiftColor: function (shift, cb) {
            return workingShiftFactory.UpdateShiftColor(shift).success(cb);
        },
        SearchWorkingShiftWeekly: function (data, cb) {
            return workingShiftFactory.SearchWorkingShiftWeekly(data).success(cb);
        },
        SearchWorkingShiftWeeklyIFrame: function (data, cb) {
            return workingShiftFactory.SearchWorkingShiftWeeklyIFrame(data).success(cb);
        },
        CopyWorkingShiftWeeklyPrevious: function (data, cb) {
            return workingShiftFactory.CopyWorkingShiftWeeklyPrevious(data).success(cb);
        },
        UpdateWorkingShiftRemark: function (data, cb) {
            return workingShiftFactory.UpdateWorkingShiftRemark(data).success(cb);
        },
        UpdateWorkingShiftCode: function (data, cb) {
            return workingShiftFactory.UpdateWorkingShiftCode(data).success(cb);
        },
        RemoveWorkingShiftCode: function (data, cb) {
            return workingShiftFactory.RemoveWorkingShiftCode(data).success(cb);
        },
        UpdateDefaultShift: function (data, cb) {
            return workingShiftFactory.UpdateDefaultShift(data).success(cb);
        },
        DefaultShiftGetWorkingShiftByUser: function (data, cb) {
            return workingShiftFactory.DefaultShiftGetWorkingShiftByUser(data).success(cb);
        },
        
        RemoveWorkingShiftDefault: function (data, cb) {
            return workingShiftFactory.RemoveWorkingShiftDefault(data).success(cb);
        },
        QuickInsertWorkingShiftColor: function (data, cb) {
            return workingShiftFactory.QuickInsertWorkingShiftColor(data).success(cb);
        },
        QuickDeleteWorkingShiftColor: function (data, cb) {
            return workingShiftFactory.QuickDeleteWorkingShiftColor(data).success(cb);
        },
        ValidateBeforeCreateWorkingShiftColor: function (data, cb) {
            return workingShiftFactory.ValidateBeforeCreateWorkingShiftColor(data).success(cb);
        },
        ValidateBeforeDeleteWorkingShiftColor: function (data, cb) {
            return workingShiftFactory.ValidateBeforeDeleteWorkingShiftColor(data).success(cb);
        },
        UpdateTimezone: function (data, cb) {
            return workingShiftFactory.UpdateTimezone(data).success(cb);
        },
        UpdateEmployeeWorkingShiftOrder: function (data, cb) {
            return workingShiftFactory.UpdateEmployeeWorkingShiftOrder(data).success(cb);
        },
        CopyWholeShifts: function (data, cb) {
            return workingShiftFactory.CopyWholeShifts(data).success(cb);
        },
        DeleteWholeShifts: function (data, cb) {
            return workingShiftFactory.DeleteWholeShifts(data).success(cb);
        },
        CopySingleShift: function (data, cb) {
            return workingShiftFactory.CopySingleShift(data).success(cb);
        },
        PublishWeeklyWorkingShift: function (data, cb) {
            return workingShiftFactory.PublishWeeklyWorkingShift(data).success(cb);
        },
        QueryShiftModifyHistory: function (data, cb) {
            return workingShiftFactory.QueryShiftModifyHistory(data).success(cb);
        }
    };
}(jQuery);