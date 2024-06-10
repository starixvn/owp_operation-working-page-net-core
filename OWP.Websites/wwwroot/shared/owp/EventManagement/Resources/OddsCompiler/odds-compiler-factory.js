/* global common */
/* exported oddsCompilerFactory */
"use strict";

var oddsCompilerFactory = function ($) {
    var urls = {
        GetOdds: 'api/odds-compiler/query',
        Insert: 'EventManagement/OddsCompiler/Insert',
        UpdateHDP: 'EventManagement/OddsCompiler/UpdateHDP',
        UpdateOdds: 'EventManagement/OddsCompiler/UpdateOdds',
        Delete: 'EventManagement/OddsCompiler/Delete'
    };

    return {
        GetOdds: function (param) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.GetOdds),
                data: param,
                dataType: 'json',
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        Insert: function (param) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.Insert),
                data: param,
                dataType: 'json',
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        UpdateHDP: function (param) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.UpdateHDP),
                data: param,
                dataType: 'json',
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        UpdateOdds: function (param) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.UpdateOdds),
                data: param,
                dataType: 'json',
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        Delete: function (param) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.Delete),
                data: JSON.stringify({
                    'criteria': param
                }),
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                method: 'POST',
                error: common.HandleAjaxError
            });
        }
    };
}(jQuery);