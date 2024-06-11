/* global common */
/* exported taskListFactory */
'use strict';

var taskListFactory = (function ($) {
    var urls = {
        GetDuty: "TaskScheduling/TaskList/TaskListTable",
        GetYesterdayEvent: "TaskScheduling/TaskList/YesterdayEventTable",
        UpdateNoteView: "TaskScheduling/TaskList/UpdateMatchNoteStatus",
        UpdateTradeReadyStatus: "TaskScheduling/TaskList/UpdateReadyTradeStatus"
    };

    function closeOpenedDialogs() {
        var activeDialogs = $(".ui-dialog:visible").find('.ui-dialog-content');
        activeDialogs.dialog('close');
    }

    function eventViewLiveDetail($grid, callback) {
        $grid.find('.js-openModalLiveDetail').on('click', function () {
            var matchId = $(this).attr('match-id');
            common.modalLiveDetail(matchId, callback);
        });
    }

    function eventViewMatchNote($grid) {
        $grid.find('.js-viewMatchNote').on('click', function () {
            var td = $(this).closest('td'),
                hasNewNote = td.find('input.js-newNote').prop('checked');

            if (hasNewNote) {
                var matchId = parseInt(td.find('input.js-newNote').data('matchid'), 10);
                taskListFactory.UpdateNoteViewStatus(matchId).success(function (result) {
                    if (result.IsSuccessful) {
                        td.find('.pulse-container').css('display', 'none');
                    }
                });
            }

            registerEventShowNotes($grid, $(this), td);
        });
    }

    function onCheckMatchTradeReadyStatus($grid) {
        $grid.find('.checkbox').on('change', function () {
            $('.loading-process').css('display', 'inline');
            const _this = $(this);
            const matchId = _this.data('matchid');
            taskListFactory.UpdateTradeReadyStatus(matchId).success(function (result) {
                if (result) {
                    const readyCheckIcon = document.createElement('i');
                    readyCheckIcon.className = 'icon-success icon-green ft-22 tooltip-tipsy';
                    readyCheckIcon.setAttribute('original-title', 'Ready at ' + result);
                    readyCheckIcon.setAttribute('matchid', matchId);
                    _this[0].parentNode.replaceWith(readyCheckIcon);
                    const readyIcon = $grid.find('.tooltip-tipsy[matchid=' + matchId + ']');
                    readyIcon.tipsy({ html: true, gravity: 'e' });
                    $('.loading-process').css('display', 'none');
                }
            });
        });
    }

    function registerEventShowNotes($grid, element, cell) {
        closeOpenedDialogs();

        var hasNewNote = cell.find('input.js-newNote').prop('checked');

        var owpMatchId = element.data('owpmatchid'),
            matchNotes = $(".js-matchNotes[data-owpmatchid='" + owpMatchId + "']"),
            positionDisplayNotes = $(".js-positionDisplayNotes[data-owpmatchid='" + owpMatchId + "']");

        if (hasNewNote) {
            cell.find('input.js-newNote').prop('checked', false);
        } else {
            matchNotes.find('p').css('font-weight', 'normal');
        }

        matchNotes.dialog({
            autoOpen: false,
            position: { of: positionDisplayNotes, my: 'left top', at: 'left bottom', collision: 'none' },
            resizable: false,
            draggable: false,
            closeText: 'x',
            width: '200px',
            minHeight: 0
        });

        matchNotes.dialog("open");
        matchNotes.parent().addClass('tasklist-notes');
        matchNotes.parent().draggable();
    }

    function registerEventHoverTableRow($grid) {
        $grid.find("tr").hover(function () {
            var rowGroup = $(this).data("row-group");
            $(".row-group" + rowGroup).addClass("hover");
        }).on("blur mouseleave", function () {
            var rowGroup = $(this).data("row-group");
            $(".row-group" + rowGroup).removeClass("hover");
        });
    }

    function initializeTooltip($grid) {
        $('div.tipsy').remove();

        $grid.find('.tooltip-tipsy').not('.icon-success').tipsy({
            html: true,
            gravity: 'w'
        });

        $grid.find('.icon-success.tooltip-tipsy').tipsy({
            html: true,
            gravity: 'e'
        });
    }

    function destroyDialogs() {
        var existDialogs = $(".ui-dialog").find('.ui-dialog-content');
        existDialogs.dialog('destroy');
    }

    function onCheckHideCompletedMatches($grid) {
        $('#hideCompletedMatchesCheckBox').on('change', function () {
            if (this.checked) {
                $grid.find('.main-completed').addClass('hide');
            } else {
                $grid.find('.main-completed').removeClass('hide');
            }
        });
    }

    return {
        GetDutyList: function () {
            return $.ajax({
                url: common.ResolveRootUrl(urls.GetDuty + "?isHideCompletedMatches=" + $("#hideCompletedMatchesCheckBox")[0].checked),
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        GetYesterdayEvent: function () {
            return $.ajax({
                url: common.ResolveRootUrl(urls.GetYesterdayEvent),
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        UpdateNoteViewStatus: function (matchId) {
            var url = urls.UpdateNoteView + "?matchid=" + matchId;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                method: 'POST',
                error: common.HandleAjaxError
            });
        },
        UpdateTradeReadyStatus: function (matchId) {
            var url = urls.UpdateTradeReadyStatus + "?matchid=" + matchId;
            return $.ajax({
                url: common.ResolveRootUrl(url),
                method: 'POST',
                error: common.HandleAjaxError
            });
        },

        EventViewMatchNote: eventViewMatchNote,

        EventViewLiveDetail: eventViewLiveDetail,

        RegisterEventHoverTableRow: registerEventHoverTableRow,

        InitializeTooltip: initializeTooltip,

        DestroyDialogs: destroyDialogs,

        CloseOpenedDialogs: closeOpenedDialogs,

        CheckMatchTradeReadyStatus: onCheckMatchTradeReadyStatus,

        CheckHideCompletedMatches: onCheckHideCompletedMatches
    };
})(jQuery);