/* global common */
/* exported workingShiftFactory */
'use strict';

var workingShiftFactory = (function ($) {
    var urls = {
        ValidateWorkingShift: "api/weekly-working-shift/validate",
        ImportWorkingShift: "api/weekly-working-shift/upload",
        SearchWorkingShiftWeekly: "api/weekly-working-shift/search",
        SearchWorkingShiftWeeklyIFrame: "api/weekly-working-shift/search-iframe",
        CopyWorkingShiftWeeklyPrevious: "api/working-shift/copy-previous",
        UpdateWorkingShiftCode: "api/working-shift/update-single-shift",
        RemoveWorkingShiftCode: "api/working-shift/remove-single-shift",
        UpdateDefaultShift: "api/working-shift-default",
        DefaultShiftGetWorkingShiftByUser: "api/working-shift/get-single-user",

        SearchWorkingShiftByIFrame: "WeeklyWorkingShift/IFrame",
        ValidateBeforeCreateWorkingShiftColor: "api/working-shift-color/validation-create",
        ValidateBeforeDeleteWorkingShiftColor: "api/working-shift-color/validation-delete",

        QuickInsertWorkingShiftColor: "api/working-shift-color",
        UpdateShiftColor: "api/working-shift-color",
        QuickDeleteWorkingShiftColor: "api/working-shift-color",

        UpdateTimezone: "api/working-shift-timezone",
        UpdateEmployeeWorkingShiftOrder: "api/working-shift-order",
        CopyWholeShifts: 'api/working-shift/copy-whole-shifts',
        CopySingleShift: 'api/working-shift/copy-single-shift',
        DeleteWholeShifts: 'api/working-shift/delete-whole-shifts',
        PublishWeeklyWorkingShift: 'api/working-shift-publish',
        QueryShiftModifyHistory: 'api/weekly-working-shift/viewlog'
    };

    return {
        ValidateWorkingShift: function (file) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.ValidateWorkingShift),
                data: file,
                processData: false,
                contentType: false,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        ImportWorkingShift: function (shift) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.ImportWorkingShift),
                data: shift,
                contentType: "application/json; charset=utf-8",
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        UpdateShiftColor: function (shift) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.UpdateShiftColor),
                data: JSON.stringify(shift),
                contentType: "application/json; charset=utf-8",
                method: 'PUT',
                error: common.HandleAjaxError
            });
        },
        SearchWorkingShiftWeekly: function (data) {
            var url = urls.SearchWorkingShiftWeekly;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        SearchWorkingShiftWeeklyIFrame: function (data) {
            var url = urls.SearchWorkingShiftWeeklyIFrame;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        CopyWorkingShiftWeeklyPrevious: function (data) {
            var url = urls.CopyWorkingShiftWeeklyPrevious;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        UpdateWorkingShiftRemark: function (data) {
            return $.ajax({
                url: common.ResolveRootUrl('state-api/simple-data/update'),
                contentType: "application/json",
                dataType: 'json',
                data: JSON.stringify(data),
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        UpdateWorkingShiftCode: function (data) {
            var url = urls.UpdateWorkingShiftCode;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        RemoveWorkingShiftCode: function (data) {
            var url = urls.RemoveWorkingShiftCode;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        UpdateDefaultShift: function (data) {
            var url = urls.UpdateDefaultShift;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        DefaultShiftGetWorkingShiftByUser: function (data) {
            var url = urls.DefaultShiftGetWorkingShiftByUser;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },

        RemoveWorkingShiftDefault: function (data) {
            var url = urls.RemoveWorkingShiftDefault;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        QuickInsertWorkingShiftColor: function (data) {
            var url = urls.QuickInsertWorkingShiftColor;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        QuickDeleteWorkingShiftColor: function (data) {
            var url = urls.QuickDeleteWorkingShiftColor;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'DELETE',
                error: common.HandleAjaxError
            });
        },
        ValidateBeforeCreateWorkingShiftColor: function (data) {
            var url = urls.ValidateBeforeCreateWorkingShiftColor;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        ValidateBeforeDeleteWorkingShiftColor: function (data) {
            var url = urls.ValidateBeforeDeleteWorkingShiftColor;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        UpdateTimezone: function (data) {
            var url = urls.UpdateTimezone;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'PUT',
                error: common.HandleAjaxError
            });
        },
        UpdateEmployeeWorkingShiftOrder: function (data) {
            var url = urls.UpdateEmployeeWorkingShiftOrder;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },

        CopyWholeShifts: function (data) {
            var url = urls.CopyWholeShifts;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        DeleteWholeShifts: function (data) {
            var url = urls.DeleteWholeShifts;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        CopySingleShift: function (data) {
            var url = urls.CopySingleShift;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        PublishWeeklyWorkingShift: function (data) {
            var url = urls.PublishWeeklyWorkingShift;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        QueryShiftModifyHistory: function (data) {
            var url = urls.QueryShiftModifyHistory;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            });
        }
    };
})(jQuery);