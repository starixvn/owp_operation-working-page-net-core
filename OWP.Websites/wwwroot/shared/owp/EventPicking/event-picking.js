/* global common, eventPickingFactory, eventInformationFactory, majorBettypes */
'use strict';

var eventPicking = function ($) {
    var domElements = {
        date: $('form.filter #Date'),
        excelBtn: $('form.filter .js-btnExcel'),
        formFilter: $('form.filter')
    };

    function initializeDate() {
        domElements.date.datepicker({
            dateFormat: 'M d, yy',
            minDate: -1
        });
    }

    function registerEvent() {
        
        $('.icon-calendar').click(function () {
            domElements.date.focus();
        });

        
        domElements.excelBtn.on('click', function () {
            $('.loading-process').css('display', 'inline');
            var model = domElements.date[0].value;
            debugger
            eventPickingFactory.ExportEventPickingExcel(model, function () {
                $('.loading-process').css('display', 'none');
            });
        });
    }

    return {
        Initialize: function () {
            initializeDate();
            registerEvent();
        }
    };
}($);

$(document).ready(function () {
    eventPicking.Initialize();
});