"use strict";

var dailyWorkingShiftService = (function ($) {
    return {
        search: function (data, cb) {
            return $.ajax({
                url: common.ResolveRootUrl("api/daily-working-shift/query"),
                data: data,
                method: 'POST',
                error: common.HandleAjaxError
            }).success(cb);
        },
        getShifts: function (cb) {
            return $.ajax({
                url: common.ResolveRootUrl("api/working-shift-color/get-all-gmt8"),
                method: 'POST',
                error: common.HandleAjaxError
            }).success(cb);
        },
        updateWorkingShiftRemark: function (data, cb) {
            return $.ajax({
                url: common.ResolveRootUrl('state-api/simple-data/update'),
                contentType: "application/json",
                dataType: 'json',
                data: JSON.stringify(data),
                method: 'POST',
                error: common.HandleAjaxError
            }).success(cb);
        },
        insertUpdateQueryEmployeeWorkingShiftNote: function (data, cb) {
            return $.ajax({
                url: common.ResolveRootUrl("state-api/simple-data/query"),
                contentType: "application/json",
                dataType: 'json',
                data: JSON.stringify(data),
                method: 'POST',
                error: common.HandleAjaxError
            }).success(cb);
        },
        deleteEmployeeWorkingShiftNote: function (data, cb) {
            return $.ajax({
                url: common.ResolveRootUrl("state-api/simple-data/update"),
                contentType: "application/json",
                dataType: 'json',
                data: JSON.stringify(data),
                method: 'POST',
                error: common.HandleAjaxError
            }).success(cb);
        }
    }
})(jQuery);