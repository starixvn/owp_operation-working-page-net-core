/* global common */
/* exported maxBetFactory */
"use strict";

var maxBetFactory = function ($) {
    var urls = {
        GetMaxBets: 'EventManagement/MaxBetManagement/GetMaxBets',
        GetLeagueOptions: 'EventManagement/MaxBetManagement/GetLeagueOptions',
        UpdateIsActiveLeague: 'EventManagement/MaxBetManagement/UpdateIsActiveLeague',
        UpdateIsBlackListLeague: 'EventManagement/MaxBetManagement/UpdateIsBlackListLeague',
        DeleteLeagueSetting: 'EventManagement/MaxBetManagement/DeleteLeagueSetting',
        InsertLeagueOption: 'EventManagement/MaxBetManagement/InsertLeagueOption',
        InsertOrUpdateLeagueSetting: 'EventManagement/MaxBetManagement/InsertOrUpdateLeagueSetting',
        CopyLeagueSetting: 'EventManagement/MaxBetManagement/CopyLeagueSetting',
        InsertOrUpdateLeagueRemarkSetting: 'EventManagement/MaxBetManagement/InsertOrUpdateLeagueRemarkSetting'
    };

    return {
        GetMaxBets: function (param) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.GetMaxBets),
                dataType: 'json',
                data: param,
                method: 'GET',
                error: common.HandleAjaxError
            });
        },

        GetLeagueOptions: function () {
            return $.ajax({
                url: common.ResolveRootUrl(urls.GetLeagueOptions),
                dataType: 'json',
                method: 'GET',
                error: common.HandleAjaxError
            });
        },

        UpdateIsActiveLeague: function (param) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.UpdateIsActiveLeague),
                dataType: 'json',
                data: param,
                method: 'PUT',
                error: common.HandleAjaxError
            });
        },

        UpdateIsBlackListLeague: function (param) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.UpdateIsBlackListLeague),
                dataType: 'json',
                data: param,
                method: 'PUT',
                error: common.HandleAjaxError
            });
        },

        DeleteLeagueSetting: function (param) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.DeleteLeagueSetting),
                dataType: 'json',
                data: param,
                method: 'DELETE',
                error: common.HandleAjaxError
            });
        },

        InsertLeagueOption: function (param) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.InsertLeagueOption),
                dataType: 'json',
                data: param,
                method: 'POST',
                error: common.HandleAjaxError
            });
        },

        CreateEmptyOption: function (categoryId, categoryName, id, name) {
            return {
                CategoryId: categoryId,
                CategoryName: categoryName,
                Id: id,
                Name: name,
                Remarks: null,
                Spread: {
                    Handicap: null,
                    OverUnder: null,
                    FirstHalfHandicap: null,
                    FirstHalfOverUnder: null
                },
                MaxBetWeekday: {
                    OptionId: 2,
                    Handicap: null,
                    OverUnder: null,
                    FirstHalfHandicap: null
                },
                MaxBetWeekend: {
                    OptionId: 4,
                    Handicap: null,
                    OverUnder: null,
                    FirstHalfHandicap: null
                },
                OneXTwo: {
                    Spread: null,
                    FullTimeOneXTwo: null,
                    HalfTimeOneXTwo: null
                },
                Corner: {
                    Spread: null,
                    MaxBet: null
                },
                LastUpdatedTime: null,
                LastUpdatedBy: null,
                LastUpdatedTimeToDisplay: null
            };
        },

        InsertOrUpdateLeagueSetting: function (param, callback) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.InsertOrUpdateLeagueSetting),
                dataType: 'json',
                data: param,
                method: 'PUT',
                error: common.HandleAjaxError
            }).success(function (response) {
                if (response.IsSuccessful) {
                    callback();
                }
                else {
                    common.modalErrorMessage(response.Message);
                }
            });
        },

        CopyLeagueSetting: function (leagueSettings) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.CopyLeagueSetting),
                data: { leagueSettings: leagueSettings },
                method: 'POST',
                error: common.HandleAjaxError
            });
        },

        InsertOrUpdateLeagueRemarkSetting: function (param, callback) {
            return $.ajax({
                url: common.ResolveRootUrl(urls.InsertOrUpdateLeagueRemarkSetting),
                dataType: 'json',
                data: param,
                method: 'PUT',
                error: common.HandleAjaxError
            }).success(function (response) {
                if (response.IsSuccessful) {
                    callback();
                }
                else {
                    common.modalErrorMessage(response.Message);
                }
            });
        }
    };
}(jQuery);