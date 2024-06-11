var workingShiftCore = function ($) {
    function initCategoryFilter() {
        for (var i = 0; i < workingShiftData.userCategories.length; i++) {
            var category = workingShiftData.userCategories[i];
            category.enable = true;
            category.checked = true;
        }
        $('#userCategories').fanexMultiSelect({
            minWidth: 120,
            listData: workingShiftData.userCategories,
            showFilter: true,
            customCheckAllText: 'All',
            useCustomCheckAllText: true,
            noneSelectedText: 'Select Category'
        });
    }

    function initLocationFilter() {
        for (var i = 0; i < workingShiftData.userLocations.length; i++) {
            var location = workingShiftData.userLocations[i];
            location.enable = true;
            location.checked = true;
        }
        $('#userLocations').fanexMultiSelect({
            minWidth: 120,
            listData: workingShiftData.userLocations,
            showFilter: true,
            customCheckAllText: 'All',
            useCustomCheckAllText: true,
            noneSelectedText: 'Select Location'
        });
    }
    function initDepartmentFilter() {
        for (var i = 0; i < workingShiftData.departments.length; i++) {
            var department = workingShiftData.departments[i];
            department.enable = true;
            department.checked = true;
        }
        $('#department').fanexMultiSelect({
            minWidth: 120,
            listData: workingShiftData.departments,
            showFilter: true,
            customCheckAllText: 'All',
            useCustomCheckAllText: true,
            noneSelectedText: 'Select Location'
        });
    }

    function initWeeklySectionFilter() {
        for (var i = 0; i < workingShiftData.sections.length; i++) {
            var section = workingShiftData.sections[i];
            section.enable = true;
            section.checked = true;
        }
        $('#section').fanexMultiSelect({
            listData: workingShiftData.sections,
            showFilter: true,
            minWidth: 120,
            customCheckAllText: 'All',
            useCustomCheckAllText: true,
            noneSelectedText: 'Select sections'
        });
    }

    function initSingleSectionOption(sections) {
        if (!workingShiftData.isHostWeeklySection) {
            var currentSelectedSection = (workingShiftData.currentSectionId === '') ? '0' : workingShiftData.currentSectionId;
            sections = sections.filter(function (element) {
                return element.value.toString() === currentSelectedSection;
            });
        }

        var html = [];
        html.push('<option value="" disabled selected>Choose your option</option>');
        sections.forEach(function (element) {
            html.push('<option value="' + element.value + '">' + element.text + '</option>');
        });

        $('#singleSection').html(html.join(''));
        $('#singleSection').material_select();
    }

    function initExcludeClosedAccountFilter() {
        $('#excludeClosedAccount').prop('checked', true);
    }

    function disableWeeklyFilterInOrderMode() {
        $('.js-multiSectionWrapper').hide();
        $('.js-singleSectionWrapper').show();
        $('.js-datePickerWrapper').addClass('filter-disable');
        $('.js-employeeInputWrapper').addClass('filter-disable');
        $('.js-multiCategoryWrapper').addClass('filter-disable');
        $('.js-multiLocationWrapper').addClass('filter-disable');
        $('.js-multiDepartmentWrapper').addClass('filter-disable');
        $('.js-excludeClosedAccountsWrapper').addClass('filter-disable');
        $('#btn-open-upload').addClass('disabled');
        $('#exportExcelDropdown').addClass('disabled');
        $('#btnExportAllWorkingShift').addClass('disabled');
        $('#settingTimeZoneDropdown').addClass('disabled');
        $('.prev-week').addClass('disabled');
        $('.next-week').addClass('disabled');
    }

    function enableWeeklyFilter() {
        $('.js-multiSectionWrapper').show();
        $('.js-singleSectionWrapper').hide();
        $('.js-datePickerWrapper').removeClass('filter-disable');
        $('.js-employeeInputWrapper').removeClass('filter-disable');
        $('.js-multiCategoryWrapper').removeClass('filter-disable');
        $('.js-multiLocationWrapper').removeClass('filter-disable');
        $('.js-multiDepartmentWrapper').removeClass('filter-disable');
        $('.js-excludeClosedAccountsWrapper').removeClass('filter-disable');
        $('#btn-open-upload').removeClass('disabled');
        $('#exportExcelDropdown').removeClass('disabled');
        $('#btnExportAllWorkingShift').removeClass('disabled');
        $('#settingTimeZoneDropdown').removeClass('disabled');
        $('.prev-week').removeClass('disabled');
        $('.next-week').removeClass('disabled');
    }

    function exitCopyShiftMode() {
        if (workingShiftData.isInCopyMode) {
            workingShiftData.isInCopyMode = false;
            workingShiftData.copyWholeShifts = null;
            $('.staff-row').removeClass('is-focus');
            $('.more-action-function').show();
            $('.js-pasteWholeShifts').hide();
        }
        if (workingShiftData.isInCopyModeSingle) {
            workingShiftData.isInCopyModeSingle = false;
            $('.day').removeClass('is-copied').removeClass('is-focus');
        }
    }

    return {
        InitCategoryFilter: initCategoryFilter,
        InitLocationFilter: initLocationFilter,
        InitDepartmentFilter: initDepartmentFilter,
        InitWeeklySectionFilter: initWeeklySectionFilter,
        InitSingleSectionOption: initSingleSectionOption,
        InitExcludeClosedAccountFilter: initExcludeClosedAccountFilter,
        DisableWeeklyFilterInOrderMode: disableWeeklyFilterInOrderMode,
        EnableWeeklyFilter: enableWeeklyFilter,
        ExitCopyShiftMode: exitCopyShiftMode
    };
}(jQuery);

