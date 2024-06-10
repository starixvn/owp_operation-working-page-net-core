/* global common, filterFactory, filterTypes */
/* exported filterManagement */
"use strict";

var filterManagement = function ($) {
    var rangeDate = {
        Today: 0,
        Tomorrow: 1,
        Yesterday: 2,
        CustomDate: 3
    };

    var targetObject = {
        League: 'EventManagement/LeagueManagement/GetLeague?selectTop=50',
        Employee: 'EventManagement/Filter/GetUserBySuggestion?1=1'
    };

    function validate() {
        var errorMessages = [],
            errorMessage = '';

        errorMessage = validateMaxBet();
        if (errorMessage.length > 0) {
            errorMessages.push(errorMessage);
        }

        errorMessage = validateSpread();
        if (errorMessage.length > 0) {
            errorMessages.push(errorMessage);
        }

        errorMessage = validateKOTime();
        if (errorMessage.length > 0) {
            errorMessages.push(errorMessage);
        }

        return errorMessages;
    }

    function validateKOTime() {
        var from = $('#KOTimeFrom'),
            to = $('#KOTimeTo'),
            date = $("#Date").val(),
            errorMessage = '';

        var timeFrom = new Date(Date.parse(date + " " + from.val(), "M d, yy hh:mm tt")),
            timeTo = new Date(Date.parse(date + " " + to.val(), "M d, yy hh:mm tt"));
        if (timeFrom > timeTo) {
            from.addClass('invalid');
            to.addClass('invalid');
            errorMessage = $('#errorKOTime').val();
        } else {
            from.removeClass('invalid');
            to.removeClass('invalid');
        }

        return errorMessage;
    }

    function validateMaxBet() {
        var $fromMaxBet = $('#maxbetFrom'),
            $toMaxBet = $('#maxbetTo'),
            fromMaxBet = parseFloat($fromMaxBet.val()),
            toMaxBet = parseFloat($toMaxBet.val()),
            errorMessage = '';

        if ((fromMaxBet >= 0 && !toMaxBet) || (!fromMaxBet && toMaxBet >= 0) || (fromMaxBet > toMaxBet)) {
            $fromMaxBet.addClass('invalid');
            $toMaxBet.addClass('invalid');
            errorMessage = $('#errorMaxBet').val();
        } else {
            $fromMaxBet.removeClass('invalid');
            $toMaxBet.removeClass('invalid');
        }

        return errorMessage;
    }

    function validateSpread() {
        var $fromSpread = $('#spreadFrom'),
            $toSpread = $('#spreadTo'),
            fromSpread = parseFloat($fromSpread.val()),
            toSpread = parseFloat($toSpread.val()),
            errorMessage = '';

        if ((fromSpread >= 0 && !toSpread) || (!fromSpread && toSpread >= 0) || (fromSpread > toSpread)) {
            $fromSpread.addClass('invalid');
            $toSpread.addClass('invalid');
            errorMessage = $('#errorSpread').val();
        } else {
            $fromSpread.removeClass('invalid');
            $toSpread.removeClass('invalid');
        }

        return errorMessage;
    }

    function addFilter(fieldId) {
        filterFactory.GetConditionControl(fieldId).success(function (response) {
            $('.js-controlFilter').append(response);
            if (fieldId === filterTypes.League) {
                $('.js-league').owpSearchAutocomplete({
                    url: common.ResolveRootUrl(targetObject.League)
                });
            } else if (fieldId === filterTypes.Assignee) {
                $('.js-employee').owpSearchAutocomplete({
                    url: common.ResolveRootUrl(targetObject.Employee),
                    dataMapping: { Value: 'UserId', Label: 'Username' }
                });
            }

            registerEventFilter();
        });
    }

    function removeFilter(fieldId) {
        $('form.filter').find('#field_' + fieldId).remove();
    }

    function registerAddMoreFilter() {
        $('.js-moreFilter input[type=checkbox]').on('click', function () {
            var value = $(this).data('value'),
                checked = $(this).prop('checked');

            if (checked) {
                addFilter(value);
            } else {
                removeFilter(value);
            }
        });
    }

    function registerEventFilter() {
        $('.js-time').inputmask('hh:mm t');
        $('.maxbet').inputmask('decimal');
        $('.spread').inputmask('decimal');
        $('select').material_select();
        $('ul.select-dropdown.multiple-select-dropdown li input').each(function () {
            if (!$(this).hasClass('filled-in')) {
                $(this).addClass('filled-in');
            }
        });

        $('.dropdown-button').dropdown();
    }

    function registerEventDate() {
        $('#customDate').hide();
        $('#Date').datepicker({
            minDate: -30,
            dateFormat: 'M d, yy'
        });
        $('.icon-calendar').click(function () {
            $('#Date').focus();
        });

        $('select.js-eventDate').change(function () {
            var id = parseInt($(this).val());
            if (id === rangeDate.CustomDate) {
                $('#customDate').show();
            } else {
                $('#customDate').hide();
            }
        });
    }

    function registerEvent() {
        registerAddMoreFilter();

        registerEventSaveFilter();

        $('#js-moreFilter').on('click', function (event) { event.stopPropagation(); });
    }

    function registerEventSaveFilter() {
        $('#js-btnSaveFilter').on('click', function () {
            var filterId = parseInt($('#editingFilterId').val());

            if (filterId === 0) {
                $('#js-modalConfirmSave').openModal({
                    dismissible: false,
                    ready: function () {
                        $(":input, a").attr("tabindex", "-1");
                        $('.modal-action').focus();
                    },
                    complete: function () {
                        $(":input, a").removeAttr("tabindex");
                        $('.lean-overlay').each(function () {
                            $(this).remove();
                        });
                    }
                });
            } else {
                updateFilter(filterId)
            }
        });
    }

    function updateFilter(filterId) {
        filterFactory.ShowLoadingProcess();
        var errorMessages = filterManagement.validate();

        if (errorMessages.length === 0) {
            var valueOfText = filterManagement.getValueOfTextInput(),
                valueOfSelect = filterManagement.getValueOfSelect(),
                filterDetails = $.merge(valueOfText, valueOfSelect);

            var model = {
                FilterId: filterId,
                Details: filterDetails
            };

            filterFactory.Update(model).success(function (result) {
                if (!result.IsSuccessful) {
                    Swal.fire({
                        html: result.Message,
                        type: 'error',
                        animation: false,
                        heightAuto: false,
                        scrollbarPadding: false,
                        confirmButtonText: 'OK'
                    });
                } else {
                    $('#js-advanceSearch').css('transform', '');
                    $('#js-advanceSearch').empty();

                    //set choose filter
                    var currentFilter = $("#js-filterDropdown").find(".js-chooseFilter[data-id='" + filterId + "']");
                    currentFilter.trigger('click');
                    filterFactory.HideLoadingProcess();
                    Swal.fire({
                        html: 'Saved successfully.',
                        type: 'success',
                        animation: false,
                        heightAuto: false,
                        scrollbarPadding: false,
                        confirmButtonText: 'OK'
                    });
                }
            });
        } else {
            filterFactory.HideLoadingProcess();
            common.modalErrorMessage(errorMessages);
        }
    }

    function getValueOfTextInput() {
        $('form.js-advanceFilter #leagueIds').val($('.js-league').owpSearchAutocomplete('getDataSelected'));
        $('form.js-advanceFilter #assigneeIds').val($('.js-employee').owpSearchAutocomplete('getDataSelected'));

        var filterDetails = [],
            filters = $('#js-advanceSearch .js-controlFilter .input-field');
        for (var i = 0; i < filters.length; i++) {
            var values = $(filters[i]).find('input[name="value"]'),
                id = $(filters[i]).data('id');
            for (var j = 0; j < values.length; j++) {
                var value = $(values[j]).val();
                if (value.length > 0) {
                    if (id === filterTypes.MaxBet) {
                        value = value * 1000;
                    }
                    if (id === filterTypes.Spread) {
                        value = value / 100;
                    }
                    filterDetails.push({
                        FieldId: $(filters[i]).data('id'),
                        Operator: $(values[j]).data('operator'),
                        Value: value
                    });
                }
            }
        }

        return filterDetails;
    }

    function getValueOfSelect() {
        var filterDetails = [],
            filters1 = $('#js-advanceSearch .js-controlFilter select');
        for (var i = 0; i < filters1.length; i++) {
            var value = $(filters1[i]).val().toString(),
                id = parseInt($(filters1[i]).data('id'));
            if (id === filterTypes.EventDate && parseInt(value) === rangeDate.CustomDate) {
                filterDetails.push({
                    FieldId: id,
                    Operator: $(filters1[i]).data('operator'),
                    Value: $("#Date").val()
                });
            } else if (value.length > 0) {
                filterDetails.push({
                    FieldId: id,
                    Operator: $(filters1[i]).data('operator'),
                    Value: value
                });
            }
        }

        return filterDetails;
    }

    return {
        Initialize: function () {
            registerEvent();

            registerEventFilter();

            registerEventDate();
        },
        getValueOfTextInput: getValueOfTextInput,
        getValueOfSelect: getValueOfSelect,
        validate: validate,
        TargetObject: targetObject
    };
}($);