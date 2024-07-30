/* global common */
/* exported eventPickingFactory */
'use strict';

var eventPickingFactory = (function ($) {
    var urls = {
        RenderEventPickingTable: 'TaskScheduling/EventPicking/EventPickingTable',
        ExportEventPickingExcel: 'api/event-assignment/event-picking-old',
        ValidateParamGetEventPicking: 'TaskScheduling/EventPicking/ValidateParamGetEventPicking',
        UrlOfSearchLeague: 'EventManagement/LeagueManagement/GetLeague?selectTop=50'
    };

    return {
        RenderEventPickingTable: function (model) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.RenderEventPickingTable),
                dataType: 'json',
                data: model,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        ExportEventPickingExcel: function (date, success) {
         
            var xhr = new XMLHttpRequest();
            xhr.onload = function () {
                success();
            };
            xhr.open("GET", `${common.ResolveRootUrl(urls.ExportEventPickingExcel)}?date=${date}`, true);
            xhr.setRequestHeader("Content-Type", "application/json; charset=UTF-8");
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                  
                    const urlFake = window.URL.createObjectURL(xhr.response);
                    const link = document.createElement('a');
                    link.href = urlFake;
                    link.setAttribute('download', 'Event Picking-' + new Date(date).toLocaleDateString('fr-CA') +'.xlsx');
                    document.body.appendChild(link);
                    link.click();
                }
            };
            xhr.responseType = 'blob';
            xhr.send();
        },
        ValidateParamGetEventPicking: function (model) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.ValidateParamGetEventPicking),
                dataType: 'json',
                data: model,
                error: common.HandleAjaxError
            });
        },
        
        GetUrlOfLeague: function () {
            return urls.UrlOfSearchLeague;
        }
    };
})(jQuery);