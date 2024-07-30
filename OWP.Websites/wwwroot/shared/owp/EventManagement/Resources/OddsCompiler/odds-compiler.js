"use strict";

var oddCompiler = function ($) {
    var triggerByProgramming = false;

    var isHost = $("input#isHost").prop('checked');

    var pagingData = {
        TotalItems: 50,
        ItemsPerPage: 50,
        CurrentPage: 1,
        CurrentFromItem: 1,
        CurrentToItem: 50
    };

    var odds = {};
    var oddsFilterData = {};
    var dataSource = {};
    var choice = {
        One: 1,
        Two: 2,
        Three: 3,
        None: null
    };
    var rowNeedRenderStartIndex;
    var isOdd = true,
        gridOptions,
        maxIndex;
    var domElements = {
        date: $('#date'),
        league: $('#league'),
        keyword: $('#keyword'),
        pageIndex: $('#pageIndex'),
        pageSize: $('#pageSize'),
        itemperpageOption: $('.js-pagination .js-itemperpageOption'),
        pagination: $('.js-pagination'),
        searchBtn: $('.js-btnSearch'),
        oddCompilerGrid: $('#js-odds')
    };
    var css = {
        Handicap: 'col-home-away input_value',
        OverUnder: 'right-align col-over-under input_value',
        OneXTwo: 'right-align input_value col-1x2'
    };

    function initializeDate() {
        domElements.date.datepicker({
            dateFormat: 'M d, yy'
        });
    }

    function generateColumnHeaders() {
        var columnHeaders = [
            { HeaderText: "No", Width: 50, Name: "No", HeaderAlign: "center", CellAlign: "center", TdClass: "js-rowspan" },
            { HeaderText: "Time", Width: 70, Name: "Time", HeaderAlign: "center", CellAlign: "center", TdClass: "js-rowspan" },
            { HeaderText: "Event", Width: 300, Name: "Event", HeaderAlign: "left", CellAlign: "left", TdClass: "js-rowspan" },
            { HeaderText: "Remark", Width: 230, Name: "Remark", HeaderAlign: "left", CellAlign: "left", TdClass: "js-rowspan" },
            { HeaderText: "Provider", Width: 120, Name: "Provider", HeaderAlign: "left", CellAlign: "left", TdClass: "js-rowspan" },
            { HeaderText: "Web Link", Width: 90, Name: "WekLink", HeaderAlign: "center", CellAlign: "center", TdClass: "js-rowspan" },
            { HeaderText: "MB", Width: 55, Name: "MaxBet", HeaderAlign: "center", CellAlign: "center", TdClass: "js-rowspan" },
            { HeaderText: "Spread", Width: 80, Name: "Spread", HeaderAlign: "center", CellAlign: "center", TdClass: "js-rowspan" },
            { HeaderText: "HDP", Width: 170, Name: "HDP", HeaderAlign: "left", CellAlign: "left" },
            { HeaderText: "Home", Width: 60, Name: "Home", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Away", Width: 60, Name: "Away", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "O/U", Width: 60, Name: "OU", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Over", Width: 60, Name: "Over", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Under", Width: 60, Name: "Under", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "1", Width: 60, Name: "One", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "X", Width: 60, Name: "X", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "2", Width: 60, Name: "Two", HeaderAlign: "center", CellAlign: "left" },

            { HeaderText: "Action", Width: 60, Name: "Action", HeaderAlign: "center", CellAlign: "left" }
        ];

        return columnHeaders;
    }

    function getMaxBet(matchSettings) {
        var maxBet = '';
        $.each(matchSettings, function (index, setting) {
            if (setting.Bettype === majorBettypes.Handicap) {
                maxBet = setting.MaxBetToDisplay;
            }
        });

        return maxBet;
    }

    function getSpread(matchSettings) {
        var spreadHDP = '',
            spreadOU = '',
            displayText = '',
            size = matchSettings ? matchSettings.length : 0,
            i;

        for (i = 0; i < size; i++) {
            if (matchSettings[i].Bettype === majorBettypes.Handicap) {
                spreadHDP = matchSettings[i].SpreadToDisplay;
                break;
            }
        }

        for (i = 0; i < size; i++) {
            if (matchSettings[i].Bettype === majorBettypes.OverUnder) {
                spreadOU = matchSettings[i].SpreadToDisplay;
                break;
            }
        }

        if (spreadHDP && !spreadOU) {
            displayText = spreadHDP;
        }
        if (!spreadHDP && spreadOU) {
            displayText = spreadOU;
        }
        if (spreadHDP && spreadOU) {
            displayText = spreadHDP + ' - ' + spreadOU;
        }

        return displayText;
    }

    function findOddsHandicap(odds) {
        var oddsHandicap = odds.filter(function (item) {
            return item.Bettype === majorBettypes.Handicap;
        });

        return oddsHandicap;
    }

    function findOddsOU(odds) {
        var oddsHandicap = odds.filter(function (item) {
            return item.Bettype === majorBettypes.OverUnder;
        });

        return oddsHandicap;
    }

    function findOddsOneXTwo(odds) {
        var oddsHandicap = odds.filter(function (item) {
            return item.Bettype === majorBettypes.OneXTwo;
        });

        return oddsHandicap;
    }

    function RenderOddsCell(matchId, bettype, choice, value, index, id) {
        if (!isHost) {
            var readInput = String.format("<span class={0}>{1}</span>",
                value < 0 ? "red-text" : "",
                value == null ? "" : value);
            return readInput;
        }

        var input = String.format("<input maxlength='5' type='text' value='{0}' data-bettype='{1}' data-choice='{2}' class='input_field {3}' data-oldvalue='{4}' data-index='{5}' data-id='{6}' />",
            value == null ? "" : value,
            bettype,
            choice,
            value < 0 ? "red-text" : "",
            value == null ? "" : value,
            index,
            id === undefined ? 0 : id);

        return input;
    }

    function Render1X2Cell(matchId, bettype, choice, odds, isInsert, index, id) {
        if (!isHost) {
            return String.format("<span class={0}>{1}</span>",
                odds < 0 && isOdd ? "red-text" : "",
                odds == null ? "" : odds);
        }

        var input = String.format("<input maxlength='5' type='text' value='{0}' data-bettype='{1}' data-choice='{2}' data-matchId='{3}' class='input_field js-1X2-input {4}' data-oldvalue='{5}' data-isinsert='{6}' data-index='{7}' data-id='{8}' />",
            odds == null ? "" : odds,
            bettype,
            choice,
            matchId,
            odds < 0 ? "red-text" : "",
            odds == null ? "" : odds,
            isInsert,
            index,
            id === undefined ? 0 : id);

        return input;
    }

    function RenderOUCell(matchId, bettype, choice, value, isInsert, index, id) {
        if (!isHost) {
            return (value == null || value.length === 0) ? "" : String.format("<span>@{0}</span>", value);
        }

        var input = String.format("<input maxlength='5' type='text' value='{0}' data-bettype='{1}' data-choice='{2}' data-matchId='{3}' class='input_field js-OU-input {8}' data-oldvalue='{4}' data-isinsert='{5}' data-index='{6}' data-id='{7}' />",
            value == null ? "" : value,
            bettype,
            choice,
            matchId,
            value == null ? "" : value,
            isInsert,
            index,
            id === undefined ? 0 : id,
            (value !== null && value < 0) ? "red-text" : "");

        return input;
    }

    function RenderHandicapCell(matchId, homeName, awayName, bettype, handicap, isInsert, index, id, teamChoice) {
        var isHome = 0;
        var hdpText = '';
        var dropdownText = '';

        if (teamChoice != 2) {
            isHome = 1;
            hdpText = homeName;
            dropdownText = '<div class="select-wrapper-default"><select class="js-teamhandicap browser-default"><option value="1" "selected">Home</option> <option value="2">Away</option></select></div><span class="value-hdp">@';
        }
        else {
            hdpText = awayName;
            dropdownText = '<div class="select-wrapper-default"><select class="js-teamhandicap browser-default"><option value="1">Home</option> <option value="2" selected>Away</option></select></div><span class="value-hdp">@';
        }

        if (handicap !== null && handicap.toString().length > 0) {
            hdpText = String.format("<label class='label'> {0}@</label><span>{1}</span>",
                hdpText,
                handicap == null ? "" : handicap);
        }
        else {
            hdpText = '';
        }

        if (!isHost) {
            return hdpText;
        }

        var input = String.format("<input maxlength='5' type='text' value='{0}' data-bettype='{1}'"
            + "data-choice='{2}' data-matchId='{3}' class='input_field js-handicap-input {8}' data-oldvalue='{4}' data-isinsert='{5}' data-index='{6}' data-id='{7}' /></span>",
            handicap == null ? "" : handicap,
            bettype,
            choice.None,
            matchId,
            handicap == null ? "" : handicap,
            isInsert,
            index,
            id === undefined ? 0 : id,
            handicap < 0 ? "red-text" : "",
        );

        return dropdownText + input;
    }

    function save(object, matchId, bettype, isOdd, teamChoice, choice, value, isInsert, isSaveFromSelect, index, id) {
        var criteria;
        function afterSuccess(response) {
            if (!response.IsSuccessful) {
                common.modalErrorMessage(response.Message);
                if (!isOdd) {
                    $(object).addClass('invalid');
                    $(object).val(value);
                }

                var hdpObject;
                if (bettype === majorBettypes.Handicap) {
                    hdpObject = $(object).closest('tr').find('.js-handicap-input');
                }
                else if (bettype === majorBettypes.OverUnder) {
                    hdpObject = $(object).closest('tr').find('.js-OU-input');
                }
                else {
                    hdpObject = $(object).closest('tr').find('.js-1X2-input');
                }

                $(hdpObject).data('isinsert', isInsert);
            } else {
                if (!isOdd) {
                    $(object).removeClass('invalid');
                }
                if (isInsert) {
                    var id = parseInt(response.Message);
                    var inputs = $(object).closest('tr').find("[data-bettype='" + bettype + "']");
                    inputs.attr('data-id', id);
                    inputs.data('id', id);
                }
                updateViewModel(criteria, index);
                updateDataAttribute(object, criteria);
            }
        }

        if (isInsert) {
            var criteria = {
                OWPMatchId: matchId,
                Bettype: bettype,
                TeamChoice: teamChoice,
                HDP: isOdd ? null : value,
                Choice: choice,
                Odds: isOdd ? value : null,
                isInsert: isInsert,
                IsOdd: isOdd,
                Value: value
            };
            var inputs = $(object).closest('tr').find("[data-bettype='" + bettype + "']");
            inputs.prop('disabled', true);
            oddsCompilerFactory.Insert(criteria).success(function (response) {
                afterSuccess(response);
                inputs.prop('disabled', false);
            });
        } else {
            if (isOdd) {
                var criteria = {
                    Id: id,
                    OWPMatchId: matchId,
                    Bettype: bettype,
                    Choice: choice,
                    Odds: value,
                    isInsert: isInsert,
                    IsOdd: isOdd,
                    Value: value
                };
                oddsCompilerFactory.UpdateOdds(criteria).success(function (response) {
                    afterSuccess(response);
                });
            } else {
                var criteria = {
                    Id: id,
                    OWPMatchId: matchId,
                    Bettype: bettype,
                    TeamChoice: teamChoice,
                    HDP: value,
                    isInsert: isInsert,
                    IsOdd: isOdd,
                    Value: value
                };
                oddsCompilerFactory.UpdateHDP(criteria).success(function (response) {
                    afterSuccess(response);
                });
            }
        }
    }

    function saveFromInput(e) {
        var object = $(e.target);
        var newValue = $(object).val();
        var oldValue = $(object).data('oldvalue');
        var bettype = $(object).data('bettype');
        var hdpObject;
        var isOdd = true;
        var index;
        var id;
        var teamChoice = 0;
        if (oldValue === null) {
            oldValue = "";
        }

        if (oldValue != newValue) {
            if (bettype === majorBettypes.Handicap) {
                hdpObject = $(object).parent().parent().parent().find('.js-handicap-input');
                isOdd = !$(object).hasClass('js-handicap-input');
                teamChoice = parseInt($(object).parent().parent().parent().find('.js-teamhandicap').val());
            }
            else if (bettype === majorBettypes.OverUnder) {
                hdpObject = $(object).parent().parent().parent().find('.js-OU-input');
                isOdd = !$(object).hasClass('js-OU-input');
            }
            else {
                hdpObject = $(object).parent().parent().parent().find('.js-1X2-input');
            }

            index = $(object).data('index');
            id = $(object).data('id');
            var isInsert = $(hdpObject).data('isinsert');
            $(hdpObject).data('isinsert', false);
            save(object
                , $(hdpObject).data('matchid')
                , $(object).data('bettype')
                , isOdd
                , teamChoice
                , $(object).data('choice')
                , newValue
                , isInsert
                , false
                , index,
                id);
        }
    }

    function saveFromSelect(e) {
        var object = $(e.target);
        var value = $(object).val();
        var hdpObject = $(object).parent().parent().parent().find('.js-handicap-input');
        var hdpValueString = hdpObject.val();
        var hdpValue = 0;
        if (hdpValueString.length > 0) {
            var oldhdpvalue = parseFloat(hdpValueString);
            save(
                object
                , $(hdpObject).data('matchid')
                , $(hdpObject).data('bettype')
                , false
                , value
                , choice.None
                , oldhdpvalue
                , $(hdpObject).data('isinsert')
                , true
                , null
                , $(hdpObject).data('id'));
        }
    }

    function checkEmptyRow(object) {
        var isEmptyRow = true;

        $(object).closest('tr').find('input.input_field').each(function () {
            if ($.trim($(this).val()).length !== 0) {
                isEmptyRow = false;
            }
        });

        return isEmptyRow;
    }

    function updateDataAttribute(object, model) {
        $(object).data("oldvalue", model.Value);
        $(object).attr("data-oldvalue", model.Value);

        var hdpObject;
        if (model.Bettype === majorBettypes.Handicap) {
            hdpObject = $(object).closest('tr').find('.js-handicap-input');
        }
        else if (model.Bettype === majorBettypes.OverUnder) {
            hdpObject = $(object).closest('tr').find('.js-OU-input');
        }
        else {
            hdpObject = $(object).closest('tr').find('.js-1X2-input');
        }

        $(hdpObject).data("isinsert", false);

        if ($(object).val() < 0) { //  && model.IsOdd
            $(object).addClass("red-text");
        }
        else {
            $(object).removeClass("red-text");
        }

        updateDeleteStatus(object, false);
    }

    function updateDeleteStatus(object, isEnabled) {
        if (isEnabled) {
            $(object).closest('tr').find('.js-deleteIcon').removeClass('icon-disabled').attr('original-title', 'Delete odds');
        }
        else if ($(object).closest('tr').hasClass('js-parentRow') && checkEmptyRow(object) && $(object).closest('tr').find('.js-rowspan').first().attr('rowspan') < 2) {
            $(object).closest('tr').find('.js-deleteIcon').addClass('icon-disabled').attr('original-title', '');
        }
        else {
            $(object).closest('tr').find('.js-deleteIcon').removeClass('icon-disabled').attr('original-title', 'Delete odds');
        }
    }

    function updateViewModel(model, index) {
        for (var i = 0; i < odds.OddsDetail.length; i++) {
            if (odds.OddsDetail[i].OWPMatchId === model.OWPMatchId
                && odds.OddsDetail[i].Bettype === model.Bettype
                && (model.Bettype === majorBettypes.OneXTwo)) {
                if (model.IsOdd) {
                    if (odds.OddsDetail[i].Choice === model.Choice
                        && (odds.OddsDetail[i].Index === index || model.Bettype === majorBettypes.OneXTwo)) {
                        odds.OddsDetail[i].Odds = model.Value;
                        odds.OddsDetail[i].IsInsert = false;
                        if (model.Choice === choice.One) {
                            odds.OddsDetail[i + 1].IsInsert = false;
                        }
                        else if (model.Choice === choice.Two) {
                            odds.OddsDetail[i - 1].IsInsert = false;
                        }
                    }
                } else {
                    if (odds.OddsDetail[i].Index === index || model.Bettype === majorBettypes.OneXTwo) {
                        odds.OddsDetail[i].Handicap = model.Value;
                        odds.OddsDetail[i + 1].Handicap = model.Value;
                        odds.OddsDetail[i].IsInsert = false;
                        odds.OddsDetail[i + 1].IsInsert = false;
                    }
                }
            }
        }
    }

    function combineObject(inputObject) {
        var detailOdds = {
            Bettype: $(inputObject).attr('data-bettype'),
            Handicap: $(inputObject).attr('data-oldvalue'),
            IsInsert: $(inputObject).data('isinsert'),
            Index: $(inputObject).data('index'),
            Id: $(inputObject).data('id')
        }

        if (detailOdds.Bettype !== undefined) {
            if (detailOdds.Bettype.length > 0) {
                if (detailOdds.Handicap.length === 0) {
                    detailOdds.Handicap = null;
                }
                else {
                    if (parseInt(detailOdds.Bettype) === majorBettypes.Handicap) {
                        detailOdds.Handicap = parseFloat(detailOdds.Handicap) * parseInt(inputObject.closest('tr').find('.js-teamhandicap').val());
                    }
                }
            }
        }

        return detailOdds;
    }

    function getDeletedOddsByClient(objectHDP, objectOU, object1X2) {
        var oddsDetail = [];

        if (objectHDP.Bettype.length > 0) {
            oddsDetail.push(
                {
                    Bettype: objectHDP.Bettype,
                    Handicap: objectHDP.Handicap === null ? '' : objectHDP.Handicap,
                    Index: objectHDP.Index
                });
        }

        if (objectOU.Bettype.length > 0) {
            oddsDetail.push(
                {
                    Bettype: objectOU.Bettype,
                    Handicap: objectOU.Handicap === null ? '' : objectOU.Handicap,
                    Index: objectOU.Index
                });
        }

        if (object1X2.Bettype !== undefined) {
            if (object1X2.Bettype.length > 0) {
                oddsDetail.push(
                    {
                        Bettype: object1X2.Bettype,
                        Handicap: object1X2.Handicap
                    });
            }
        }

        return oddsDetail;
    }

    function getDeletedOddsByServer(objectHDP, objectOU, object1X2) {
        var oddsDeleteDetail = [];

        if (objectHDP.Bettype.length > 0) {
            if (objectHDP.IsInsert === false) {
                oddsDeleteDetail.push(
                    {
                        Bettype: objectHDP.Bettype,
                        Handicap: objectHDP.Handicap
                    });
            }
        }

        if (objectOU.Bettype.length > 0) {
            if (objectOU.IsInsert === false) {
                oddsDeleteDetail.push(
                    {
                        Bettype: objectOU.Bettype,
                        Handicap: objectOU.Handicap
                    });
            }
        }

        if (object1X2.Bettype !== undefined) {
            if (object1X2.Bettype.length > 0) {
                oddsDeleteDetail.push(
                    {
                        Bettype: object1X2.Bettype,
                        Handicap: object1X2.Handicap
                    });
            }
        }

        return oddsDeleteDetail;
    }

    function deleteOdds(e) {
        var modalConfirm = $('#js-modalConfirmDeleteOption'),
            $row = $(e.target).closest('tr'),
            object = $(e.target);

        if (checkEmptyRow(object)) {
            var objectHDP = combineObject($row.find('.js-handicap-input'));
            var objectOU = combineObject($row.find('.js-OU-input'));
            var object1X2 = combineObject($row.find('.js-1X2-input'));
            var oddsDetail = getDeletedOddsByClient(objectHDP, objectOU, object1X2);
            var oddsDeleteDetail = getDeletedOddsByServer(objectHDP, objectOU, object1X2);

            removeRow($row, parseInt($(object).attr('data-matchid')), oddsDetail, objectHDP.Index, objectOU.Index);
        }
        else {
            Swal.fire({
                html: 'Are you sure you want to permanently delete this recommend odd?',
                type: 'question',
                showCancelButton: true,
                reverseButtons: true,
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            }).then((result) => {
                if (result.value) {
                    var objectHDP = combineObject($row.find('.js-handicap-input'));
                    var objectOU = combineObject($row.find('.js-OU-input'));
                    var object1X2 = combineObject($row.find('.js-1X2-input'));
                    var oddsDetail = getDeletedOddsByClient(objectHDP, objectOU, object1X2);
                    var oddsDeleteDetail = getDeletedOddsByServer(objectHDP, objectOU, object1X2);

                    var arrMatchRecommendOddsId = [];
                    if (objectHDP && objectHDP.Id) {
                        arrMatchRecommendOddsId.push(objectHDP.Id);
                    }
                    if (objectOU && objectOU.Id) {
                        arrMatchRecommendOddsId.push(objectOU.Id);
                    }
                    if (object1X2 && object1X2.Id) {
                        arrMatchRecommendOddsId.push(object1X2.Id);
                    }

                    if (oddsDeleteDetail.length > 0) {
                        var criteria = {
                            MatchRecommendOddsIds: arrMatchRecommendOddsId.toString()
                        };

                        oddsCompilerFactory.Delete(criteria).success(function (response) {
                            if (!response.IsSuccessful) {
                                common.modalErrorMessage(response.Message);
                            } else {
                                // removeRow($row, parseInt($(object).attr('data-matchid')), oddsDetail, objectHDP.Index, objectOU.Index);
                                searchOddsFromServer();
                            }
                        });
                    }
                    else {
                        removeRow($row, parseInt($(object).attr('data-matchid')), oddsDetail, objectHDP.Index, objectOU.Index);
                    }
                }
            });
        }

        $('div.tipsy').remove();
        domElements.oddCompilerGrid.fanexGrid('resizeGrid');
    }

    function removeRow($row, matchId, removeOdds, indexHDP, indexOU) {
        var oddsIndex = odds.Matchs.map(function (x) { return x.OWPMatchId; }).indexOf(matchId);
        var newMatch = odds.Matchs[oddsIndex];
        newMatch.IsTemplate = true;

        var isExists = false;
        var index = [];
        for (var i = 0; i < odds.OddsDetail.length; i++) {
            isExists = false;
            if (odds.OddsDetail[i].OWPMatchId === matchId) {
                for (var j = 0; j < removeOdds.length; j++) {
                    var handicapTemp1 = removeOdds[j].Handicap === null ? '' : removeOdds[j].Handicap.toString();
                    var handicapTemp2 = odds.OddsDetail[i].Handicap === null ? '' : odds.OddsDetail[i].Handicap.toString();
                    if (odds.OddsDetail[i].Bettype.toString() == removeOdds[j].Bettype &&
                        (odds.OddsDetail[i].Index === indexHDP || odds.OddsDetail[i].Index === indexOU
                            || odds.OddsDetail[i].Index === (indexHDP + 1) || odds.OddsDetail[i].Index === (indexOU + 1)
                        ) &&
                        ((handicapTemp1 == handicapTemp2)
                            || (handicapTemp1.length > 0))
                    ) {
                        isExists = true;
                        break;
                    }
                }

                if (removeOdds.length === 3 && odds.OddsDetail[i].Bettype === majorBettypes.OneXTwo) {
                    index.push(i);
                }
            }

            if (isExists) {
                index.push(i);
            }
        }

        for (var i = index.length - 1; i >= 0; i--) {
            odds.OddsDetail.splice(index[i], 1);
        }

        var oddsOfMatch = odds.OddsDetail.filter(function (item) {
            return item.OWPMatchId === matchId;
        });

        if (oddsOfMatch.length === 0) {
            addNewRow(matchId);
        }

        var newMatchOdds = {
            Matchs: [newMatch],
            OddsDetail: odds.OddsDetail
        }

        var oldRowNeedRenderStartIndex = rowNeedRenderStartIndex;
        domElements.oddCompilerGrid.fanexGrid('appendBody', gridOptions, generateBodyRows(newMatchOdds, 0, common.Constants.VirtualThresholdNumber));
        rowNeedRenderStartIndex = oldRowNeedRenderStartIndex;

        domElements.oddCompilerGrid.find('tr:not(.js-template) input.js-handicap-input[data-matchid="' + matchId + '"]').closest('tr').wrapAll('<div>').parent().addClass('js-tr-remove');
        domElements.oddCompilerGrid.find('tr.js-template').wrapAll('<div>').parent().addClass('js-tr-new');

        var newHTML = domElements.oddCompilerGrid.find('.js-tr-new').html();
        domElements.oddCompilerGrid.find('.js-tr-new').remove();
        domElements.oddCompilerGrid.find('.js-tr-remove').html(newHTML);

        domElements.oddCompilerGrid.find('tr.js-template').unwrap('<div>').removeClass('js-template');
    }

    function getMaxIndex() {
        maxIndex = 0;
        for (var j = 0; j < odds.OddsDetail.length; j++) {
            if (odds.OddsDetail[j].Index > maxIndex) {
                maxIndex = odds.OddsDetail[j].Index;
            }
        }
    }

    function addNewRow(matchId) {
        var handicap1 = {
            Handicap: '',
            Bettype: majorBettypes.Handicap,
            Choice: choice.One,
            Odds: null,
            OWPMatchId: matchId,
            IsInsert: true,
            Index: ++maxIndex
        };
        var handicap2 = {
            Handicap: '',
            Bettype: majorBettypes.Handicap,
            Choice: choice.Two,
            Odds: null,
            OWPMatchId: matchId,
            IsInsert: true,
            Index: ++maxIndex
        };

        var over = {
            Handicap: '',
            Bettype: majorBettypes.OverUnder,
            Choice: choice.One,
            Odds: null,
            OWPMatchId: matchId,
            IsInsert: true,
            Index: ++maxIndex
        };

        var under = {
            Handicap: '',
            Bettype: majorBettypes.OverUnder,
            Choice: choice.Two,
            Odds: null,
            OWPMatchId: matchId,
            IsInsert: true,
            Index: ++maxIndex
        };

        var one = {
            Handicap: '',
            Bettype: majorBettypes.OneXTwo,
            Choice: choice.One,
            Odds: null,
            OWPMatchId: matchId,
            IsInsert: true,
            Index: ++maxIndex
        };
        var two = {
            Handicap: '',
            Bettype: majorBettypes.OneXTwo,
            Choice: choice.Two,
            Odds: null,
            OWPMatchId: matchId,
            IsInsert: true,
            Index: ++maxIndex
        };
        var draw = {
            Handicap: '',
            Bettype: majorBettypes.OneXTwo,
            Choice: choice.Three,
            Odds: null,
            OWPMatchId: matchId,
            IsInsert: true,
            Index: ++maxIndex
        };

        odds.OddsDetail = odds.OddsDetail.concat([handicap1, handicap2, over, under]);
    }

    function plusIconClick(e) {
        var matchId = $(e.target).data('match-id'),
            $row = $(e.target).closest('tr'),
            object = $(e.target).closest('td'),
            oldRowNeedRenderStartIndex = rowNeedRenderStartIndex,
            rowspan = 0;

        addNewRow(matchId);

        var oddsIndex = odds.Matchs.map(function (x) { return x.OWPMatchId; }).indexOf(matchId);

        var newMatch = odds.Matchs[oddsIndex];
        newMatch.IsTemplate = true;
        var newOdds = {
            Matchs: [newMatch],
            OddsDetail: odds.OddsDetail
        }
        domElements.oddCompilerGrid.fanexGrid('appendBody', gridOptions, generateBodyRows(newOdds, 0, common.Constants.VirtualThresholdNumber));
        rowNeedRenderStartIndex = oldRowNeedRenderStartIndex;

        var parentRow = $row.hasClass('js-parentRow') ? $row : $($row.prevAll('.js-parentRow:first')[0]);
        var templates = domElements.oddCompilerGrid.find('.js-template').clone();
        rowspan = domElements.oddCompilerGrid.find('.js-template').find('.js-rowspan').first().attr('rowspan');

        domElements.oddCompilerGrid.find('.js-template').remove();

        parentRow.find('td.js-rowspan').attr('rowspan', rowspan);

        var $clone = $(templates[templates.length - 1]);
        $clone.wrap('<div>');
        $row.after($clone.parent().html());
        domElements.oddCompilerGrid.find('.js-template').removeClass('js-template');

        $row.find('.js-plusIcon').remove();

        $('div.tipsy').remove();
        domElements.oddCompilerGrid.fanexGrid('resizeGrid');
        domElements.oddCompilerGrid.find('.tooltip-tipsy').tipsy('disable').tipsy({ html: true, gravity: 's' });

        updateDeleteStatus(object, true);

        var scrollTop = domElements.oddCompilerGrid.find('.y-scrollbar').scrollTop();
        domElements.oddCompilerGrid.find('.y-scrollbar').scrollTop(scrollTop + $row.height());
    }

    function newGuid() {
        var sGuid = '';
        for (var i = 0; i < 32; i++) {
            sGuid += Math.floor(Math.random() * 0xF).toString(0xF);
        }
        return sGuid;
    }

    function validURL(url) {
        var pattern = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/;
        if (pattern.test(url)) {
            return url;
        }
        return null;
    }

    function generateBodyRows(data, currentIndex, numberOfVirtualRowsRender) {
        var bodyRows = [],
            numberOfVirtualRows = 0;
        var totalRows = data.Matchs.length;
        for (let i = currentIndex; i < totalRows; i++) {
            var matchItem = data.Matchs[i];
            var matchId = matchItem.OWPMatchId;

            var odds = data.OddsDetail.filter(function (item) {
                return item.OWPMatchId === matchId;
            });

            var oddsHandicap = findOddsHandicap(odds);

            var oddsOU = findOddsOU(odds);
            var oddsOneXTwo = findOddsOneXTwo(odds);

            var mergeRowNumber = Math.max(oddsHandicap.length / 2, oddsOU.length / 2, oddsOneXTwo.length / 3, 1);
            console.log(mergeRowNumber);
            console.log(oddsHandicap, oddsOU, oddsOneXTwo);
            var indexHandicap = 0, indexOU = 0, index1X2 = 0;
            var isGenerated1X2 = false;

            var plusIcon = String.format("<i original-title='Add odds' class='icon icon-plus mdi mdi-plus tooltip-tipsy js-plusIcon' data-match-id={0}></i>", matchId);
            var deleteIcon = String.format("<i original-title='Delete odds' class='icon icon-delete mdi mdi-delete tooltip-tipsy js-deleteIcon' data-matchid={0}></i>", matchId);
            var deleteDisabledIcon = String.format("<i class='icon icon-delete mdi mdi-delete tooltip-tipsy js-deleteIcon icon-disabled' data-matchid={0}></i>", matchId);

            var isEmptyRow = true;
            for (let j = 0; j < mergeRowNumber; j++) {
                var currentColumns = [];
                numberOfVirtualRows++;
                if (j == 0) {
                    var eventText = "<span class='match-name'>" + common.HtmlEncode(matchItem.MatchName) + "</span><br/>";
                    eventText += "<span class='league-name'>" + common.HtmlEncode(matchItem.League) + "</span>";
                    var remarkText = "";

                    if (matchItem.Remark) {
                        remarkText += common.HtmlEncode(matchItem.Remark) + "<br/>";
                    }
                    if (matchItem.Round) {
                        remarkText += "<b>Round: </b>" + common.HtmlEncode(matchItem.Round) + "<br/>";
                    }
                    if (matchItem.Division) {
                        remarkText += "<b>Division: </b>" + common.HtmlEncode(matchItem.Division) + "<br/>";
                    }

                    var providerText = "";
                    if (matchItem.Provider) {
                        providerText += "<b>Provider: </b>" + matchItem.Provider + "<br/>";
                    }
                    if (matchItem.WatchBet) {
                        providerText += "<b>WB: </b>" + matchItem.WatchBet + "<br/>";
                    }
                    if (matchItem.OtherProvider) {
                        providerText += '<div class="icon-bottom">';
                        providerText += '<i class="icon mdi mdi-format-list-bulleted icon-list-provider dropdown-button tooltip-tipsy"  original-title="' + matchItem.OtherProvider + '"></i>';
                        providerText += '</div>';
                    }
                    var webLink = "";
                    matchItem.Website1 = validURL(matchItem.Website1);
                    matchItem.Website2 = validURL(matchItem.Website2);
                    matchItem.Website3 = validURL(matchItem.Website3);
                    matchItem.Website4 = validURL(matchItem.Website4);

                    if (matchItem.Website1) {
                        webLink += "<a class='external-link' target='_blank' href='" + matchItem.Website1 + "' >[Link1]</a>";
                    }
                    if ((matchItem.Website1 && matchItem.Website2) || (matchItem.Website1 && matchItem.Website3) || (matchItem.Website1 && matchItem.Website4)) {
                        webLink += ", ";
                    }
                    if (matchItem.Website2) {
                        webLink += "<a class='external-link' target='_blank' href='" + matchItem.Website2 + "' >[Link2]</a>";
                    }
                    if (matchItem.Website1 && matchItem.Website2 && matchItem.Website3) {
                        webLink += "<br/>";
                    }
                    if (!matchItem.Website1 && (matchItem.Website2 && matchItem.Website3 || matchItem.Website2 && matchItem.Website4)) {
                        webLink += ", ";
                    }
                    if (matchItem.Website3) {
                        webLink += "<a class='external-link' target='_blank' href='" + matchItem.Website3 + "' >[Link3]</a>";
                    }
                    if (matchItem.Website3 && matchItem.Website4) {
                        webLink += ", ";
                    }
                    if (matchItem.Website4) {
                        webLink += "<a class='external-link' target='_blank' href='" + matchItem.Website4 + "' >[Link4]</a>";
                    }

                    currentColumns = [
                        { Name: "No", Value: matchItem.MatchNo, MergeRowNumber: mergeRowNumber },
                        { Name: "Time", Value: matchItem.KickOffTimeToDisplay, MergeRowNumber: mergeRowNumber },
                        { Name: "Event", Value: eventText, MergeRowNumber: mergeRowNumber },
                        { Name: "Remark", Value: remarkText, MergeRowNumber: mergeRowNumber },
                        { Name: "Provider", Value: providerText, MergeRowNumber: mergeRowNumber },
                        { Name: "WekLink", Value: webLink, MergeRowNumber: mergeRowNumber },
                        { Name: "MaxBet", Value: getMaxBet(matchItem.MatchSettings), MergeRowNumber: mergeRowNumber, CellAlign: "center" },
                        { Name: "Spread", Value: getSpread(matchItem.MatchSettings), MergeRowNumber: mergeRowNumber, CellAlign: "center" }];
                }

                if (oddsHandicap.length > indexHandicap) {
                    currentColumns.push({
                        ThClass: css.Handicap, TdClass: css.Handicap, Name: "HDP", Value:
                            RenderHandicapCell(matchId, matchItem.Home, matchItem.Away, majorBettypes.Handicap, oddsHandicap[indexHandicap].Handicap, oddsHandicap[indexHandicap].IsInsert, oddsHandicap[indexHandicap].Index, oddsHandicap[indexHandicap].Id, oddsHandicap[indexHandicap].TeamChoice)
                    });
                    currentColumns.push({ CellAlign: "right", ThClass: css.Handicap, TdClass: css.Handicap, Name: "Home", Value: RenderOddsCell(matchId, majorBettypes.Handicap, choice.One, oddsHandicap[indexHandicap].Odds, oddsHandicap[indexHandicap].Index, oddsHandicap[indexHandicap].Id) });
                    currentColumns.push({
                        CellAlign: "right", ThClass: css.Handicap, TdClass: css.Handicap, Name: "Away", Value: RenderOddsCell(matchId, majorBettypes.Handicap, choice.Two, oddsHandicap[indexHandicap + 1].Odds, oddsHandicap[indexHandicap + 1].Index, oddsHandicap[indexHandicap].Id)
                    });

                    if ((oddsHandicap[indexHandicap].Handicap !== null && oddsHandicap[indexHandicap].Handicap !== "")
                        || (oddsHandicap[indexHandicap].Odds !== null && oddsHandicap[indexHandicap].Odds !== "")
                        || (oddsHandicap[indexHandicap + 1].Odds !== null && oddsHandicap[indexHandicap + 1].Odds !== "")
                    ) {
                        isEmptyRow = false;
                    }

                    indexHandicap += 2;
                } else {
                    currentColumns.push({ ThClass: css.Handicap, TdClass: css.Handicap, Name: "HDP", Value: RenderHandicapCell(matchId, matchItem.Home, matchItem.Away, majorBettypes.Handicap, '', true, maxIndex + 1) });
                    currentColumns.push({ CellAlign: "right", ThClass: css.Handicap, TdClass: css.Handicap, Name: "Home", Value: RenderOddsCell(matchId, majorBettypes.Handicap, choice.One, "", maxIndex + 1) });
                    currentColumns.push({ CellAlign: "right", ThClass: css.Handicap, TdClass: css.Handicap, Name: "Away", Value: RenderOddsCell(matchId, majorBettypes.Handicap, choice.Two, "", maxIndex + 2) });

                    addHDPEmpty(matchId);
                }

                if (oddsOU.length > indexOU) {
                    currentColumns.push({ CellAlign: "right", ThClass: css.OverUnder, TdClass: css.OverUnder, Name: "OU", Value: RenderOUCell(matchId, majorBettypes.OverUnder, choice.None, oddsOU[indexOU].Handicap, oddsOU[indexOU].IsInsert, oddsOU[indexOU].Index, oddsOU[indexOU].Id) });
                    currentColumns.push({ CellAlign: "right", ThClass: css.OverUnder, TdClass: css.OverUnder, Name: "Over", Value: RenderOddsCell(matchId, majorBettypes.OverUnder, choice.One, oddsOU[indexOU].Odds, oddsOU[indexOU].Index, oddsOU[indexOU].Id) });
                    currentColumns.push({ CellAlign: "right", ThClass: css.OverUnder, TdClass: css.OverUnder, Name: "Under", Value: RenderOddsCell(matchId, majorBettypes.OverUnder, choice.Two, oddsOU[indexOU + 1].Odds, oddsOU[indexOU + 1].Index, oddsOU[indexOU].Id) });

                    if ((oddsOU[indexOU].Handicap !== null && oddsOU[indexOU].Handicap !== "")
                        || (oddsOU[indexOU].Odds !== null && oddsOU[indexOU].Odds !== "")
                        || (oddsOU[indexOU + 1].Odds !== null && oddsOU[indexOU + 1].Odds !== "")
                    ) {
                        isEmptyRow = false;
                    }
                    indexOU += 2;
                } else {
                    currentColumns.push({ CellAlign: "right", ThClass: css.OverUnder, TdClass: css.OverUnder, Name: "OU", Value: RenderOUCell(matchId, majorBettypes.OverUnder, choice.None, "", true, maxIndex + 1) });
                    currentColumns.push({ CellAlign: "right", ThClass: css.OverUnder, TdClass: css.OverUnder, Name: "Over", Value: RenderOddsCell(matchId, majorBettypes.OverUnder, choice.One, "", maxIndex + 1) });
                    currentColumns.push({ CellAlign: "right", ThClass: css.OverUnder, TdClass: css.OverUnder, Name: "Under", Value: RenderOddsCell(matchId, majorBettypes.OverUnder, choice.Two, "", maxIndex + 1) });

                    addOUEmpty(matchId);
                }

                if (isGenerated1X2) {
                    currentColumns.push({ ThClass: css.OneXTwo, TdClass: css.OneXTwo, Name: "One", Value: '' });
                    currentColumns.push({ ThClass: css.OneXTwo, TdClass: css.OneXTwo, Name: "X", Value: '' });
                    currentColumns.push({ ThClass: css.OneXTwo, TdClass: css.OneXTwo, Name: "Two", Value: '' });
                } else {
                    isGenerated1X2 = true;
                    if (oddsOneXTwo.length > index1X2) {
                        currentColumns.push({
                            CellAlign: "right", ThClass: css.OneXTwo, TdClass: css.OneXTwo, Name: "One", Value: Render1X2Cell(matchId, majorBettypes.OneXTwo, choice.One, oddsOneXTwo[index1X2].Odds, oddsOneXTwo[index1X2].IsInsert, oddsOneXTwo[index1X2].Index, oddsOneXTwo[index1X2].Id)
                        });
                        currentColumns.push({ CellAlign: "right", ThClass: css.OneXTwo, TdClass: css.OneXTwo, Name: "X", Value: RenderOddsCell(matchId, majorBettypes.OneXTwo, choice.Three, oddsOneXTwo[index1X2 + 2].Odds, oddsOneXTwo[index1X2 + 2].Index, oddsOneXTwo[index1X2].Id) });
                        currentColumns.push({ CellAlign: "right", ThClass: css.OneXTwo, TdClass: css.OneXTwo, Name: "Two", Value: RenderOddsCell(matchId, majorBettypes.OneXTwo, choice.Two, oddsOneXTwo[index1X2 + 1].Odds, oddsOneXTwo[index1X2 + 1].Index, oddsOneXTwo[index1X2].Id) });

                        if ((oddsOneXTwo[index1X2].Odds !== null && oddsOneXTwo[index1X2].Odds !== "")
                            || (oddsOneXTwo[index1X2 + 2].Odds !== null && oddsOneXTwo[index1X2 + 2].Odds !== "")
                            || (oddsOneXTwo[index1X2 + 1].Odds !== null && oddsOneXTwo[index1X2 + 1].Odds !== "")
                        ) {
                            isEmptyRow = false;
                        }

                        index1X2 += 3;
                    } else {
                        currentColumns.push({ CellAlign: "right", ThClass: css.OneXTwo, TdClass: css.OneXTwo, Name: "One", Value: Render1X2Cell(matchId, majorBettypes.OneXTwo, choice.One, "", true, maxIndex + 1) });
                        currentColumns.push({ CellAlign: "right", ThClass: css.OneXTwo, TdClass: css.OneXTwo, Name: "X", Value: RenderOddsCell(matchId, majorBettypes.OneXTwo, choice.Three, "", maxIndex + 3) });
                        currentColumns.push({ CellAlign: "right", ThClass: css.OneXTwo, TdClass: css.OneXTwo, Name: "Two", Value: RenderOddsCell(matchId, majorBettypes.OneXTwo, choice.Two, "", maxIndex + 2) });

                        add1X2Empty(matchId);
                    }
                }

                var actionIcon = deleteIcon;
                if (isEmptyRow && j === 0) {
                    actionIcon = deleteDisabledIcon;
                }

                if (j == mergeRowNumber - 1) {
                    actionIcon += plusIcon;
                }
                currentColumns.push({
                    Name: "Action", Value: actionIcon
                });

                bodyRows.push({
                    Columns: currentColumns,
                    CssClass: (j === 0 ? 'js-parentRow' : '') + (matchItem.IsTemplate === true ? ' js-template' : '')
                });
            }

            if (numberOfVirtualRows > numberOfVirtualRowsRender) {
                rowNeedRenderStartIndex = i + 1;

                break;
            } else {
                rowNeedRenderStartIndex = 0;
            }
        }

        return bodyRows;
    }

    function addHDPEmpty(matchId) {
        var handicap1 = {
            Handicap: '',
            Bettype: majorBettypes.Handicap,
            Choice: choice.One,
            Odds: null,
            OWPMatchId: matchId,
            IsInsert: true,
            Index: ++maxIndex
        };
        var handicap2 = {
            Handicap: '',
            Bettype: majorBettypes.Handicap,
            Choice: choice.Two,
            Odds: null,
            OWPMatchId: matchId,
            IsInsert: true,
            Index: ++maxIndex
        };

        odds.OddsDetail = odds.OddsDetail.concat([handicap1, handicap2]);
    }

    function addOUEmpty(matchId) {
        var over = {
            Handicap: '',
            Bettype: majorBettypes.OverUnder,
            Choice: choice.One,
            Odds: null,
            OWPMatchId: matchId,
            IsInsert: true,
            Index: ++maxIndex
        };

        var under = {
            Handicap: '',
            Bettype: majorBettypes.OverUnder,
            Choice: choice.Two,
            Odds: null,
            OWPMatchId: matchId,
            IsInsert: true,
            Index: ++maxIndex
        };

        odds.OddsDetail = odds.OddsDetail.concat([over, under]);
    }

    function add1X2Empty(matchId) {
        var one = {
            Handicap: '',
            Bettype: majorBettypes.OneXTwo,
            Choice: choice.One,
            Odds: null,
            OWPMatchId: matchId,
            IsInsert: true,
            Index: ++maxIndex
        };
        var two = {
            Handicap: '',
            Bettype: majorBettypes.OneXTwo,
            Choice: choice.Two,
            Odds: null,
            OWPMatchId: matchId,
            IsInsert: true,
            Index: ++maxIndex
        };
        var draw = {
            Handicap: '',
            Bettype: majorBettypes.OneXTwo,
            Choice: choice.Three,
            Odds: null,
            OWPMatchId: matchId,
            IsInsert: true,
            Index: ++maxIndex
        };

        odds.OddsDetail = odds.OddsDetail.concat([one, two, draw]);
    }

    function resetPagingValue() {
        pagingData.CurrentPage = 1;
        pagingData.ItemsPerPage = 50;
        pagingData.TotalItems = 50;
        domElements.itemperpageOption.val(50);
        domElements.itemperpageOption.material_select();
    }

    function initPagination() {
        domElements.pagination.find('.detailtext').text('Showing ' + pagingData.CurrentFromItem + ' to ' + pagingData.CurrentToItem + ' of ' + pagingData.TotalItems + ' entries');
        common.InitPagination(
            domElements.pagination.find('#numberlist'),
            pagingData.TotalItems,
            pagingData.ItemsPerPage,
            pagingData.CurrentPage,
            onPageChanging);
    }

    function onPageChanging(pageNumber) {
        pagingData.CurrentPage = pageNumber;
        searchOdds();
        domElements.pagination.find('.detailtext').text('Showing ' + pagingData.CurrentFromItem + ' to ' + pagingData.CurrentToItem + ' of ' + pagingData.TotalItems + ' entries');
    }

    function renderGrid(data) {
        var totalColumns = generateColumnHeaders().length;
        gridOptions = {
            columns: generateColumnHeaders(),
            bodyRows: generateBodyRows(data, 0, common.Constants.NumOfVirtualRows),
            gridExpandHeight: 280,
            numberOfFrozenColumn: 0,
            autoFixFrozenRowHeight: true,
            autoResizeColumn: true,
            columnsNeedHide: [totalColumns - 1],
            showColumnsShowHide: isHost
        };

        if (!domElements.oddCompilerGrid.hasClass('freegrid')) {
            domElements.oddCompilerGrid.addClass('freegrid');
        }

        domElements.oddCompilerGrid.fanexGrid(gridOptions);

        domElements.oddCompilerGrid.find('.tooltip-tipsy').tipsy('disable').tipsy({ html: true, gravity: 's' });

        domElements.oddCompilerGrid.find('.dropdown-button').dropdown({ belowOrigin: true });

        domElements.oddCompilerGrid.find('.grid-container').on('scroll', function () {
            var yscrollbar = domElements.oddCompilerGrid.find('.y-scrollbar'),
                container = domElements.oddCompilerGrid.find('.grid-container'),
                content = container.find('.content-container');

            var left = $(this).scrollLeft();
            domElements.oddCompilerGrid.find('.col_header').prop('scrollLeft', left);
            domElements.oddCompilerGrid.find('.x-scrollbar').prop('scrollLeft', left);

            var contentTwoThirdsHeight = parseInt(content.height() * 2 / 3),
                yScrollTop = yscrollbar.scrollTop(),
                containerScrollTop = container.scrollTop();

            if (rowNeedRenderStartIndex > 0
                && rowNeedRenderStartIndex < data.Matchs.length
                && (yScrollTop > contentTwoThirdsHeight || containerScrollTop > contentTwoThirdsHeight)) {
                domElements.oddCompilerGrid.fanexGrid('appendBody', gridOptions, generateBodyRows(data, rowNeedRenderStartIndex, common.Constants.VirtualThresholdNumber));
                domElements.oddCompilerGrid.find('.tooltip-tipsy').tipsy('disable').tipsy({ html: true, gravity: 's' });
            }
        });
    }
    function searchOddsFromServer() {
        $('.loading-process').css('display', 'inline');
        var filter = {
            Date: domElements.date.val(),
            League: domElements.league.val(),
            MatchStatusIds: [4, 3, 2, 1, 5, 6]
        };

        oddsCompilerFactory.GetOdds(filter).success(function (response) {
            dataSource = response;
            oddsFilterData = response;
            searchOdds();
        });
    }
    function searchOdds() {
        $('.loading-process').css('display', 'inline');
        $('div.tipsy').remove();

        odds = { Matchs: [], OddsDetail: [] };

        var cloneOdds = JSON.parse(JSON.stringify(oddsFilterData));

        if (cloneOdds.Matchs && cloneOdds.Matchs.length) {
            domElements.pagination.show();

            var totalItems = cloneOdds.Matchs.length;
            var fromItem = (pagingData.CurrentPage - 1) * pagingData.ItemsPerPage;
            var toItem = pagingData.CurrentPage * pagingData.ItemsPerPage - 1;
            toItem = (totalItems - 1) < toItem ? (totalItems - 1) : toItem;

            pagingData.TotalItems = totalItems;
            pagingData.CurrentFromItem = fromItem + 1;
            pagingData.CurrentToItem = toItem + 1;

            for (var i = fromItem; i <= toItem; i++) {
                var data = cloneOdds.Matchs[i];
                odds.Matchs.push(data);
                odds.OddsDetail.concat(data.OddsDetails);
                var isExistsRow = false;
                var oddsIndex = 0;
                for (var j = 0; j < data.OddsDetails.length; j++) {
                    oddsIndex++;
                    data.OddsDetails[j].Index = oddsIndex;
                    if (data.OddsDetails[j].OWPMatchId === data.OWPMatchId) {
                        isExistsRow = true;
                    }
                }

                if (isExistsRow === false) {
                    addNewRow(data.OWPMatchId);
                }
            }
        }
        else {
            domElements.pagination.hide();
        }

        odds.OddsDetail = cloneOdds.OddsDetail;
        getMaxIndex();
        initPagination();

        renderGrid(odds);

        $('.loading-process').css('display', 'none');
    };

    function registerEvents() {
        domElements.itemperpageOption.on('change', function () {
            var oldValue = pagingData.ItemsPerPage,
                newValue = parseInt($(this).val());

            if (oldValue !== newValue) {
                pagingData.CurrentPage = 1;
                pagingData.ItemsPerPage = newValue;

                searchOdds();
            }
        });

        domElements.searchBtn.on('click', function () {
            resetPagingValue();
            odds = {};
            oddsFilterData = {};
            dataSource = {};
            searchOddsFromServer();
        });

        domElements.league.keypress(function (e) {
            var keycode = (e.keyCode ? e.keyCode : e.which);
            if (keycode === 13) {
                domElements.searchBtn.trigger('click');
            }
        });

        domElements.keyword.keypress(function (e) {
            var keycode = (e.keyCode ? e.keyCode : e.which);
            if (keycode === 13) {
                resetPagingValue();
                $('.loading-process').css('display', 'inline');
                if (dataSource.Matchs) {
                    var keyword = domElements.keyword.val();
                    oddsFilterData = JSON.parse(JSON.stringify(dataSource));

                    let matchs = oddsFilterData.Matchs;

                    if (keyword) {
                        keyword = keyword.trim().toLowerCase();
                        matchs = matchs.filter(x => (x.League !== null && x.League.toLowerCase().indexOf(keyword) > -1)
                            || (x.MatchNo !== null && x.MatchNo.toLowerCase().indexOf(keyword) > -1)
                            || (x.MatchName !== null && x.MatchName.toLowerCase().indexOf(keyword) > -1)
                            || (x.Division !== null && x.Division.toLowerCase().indexOf(keyword) > -1)
                            || (x.Channel !== null && x.Channel.toLowerCase().indexOf(keyword) > -1)
                            || (x.OtherProvider !== null && x.OtherProvider.toLowerCase().indexOf(keyword) > -1)
                            || (x.Remark !== null && x.Remark.toLowerCase().indexOf(keyword) > -1)
                            || (x.Round !== null && x.Round.toLowerCase().indexOf(keyword) > -1)
                            || (x.Provider !== null && x.Provider.toLowerCase().indexOf(keyword) > -1)
                            || (x.WatchBet !== null && x.WatchBet.toLowerCase().indexOf(keyword) > -1)
                            || (x.KickOffTimeToDisplay !== null && x.KickOffTimeToDisplay.toLowerCase().indexOf(keyword) > -1)
                            || (x.MatchSettings !== null && x.MatchSettings.filter(y => y.MaxBetToDisplay !== null && y.MaxBetToDisplay.toLowerCase().indexOf(keyword) > -1).length)
                            || (x.MatchSettings !== null && x.MatchSettings.filter(y => y.SpreadToDisplay !== null && y.SpreadToDisplay.toLowerCase().indexOf(keyword) > -1).length));
                    }

                    oddsFilterData.Matchs = matchs;
                    oddsFilterData.TotalItem = matchs.length;

                    searchOdds();
                }
                else {
                    domElements.searchBtn.trigger('click');
                }
            }
        });

        initializeDate();

        $(".icon-calendar").click(function () {
            domElements.date.focus();
        });

        domElements.oddCompilerGrid.on('keyup', 'td .input_field', function (e) {
            bindingEventKeyupForOdds(e);
        });

        domElements.oddCompilerGrid.on('keyup', 'td .js-handicap-input', function (e) {
            bindingEventKeyupForHandicap(e);
        });

        domElements.oddCompilerGrid.on('keyup', 'td .js-OU-input', function (e) {
            bindingEventKeyupForHandicap(e);
        });

        domElements.oddCompilerGrid.on('click', 'td .js-plusIcon', function (e) {
            plusIconClick(e);
        });

        domElements.oddCompilerGrid.on('click', 'td .js-deleteIcon', function (e) {
            deleteOdds(e);
        });

        domElements.oddCompilerGrid.on('focusout', 'td .input_field', function (e) {
            if (triggerByProgramming) {
                triggerByProgramming = false;
            } else {
                saveFromInput(e);
            }
        });

        domElements.oddCompilerGrid.on('change', 'td .js-teamhandicap', function (e) {
            saveFromSelect(e);
        });
    };

    function bindingEventKeyupForHandicap(e) {
        var val = e.target.value,
            regex = /^(\-)?\d*\.?([0-9]{1,2})?$/g,
            regex1 = /^(\-)?\d*\.?([0-9]{1,2})?/g;
        var code = e.which;
        // Enter, Escape
        if (code === 13 || code === 27) {
            triggerByProgramming = true;
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
    };

    function bindingEventKeyupForOdds(e) {
        var val = e.target.value,
            regex = /^(\-)?\d*\.?([0-9]{1,2})?$/g,
            regex1 = /^(\-)?\d*\.?([0-9]{1,2})?/g;

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

    return {
        Initialize: function () {
            registerEvents();
        }
    }
}($);

$(function () {
    oddCompiler.Initialize();
})