/* global common, specialLiveProductFactory */
"use strict";

var specialLiveProduct = (function ($) {
    var $specialLiveProductGrid = $("#special-live-product");
    var model;

    function buildGroupHeaderRows(isHost, maxbetRanges) {
        var columns = [
            { HeaderText: "FT HDP", Name: "Bettype", MergeColNumber: 1, ThClass: 'dark-bg' },
        ];

        for (var i = 0; i < maxbetRanges.length; i++) {
            columns.push({ HeaderText: maxbetRanges[i].Name, Name: maxbetRanges[i].Id + ".Spread", MergeColNumber: 2, ThClass: 'dark-bg' });
        }

        if (isHost) {
            columns.push({ HeaderText: "Clear", Name: "Reset", MergeRowNumber: 2, ThClass: 'dark-bg' });
        }

        return [{ Columns: columns }];
    }

    function buildColumns(isHost, maxbetRanges) {
        var columns = [
            { HeaderText: "Bet Type", Width: 440, Name: "Bettype", HeaderAlign: "left", CellAlign: "left" },
        ];

        for (var i = 0; i < maxbetRanges.length; i++) {
            columns.push({ HeaderText: "Spread", Width: 85, Name: maxbetRanges[i].Id + ".Spread", HeaderAlign: "center", CellAlign: "right" });
            columns.push({ HeaderText: "Max Bet", Width: 85, Name: maxbetRanges[i].Id + ".Maxbet", HeaderAlign: "center", CellAlign: "right" });
        }
        if (isHost) {
            columns.push({ HeaderText: "Reset", Width: 55, Name: "Reset", HeaderAlign: "center", CellAlign: "center" });
        }

        return columns;
    }

    function buildBodyRows(data) {
        var bodyRows = [],
            isHost = data.IsHost;

        $.each(data.Settings, function (index, setting) {
            var columns = [{ Name: "Bettype", Value: setting.MinorBettypeName }];

            var details = setting.Details;
            var hasSetting = 0;
            $.each(details, function (index, detail) {
                if (detail.Spread || detail.MaxBet) {
                    hasSetting = 1;
                }

                columns.push({
                    Name: detail.MinorSettingId + ".Spread",
                    Value: renderCell(isHost, setting.MinorBettypeId, detail.MinorSettingId, false, detail.Spread),
                    CssClass: (isHost ? 'input_value' : '')
                });
                columns.push({
                    Name: detail.MinorSettingId + ".Maxbet",
                    Value: renderCell(isHost, setting.MinorBettypeId, detail.MinorSettingId, true, detail.MaxBet),
                    CssClass: (isHost ? 'input_value' : '')
                });
            });
            if (isHost) {
                if (hasSetting === 1) {
                    columns.push({
                        Name: "Reset",
                        Value: '<i original-title="Clear" class="icon icon-clear mdi mdi-broom tooltip-tipsy js-resetSetting" data-minorbettypeid="' + setting.MinorBettypeId + '"></i>'
                    });
                }
                else {
                    columns.push({
                        Name: "Reset",
                        Value: '<i original-title="Clear" style="display: none" class="icon icon-clear mdi mdi-broom tooltip-tipsy js-resetSetting" data-minorbettypeid="' + setting.MinorBettypeId + '"></i>'
                    });
                }
            }

            var row = {
                Columns: columns
            };

            bodyRows.push(row);
        });
        return bodyRows;
    }

    function buildGridOptions(data) {
        var gridOptions = {
            customizeHeader: true,
            groupHeaderRows: buildGroupHeaderRows(data.IsHost, data.Ranges),
            columns: buildColumns(data.IsHost, data.Ranges),
            bodyRows: buildBodyRows(data),
            gridExpandHeight: 200
        };

        return gridOptions;
    }

    function renderCell(isHost, minorBettypeId, minorSettingId, isMaxBet, value) {
        var unit = isMaxBet ? "K" : "%";

        if (!isHost) {
            return value !== null ? (value + unit) : '';
        }

        var setting = {
            value: value !== null ? value : '',
            minorBettypeId: minorBettypeId,
            minorSettingId: minorSettingId,
            isMaxBet: isMaxBet,
            spread: isMaxBet ? null : value,
            maxBet: isMaxBet ? value : null,
        };

        var result = String.format("<input class='input_field {0}' type='text' value='{1}' data-setting-json='{2}' maxlength='{3}' /> <label class='label active'> {4} </label>",
            isMaxBet ? 'maxbet' : 'spread',
            setting.value,
            JSON.stringify(setting),
            isMaxBet ? 8 : 3,
            unit);

        return result;
    }

    function bindingEventKeyupForSpread(e) {
        var val = e.target.value,
            regex = /^([1-9]?[0-9])$/g,
            regex1 = /^([1-9]?[0-9])/g;

        var code = e.which;
        // Enter, Escape
        if (code === 13 || code === 27) {
            $(e.target).blur();
            return;
        }

        if (!regex.test(val)) {
            val = regex1.exec(val);

            if (val) {
                e.target.value = val[0];
            } else {
                e.target.value = '';
            }

            return;
        }
    }

    function bindingEventKeyupForMaxBet(e) {
        var val = e.target.value,
            regex = /^(\+)?\d*\.?([0-9])?$/g,
            regex1 = /^(\+)?\d*\.?([0-9])?/g;

        var code = e.which;
        // Enter, Escape
        if (code === 13 || code === 27) {
            $(e.target).blur();
            return;
        }

        if (!regex.test(val)) {
            val = regex1.exec(val);

            if (val) {
                e.target.value = val[0];
            } else {
                e.target.value = '';
            }

            return;
        }
    }

    function renderSpecialLiveProductGrid(data) {
        var gridOptions = buildGridOptions(data);
        $specialLiveProductGrid.fanexGrid(gridOptions);

        $('.tooltip-tipsy').tipsy({ html: true, gravity: 's' });

        $specialLiveProductGrid.find('.grid-container').on('scroll', function () {
            var left = $(this).scrollLeft();

            $specialLiveProductGrid.find('.col_header').prop('scrollLeft', left);
            $specialLiveProductGrid.find('.x-scrollbar').prop('scrollLeft', left);
        });
    }

    function getSpecialLiveProductData() {
        $('.loading-process').css('display', 'inline');
        specialLiveProductFactory.GetMinorSettings().success(function (data) {
            model = data;
            renderSpecialLiveProductGrid(data);

            $('.loading-process').css('display', 'none');
        });
    }

    function updateMinorSetting(e) {
        var setting = JSON.parse($(e.target).attr("data-setting-json"));
        var oldValue = setting.value;
        var newValue = e.target.value === "" ? null : parseFloat(e.target.value);

        if (oldValue === newValue) {
            return;
        }

        setting.isMaxBet === true ? setting.maxBet = newValue : setting.spread = newValue;
        specialLiveProductFactory.UpdateMinorSetting(setting).success(function () {
            updateCell($(e.target), newValue);
        });
    }

    function updateCell($cell, newValue) {
        var setting = JSON.parse($cell.attr("data-setting-json"));
        setting.value = newValue;
        setting.isMaxBet === true ? setting.maxBet = newValue : setting.spread = newValue;

        $cell.attr("data-setting-json", JSON.stringify(setting));

        var inputs = $cell.closest('tr').find('input.input_field');
        var enable = 0;
        $.each(inputs, function (index, input) {
            if (input.value) {
                enable = 1;
            }
        });

        var $reset = $cell.closest('tr').find('.js-resetSetting');
        if (enable === 1) {
            $reset.show();
        }
        else {
            $reset.hide();
        }
    }

    function resetMinorSettingHandler() {
        var el = $(this);
        Swal.fire({
            html: 'Are you sure you want to reset this setting?',
            type: 'question',
            showCancelButton: true,
            reverseButtons: true,
            animation: false,
            heightAuto: false,
            scrollbarPadding: false,
            confirmButtonText: 'OK'
        }).then((result) => {
            if (result.value) {
                var criteria = {
                    MinorBettypeId: parseInt(el.data('minorbettypeid'), 10)
                };

                specialLiveProductFactory.ResetMinorSetting(criteria).success(function (response) {
                    if (!response.IsSuccessful) {
                        common.modalErrorMessage(response.Message);
                    } else {
                        getSpecialLiveProductData();
                    }
                });
            }
        });
    }

    function initModule() {
        $('#tab-special-live-product').on('click', getSpecialLiveProductData);

        // max bet, spread
        $specialLiveProductGrid.on('keyup', '.input_field.spread', function (e) {
            bindingEventKeyupForSpread(e);
        });
        $specialLiveProductGrid.on('keyup', '.input_field.maxbet', function (e) {
            bindingEventKeyupForMaxBet(e);
        });
        $specialLiveProductGrid.on('focusout', '.input_field.spread, .input_field.maxbet', function (e) {
            updateMinorSetting(e);
        });

        $specialLiveProductGrid.on('click', '.js-resetSetting', resetMinorSettingHandler);
    }

    return {
        initModule: initModule
    };
}(jQuery));

$(function () {
    specialLiveProduct.initModule();
});