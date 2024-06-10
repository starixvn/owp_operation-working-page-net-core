/*jshint strict: false*/
/* global  common */
/* exported messageBoxFactory */

var messageBoxFactory = (function ($) {
    var urls = {
        GetListMessage: 'api/application/message',
        UpdateMessagesStatus: 'api/application/message',
        GetFunctionPolicy: 'api/application/authorize'
    };

    return {
        GetListMessage: function (model) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.GetListMessage),
                dataType: 'json',
                data: model,
                method: 'GET',
                error: common.HandleAjaxError
            });
        },
        UpdateMessagesStatus: function (model) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.UpdateMessagesStatus),
                data: model,
                method: 'PUT',
                error: common.HandleAjaxError
            });
        },
        GetFunctionPolicy: function (functionKey) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.GetFunctionPolicy),
                data: {
                    functionKey: functionKey
                },
                method: 'POST',
                error: common.HandleAjaxError
            });
        }
    };
})(jQuery);