/* global common */
/* exported eventInformationFactory */
"use strict";

var eventInformationFactory = function ($) {
    var urls = {
        UpdateIsSelected: "EventManagement/EventInformation/UpdateIsSelected",
        GetDataEventInformation: 'EventManagement/EventInformation/GetDataEventInformation',
        GetColumnShowHideByFilterId: 'EventManagement/EventInformation/GetColumnShowHideByFilterId',
        RenderCreateFilter: 'EventManagement/Filter/Create',
        RenderFilterDropdown: 'EventManagement/EventInformation/GetFilter',
        SetDefaultFilter: 'EventManagement/Filter/SetDefaultFilter',
        RemoveDefaultFilter: 'EventManagement/Filter/DeleteDefaultFilter',
        RemoveFilter: 'EventManagement/Filter/Delete',
        GetDuty: "TaskScheduling/TaskList/TaskListTable",
        RenderEditFilter: 'EventManagement/Filter/Edit',
    };

    return {
        RemoveFilter: function (filterId) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.RemoveFilter),
                data: {
                    filterId: filterId
                },
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        SetDefaultFilter: function (filterId) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.SetDefaultFilter),
                data: {
                    filterId: filterId
                },
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        RemoveDefaultFilter: function () {
            return $.ajax({
                url: common.ResolveRootUrl(urls.RemoveDefaultFilter),
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        RenderFilterDropdown: function () {
            return $.ajax({
                url: common.ResolveRootUrl(urls.RenderFilterDropdown),
                dataType: 'html',
                method: 'GET',
                error: common.HandleAjaxError
            });
        },
        RenderCreateFilter: function () {
            return $.ajax({
                url: common.ResolveRootUrl(urls.RenderCreateFilter),
                dataType: 'html',
                method: 'GET',
                error: common.HandleAjaxError
            });
        },
        UpdateIsSelected: function (criteria) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.UpdateIsSelected),
                data: criteria,
                method: 'PUT',
                error: common.HandleAjaxError
            });
        },
        GetDataEventInformation: function (model) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.GetDataEventInformation),
                data: model,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        GetColumnShowHideByFilterId: function (filterId) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.GetColumnShowHideByFilterId),
                dataType: 'html',
                data: { filterId: filterId },
                method: 'GET',
                error: common.HandleAjaxError
            });
        },
        GetDutyList: function () {
            return $.ajax({
                url: common.ResolveRootUrl(urls.GetDuty),
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        RenderEditFilter: function (filterId) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.RenderEditFilter),
                data: {
                    filterId: filterId
                },
                dataType: 'html',
                method: 'GET',
                error: common.HandleAjaxError
            });
        },
    };
}(jQuery);