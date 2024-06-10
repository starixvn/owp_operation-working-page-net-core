/* global common, maxBetFactory */
"use strict";

var maxBetManagement = (function ($) {
    var $formFilter,
        $unknownCategory,
        $categoryFilter,
        $optionFilter,
        $optionIds,
        $search,
        $maxBetGrid,
        $optionModal,
        $itemperpageOption,
        $league;

    var model,
        bettypes,
        gridOptions;

    var rowNeedRenderStartIndex;

    function searchHanlder() {
        $('.loading-process').css('display', 'inline');

        setOptionIds();
        var param = $formFilter.serialize();
        maxBetFactory.GetMaxBets(param).success(function (data) {
            model = data;
            bettypes = data.Bettypes;
            renderMaxBetGrid(data);

            $('.loading-process').css('display', 'none');
        });
    }

    // Category Filter
    function setOptionIds() {
        var ids = "";
        if ($unknownCategory.prop("checked")) {
            ids += "0,";
        }

        $optionFilter.each(function (index, option) {
            if ($(option).prop("checked")) {
                ids += $(option).attr("data-id") + ",";
            }
        });
        $optionIds.val(ids.slice(0, -1));
    }

    function regiterCategoryFilterHandler() {
        $categoryFilter.change(function (e) {
            var $options = $(e.target).parent().next().find("input");
            if ($(e.target).prop("checked")) {
                $options.each(function (index, option) {
                    $(option).prop("checked", true);
                });
            }
            else {
                $options.each(function (index, option) {
                    $(option).prop("checked", false);
                });
            }
            updateTextStatus();
        });

        $optionFilter.change(function (e) {
            var $category = $(e.target).closest("ul").prev().find("input");
            if ($(e.target).prop("checked")) {
                $category.prop("checked", true);
            }
            else {
                var checkedOption = $(e.target).closest("li").siblings().find("input").filter(":checked");
                if (checkedOption.length === 0) {
                    $category.prop("checked", false);
                }
            }
            updateTextStatus();
        });

        function updateTextStatus() {
            var numSelected = 0;
            $optionFilter.each(function (index, element) {
                if ($(element).prop("checked")) {
                    numSelected++;
                }
            });

            if ($unknownCategory.prop("checked")) {
                numSelected++;
            }

            var $categoryStatus = $("#category-status");
            if (numSelected === 0) {
                $categoryStatus.val("None selected");
            }
            else if (numSelected === $optionFilter.length + 1) {
                $categoryStatus.val("All selected");
            }
            else {
                $categoryStatus.val(numSelected + " selected");
            }
        }
    }

    function addOptionHandler(e) {
        var leagueId = parseInt($(e.target).attr("data-league-id")),
            tr = $(e.target).closest('tr');

        maxBetFactory.GetLeagueOptions().success(function (categories) {
            buildLeagueOptions(leagueId, categories);

            $optionModal.openModal();

            $("#submit-option").off('click').on('click', function () {
                optionSubmitHandler(tr);
            });
        });
    }

    function optionSubmitHandler($row) {
        var $option = $("input[name=optionGroup]:checked", ".option-block");
        if ($option.length === 0) {
            common.modalErrorMessage(messageResources.SelectOption);
            return;
        }

        var leagueId = parseInt($option.attr("data-league-id"));
        var categoryId = parseInt($option.attr("data-category-id"));
        var categoryName = $option.attr("data-category-name");
        var optionId = parseInt($option.val());
        var optionName = $option.next().text();

        if (optionId === -1) {
            // Insert Option
            var selectedCategoryId = $("#add-option-select").val();
            optionName = $option.next().next().val().trim();

            if (optionName === "") {
                common.modalErrorMessage(messageResources.InputOptionName);
                return;
            }

            if (selectedCategoryId === "-1") {
                common.modalErrorMessage(messageResources.SelectCategory);
                return;
            }

            $('.loading-process').css('display', 'inline');
            maxBetFactory.InsertLeagueOption({
                Name: optionName,
                CategoryId: selectedCategoryId
            }).success(function (response) {
                if (response.IsSuccessful === true) {
                    optionId = response.OptionId;

                    addCategoryOption(selectedCategoryId, optionId, optionName);
                    addOptionToGrid($row, leagueId, categoryId, categoryName, optionId, optionName);
                }
                else {
                    common.modalErrorMessage(response.Message);
                }

                $('.loading-process').css('display', 'none');
            });
        }
        else {
            // Select Option
            addOptionToGrid($row, leagueId, categoryId, categoryName, optionId, optionName);
        }

        function addCategoryOption(selectedCategoryId, optionId, optionName) {
            var $selCategory = $("#ddl-category").find("input[id^='mb-category-']").filter("#mb-category-" + selectedCategoryId);

            var $newOption = $(String.format(
                '<li><a href="javascript:;">'
              + '<input type="checkbox" class="filled-in js-checkboxChild" id="mb-option-{0}" data-id="{1}" autocomplete="off" checked>'
              + '<label for="mb-option-{2}">{3}</label>'
              + '</a></li>', optionId, optionId, optionId, optionName));

            $selCategory.parent().next().append($newOption);
            $optionFilter = $("input[id^='mb-option-']"); // Reload
        }
    }

    function addOptionToGrid($row, leagueId, categoryId, categoryName, optionId, optionName) {
        // Add fake league setting for option
        var option = {
            leagueId: leagueId,
            optionIds: optionId,
            bettypeIds: model.Bettypes.Handicap,
            isMaxBet: false,
            spread: null,
            maxBet: null
        };

        maxBetFactory.InsertOrUpdateLeagueSetting(option, function () {
            var leagueIndex = model.Leagues.map(function (x) { return x.LeagueId; }).indexOf(leagueId),
                rowspan = 0,
                oldModel = $.extend(true, {}, model),
                oldRowNeedRenderStartIndex = rowNeedRenderStartIndex;
            var numberOfOptions = model.Leagues[leagueIndex].Options ? model.Leagues[leagueIndex].Options.length : 0;
            var optionIndex = numberOfOptions > 0 ? model.Leagues[leagueIndex].Options.map(function (x) { return x.Id; }).indexOf(optionId) : -1;

            if (optionIndex < 0) {
                model.Leagues[leagueIndex].Options.push(maxBetFactory.CreateEmptyOption(categoryId, categoryName, optionId, optionName));
                rowspan = ++model.Leagues[leagueIndex].RowSpan;

                var leagueNeedCopy = $.extend(true, {}, model.Leagues[leagueIndex]);
                leagueNeedCopy.IsTemplate = true;
                oldModel.Leagues = [leagueNeedCopy];
            }

            $maxBetGrid.fanexGrid('appendBody', gridOptions, buildBodyRows(oldModel, 0, common.Constants.VirtualThresholdNumber));
            rowNeedRenderStartIndex = oldRowNeedRenderStartIndex;

            var parentRow = $row.hasClass('js-parentRow') ? $row : $($row.prevAll('.js-parentRow:first')[0]);
            var templates = $maxBetGrid.find('.js-template').clone();
            $maxBetGrid.find('.js-template').remove();

            var rowHeight = 0,
                scrollTop = $maxBetGrid.find('.y-scrollbar').scrollTop();
            if (templates.length > 1) {
                parentRow.find('td.js-rowspan').attr('rowspan', rowspan);

                var $clone = $(templates[templates.length - 1]);
                rowHeight = $clone.height();
                $clone.wrap('<div>');
                $row.after($clone.parent().html());
                $maxBetGrid.find('.js-template').removeClass('js-template');
                $row.find('td a.js-addOption').remove();

                autoresizeRemarks(model.IsHost);
                rowHeight = $row.next().height();
            } else if (templates.length === 1) {
                $row.html($(templates[0]).html());

                autoresizeRemarks(model.IsHost);
                rowHeight = $row.height();
            }

            setTimeout(function () {
                bindingTooltip();
                $maxBetGrid.find('.y-scrollbar').scrollTop(scrollTop + rowHeight);
            }, 200);
            $optionModal.closeModal();
        });
    }

    function updateIsActiveHandler() {
        var el = $(this);

        var criteria = {
            OWPLeagueId: parseInt(el.data('leagueid'), 10),
            IsActive: el.prop('checked')
        };

        maxBetFactory.UpdateIsActiveLeague(criteria).success(function (response) {
            if (!response.IsSuccessful) {
                common.modalErrorMessage(response.Message);
            } else {
                var index = model.Leagues.map(function (x) { return x.LeagueId; }).indexOf(criteria.OWPLeagueId);
                if (index !== -1) {
                    model.Leagues[index].IsActive = criteria.IsActive;
                }
            }
        });
    }

    function updateIsBlackListHandler() {
        var el = $(this),
            column = el.closest('td'),
            rowIndex = el.closest('tr').index();

        var criteria = {
            OWPLeagueId: parseInt(el.data('leagueid'), 10),
            IsBlackList: el.prop('checked')
        };

        maxBetFactory.UpdateIsBlackListLeague(criteria).success(function (response) {
            if (!response.IsSuccessful) {
                common.modalErrorMessage(response.Message);
            } else {
                var rowspan = column.attr('rowspan') ? column.attr('rowspan') : 1,
                    rows = el.closest('tbody').find('tr');

                for (var i = 0; i < rowspan; i++) {
                    if ($(rows[rowIndex + i]).hasClass('blacklist')) {
                        $(rows[rowIndex + i]).removeClass('blacklist');
                    } else {
                        $(rows[rowIndex + i]).addClass('blacklist');
                    }
                }

                var index = model.Leagues.map(function (x) { return x.LeagueId; }).indexOf(criteria.OWPLeagueId);
                if (index !== -1) {
                    model.Leagues[index].IsBlackList = criteria.IsBlackList;
                }
            }
        });
    }

    function deleteOptionHandler() {
        var el = $(this),
            tr = el.closest('tr');
        Swal.fire({
            html: 'Are you sure you want to permanently delete this option?',
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
                    OWPLeagueId: parseInt(el.data('leagueid'), 10),
                    OptionId: parseInt(el.data('optionid'), 10)
                };

                maxBetFactory.DeleteLeagueSetting(criteria).success(function (response) {
                    if (!response.IsSuccessful) {
                        common.modalErrorMessage(response.Message);
                    } else {
                        var totalItems = model.Leagues.length,
                            rowspan = 0,
                            isLastOption = false;
                        for (var i = 0; i < totalItems; i++) {
                            if (model.Leagues[i].LeagueId === criteria.OWPLeagueId) {
                                var index = model.Leagues[i].Options.map(function (x) { return x.Id; }).indexOf(criteria.OptionId);

                                if (index > -1) {
                                    if (index === model.Leagues[i].Options.length - 1) {
                                        isLastOption = true;
                                    }

                                    model.Leagues[i].Options.splice(index, 1);
                                    rowspan = --model.Leagues[i].RowSpan;
                                    break;
                                }
                            }
                        }

                        if (isLastOption && rowspan > 1) {
                            tr.prev().find('td.js-colOption div').first().append(tr.find('td a.js-addOption').clone());
                        }

                        if (rowspan === 0) {
                            var iconAddOption = tr.find('td a.js-addOption').clone();
                            tr.find('.js-clearContent').empty();
                            tr.find('td.js-colOption div.js-clearContent').append(iconAddOption);
                        } else {
                            if (tr.hasClass('js-parentRow')) {
                                tr.find('td.js-rowspan').attr('rowspan', rowspan);
                                var nextRow = tr.next('tr'),
                                    nextRowCells = nextRow.find('td div.js-clearContent');

                                tr.find('td div.js-clearContent').each(function (index) {
                                    $(this).empty().append($(nextRowCells[index]).html());
                                });

                                nextRow.remove();
                            } else {
                                var parentRow = $(tr.prevAll('.js-parentRow:first')[0]);
                                parentRow.find('td.js-rowspan').attr('rowspan', rowspan);
                                tr.remove();
                            }
                        }

                        setTimeout(function () { $maxBetGrid.fanexGrid('resizeGrid'); }, 100);
                    }
                });
            }
        });
    }

    function insertOrUpdateRemarks(e) {
        var leagueRemarkSetting = JSON.parse($(e.target).attr("data-remarks-json"));
        var oldValue = leagueRemarkSetting.remarks ? leagueRemarkSetting.remarks : '';
        var newValue = e.target.value.trim();

        if (oldValue === newValue) {
            return;
        }

        leagueRemarkSetting.remarks = common.HtmlEncode(newValue);

        maxBetFactory.InsertOrUpdateLeagueRemarkSetting(leagueRemarkSetting, function () {
            updateRemarkCell($(e.target), newValue);
        });
    }

    function updateRemarkCell($cell, newValue) {
        var leagueRemarkSetting = JSON.parse($cell.attr("data-remarks-json"));
        leagueRemarkSetting.remarks = newValue;
        $cell.attr("data-remarks-json", JSON.stringify(leagueRemarkSetting));

        // Update Model
        $.each(model.Leagues, function (index, league) {
            if (league.LeagueId === leagueRemarkSetting.leagueId) {
                $.each(league.Options, function (index, option) {
                    if (option.Id === leagueRemarkSetting.optionId) {
                        option.Remarks = leagueRemarkSetting.remarks;
                    }
                });
            }
        });
    }

    function insertOrUpdateLeagueSetting(e) {
        var option = JSON.parse($(e.target).attr("data-option-json"));
        var oldValue = option.value;
        var newValue = e.target.value === "" ? null : parseFloat(e.target.value);

        if (oldValue === newValue) {
            return;
        }

        option.isMaxBet === true ? option.maxBet = newValue : option.spread = newValue;
        maxBetFactory.InsertOrUpdateLeagueSetting(option, function () {
            updateCell($(e.target), newValue);
        });
    }

    function updateCell($cell, newValue) {
        var option = JSON.parse($cell.attr("data-option-json"));
        option.value = newValue;
        option.isMaxBet === true ? option.maxBet = newValue : option.spread = newValue;

        $cell.val(newValue);
        $cell.attr('value', newValue);
        $cell.attr("data-option-json", JSON.stringify(option));
        // Update Model
        var groupColName = option.bindColName.split(".")[0];
        var colName = option.bindColName.split(".")[1];
        $.each(model.Leagues, function (index, league) {
            if (league.LeagueId === option.leagueId) {
                $.each(league.Options, function (index, oldOption) {
                    if (oldOption.Id === option.optionIds
                        || (oldOption.Id + oldOption.MaxBetWeekday.OptionId) === option.optionIds
                        || (oldOption.Id + oldOption.MaxBetWeekend.OptionId) === option.optionIds) {
                        oldOption[groupColName][colName] = option.value;
                    }
                });
            }
        });
    }

    function copySettingHandler() {
        var el = $(this),
            optionJson = JSON.parse(el.attr('data-option-json')),
            totalItems = model.Leagues.length,
            value = null,
            reRenderGrid = false,
            leagueSettings = [],
            i, j;
        var copyIndex = el.data('copy-index');

        var oldData = $.extend(true, {}, model);

        for (i = 0; i < totalItems; i++) {
            var league = model.Leagues[i];

            if (league.Options.length > 0) {
                value = league.Options[0][optionJson.parentField][optionJson.childField];

                break;
            }
        }

        for (i = 0; i < totalItems; i++) {
            var leagueId = model.Leagues[i].LeagueId;

            if (model.Leagues[i].Options.length > 0) {
                var totalOptions = model.Leagues[i].Options.length;

                for (j = 0; j < totalOptions; j++) {
                    var option = model.Leagues[i].Options[j];

                    if (option[optionJson.parentField][optionJson.childField] !== value) {
                        var leagueSetting = {
                            LeagueId: leagueId,
                            OptionIds: option.Id + (option[optionJson.parentField].OptionId ? option[optionJson.parentField].OptionId : 0),
                            BettypeIds: optionJson.bettypeIds,
                            Value: value,
                            IsMaxBet: optionJson.isMaxBet
                        };

                        model.Leagues[i].Options[j][optionJson.parentField][optionJson.childField] = value;

                        leagueSettings.push(leagueSetting);
                    }
                }
            }
        }

        if (leagueSettings.length > 0) {
            Swal.fire({
                html: 'Are you sure you want to apply setting another row?',
                type: 'question',
                showCancelButton: true,
                reverseButtons: true,
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            }).then((result) => {
                if (result.value) {
                    $('.loading-process').css('display', 'inline');
                    maxBetFactory.CopyLeagueSetting(JSON.stringify(leagueSettings)).success(function (response) {
                        if (!response.IsSuccessful) {
                            common.modalErrorMessage(response.Message);
                            model = oldData;
                        } else {
                            $maxBetGrid.find('.content-container tbody tr').each(function () {
                                var cell = $(this).find('td .input_field[data-copy-index="' + copyIndex + '"]');
                                cell.val(value);
                                cell.attr('value', value);

                                if (cell.attr("data-option-json")) {
                                    var option = JSON.parse(cell.attr("data-option-json"));
                                    option.value = value;
                                    option.isMaxBet === true ? (option.maxBet = value) : (option.spread = value);

                                    cell.attr("data-option-json", JSON.stringify(option));
                                }
                            });
                            $('.loading-process').css('display', 'none');
                        }
                    });
                } else {
                    model = oldData;
                }
            });
        } else {
            model = oldData;
        }
    }

    function exportExcelHandler() {
        var league = $formFilter.find('#League').val(),
            leagueGroupId = $formFilter.find('#LeagueGroupId').val(),
            isActive = $formFilter.find('#IsActive').prop('checked'),
            leagueGroupName = $formFilter.find('#LeagueGroupId option:selected').text(),
            optionIds = $formFilter.find('#optionIds').val();

        window.location.href = common.ResolveRootUrl('EventManagement/MaxBetManagement/ExportExcel')
            + '?League=' + league
            + '&LeagueGroupId=' + leagueGroupId
            + '&OptionIds=' + optionIds
            + '&LeagueGroupName=' + leagueGroupName
            + '&IsActive=' + isActive;
    }

    function buildLeagueOptions(leagueId, categories) {
        // Remove Existing Options
        var curOptions = $.grep(model.Leagues, function (league) {
            return leagueId === league.LeagueId;
        })[0].Options;

        $(categories).each(function (index, options) {
            $(curOptions).each(function (optIndex, curOption) {
                for (var i = options.length - 1; i >= 0; i--) {
                    if (curOption.CategoryId === options[i].CategoryId
                        && curOption.Id === options[i].OptionId) {
                        options.splice(i, 1);
                    }
                }
            });
        });

        var $categoryTemplate = $("#category-option").children(0);
        var $optionTemplate = $("#select-option").children(0);

        var htmlCategories = "";
        for (var i = 0; i < categories.length; i++) {
            htmlCategories += getHtmlCategory($categoryTemplate.clone(), $optionTemplate.clone(), categories[i], i);
        }

        // Last row
        var $addOption = getJAddOption();
        if ($(htmlCategories).length >= 7) {
            $addOption.addClass("dropup");
        }
        htmlCategories += $addOption[0].outerHTML;
        $(".option-block").html(htmlCategories);

        // Bind Events
        $("#add-option-radio").click(function () {
            $("#add-option-input").focus();
        });
        $("#add-option-input").click(function () {
            $("#add-option-radio").prop('checked', true);
        });

        $("#add-option-select").change(function (e) {
            $("#add-option-radio").attr("data-category-id", e.target.value);
            $("#add-option-radio").attr("data-category-name", e.target.options[e.target.selectedIndex].text);
        });

        $("#add-option-select").material_select();

        function getHtmlCategory($categoryTmpl, $optionTmpl, category, categoryIndex) {
            var categoryHtml = "", optionsHtml = "";
            for (var i = 0; i < category.length; i++) {
                if (i === 0) {
                    categoryHtml = $categoryTmpl.text(category[0].CategoryName)[0].outerHTML;
                }

                var id = "category-" + categoryIndex + "-option-" + i;
                $optionTmpl.find("input").attr("id", id);
                $optionTmpl.find("input").attr("value", category[i].OptionId);
                $optionTmpl.find("input").attr("data-league-id", leagueId);
                $optionTmpl.find("input").attr("data-category-id", category[0].CategoryId);
                $optionTmpl.find("input").attr("data-category-name", category[0].CategoryName);

                $optionTmpl.find("label").attr("for", id);
                $optionTmpl.find("label").text(category[i].OptionName);

                optionsHtml += $optionTmpl[0].outerHTML;
            }

            return categoryHtml + optionsHtml;
        }

        function getJAddOption() {
            var $addOptionTmpl = $("#add-option").children(0).clone();
            $addOptionTmpl.children().eq(0).attr("id", "add-option-radio");
            $addOptionTmpl.children().eq(0).attr("data-league-id", leagueId);
            $addOptionTmpl.children().eq(0).attr("id", "add-option-radio");
            $addOptionTmpl.children().eq(1).attr("for", "add-option-radio");
            $addOptionTmpl.children().eq(2).attr("id", "add-option-input");
            $addOptionTmpl.children().eq(3).attr("id", "add-option-select");

            return $addOptionTmpl;
        }
    }

    function autoresizeRemarks(isHost) {
        if (isHost) {
            $maxBetGrid.find('.materialize-textarea.remarks:not(.js-resized)').each(function () {
                $(this).removeClass('js-resized').addClass('js-resized');
                $(this).trigger('autoresize');
            });

            setTimeout(function () { $maxBetGrid.fanexGrid('resizeGrid'); }, 200);
        }
    }

    function bindingTooltip() {
        $('.tooltip-tipsy:not(.js-bindedTooltip)').each(function () {
            $(this).removeClass('js-bindedTooltip').addClass('js-bindedTooltip');
            $(this).tipsy({ html: true, gravity: 's' });
        });
    }

    function renderMaxBetGrid(data) {
        rowNeedRenderStartIndex = 0;
        buildGridOptions(data);

        if (!$maxBetGrid.hasClass('freegrid')) {
            $maxBetGrid.addClass('freegrid');
        }

        $maxBetGrid.fanexGrid(gridOptions);
        initPagination(data);
        $('#btn-export-excel').off('click').on('click', exportExcelHandler);
        bindingTooltip();
        autoresizeRemarks(data.IsHost);

        $maxBetGrid.find('.grid-container').on('scroll', function () {
            var yscrollbar = $maxBetGrid.find('.y-scrollbar'),
                container = $maxBetGrid.find('.grid-container'),
                content = container.find('.content-container');

            var left = $(this).scrollLeft();
            $maxBetGrid.find('.col_header').prop('scrollLeft', left);
            //$maxBetGrid.find('.x-scrollbar').prop('scrollLeft', left);

            var contentTwoThirdsHeight = parseInt(content.height() * 2 / 3),
                yScrollTop = yscrollbar.scrollTop(),
                containerScrollTop = container.scrollTop();

            if (rowNeedRenderStartIndex > 0
                && rowNeedRenderStartIndex < model.Leagues.length
                && (yScrollTop > contentTwoThirdsHeight || containerScrollTop > contentTwoThirdsHeight)) {
                $maxBetGrid.fanexGrid('appendBody', gridOptions, buildBodyRows(model, rowNeedRenderStartIndex, common.Constants.VirtualThresholdNumber));
                bindingTooltip();
                autoresizeRemarks(data.IsHost);
            }
        });
    }

    function renderRemarksCell(isHost, leagueId, optionId, remarks) {
        if (!isHost) {
            return remarks;
        }

        return String.format("<textarea class='materialize-textarea remarks' data-remarks-json='{0}' maxlength='100' spellcheck='false'>{1}</textarea>",
            JSON.stringify({
                leagueId: leagueId,
                optionId: optionId,
                remarks: remarks
            }),
            remarks ? remarks : '');
    }

    function renderCell(isHost, copyIndex, leagueId, optionIds, bettypeIds, value, isMaxBet, bindColName) {
        var unit = isMaxBet ? "K" : "%";

        if (!isHost) {
            return value != null ? (value + unit) : '';
        }

        var option = {
            value: value !== null ? value : "",
            leagueId: leagueId,
            optionIds: optionIds,
            bettypeIds: bettypeIds,
            isMaxBet: isMaxBet,
            spread: isMaxBet ? null : value,
            maxBet: isMaxBet ? value : null,
            bindColName: bindColName
        };

        var result = String.format("<input class='input_field {0}' type='text' value='{1}' data-option-json='{2}' maxlength='{3}' data-copy-index='{4}' />"
                + "<label class='label active'> {5} </label>",
            isMaxBet ? 'maxbet' : 'spread',
            option.value,
            JSON.stringify(option),
            isMaxBet ? 8 : 3,
            copyIndex,
            unit);

        return result;
    }

    function renderHeaderCell(isHost, copyIndex, bettypeIds, parentField, childField, isMaxBet) {
        if (!isHost) {
            return '';
        }

        var option = {
            bettypeIds: bettypeIds,
            isMaxBet: isMaxBet,
            parentField: parentField,
            childField: childField
        };

        var result = String.format("<a title='Copy all' href='javascript:;'"
                + " class='icon icon-copyAll mdi mdi-content-copy right tooltip-tipsy js-copySetting'"
                + " data-option-json={0} data-copy-index='{1}'></a>",
            JSON.stringify(option), copyIndex);

        return result;
    }

    function renderActiveCell(isHost, leagueId, isActive) {
        return '<div class="checkbox">' +
                '<input type="checkbox" class="filled-in ' + (isHost ? 'js-active' : '') + '" id="active-' + leagueId
                    + '" data-leagueid="' + leagueId + '" ' + (isActive ? 'checked ' : '') + (isHost ? '' : ' disabled') + '>' +
                '<label for="active-' + leagueId + '"></label>' +
            '</div>';
    }

    function renderBlackListCell(isHost, leagueId, isBlackList) {
        return '<div class="checkbox">' +
                '<input type="checkbox" class="filled-in ' + (isHost ? 'js-blackList' : '') + '" id="black-list-' + leagueId
                    + '" data-leagueid="' + leagueId + '" ' + (isBlackList ? 'checked ' : '') + (isHost ? '' : ' disabled') + '>' +
                '<label for="black-list-' + leagueId + '"></label>' +
            '</div>';
    }

    function onPageChanging(pageNumber) {
        $formFilter.find('#PageIndex').val(pageNumber);
        searchHanlder();
    }

    function initPagination(result) {
        var pagination = $('.js-pagination');
        if (result.TotalItem > 1) {
            pagination.show();
            var from = (result.CurrentPage - 1) * result.ItemsPerPage + 1,
                to = from + (result.Leagues.length - 1);

            pagination.find('.detailtext').text('Showing ' + from + ' to ' + to + ' of ' + result.TotalItem + ' entries');

            common.InitPagination(
                pagination.find('#numberlist'),
                result.TotalItem,
                result.ItemsPerPage,
                result.CurrentPage,
                onPageChanging);
        } else {
            pagination.hide();
        }
    }

    function buildGroupHeaderRows() {
        return [
             {
                 Columns: [
                    { HeaderText: "#", Name: "No", MergeRowNumber: 2 },
                    { HeaderText: "League Group", Name: "LeagueGroup", MergeRowNumber: 2 },
                    { HeaderText: "League", Name: "League", MergeRowNumber: 2 },
                    { HeaderText: "Category", Name: "Category", MergeRowNumber: 2 },
                    { HeaderText: "Option", Name: "Option", MergeRowNumber: 2 },
                    { HeaderText: "Remarks", Name: "Remarks", MergeRowNumber: 2 },

                    { HeaderText: "Spread", Name: "Spread.Handicap", MergeColNumber: 4, ThClass: 'th-col-even' },
                    { HeaderText: "Max Bet - Weekday", Name: "MaxBetWeekday.Handicap", MergeColNumber: 3, ThClass: 'th-col-odd' },
                    { HeaderText: "Max Bet - Weekend", Name: "MaxBetWeekend.Handicap", MergeColNumber: 3, ThClass: 'th-col-even' },
                    { HeaderText: "1X2", Name: "OneXTwo.Spread", MergeColNumber: 3, ThClass: 'th-col-odd' },

                    { HeaderText: "Last Updated Time", Name: "LastUpdatedTime", MergeRowNumber: 2 },
                    { HeaderText: "Last Updated By", Name: "LastUpdatedBy", MergeRowNumber: 2 },
                    { HeaderText: "Action", Name: "Action", MergeRowNumber: 2 },

                    { HeaderText: "Last Using", Name: "LastUsing", MergeRowNumber: 2 },
                    { HeaderText: "Active", Name: "Active", MergeRowNumber: 2 },
                    { HeaderText: "Black List", Name: "BlackList", MergeRowNumber: 2 }
                 ]
             }
        ];
    }

    function buildColumns(isHost) {
        var copyIndex = 6;
        return [
            {
                HeaderText: "#", Width: 25, Name: "No", HeaderAlign: "center",
                CellAlign: "center", TdClass: "align-middle js-rowspan"
            },
            {
                HeaderText: "League Group", Width: 85, Name: "LeagueGroup",
                HeaderAlign: "left", CellAlign: "left", TdClass: "align-middle js-rowspan"
            },
            {
                HeaderText: "League", Width: 250, Name: "League",
                HeaderAlign: "left", CellAlign: "left", TdClass: "align-middle js-rowspan"
            },
            {
                HeaderText: "Category", Width: 85, Name: "Category",
                HeaderAlign: "left", CellAlign: "left", TdClass: "align-middle"
            },
            {
                HeaderText: "Option", Width: 250, Name: "Option",
                HeaderAlign: "left", CellAlign: "left", TdClass: "align-middle js-colOption"
            },
            {
                HeaderText: "Remarks", Width: 300, Name: "Remarks",
                HeaderAlign: "left", CellAlign: "left", AllowTruncate: isHost ? false : true
            },
            // Spread
            {
                HeaderText: "HDP" + renderHeaderCell(isHost, copyIndex++, bettypes.Handicap, 'Spread', 'Handicap', false),
                Width: 75, Name: "Spread.Handicap", HeaderAlign: "center", CellAlign: "right"
            },
            {
                HeaderText: "O/U" + renderHeaderCell(isHost, copyIndex++, bettypes.OverUnder, 'Spread', 'OverUnder', false),
                Width: 75, Name: "Spread.OverUnder", HeaderAlign: "center", CellAlign: "right"
            },
            {
                HeaderText: "1H HDP" + renderHeaderCell(isHost, copyIndex++, bettypes.FirstHalfHandicap, 'Spread', 'FirstHalfHandicap', false),
                Width: 75, Name: "Spread.FirstHalfHandicap", HeaderAlign: "center", CellAlign: "right"
            },
            {
                HeaderText: "1H O/U" + renderHeaderCell(isHost, copyIndex++, bettypes.FirstHalfOverUnder, 'Spread', 'FirstHalfOverUnder', false),
                Width: 75, Name: "Spread.FirstHalfOverUnder", HeaderAlign: "center", CellAlign: "right"
            },
            // MaxBet - Weekday
            {
                HeaderText: "HDP" + renderHeaderCell(isHost, copyIndex++, bettypes.Handicap, 'MaxBetWeekday', 'Handicap', true),
                Width: 75, Name: "MaxBetWeekday.Handicap", HeaderAlign: "center", CellAlign: "right", ThClass: 'col-odd', TdClass: 'col-odd'
            },
            {
                HeaderText: "O/U" + renderHeaderCell(isHost, copyIndex++, bettypes.OverUnder, 'MaxBetWeekday', 'OverUnder', true),
                Width: 75, Name: "MaxBetWeekday.OverUnder", HeaderAlign: "center", CellAlign: "right", ThClass: 'col-odd', TdClass: 'col-odd'
            },
            {
                HeaderText: "1H" + renderHeaderCell(isHost, copyIndex++, bettypes.FirstHalfHandicap + ',' + bettypes.FirstHalfOverUnder, 'MaxBetWeekday', 'FirstHalfHandicap', true),
                Width: 75, Name: "MaxBetWeekday.FirstHalfHandicap", HeaderAlign: "center", CellAlign: "right", ThClass: 'col-odd', TdClass: 'col-odd'
            },
            // MaxBet - Weekend
            {
                HeaderText: "HDP" + renderHeaderCell(isHost, copyIndex++, bettypes.Handicap, 'MaxBetWeekend', 'Handicap', true),
                Width: 75, Name: "MaxBetWeekend.Handicap", HeaderAlign: "center", CellAlign: "right"
            },
            {
                HeaderText: "O/U" + renderHeaderCell(isHost, copyIndex++, bettypes.OverUnder, 'MaxBetWeekend', 'OverUnder', true),
                Width: 75, Name: "MaxBetWeekend.OverUnder", HeaderAlign: "center", CellAlign: "right"
            },
            {
                HeaderText: "1H" + renderHeaderCell(isHost, copyIndex++, bettypes.FirstHalfHandicap + ',' + bettypes.FirstHalfOverUnder, 'MaxBetWeekend', 'FirstHalfHandicap', true),
                Width: 75, Name: "MaxBetWeekend.FirstHalfHandicap", HeaderAlign: "center", CellAlign: "right"
            },
            // 1X2
            {
                HeaderText: "Spread" + renderHeaderCell(isHost, copyIndex++, bettypes.FullTimeOneXTwo + ',' + bettypes.HalfTimeOneXTwo, 'OneXTwo', 'Spread', false),
                Width: 75, Name: "OneXTwo.Spread", HeaderAlign: "center", CellAlign: "right", ThClass: 'col-odd', TdClass: 'col-odd'
            },
            {
                HeaderText: "FT. 1X2" + renderHeaderCell(isHost, copyIndex++, bettypes.FullTimeOneXTwo, 'OneXTwo', 'FullTimeOneXTwo', true),
                Width: 75, Name: "OneXTwo.FullTimeOneXTwo", HeaderAlign: "center", CellAlign: "right", ThClass: 'col-odd', TdClass: 'col-odd'
            },
            {
                HeaderText: "1H. 1X2" + renderHeaderCell(isHost, copyIndex++, bettypes.HalfTimeOneXTwo, 'OneXTwo', 'HalfTimeOneXTwo', true),
                Width: 75, Name: "OneXTwo.HalfTimeOneXTwo", HeaderAlign: "center", CellAlign: "right", ThClass: 'col-odd', TdClass: 'col-odd'
            },
            //
            {
                HeaderText: "Last Updated Time", Width: 150, Name: "LastUpdatedTime",
                HeaderAlign: "left", CellAlign: "right", TdClass: 'align-middle'
            },
            {
                HeaderText: "Last Updated By", Width: 80, Name: "LastUpdatedBy",
                HeaderAlign: "left", CellAlign: "left", TdClass: 'align-middle'
            },
            //
            { HeaderText: "Action", Width: 60, Name: "Action", HeaderAlign: "left", CellAlign: "center", TdClass: "align-middle" },
            { HeaderText: "Last Using", Width: 100, Name: "LastUsing", HeaderAlign: "left", CellAlign: "left", TdClass: "align-middle js-rowspan" },
            { HeaderText: "Active", Width: 60, Name: "Active", HeaderAlign: "left", CellAlign: "center", TdClass: 'js-rowspan' },
            { HeaderText: "Black List", Width: 60, Name: "BlackList", HeaderAlign: "left", CellAlign: "center", TdClass: 'js-rowspan' }
        ];
    }

    function buildBodyRows(data, currentIndex, numberOfVirtualRowsRender) {
        var bodyRows = [],
            isHost = data.IsHost,
            numberOfVirtualRows = 0,
            numberOfRows = data.Leagues.length;

        for (var i = currentIndex; i < numberOfRows; i++) {
            var league = data.Leagues[i],
                numberOfOptions = league.Options ? league.Options.length : 0;

            if (numberOfOptions === 0) {
                numberOfVirtualRows++;
                var row = {
                    Columns: [
                        { Name: "No", Value: league.RowIndex },
                        { Name: "LeagueGroup", Value: common.HtmlEncode(league.LeagueGroupName) },
                        { Name: "League", Value: common.HtmlEncode(league.LeagueName) },
                        { Name: "Category", Value: "" },
                        {
                            Name: "Option",
                            Value: isHost ? String.format("<a class='modal-trigger js-addOption' href='#optionModal'>"
                                + "<i original-title='Add option' class='icon icon-plus mdi mdi-plus right tooltip-tipsy' data-league-id={0}></i>"
                                + "</a>", league.LeagueId) : ''
                        },
                        { Name: "Remarks", Value: "" },
                        // Spread
                        { Name: "Spread.Handicap", Value: '' },
                        { Name: "Spread.OverUnder", Value: '' },
                        { Name: "Spread.FirstHalfHandicap", Value: '' },
                        { Name: "Spread.FirstHalfOverUnder", Value: '' },
                        // MaxBet - Weekday
                        { Name: "MaxBetWeekday.Handicap", Value: '' },
                        { Name: "MaxBetWeekday.OverUnder", Value: '' },
                        { Name: "MaxBetWeekday.FirstHalfHandicap", Value: '' },
                        // MaxBet - Weekend
                        { Name: "MaxBetWeekend.Handicap", Value: '' },
                        { Name: "MaxBetWeekend.OverUnder", Value: '' },
                        { Name: "MaxBetWeekend.FirstHalfHandicap", Value: '' },
                        // 1X2
                        { Name: "OneXTwo.Spread", Value: '' },
                        { Name: "OneXTwo.FullTimeOneXTwo", Value: '' },
                        { Name: "OneXTwo.HalfTimeOneXTwo", Value: '' },
                        //
                        { Name: "LastUpdatedTime", Value: '' },
                        { Name: "LastUpdatedBy", Value: '' },
                        //
                        { Name: "Action", Value: '' },
                        { Name: "LastUsing", Value: league.LastUsingToDisplay },
                        { Name: "Active", Value: renderActiveCell(isHost, league.LeagueId, league.IsActive) },
                        { Name: "BlackList", Value: renderBlackListCell(isHost, league.LeagueId, league.IsBlackList) }
                    ],
                    CssClass: league.IsBlackList ? 'blacklist js-parentRow' : 'js-parentRow'
                };

                bodyRows.push(row);

                if (numberOfVirtualRows > numberOfVirtualRowsRender) {
                    rowNeedRenderStartIndex = i + 1;

                    break;
                } else {
                    rowNeedRenderStartIndex = 0;
                }
            } else {
                for (var j = 0; j < numberOfOptions; j++) {
                    var copyIndex = 6;
                    numberOfVirtualRows++;
                    var option = data.Leagues[i].Options[j];

                    var currentColumms = [],
                    optionColumn = String.format("<span class='truncate option-truncate' title='{0}'>{1}</span>", common.HtmlEncode(option.Name), common.HtmlEncode(option.Name)) +
                            (isHost && j === (league.Options.length - 1) ? String.format("<a class='modal-trigger js-addOption' href='#optionModal'>"
                                + "<i original-title='Add option' class='icon icon-plus mdi mdi-plus right tooltip-tipsy' data-league-id={0}></i>"
                                + "</a>", league.LeagueId) : '');

                    if (j === 0) {
                        currentColumms.push({ Name: "No", Value: league.RowIndex, MergeRowNumber: league.RowSpan });
                        currentColumms.push({ Name: "LeagueGroup", Value: common.HtmlEncode(league.LeagueGroupName), MergeRowNumber: league.RowSpan });
                        currentColumms.push({ Name: "League", Value: common.HtmlEncode(league.LeagueName), MergeRowNumber: league.RowSpan });
                    }

                    var row = {
                        Columns: currentColumms.concat([
                            { Name: "Category", Value: option.CategoryName, CssClass: (isHost ? 'js-clearContent' : '') },
                            { Name: "Option", Value: optionColumn, CssClass: (isHost ? 'js-clearContent' : '') },
                            {
                                Name: "Remarks", Value: renderRemarksCell(isHost, league.LeagueId, option.Id, option.Remarks),
                                CssClass: (isHost ? 'js-clearContent' : '')
                            },
                            // Spread
                            {
                                Name: "Spread.Handicap",
                                Value: renderCell(isHost, copyIndex++, league.LeagueId, option.Id, data.Bettypes.Handicap, option.Spread.Handicap, false, "Spread.Handicap"),
                                CssClass: (isHost ? 'input_value js-clearContent' : '')
                            },
                            {
                                Name: "Spread.OverUnder",
                                Value: renderCell(isHost, copyIndex++, league.LeagueId, option.Id, data.Bettypes.OverUnder, option.Spread.OverUnder, false, "Spread.OverUnder"),
                                CssClass: (isHost ? 'input_value js-clearContent' : '')
                            },
                            {
                                Name: "Spread.FirstHalfHandicap",
                                Value: renderCell(isHost, copyIndex++, league.LeagueId, option.Id, data.Bettypes.FirstHalfHandicap, option.Spread.FirstHalfHandicap, false, "Spread.FirstHalfHandicap"),
                                CssClass: (isHost ? 'input_value js-clearContent' : '')
                            },
                            {
                                Name: "Spread.FirstHalfOverUnder",
                                Value: renderCell(isHost, copyIndex++, league.LeagueId, option.Id, data.Bettypes.FirstHalfOverUnder, option.Spread.FirstHalfOverUnder, false, "Spread.FirstHalfOverUnder"),
                                CssClass: (isHost ? 'input_value js-clearContent' : '')
                            },
                            // MaxBet - Weekday
                            {
                                Name: "MaxBetWeekday.Handicap",
                                Value: renderCell(isHost, copyIndex++, league.LeagueId, option.Id + option.MaxBetWeekday.OptionId, data.Bettypes.Handicap, option.MaxBetWeekday.Handicap, true, "MaxBetWeekday.Handicap"),
                                CssClass: (isHost ? 'input_value js-clearContent' : '')
                            },
                            {
                                Name: "MaxBetWeekday.OverUnder",
                                Value: renderCell(isHost, copyIndex++, league.LeagueId, option.Id + option.MaxBetWeekday.OptionId, data.Bettypes.OverUnder, option.MaxBetWeekday.OverUnder, true, "MaxBetWeekday.OverUnder"),
                                CssClass: (isHost ? 'input_value js-clearContent' : '')
                            },
                            {
                                Name: "MaxBetWeekday.FirstHalfHandicap",
                                Value: renderCell(isHost, copyIndex++, league.LeagueId, option.Id + option.MaxBetWeekday.OptionId, data.Bettypes.FirstHalfHandicap + ',' + data.Bettypes.FirstHalfOverUnder, option.MaxBetWeekday.FirstHalfHandicap, true, "MaxBetWeekday.FirstHalfHandicap"),
                                CssClass: (isHost ? 'input_value js-clearContent' : '')
                            },
                            // MaxBet - Weekend
                            {
                                Name: "MaxBetWeekend.Handicap",
                                Value: renderCell(isHost, copyIndex++, league.LeagueId, option.Id + option.MaxBetWeekend.OptionId, data.Bettypes.Handicap, option.MaxBetWeekend.Handicap, true, "MaxBetWeekend.Handicap"),
                                CssClass: (isHost ? 'input_value js-clearContent' : '')
                            },
                            {
                                Name: "MaxBetWeekend.OverUnder",
                                Value: renderCell(isHost, copyIndex++, league.LeagueId, option.Id + option.MaxBetWeekend.OptionId, data.Bettypes.OverUnder, option.MaxBetWeekend.OverUnder, true, "MaxBetWeekend.OverUnder"),
                                CssClass: (isHost ? 'input_value js-clearContent' : '')
                            },
                            {
                                Name: "MaxBetWeekend.FirstHalfHandicap",
                                Value: renderCell(isHost, copyIndex++, league.LeagueId, option.Id + option.MaxBetWeekend.OptionId, data.Bettypes.FirstHalfHandicap + ',' + data.Bettypes.FirstHalfOverUnder, option.MaxBetWeekend.FirstHalfHandicap, true, "MaxBetWeekend.FirstHalfHandicap"),
                                CssClass: (isHost ? 'input_value js-clearContent' : '')
                            },
                            // 1X2
                            {
                                Name: "OneXTwo.Spread",
                                Value: renderCell(isHost, copyIndex++, league.LeagueId, option.Id, data.Bettypes.FullTimeOneXTwo + ',' + data.Bettypes.HalfTimeOneXTwo, option.OneXTwo.Spread, false, "OneXTwo.Spread"),
                                CssClass: (isHost ? 'input_value js-clearContent' : '')
                            },
                            {
                                Name: "OneXTwo.FullTimeOneXTwo",
                                Value: renderCell(isHost, copyIndex++, league.LeagueId, option.Id, data.Bettypes.FullTimeOneXTwo, option.OneXTwo.FullTimeOneXTwo, true, "OneXTwo.FullTimeOneXTwo"),
                                CssClass: (isHost ? 'input_value js-clearContent' : '')
                            },
                            {
                                Name: "OneXTwo.HalfTimeOneXTwo",
                                Value: renderCell(isHost, copyIndex++, league.LeagueId, option.Id, data.Bettypes.HalfTimeOneXTwo, option.OneXTwo.HalfTimeOneXTwo, true, "OneXTwo.HalfTimeOneXTwo"),
                                CssClass: (isHost ? 'input_value js-clearContent' : '')
                            },
                            //
                            { Name: "LastUpdatedTime", Value: option.LastUpdatedTimeToDisplay, CssClass: (isHost ? 'js-clearContent' : '') },
                            { Name: "LastUpdatedBy", Value: option.LastUpdatedBy, CssClass: (isHost ? 'js-clearContent' : '') },
                            //
                            {
                                Name: "Action",
                                Value: '<i original-title="Delete option" class="icon icon-delete mdi mdi-delete tooltip-tipsy js-deleteOption" data-leagueid="' + league.LeagueId + '" data-optionid="' + option.Id + '"></i>',
                                CssClass: (isHost ? 'js-clearContent' : '')
                            }
                        ]),
                        CssClass: (league.IsBlackList ? 'blacklist' : '') + (j === 0 ? ' js-parentRow' : '') + (league.IsTemplate ? ' js-template' : '')
                    };

                    if (j === 0) {
                        row.Columns.push({ Name: "LastUsing", Value: league.LastUsingToDisplay, MergeRowNumber: league.RowSpan }),
                        row.Columns.push({
                            Name: "Active", Value: renderActiveCell(isHost, league.LeagueId, league.IsActive), MergeRowNumber: league.RowSpan
                        });
                        row.Columns.push({
                            Name: "BlackList", Value: renderBlackListCell(isHost, league.LeagueId, league.IsBlackList), MergeRowNumber: league.RowSpan
                        });
                    }

                    bodyRows.push(row);
                }

                if (numberOfVirtualRows > numberOfVirtualRowsRender) {
                    rowNeedRenderStartIndex = i + 1;

                    break;
                } else {
                    rowNeedRenderStartIndex = null;
                }
            }
        }

        return bodyRows;
    }

    function buildGridOptions(data) {
        gridOptions = {
            customizeHeader: true,
            groupHeaderRows: buildGroupHeaderRows(),
            columns: buildColumns(data.IsHost),
            bodyRows: buildBodyRows(data, 0, common.Constants.NumOfVirtualRows),
            gridExpandHeight: 300,
            columnsNeedHide: !data.IsHost ? [21] : [],
            showColumnsShowHide: false,
            showTitle: true,
            headerColumnHeight: 22,
            bodyRowHeight: 22,
            titleInfo: {
                text: '',
                customButtons: '<a class="btnExcel tooltip-tipsy" id="btn-export-excel" title="Export to excel"></a>'
            }
        };
    }

    function loadJqueryMap() {
        $formFilter = $('form.filter');
        $unknownCategory = $("#mb-category-unknown");
        $categoryFilter = $("input[id^='mb-category-']");
        $optionFilter = $("input[id^='mb-option-']");
        $optionIds = $("#optionIds");
        $search = $("#max-bet-search");
        $maxBetGrid = $("#max-bet-grid");
        $optionModal = $("#optionModal");
        $itemperpageOption = $('.js-pagination select.js-itemperpageOption');
        $league = $('#League');
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

    function bindingEventKeydownForRemarks(e) {
        var code = e.which;
        // Enter, Escape
        if (code === 13 || code === 27) {
            $(e.target).blur();
            return;
        }
    }

    function pageChangeHandler() {
        var oldValue = parseInt($formFilter.find('#PageSize').val()),
            newValue = parseInt($(this).val());

        if (oldValue !== newValue) {
            $formFilter.find('#PageIndex').val(1);
            $formFilter.find('#PageSize').val(newValue);

            $search.trigger('click');
        }
    }

    function registerEvents() {
        $search.on('click', function () {
            $formFilter.find('#PageIndex').val(1);
            searchHanlder();
        });

        $league.keypress(function (e) {
            var keycode = (e.keyCode ? e.keyCode : e.which);
            if (keycode === 13) {
                $search.trigger('click');
            }
        });

        $itemperpageOption.on('change', pageChangeHandler);
        regiterCategoryFilterHandler();

        //$maxBetGrid.on('mouseenter', '.column-truncate:not(.js-bindedTooltip)', function () {
        //    $(this).removeClass('js-bindedTooltip').addClass('js-bindedTooltip');
        //    $(this).tipsy({ html: true, gravity: 's' });
        //});
        $maxBetGrid.on('click', 'td .js-addOption', addOptionHandler);
        $maxBetGrid.on('click', 'td input.js-active', updateIsActiveHandler);
        $maxBetGrid.on('click', 'td input.js-blackList', updateIsBlackListHandler);
        $maxBetGrid.on('click', 'td .js-deleteOption', deleteOptionHandler);
        $maxBetGrid.on('click', 'th .js-copySetting', copySettingHandler);

        // max bet, spread
        $maxBetGrid.on('keyup', 'td .input_field.spread', function (e) {
            bindingEventKeyupForSpread(e);
        });
        $maxBetGrid.on('keyup', 'td .input_field.maxbet', function (e) {
            bindingEventKeyupForMaxBet(e);
        });
        $maxBetGrid.on('focusout', 'td .input_field.spread, td .input_field.maxbet', function (e) {
            insertOrUpdateLeagueSetting(e);
        });

        // remarks
        $maxBetGrid.on('keydown', 'td .materialize-textarea.remarks', function (e) {
            bindingEventKeydownForRemarks(e);
        });
        $maxBetGrid.on('focusout', 'td .materialize-textarea.remarks', function (e) {
            insertOrUpdateRemarks(e);
        });
    }

    function initModule() {
        loadJqueryMap();
        registerEvents();
    }

    return {
        initModule: initModule
    };
}(jQuery));

$(function () {
    maxBetManagement.initModule();

    $('#ddl-category').click(function (event) {
        event.stopPropagation();
    });

    $(document).mouseup(function (e) {
        var container = $("#ddl-category");

        if (!container.is(e.target) // if the target of the click isn't the container...
            && container.has(e.target).length === 0 // ... nor a descendant of the container
            && container.is(':visible')) {
            container.hide();
        }
    });
});