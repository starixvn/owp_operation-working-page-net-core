/* global common */
/* exported filterFactory */
"use strict";

var filterFactory = (function ($) {
    var urls = {
        AddConditionControl: "EventManagement/Filter/AddConditionControl",
        Create: "EventManagement/Filter/Create",
        Update: "EventManagement/Filter/Update"
    };

    function showLoadingProcess() {
        $('.loading-process').css('display', 'inline');
    }

    function hideLoadingProcess() {
        $('.loading-process').css('display', 'none');
    }

    return {
        Create: function (model) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.Create),
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    'model': model
                }),
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        Update: function (model) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.Update),
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    'model': model
                }),
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        GetConditionControl: function (id) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.AddConditionControl),
                dataType: 'html',
                data: { fieldId: id },
                method: 'GET',
                error: common.HandleAjaxError
            });
        },
        ShowLoadingProcess: showLoadingProcess,
        HideLoadingProcess: hideLoadingProcess
    };
})(jQuery);