// -------- Filter ------------- //
jQuery(document).ready(function ($) {
    var queryDate = common.urlParam('date');
    var currentDate = queryDate || currentDateServer;
    var today = moment(currentDate);
    var mondayofCurrentWeek = today.startOf('isoweek');
    var fourlatestWeeks = moment(mondayofCurrentWeek.subtract(28, "days"));
    var maxQueryTime = moment(currentDate).diff(fourlatestWeeks, 'days');
    function initializeFilters() {
        $('#employee').val('');
        workingShiftCore.InitCategoryFilter();
        workingShiftCore.InitLocationFilter();
        workingShiftCore.InitDepartmentFilter();
        workingShiftCore.InitExcludeClosedAccountFilter();
        workingShiftCore.InitWeeklySectionFilter();
        workingShiftCore.InitSingleSectionOption(workingShiftData.sections);
    }
    initializeFilters();

    $(document)
        .on('mousemove', '.ui-datepicker-calendar tr', function () {
            $(this).find('td a').addClass('ui-state-hover');
        })
        .on('mouseleave', '.ui-datepicker-calendar tr', function () {
            $(this).find('td a').removeClass('ui-state-hover');
        });

    $('#hidden-weekpicker').weekpicker({
        minDate: -maxQueryTime,
        firstDay: 1,
        currentText: currentDate,
        onSelect: function (dateText, startDateText, startDate, endDate, inst) {
            var startDateString = moment(startDate).format('MMM DD, YYYY');
            var endDateString = moment(endDate).format('MMM DD, YYYY');
            $('.weekpicker').data('startDate', startDateString);
            $('.weekpicker').data('endDate', endDateString);
            $('.weekpicker').val(startDateString + ' - ' + moment(endDate).add(1, 'days').format('MMM DD, YYYY'));
        }
    });

    //$('.datepicker').datepicker({
    //    firstDay: 1,
    //    currentText: currentDate
    //}).val(currentDate);

    $('.weekpicker').on('click', function () {
        $('#hidden-weekpicker').focus();
    });

    $('.icon-calendar').on('click', function () {
        $('#hidden-weekpicker').focus();
    });

    workingShiftWeekly.init();

    $('.btn-search').on('click', function () {
        var startTime = moment();
        workingShiftCore.ExitCopyShiftMode();
        $('#first_name2').val('');

        var sectionIds = [];
        if (workingShiftData.isInOrderMode) {
            sectionIds.push($('#singleSection').val());
        } else {
            sectionIds = $('#section').fanexMultiSelect('getItems', true);
        }
        if (sectionIds.length === 0) {
            Swal.fire({
                html: 'Please select at least one section',
                type: 'error',
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            });
            return;
        }
        var userCategoryIds = $('#userCategories').fanexMultiSelect('getItems', true);
        if (userCategoryIds.length === 0) {
            Swal.fire({
                html: 'Please select at least one category',
                type: 'error',
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            });
            return;
        }

        var userLocationCodes = $('#userLocations').fanexMultiSelect('getItems', true);
        if (userLocationCodes.length === 0) {
            Swal.fire({
                html: 'Please select at least one location',
                type: 'error',
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            });
            return;
        }
        var departmentIds = $('#department').fanexMultiSelect('getItems', true);
        if (departmentIds.length === 0) {
            Swal.fire({
                html: 'Please select at least one department',
                type: 'error',
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            });
            return;
        }

        var offsetNumber = parseInt($("input[name='TimeZone']:checked").val());
        var isIFrame = $('.body-content').attr('data-isIFrame') === 'True';
        var searchWorkingShiftWeeklyFn = workingShiftService.SearchWorkingShiftWeekly;
        if (isIFrame) {
            searchWorkingShiftWeeklyFn = workingShiftService.SearchWorkingShiftWeeklyIFrame;
        }
        var toDate = moment($('.weekpicker').data('endDate')).add(1, 'days').format('MMM DD, YYYY');
        var model = {
            fromDate: $('.weekpicker').data('startDate'),
            toDate: toDate,
            user: $('#employee').val().trim(),
            section: sectionIds.join(','),
            category: userCategoryIds.join(','),
            location: userLocationCodes.join(','),
            departmentId: departmentIds.join(','),
            excludeClosedAccount: $('#excludeClosedAccount').prop('checked'),
            TimeZoneOffset: offsetNumber
        };
        $('.loading-process').css('display', 'inline');

        searchWorkingShiftWeeklyFn(model, function (result) {
            $('.loading-process').css('display', 'none');
            workingShiftWeekly.render(result);
            onResize();
        }).error(function (error) {
            Raven.captureMessage(JSON.stringify(error));
            $('.loading-process').css('display', 'none');
        });
    });

    $('#ExcelFile').on('change', function () {
        if (!$(this).val()) {
            $(this).replaceWith($(this).val('').clone(true));
            $('#fileWrapper').val('');
        }
    });

    function exportWorkingShiftToExcel(user,userDepartmentIds, sectionIds, userLocationCodes, userCategoryIds, exportType, isIFrame) {
        workingShiftCore.ExitCopyShiftMode();
        
        var offsetNumber = parseInt($("input[name='TimeZone']:checked").val());
        var excludeClosedAccount = $('#excludeClosedAccount').prop('checked');
        var toDate = moment($('.weekpicker').data('endDate')).add(1, 'days').format('MMM DD, YYYY');
        var model = {
            fromDate: $('.weekpicker').data('startDate'),
            toDate: toDate,
            user: user,
            section: sectionIds.join(','),
            category: userCategoryIds.join(','),
            location: userLocationCodes.join(','),
            DepartmentId: userDepartmentIds.join(','),
            excludeClosedAccount: $('#excludeClosedAccount').prop('checked'),
            TimeZoneOffset: parseInt($("input[name='TimeZone']:checked").val()),
            exportType: exportType,
            isFrame: isIFrame
        };
        var url = common.ResolveRootUrl('api/weekly-working-shift/export');
        window.location.href = url + '?' + $.param(model);
    }

    $('#btnExportAllWorkingShift').on('click', function () {
        var sectionIds = [];
        if (workingShiftData.isInOrderMode) {
            sectionIds.push($('#singleSection').val());
        } else {
            sectionIds = $('#section').fanexMultiSelect('getItems', true);
        }
        if (sectionIds.length === 0) {
            Swal.fire({
                html: 'Please select at least one section',
                type: 'error',
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            });
            return;
        }
        var userCategoryIds = $('#userCategories').fanexMultiSelect('getItems', true);
        if (userCategoryIds.length === 0) {
            Swal.fire({
                html: 'Please select at least one category',
                type: 'error',
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            });
            return;
        }

        var userLocationCodes = $('#userLocations').fanexMultiSelect('getItems', true);
        if (userLocationCodes.length === 0) {
            Swal.fire({
                html: 'Please select at least one location',
                type: 'error',
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            });
            return;
        }
        var userDepartmentIds = $('#department').fanexMultiSelect('getItems', true);
        if (userDepartmentIds.length === 0) {
            Swal.fire({
                html: 'Please select at least one department',
                type: 'error',
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            });
            return;
        }
        var isIFrame = $('#btnExportAllWorkingShift').attr('data-isIFrame') === 'True';
        var user = $('#employee').val().trim();
        exportWorkingShiftToExcel(user, userDepartmentIds, sectionIds, userLocationCodes, userCategoryIds, 1, isIFrame);
    });

    $('#btnExportAllWorkingShiftSMSLocal').on('click', function () {
        var sectionIds = [1, 3, 5, 6, 7, 8, 13, 14, 18, 20, 21, 24, 27, 26, 31,32,33,34,35,36];
        var deparmentIds = [9, 14, 11, 15, 12, 13, 8, 3, 2, 10, 6, 1, 7, 0,16,4,5];
        var userCategoryIds = [1];
        var userLocationCodes = ['SMS', 'WAH HCMC'];
        var isIFrame = $('#btnExportAllWorkingShiftSMSLocal').attr('data-isIFrame') === 'True';
        exportWorkingShiftToExcel('', deparmentIds, sectionIds, userLocationCodes, userCategoryIds, 2, isIFrame);
    });

    $('#btnExportAllWorkingShiftLALST').on('click', function () {
        var sectionIds = [2, 15]; // Live Assistant-VN + LST
        var deparmentIds = [9, 14, 11, 15, 12, 13, 8, 3, 2, 10, 6, 1, 7, 0,16,4,5];
        var userCategoryIds = [1];
        var userLocationCodes = ['SMS', 'WAH HCMC'];
        var isIFrame = $('#btnExportAllWorkingShiftLALST').attr('data-isIFrame') === 'True';
        exportWorkingShiftToExcel('', deparmentIds, sectionIds, userLocationCodes, userCategoryIds, 3, isIFrame);
    });

    $('#btnExportAllWorkingShiftSMSExpat').on('click', function () {
        var sectionIds = [1, 2, 3, 5, 13, 14, 15, 18, 20, 31, 32, 35, 36];
        var deparmentIds = [9, 14, 11, 15, 12, 13, 8, 3, 2, 10, 6, 1, 7, 0,16,4,5];
        var userCategoryIds = [2];
        var userLocationCodes = ['SMS', 'WAH HCMC'];
        var isIFrame = $('#btnExportAllWorkingShiftSMSExpat').attr('data-isIFrame') === 'True';
        exportWorkingShiftToExcel('', deparmentIds, sectionIds, userLocationCodes, userCategoryIds, 4, isIFrame);
    });

    function onResize() {
        $('#sticky-filter').width($('.table-body.weekly').width() + 2);
        $('.table-body').css({ 'margin-top': $('.sticky-filter').outerHeight() });
    }

    $(window).on('resize', function () {
        onResize();
    });

    $(".btn-search").click();
    $(".btn-upload").on('click', function () {
        workingShiftCore.ExitCopyShiftMode();
    })
});