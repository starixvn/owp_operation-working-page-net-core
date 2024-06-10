/* global common */
/* exported specialLiveProductFactory */
"use strict";

var specialLiveProductFactory = function ($) {
    var urls = {
        GetMinorSettings: 'EventManagement/MaxBetManagement/GetMinorSettings',
        UpdateMinorSetting: 'EventManagement/MaxBetManagement/UpdateMinorSetting',
        ResetMinorSetting: 'EventManagement/MaxBetManagement/ResetMinorSetting'
    };

    return {
        GetMinorSettings: function () {
            return $.ajax({
                url: common.ResolveRootUrl(urls.GetMinorSettings),
                dataType: 'json',
                method: 'GET',
                error: common.HandleAjaxError
            });
        },

        UpdateMinorSetting: function (param) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.UpdateMinorSetting),
                dataType: 'json',
                data: param,
                method: 'PUT',
                error: common.HandleAjaxError
            });
        },

        ResetMinorSetting: function (param) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.ResetMinorSetting),
                dataType: 'json',
                data: param,
                method: 'PUT',
                error: common.HandleAjaxError
            });
        }
    };
}(jQuery);