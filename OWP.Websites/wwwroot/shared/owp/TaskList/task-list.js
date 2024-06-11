/* global taskListFactory */
'use strict';

var taskListForToday = (function ($) {
    var $selectTime, $taskListGrid, $yesterdayEventGrid, $todayTab, $yesterdayTab;

    var intervalCountDown = null;

    function countDown(maxNumber) {
        if (intervalCountDown) {
            clearInterval(intervalCountDown);
        }

        intervalCountDown = setInterval(function () {
            maxNumber = maxNumber - 1;
            $('.select-dropdown').val(maxNumber + 's');

            if (maxNumber === 0) {
                getDutyTodayList();
            }
        }, 1000);
    }

    function getDutyTodayList() {
        clearInterval(intervalCountDown);
        $taskListGrid.find('.dropdown-button').dropdown('close');
        $('.loading-process').css('display', 'inline');

        taskListFactory.GetDutyList().success(function (result) {
            taskListFactory.DestroyDialogs();

            $taskListGrid.html(result);
        }).complete(function () {
            $taskListGrid.find('.dropdown-button').dropdown({ belowOrigin: true });

            setTimeout(function () {
                taskListFactory.EventViewMatchNote($taskListGrid);
                taskListFactory.EventViewLiveDetail($taskListGrid, function () {
                    getDutyTodayList();
                });
                taskListFactory.RegisterEventHoverTableRow($taskListGrid);
                taskListFactory.InitializeTooltip($taskListGrid);
                taskListFactory.CheckMatchTradeReadyStatus($taskListGrid);
                taskListFactory.CheckHideCompletedMatches($taskListGrid);
            }, 100);

            var selectedSecond = parseInt($selectTime.val(), 10);
            $('.select-dropdown').val(selectedSecond + 's');
            countDown(selectedSecond);

            if ($taskListGrid.find("tr").length >= 2){
                $('table.today-event-table').floatThead({ top: 55, zIndex: 900 });
                $('table.today-event-table').floatThead('reflow');
            }
            

            $('.loading-process').css('display', 'none');
        });
    }

    function getYesterdayEvent() {
        $yesterdayEventGrid.find('.dropdown-button').dropdown('close');
        $('.loading-process').css('display', 'inline');

        taskListFactory.GetYesterdayEvent().success(function (result) {
            taskListFactory.DestroyDialogs();

            $yesterdayEventGrid.html(result);
        }).complete(function () {
            $yesterdayEventGrid.find('.dropdown-button').dropdown({ belowOrigin: true });

            setTimeout(function () {
                taskListFactory.EventViewMatchNote($yesterdayEventGrid);
                taskListFactory.EventViewLiveDetail($yesterdayEventGrid, function () {
                    getYesterdayEvent();
                });
                taskListFactory.RegisterEventHoverTableRow($yesterdayEventGrid);
                taskListFactory.InitializeTooltip($yesterdayEventGrid);
            }, 100);

            $('table.yesterday-event-table').floatThead({ top: 55 });
            $('table.yesterday-event-table').floatThead('reflow');

            $('.loading-process').css('display', 'none');
        });
    }

    function registerEvents() {
        $('#iconRefresh').on('click', function () {
            clearInterval(intervalCountDown);
            var selectedSecond = parseInt($selectTime.val(), 10);

            if (selectedSecond > 0) {
                $('.select-dropdown').val(selectedSecond + 's');
                countDown(selectedSecond);
            }
        });

        $selectTime.on('change', function () {
            clearInterval(intervalCountDown);
            var selectedSecond = parseInt($selectTime.val(), 10);

            if (selectedSecond > 0) {
                countDown(selectedSecond);
            }
        });

        $todayTab.on('click', function () {
            var isActive = $(this).data('active');

            if (!isActive && parseInt($selectTime.val(), 10) > 0) {
                $todayTab.data('active', true);
                var selectedSecond = parseInt($('.select-dropdown').val(), 10);
                countDown(selectedSecond);
            }
        });

        $yesterdayTab.on('click', function () {
            var viewed = $(this).data('viewed');
            $todayTab.data('active', false);
            clearInterval(intervalCountDown);

            if (!viewed) {
                getYesterdayEvent();
                $(this).data('viewed', true);
            }
        });
    }

    function initModule() {
        $selectTime = $('#selectTime');
        $taskListGrid = $('#todayEventTableBody');
        $yesterdayEventGrid = $('#yesterdayEventTableBody');
        $todayTab = $('#tab-tasklist-today');
        $yesterdayTab = $('#tab-tasklist-yesterday');
        registerEvents();
        getDutyTodayList();
    }

    return {
        initModule: initModule
    };
}(jQuery));

$(function () {
    taskListForToday.initModule();
});