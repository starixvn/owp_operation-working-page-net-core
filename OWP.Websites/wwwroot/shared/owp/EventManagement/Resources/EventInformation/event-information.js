/* global common, eventInformationFactory, filterManagement, filterFactory, tasklistFactory */
"use strict";

var eventInformation = function ($) {
    var lastModel;
    var domElements = {
        applyBtn: $('.js-btnApply'),
        advancedSearchBtn: $('.js-advanceSearchButton'),
        formFilter: $('form.filter'),
        itemperpageOption: $('.js-pagination .js-itemperpageOption'),
        filterDropdown: $("#js-filterDropdown"),
        wrapperTableEventInformation: $('.js-tableEventInformation'),
        updateBtn: $('.js-btnUpdate'),
        advancedSearchArea: $('#js-advanceSearch'),
        fieldsSearch: $('.js-inputSearchLive'),
        pagination: $('.js-pagination'),
        searchLiveBtn: $('.js-btnSearchLive'),
        showUnreadBtn: $('.form-check-input'),
        tableEventInformationBody: $('.js-tableEventInformation')
    };

    if (filterTypes.SearchDisabled) {
        domElements.applyBtn.attr('disabled', '');
    }

    var pagingData = {
        TotalItems: 50,
        ItemsPerPage: 50,
        CurrentPage: 1,
        CurrentFromItem: 1,
        CurrentToItem: 50
    };

    var liveGameListDataSource = [];
    var hasReadNotePermission = true;
    var liveGameListData = [];

    var dateRange = {
        Today: 0,
        Tomorrow: 1,
        Yesterday: 2,
        CustomDate: 3
    };

    var tableColumns = {
        MatchNo: { Id: 1, Class: '.js-dataMatchNo' },
        KoTime: { Id: 2, Class: '.js-dataKoTime' },
        League: { Id: 3, Class: '.js-dataLeague' },
        MatchName: { Id: 4, Class: '.js-dataMatchName' },
        Ticket: { Id: 5, Class: '.js-ticketName' },
        Remark: { Id: 6, Class: '.js-content-remark' },
        Note: { Id: 7, Class: '.js-dataMatchNotes' },
        MaxbetHDPOU: { Id: 8, Class: '.js-dataMaxbetHDPOU' },
        FullTimeTrader: { Id: 9, Class: '.js-dataFullTimeTrader' },
        FirstHalfTrader: { Id: 10, Class: '.js-dataFirstHalfTrader' },
        Maxbet1x2: { Id: 11, Class: '.js-dataMaxbet1x2' },
        SpecialProduct: { Id: 12, Class: '.js-dataSpecialProduct' },
        LiveAssistant: { Id: 13, Class: '.js-dataLA' },
        TVIPTV: { Id: 14, Class: '.js-dataTVIPTV' },
        Provider: { Id: 15, Class: '.js-dataProvider' }
    }

    function registerChooseFilterEvent() {
        domElements.filterDropdown.on('click', '.js-chooseFilter', function () {
            var id = $(this).data('id'),
                name = $(this).data('name');

            domElements.formFilter.find('#FilterId').val(id);

            resetPagingValue();

            $('.js-lableFilter').html(name);
        });
    }

    function registerDefaultFilterEvent() {
        domElements.filterDropdown.on('click', '.js-setDefaultFilter', function () {
            var id = $(this).data('id'),
                isDefaulted = $(this).hasClass('selected');
            if (isDefaulted) {
                eventInformationFactory.RemoveDefaultFilter().success(function (result) {
                    if (result.IsSuccessful) {
                        renderFilterDropdown();
                    } else {
                        common.modalErrorMessage(result.Message);
                    }
                });
            } else {
                eventInformationFactory.SetDefaultFilter(id).success(function (result) {
                    if (result.IsSuccessful) {
                        renderFilterDropdown();
                    } else {
                        common.modalErrorMessage(result.Message);
                    }
                });
            }
        });
    }

    function registerRemoveFilterEvent() {
        domElements.filterDropdown.on('click', '.js-deleteFilter', function () {
            var id = parseInt($(this).data('id')),
                filterId = parseInt(domElements.formFilter.find('#FilterId').val());
            Swal.fire({
                html: 'Are you sure you want to delete this Filter?',
                type: 'question',
                showCancelButton: true,
                reverseButtons: true,
                animation: false,
                heightAuto: false,
                scrollbarPadding: false,
                confirmButtonText: 'OK'
            }).then((result) => {
                if (result.value) {
                    eventInformationFactory.RemoveFilter(id).success(function (result) {
                        if (result.IsSuccessful) {
                            renderFilterDropdown();
                            if (id === filterId) {
                                domElements.formFilter.find('#FilterId').val(0);
                                $('.js-lableFilter').html('Select Filter');
                            }
                        } else {
                            common.modalErrorMessage(result.Message);
                        }
                    }).complete(function () {
                        modalConfirmDeteleFilter.closeModal();
                    });
                }
            });
        });
    }

    function registerEditFilterEvent() {
        domElements.filterDropdown.on('click', '.js-editFilter', function () {
            filterFactory.ShowLoadingProcess();
            var id = parseInt($(this).data('id'));

            eventInformationFactory.RenderEditFilter(id).success(function (response) {
                resetPagingValue();
                domElements.advancedSearchArea.empty();
                domElements.advancedSearchArea.html(response);

                // close dropdown & show search area
                $('.btn-selectFilter').trigger('click');
                setChosenFilterById(id);
                domElements.advancedSearchArea.css('transform', 'translateY(0%)');

                initializeEventFilter();

                ////Init field Event Date
                var fieldEventDate = $('select.js-eventDate');
                fieldEventDate.val($('input.js-current-eventDate').val()).trigger('change');
                fieldEventDate.material_select();

                ////Init field League
                $('.js-league').owpSearchAutocomplete({
                    url: common.ResolveRootUrl(filterManagement.TargetObject.League),
                    loadDefaultItems: 1
                });

                ////Init field Assignee
                $('.js-employee').owpSearchAutocomplete({
                    url: common.ResolveRootUrl(filterManagement.TargetObject.Employee),
                    dataMapping: { Value: 'UserId', Label: 'Username' },
                    loadDefaultItems: 1
                });

                filterFactory.HideLoadingProcess();
            });
        });
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
        filterFactory.ShowLoadingProcess();
        pagingData.CurrentPage = pageNumber;

        renderLiveGameListTable(liveGameListData);

        domElements.pagination.find('.detailtext').text('Showing ' + pagingData.CurrentFromItem + ' to ' + pagingData.CurrentToItem + ' of ' + pagingData.TotalItems + ' entries');
        domElements.fieldsSearch.each(function (index, element) {
            var searchField = $(element);
            var columnId = parseInt(searchField.data('column-id'));
            showIconFilterIsSearching(searchField, columnId);
        });

        filterFactory.HideLoadingProcess();
    }

    function renderLiveGameListTable(dataRaw) {
        taskListFactory.DestroyDialogs();
        var table = new Table();
        var data = domElements.showUnreadBtn.is(":checked")
            ? dataRaw.filter(x => x.TicketInfos.some(i => !i.IsRead))
            : dataRaw;

        var html = table.generate(data);
        $('.js-tableEventInformation').html(html);
        $('.js-tableEventInformation table').floatThead('destroy');
        $('.js-tableEventInformation table').floatThead({ top: 55, zIndex: 1 });
        $('.js-tableEventInformation table').floatThead('reflow');
        $('.js-tableEventInformation table').find('.dropdown-button').dropdown({ belowOrigin: true });
        taskListFactory.EventViewLiveDetail(domElements.tableEventInformationBody, function () {
            getData(lastModel);
        });
        taskListFactory.EventViewMatchNote(domElements.tableEventInformationBody);
        taskListFactory.RegisterEventHoverTableRow(domElements.tableEventInformationBody);
        taskListFactory.InitializeTooltip(domElements.tableEventInformationBody);
        taskListFactory.CheckMatchTradeReadyStatus(domElements.tableEventInformationBody);

        if (data.length > 0) {
            initPagination();
            domElements.pagination.show();
        } else {
            domElements.pagination.hide();
        }

        domElements.fieldsSearch.each(function (index, element) {
            var searchField = $(element);
            var columnId = parseInt(searchField.data('column-id'));
            showIconFilterIsSearching(searchField, columnId);
        });
    }

    function getValueFilterToDirectSearch() {
        var $form = $('form.js-advanceFilter'),
            fromMaxBet = $form.find('input#maxbetFrom').val(),
            toMaxBet = $form.find('input#maxbetTo').val(),
            fromSpread = $form.find('input#spreadFrom').val(),
            toSpread = $form.find('input#spreadTo').val(),
            eventDate;

        switch (parseInt($form.find('select.js-eventDate').val())) {
            case dateRange.Today:
                eventDate = $form.find('input#ToDay').val();
                break;
            case dateRange.Tomorrow:
                eventDate = $form.find('input#Tomorrow').val();
                break;
            case dateRange.Yesterday:
                eventDate = $form.find('input#Yesterday').val();
                break;
            case dateRange.CustomDate:
                eventDate = $form.find('#Date').val();
                break;
        }

        var criteria = {
            EventDate: eventDate,
            FromTime: $form.find('input#KOTimeFrom').val(),
            ToTime: $form.find('input#KOTimeTo').val(),
            FromMaxBet: parseFloat(fromMaxBet) ? parseFloat(fromMaxBet) * 1000 : null,
            ToMaxBet: parseFloat(toMaxBet) ? parseFloat(toMaxBet) * 1000 : null,
            FromSpread: parseFloat(fromSpread) ? parseFloat(fromSpread) / 100 : null,
            ToSpread: parseFloat(toSpread) / 100 ? parseFloat(toSpread) / 100 : null,
            HomeName: $form.find('input#homeName').val(),
            AwayName: $form.find('input#awayName').val(),
            WatchBet: $form.find('input#watchBet').val(),
            Phone: $form.find('input#phone').val(),
            Channel: $form.find('input#channel').val(),
            LeagueIds: $('.js-league').owpSearchAutocomplete('getDataSelected'),
            AssigneeIds: $('.js-employee').owpSearchAutocomplete('getDataSelected'),
            AgentIds: $form.find('select.js-agent').val() ? $form.find('select.js-agent').val().join(',') : null,
            OtherProviderIds: $form.find('select.js-otherProvider').val() ? $form.find('select.js-otherProvider').val().join(',') : null,
            ProviderIds: $form.find('select.js-provider').val() ? $form.find('select.js-provider').val().join(',') : null,
            TVIds: $form.find('select.js-tvCode').val() ? $form.find('select.js-tvCode').val().join(',') : null,
            IsSelected: $form.find('select.js-isSelected').val() ? $form.find('select.js-isSelected').val() : 1,
            MatchNumber: $form.find('input#matchNumber').val(),
            Remark: $form.find('input#remark').val(),
            AssigneeLocation: $form.find('select.js-assigneeLocation').val() ? $form.find('select.js-assigneeLocation').val() : -1,
            MatchStatusIds: $form.find('select.js-matchstatus').val() ? $form.find('select.js-matchstatus').val().join(',') : null,
        };

        return criteria;
    }

    function createShowHideColumnMenu() {
        domElements.updateBtn.show();
        clearFilterInColumn();
        var filterId = parseInt(domElements.formFilter.find('#FilterId').val());

        if (filterId > 0) {
            getDataEventInformation();
        } else if (filterId === -1) {
            getDataEventInformation();
            domElements.advancedSearchArea.css('transform', '');
        }
    }

    function getDataEventInformation() {
        filterFactory.ShowLoadingProcess();
        var filterId = parseInt(domElements.formFilter.find('#FilterId').val()),
            model;
        if (filterId > 0) {
            model = domElements.formFilter.serialize();
        } else if (filterId === -1) {
            model = getValueFilterToDirectSearch();
            model.FilterId = -1;
        }

        lastModel = model;
        getData(lastModel);
    }

    function getData(model) {
        eventInformationFactory.GetDataEventInformation(model).success(function (data) {
            var listMatchInfo = data.listMatchInfo;
            $.each(listMatchInfo, function (index, value) {
                value.KickOffTime = formatTimespan(value.KickOffTime);
                if (value.OtherTime) {
                    value.OtherTime = formatTimespan(value.OtherTime);
                }
            });

            liveGameListDataSource = listMatchInfo;
            liveGameListData = listMatchInfo;
            hasReadNotePermission = data.hasReadNotePermission;
            renderLiveGameListTable(liveGameListData);
            $('.js-tableEventInformation').show();
        }).error(function () {
            filterFactory.HideLoadingProcess();
        }).complete(function () {
            filterFactory.HideLoadingProcess();
        });
    }

    function registerEventSearchDirection() {
        $('#js-btnSearch').on('click', function () {
            if (filterTypes.SearchDisabled) {
                return;
            }

            var errorMessages = filterManagement.validate();

            if (errorMessages.length > 0) {
                common.modalErrorMessage(errorMessages);
            } else {
                domElements.formFilter.find('#FilterId').val(-1);
                $('.js-lableFilter').html('Direct Search');

                createShowHideColumnMenu();
            }
        });
    }

    function setChosenFilter(filterName) {
        var currentFilter = $("#js-filterDropdown").find(".js-chooseFilter[data-name='" + filterName + "']");

        currentFilter.trigger('click');
    }

    function setChosenFilterById(filterId) {
        var currentFilter = $("#js-filterDropdown").find(".js-chooseFilter[data-id='" + filterId + "']");

        currentFilter.trigger('click');
    }

    function renderFilterDropdown(filterName) {
        return eventInformationFactory.RenderFilterDropdown()
            .success(function (response) {
                domElements.filterDropdown.empty().append(response);
            })
            .complete(function () {
                setChosenFilter(filterName);
            });
    }

    function saveFilter() {
        var errorMessages = filterManagement.validate();

        if (errorMessages.length === 0) {
            var valueOfText = filterManagement.getValueOfTextInput(),
                valueOfSelect = filterManagement.getValueOfSelect(),
                filterDetails = $.merge(valueOfText, valueOfSelect);

            var model = {
                Name: $('#js-modalConfirmSave input[name="NameFilter"]').val(),
                IsPublic: $('#js-modalConfirmSave input[name="IsPublicFilter"]').prop('checked'),
                Details: filterDetails
            };

            filterFactory.Create(model).success(function (result) {
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
                    domElements.advancedSearchArea.css('transform', '');

                    renderFilterDropdown(model.Name);
                    Swal.fire({
                        html: 'Your filter has been created.',
                        type: 'success',
                        animation: false,
                        heightAuto: false,
                        scrollbarPadding: false,
                        confirmButtonText: 'OK'
                    });
                }
            });
        } else {
            common.modalErrorMessage(errorMessages);
        }
    }

    function renderCreateFilter() {
        eventInformationFactory.RenderCreateFilter().success(function (response) {
            domElements.advancedSearchArea.html(response);

            initializeEventFilter();
        });
    }

    function initializeEventFilter() {
        filterManagement.Initialize();

        if (filterTypes.SearchDisabled) {
            domElements.advancedSearchArea.find('#js-btnSearch').attr('disabled', '');
        }

        $('#js-btnCancel').click(function () {
            domElements.advancedSearchArea.css('transform', '');

            var filterId = parseInt($('#editingFilterId').val());
            if (filterId !== 0) {
                domElements.advancedSearchArea.empty();
            }
        });

        registerEventSearchDirection();
    }

    function registerEvent() {
        domElements.itemperpageOption.on('change', function () {
            var oldValue = pagingData.ItemsPerPage,
                newValue = parseInt($(this).val());
            if (oldValue !== newValue) {
                pagingData.CurrentPage = 1;
                pagingData.ItemsPerPage = newValue;
                domElements.itemperpageOption.val(pagingData.ItemsPerPage);
                domElements.itemperpageOption.material_select();
                renderLiveGameListTable(liveGameListData);
            }
        });

        domElements.applyBtn.on('click', function () {
            if (filterTypes.SearchDisabled) {
                return;
            }

            resetPagingValue();
            renderDataWhenSearchFromFilter();
        });

        domElements.showUnreadBtn.on('change', function () {
            resetPagingValue();
            renderLiveGameListTable(liveGameListData);
        })

        domElements.advancedSearchBtn.on('click', function () {
            domElements.advancedSearchArea.css('transform', 'translateY(0%)');
            resetPagingValue();

            if (domElements.advancedSearchArea.children().length === 0) {
                renderCreateFilter();
            }
        });

        $('#js-filterDropdown').on('click', function (event) {
            event.stopPropagation();
        });

        $('.js-confirmSave').on('click', function () {
            saveFilter();
        });

        registerDefaultFilterEvent();

        registerRemoveFilterEvent();

        registerChooseFilterEvent();

        registerRefreshEvent();

        registerEditFilterEvent();

        registerEventLiveSearch();

        registerOnScrollEvent();
    }

    function renderDataWhenSearchFromFilter() {
        var filterId = parseInt(domElements.formFilter.find('#FilterId').val());
        if (filterId > 0 || filterId === -1) {
            createShowHideColumnMenu();
        } else {
            var warningMsg = $('input#missingFilter').val();
            common.modalWarningMessage(warningMsg);
        }
    }

    function resetPagingValue() {
        pagingData.CurrentPage = 1;
        pagingData.ItemsPerPage = 50;
        pagingData.TotalItems = 50;
        domElements.itemperpageOption.val(50);
        domElements.itemperpageOption.material_select();
        //domElements.formFilter.find('#CurrentPage').val(1);
        //domElements.formFilter.find('#ItemsPerPage').val(50);
    }

    function registerRefreshEvent() {
        domElements.updateBtn.on('click', function () {
            renderDataWhenSearchFromFilter();
        });
    }

    function registerOnScrollEvent() {
        $(document).on('scroll', function () {
            //Close dialog search live when document scroll
            var dialogSearchLive = $(".ui-dialog:visible").find('.js-dialogSearchLive.ui-dialog-content');
            dialogSearchLive.dialog('close');
        });
    }

    /*********************************
    ***** Live search in column ******
    **********************************/
    function registerEventLiveSearch() {
        clickIconFilterTableEvent();
        inputSearchKeywordEvent();
        clickButtonSearchEvent();
        clickClearAllFilterEvent();
    }

    function clickIconFilterTableEvent() {
        domElements.wrapperTableEventInformation.on('click', '.js-iconFilter', function () {
            taskListFactory.CloseOpenedDialogs();

            var columnId = $(this).data('column-id');
            var dialogSearchLive = $(".js-dialogSearchLive[data-column-id='" + columnId + "']");
            var positionDisplayDialog = $(this);
            var displayPosition = '',
                classDialog = '';
            if (positionDisplayDialog.offset().left > ($(document).scrollLeft() + $(window).width() / 2)) {
                displayPosition = 'right+3';
                classDialog = 'popup-search-left';
            } else {
                displayPosition = 'left-2';
                classDialog = 'popup-search-right';
            }

            dialogSearchLive.dialog({
                autoOpen: false,
                position: { of: positionDisplayDialog, my: displayPosition + ' top+4', at: displayPosition + ' bottom', collision: 'none' },
                resizable: false,
                draggable: false,
                closeText: 'x',
                width: '300px',
                minHeight: 0,
                dialogClass: classDialog
            });

            dialogSearchLive.dialog("open");
            //dialogSearchLive.parent().draggable();
        });
    }

    function inputSearchKeywordEvent() {
        var enterKeyEventHandler = function (e) {
            var keycode = (e.keyCode ? e.keyCode : e.which);
            if (keycode === 13) {
                $(this).parent().find('.js-btnSearchLive').trigger('click');
            }
        };
        var enterKeyDownEventHandler = function (e) {
            var currentValue = e.target.value;
            if (currentValue && currentValue.length > 0) {
                $('.js-clearAllFilter').removeClass('is-disabled');
            } else {
                $(this).parent().find('.js-clearAllFilter').addClass('is-disabled');
            }
        }

        domElements.fieldsSearch.keypress(enterKeyEventHandler);
        domElements.fieldsSearch.keyup(enterKeyDownEventHandler);
    }

    function clickButtonSearchEvent() {
        domElements.searchLiveBtn.on('click', function () {
            filterFactory.ShowLoadingProcess();
            markFilterIsSearching($(this));
            liveGameListData = liveGameListDataSource;

            domElements.fieldsSearch.each(function (index, element) {
                var searchField = $(element);
                var keyword = $.trim(searchField.val().toLowerCase());

                //Note: this is for enhance performance : trim keyword, if === '' return, not search
                if ((keyword === '') && (liveGameListData.length > 0)) {
                    return;
                }
                var columnId = parseInt(searchField.data('column-id'));
                switch (columnId) {
                    case tableColumns.MatchNo.Id:
                        liveGameListData = liveGameListData.filter(item => item.MatchStatus.toLowerCase().indexOf(keyword) >= 0
                            || item.MatchNo.toLowerCase().indexOf(keyword) >= 0);
                        break;
                    case tableColumns.KoTime.Id:
                        liveGameListData = liveGameListData.filter(item => (item.KickOffTime !== null && item.KickOffTime.toLowerCase().indexOf(keyword) >= 0)
                            || (item.OtherTime !== null && item.OtherTime.toLowerCase().indexOf(keyword) >= 0));
                        break;
                    case tableColumns.League.Id:
                        liveGameListData = liveGameListData.filter(item => item.League.toLowerCase().indexOf(keyword) >= 0
                            || (item.NoOfHDP && item.NoOfHDP.toLowerCase().indexOf(keyword) >= 0)
                            || item.MatchOffers.filter(mo => mo.MatchOfferValue.toLowerCase().indexOf(keyword) >= 0).length);
                        break;
                    case tableColumns.MatchName.Id:
                        liveGameListData = liveGameListData.filter(item => item.Home.toLowerCase().indexOf(keyword) >= 0
                            || item.Away.toLowerCase().indexOf(keyword) >= 0);
                        break;
                    case tableColumns.Remark.Id:
                        liveGameListData = liveGameListData.filter(item => item.Remark.toLowerCase().indexOf(keyword) >= 0
                            || item.Division.toLowerCase().indexOf(keyword) >= 0
                            || item.Round.toLowerCase().indexOf(keyword) >= 0);
                        break;
                    case tableColumns.Note.Id:
                        liveGameListData = liveGameListData.filter(item => item.MatchNotes.length && (item.MatchNotes.filter(temp => temp.Note.toLowerCase().indexOf(keyword) >= 0).length));
                        break;
                    case tableColumns.MaxbetHDPOU.Id:
                        liveGameListData = liveGameListData.filter(item =>
                            item.MatchSetting.Handicap.MaxbetSpreadToDisplay.toLowerCase().indexOf(keyword) >= 0
                            || item.MatchSetting.OverUnder.MaxbetSpreadToDisplay.toLowerCase().indexOf(keyword) >= 0);
                        break;
                    case tableColumns.FullTimeTrader.Id:
                        liveGameListData = liveGameListData.filter(item => (item.Assignment.Handicap.length && (item.Assignment.Handicap.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length))
                            || item.Assignment.HandicapOthers.length && (item.Assignment.HandicapOthers.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length)
                            || item.Assignment.OverUnder.length && (item.Assignment.OverUnder.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length)
                            || item.Assignment.OverUnderOthers.length && (item.Assignment.OverUnderOthers.filter(temp => (temp.Nickname != null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length));
                        break;
                    case tableColumns.FirstHalfTrader.Id:
                        liveGameListData = liveGameListData.filter(item =>
                            item.MatchSetting.OverUnder1st.MaxbetSpreadToDisplay.toLowerCase().indexOf(keyword) >= 0
                            || item.MatchSetting.Handicap1st.MaxbetSpreadToDisplay.toLowerCase().indexOf(keyword) >= 0
                            || (item.Assignment.Handicap1st.length && (item.Assignment.Handicap1st.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length))
                            || item.Assignment.Handicap1stOthers.length && (item.Assignment.Handicap1stOthers.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username != null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length)
                            || item.Assignment.OverUnder1st.length && (item.Assignment.OverUnder1st.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username != null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length)
                            || item.Assignment.OverUnder1stOthers.length && (item.Assignment.OverUnder1stOthers.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username != null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length))
                        break;
                    case tableColumns.Maxbet1x2.Id:
                        liveGameListData = liveGameListData.filter(item =>
                            item.MatchSetting.FastMarket.MaxbetSpreadToDisplay.toLowerCase().indexOf(keyword) >= 0
                            || item.MatchSetting.OneXTwo.MaxbetSpreadToDisplay.toLowerCase().indexOf(keyword) >= 0
                            || item.MatchSetting.OneXTwo1st.MaxbetSpreadToDisplay.toLowerCase().indexOf(keyword) >= 0
                            || item.MatchSetting.Specific10Mins.MaxbetSpreadToDisplay.toLowerCase().indexOf(keyword) >= 0
                            || item.MatchSetting.Specific15Mins.MaxbetSpreadToDisplay.toLowerCase().indexOf(keyword) >= 0)
                        break;
                    case tableColumns.SpecialProduct.Id:
                        liveGameListData = liveGameListData.filter(item =>
                            item.Assignment.Specific10MinsOthers.length && (item.Assignment.Specific10MinsOthers.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length)
                            || item.Assignment.Specific15MinsOthers.length && (item.Assignment.Specific15MinsOthers.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length)
                            || item.Assignment.Specific10Mins.length && (item.Assignment.Specific10Mins.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length)
                            || item.Assignment.Specific15Mins.length && (item.Assignment.Specific15Mins.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length)
                            || (item.Assignment.AllInOne.length && (item.Assignment.AllInOne.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length))
                            || item.Assignment.AllInOneOthers.length && (item.Assignment.AllInOneOthers.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length)
                            || item.Assignment.FastMarket.length && (item.Assignment.FastMarket.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length)
                            || item.Assignment.FastMarketOthers.length && (item.Assignment.FastMarketOthers.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length)
                            || item.Assignment.OneXTwo.length && (item.Assignment.OneXTwo.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length)
                            || item.Assignment.OneXTwoOthers.length && (item.Assignment.OneXTwoOthers.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length)
                            || item.Assignment.OneXTwo1st.length && (item.Assignment.OneXTwo1st.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length)
                            || item.Assignment.OneXTwo1stOthers.length && (item.Assignment.OneXTwo1stOthers.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length));
                        break;
                    case tableColumns.LiveAssistant.Id:
                        liveGameListData = liveGameListData.filter(item => (item.Assignment.LA.length && (item.Assignment.LA.filter(temp => (temp.Nickname != null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username != null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length))
                            || item.Assignment.LAOthers.length && (item.Assignment.LAOthers.filter(temp => (temp.Nickname !== null && temp.Nickname.toLowerCase().indexOf(keyword) >= 0) || (temp.Username !== null && temp.Username.toLowerCase().indexOf(keyword) >= 0)).length));
                        break;
                    case tableColumns.TVIPTV.Id:
                        liveGameListData = liveGameListData.filter(item => (item.Televisions.length && (item.Televisions.filter(temp => (temp.TVCode !== null && temp.TVCode.toLowerCase().indexOf(keyword) >= 0) || (temp.TVName !== null && temp.TVName.toLowerCase().indexOf(keyword) >= 0))).length)
                            || (item.Channel !== null && item.Channel.toLowerCase().indexOf(keyword) >= 0));
                        break;
                    case tableColumns.Provider.Id:
                        liveGameListData = liveGameListData.filter(item => item.WatchBet.toLowerCase().indexOf(keyword) >= 0
                            || item.Phone.toLowerCase().indexOf(keyword) >= 0
                            || (item.Provider !== null && item.Provider.toLowerCase().indexOf(keyword) >= 0)
                            || (item.OtherProvider !== null && item.OtherProvider.toLowerCase().indexOf(keyword) >= 0));
                        break;
                }
            });

            resetPagingValue();
            renderLiveGameListTable(liveGameListData);

            filterFactory.HideLoadingProcess();
        });
    }

    function clickClearAllFilterEvent() {
        $('.js-clearAllFilter').on('click', function () {
            $('.js-clearAllFilter').addClass('is-disabled');
            clearFilterInColumn();
            domElements.searchLiveBtn.first().trigger('click');
        });
    }

    function markFilterIsSearching($element) {
        var currentSearchField = $element.parent().find('.js-inputSearchLive');
        var keyword = $.trim(currentSearchField.val());
        if (keyword === '') {
            currentSearchField.attr('data-is-searching', '0');
            currentSearchField.attr('data-old-value', '');
        } else {
            currentSearchField.attr('data-is-searching', '1');
            currentSearchField.attr('data-old-value', keyword);
        }
    }

    function showIconFilterIsSearching($searchField, columnId) {
        var iconFilter = $('.js-iconFilter[data-column-id="' + columnId + '"]');
        iconFilter.removeClass('is-filter');
        if ($searchField.attr('data-is-searching') === '0') {
            // reset input keyword if not search
            $searchField.val('');
        } else {
            if ($.trim($searchField.val()) === '') {
                $searchField.val($searchField.data('old-value'));
            }

            iconFilter.addClass('is-filter');
        }
    }

    function clearFilterInColumn() {
        var iconFilters = $('.js-iconFilter');
        iconFilters.removeClass('is-filter');

        domElements.fieldsSearch.val('');
        domElements.fieldsSearch.attr('data-is-searching', '0');
        domElements.fieldsSearch.attr('data-old-value', '');
    }

    function isNullOrWhitespace(str) {
        return str === null || str.match(/^ *$/) !== null;
    }

    function IsNullOrEmpty(str) {
        return str === null || str === '';
    }

    function isHasMaxbetSpread(matchSetting) {
        return matchSetting.Maxbet !== null ||
            matchSetting.Spread !== null;
    }

    function newGuid() {
        var sGuid = '';
        for (var i = 0; i < 32; i++) {
            sGuid += Math.floor(Math.random() * 0xF).toString(0xF);
        }
        return sGuid;
    }

    function formatTimespan(time) {
        return moment.utc(moment.duration(time).asMilliseconds()).format("hh:mmA");
    }

    function Table() {
        this.generateHeader = function () {
            var columns = [
                { text: 'No#', className: 'no-col' },
                { text: 'KO Time', className: 'ko-col' },
                { text: 'League', className: 'league-col' },
                { text: 'Match', className: 'match-col' },
                { text: 'Ticket', className: 'ticket-col' },
                { text: 'Remark', className: 'remarks-col' },
                { text: 'Note', className: 'note-col' },
                { text: 'Max Bet', className: 'maxbet-col', colSpan: 2 },
                { text: 'FT Trader' },
                { text: '1H Trader', className: 'center', colSpan: 2 },
                { text: 'Max Bet', className: 'maxbet-col', colSpan: 2 },
                { text: 'Special Product' },
                { text: 'LA' },
                { text: 'TV/IPTV', className: 'tv-col' },
                { text: 'Provider', className: 'provider-col' }
            ];
         
            var html = [];
            html.push('<thead>');
            for (var i = 0; i < columns.length; i++) {
                var column = columns[i];
                if (column.text == 'Note' && !hasReadNotePermission) {
                    continue;
                }

                html.push('<th');
                if (column.className) {
                    html.push(' class="' + column.className + '"');
                }
                if (column.colSpan) {
                    html.push(' colspan="' + column.colSpan + '"');
                }

                html.push('>');
                html.push(column.text);
                columns[i].text != "Ticket" && html.push('<i class="icon icon-filter mdi mdi-filter js-iconFilter" data-column-id="' + (i + 1) + '"></i>');
                html.push('</th>');
            }
            html.push('</thead>');
            return html.join('');
        }

        this.generateBody = function (data) {
            var html = [];
            html.push('<tbody id="tableEventInformationBody">');
            var totalItems = data.length;
            if (totalItems === 0) {
                html.push('<tr><td colspan="18">There is no data.</td></tr>');
            }
            else {
                var fromItem = (pagingData.CurrentPage - 1) * pagingData.ItemsPerPage;
                var toItem = pagingData.CurrentPage * pagingData.ItemsPerPage - 1;
                toItem = (totalItems - 1) < toItem ? (totalItems - 1) : toItem;

                pagingData.TotalItems = totalItems;
                pagingData.CurrentFromItem = fromItem + 1;
                pagingData.CurrentToItem = toItem + 1;

                for (var i = fromItem; i <= toItem; i++) {
                    const popupClassName = i === toItem || i === toItem - 1 || i === toItem - 2
                        ? 'last-popup'
                        : null;

                    var matchInfo = data[i];
                    var match = new Match(matchInfo, i);
                    html.push(match.generate(popupClassName));
                }
            }
            html.push('</tbody>');
            return html.join('');
        }

        this.generate = function (data) {
            var html = [];
            html.push('<table>');
            html.push(this.generateHeader(data));
            html.push(this.generateBody(data));
            html.push('</table>');
            return html.join('');
        }
    }

    function Match(matchInfo, index) {
        this.matchInfo = matchInfo;
        this.index = index;
        this.rowspan = 2;
        if (matchInfo.Assignment.FastMarket.length > 0 || isHasMaxbetSpread(matchInfo.MatchSetting.FastMarket)) {
            this.rowspan++;
        }

        if (matchInfo.Assignment.AllInOne.length > 0) {
            this.rowspan++;
        }

        if (matchInfo.Assignment.Specific15Mins.length > 0 || isHasMaxbetSpread(matchInfo.MatchSetting.Specific15Mins)) {
            this.rowspan++;
        }

        if (matchInfo.Assignment.Specific10Mins.length > 0 || isHasMaxbetSpread(matchInfo.MatchSetting.Specific10Mins)) {
            this.rowspan++;
        }

        this.getRowClass = function () {
            var rowClass = this.getRowClassByMatchStatus();
            rowClass += this.index % 2 === 0 ? ' odd' : ' even';
            rowClass += this.matchInfo.IsFIFA ? ' fifa' : '';
            rowClass += this.matchInfo.IsPes ? ' pes' : '';
            return rowClass;
        }

        this.getRowClassByMatchStatus = function () {
            var matchStatus = this.matchInfo.MatchStatus;
            switch (matchStatus) {
                case 'In-play':
                    return 'match-inplay';
                case 'Cancel':
                case 'Postponed':
                    return 'match-cancelled';
                case 'Suspended':
                    return 'match-suspended';
            }

            return '';
        }

        this.generateOpeningTd = function (className) {
            return '<td rowspan="' + this.rowspan + '" class="' + className + '">';
        }

        this.generateNOCell = function () {
            var html = [];
            html.push(this.generateOpeningTd('center js-dataMatchNo'));
            var matchNo = this.matchInfo.MatchNo;
            if (!isNullOrWhitespace(matchNo)) {
                html.push('<span>' + matchNo + '</span><br />');
            }

            var classNames = {
                'Live': 'live',
                'In-play': 'inplay',
                'Completed': 'completed',
                'Cancel': 'cancelled',
                'Postponed': 'postponed',
                'Suspended': 'suspended'
            };
            var matchStatus = this.matchInfo.MatchStatus;
            var className = classNames[matchStatus];
            switch (matchStatus) {
                case 'Live':
                    html.push('<span class="badge-label ' + className + '" style="display:none">' + matchStatus + '</span>');
                    html.push('<i class="' + className + '" aria-label="' + matchStatus + '"></i>');
                    break;
                default:
                    html.push('<span class="badge-label ' + className + '" style="display:none">' + matchStatus + '</span>');
                    html.push('<i class="icon-status icon-' + className + ' tooltip-tipsy" aria-label="' + matchStatus + '" original-title="' + matchStatus + '"></i>');
                    break;
            }
            html.push('</td>');
            return html.join('');
        }

        this.generateKOTimeCell = function () {
            var html = [];
            html.push(this.generateOpeningTd('center js-dataKoTime'));
            html.push('<p>' + this.matchInfo.KickOffTime + '</p>');
            var otherTime = this.matchInfo.OtherTime;
            if (otherTime) {
                html.push('<p><span class="badge-label other-time">' + otherTime + '</span></p>');
            }
            if (this.matchInfo.IsPES) {
                html.push('<i class="icon-match icon-pes tooltip-tipsy" original-title="PES"></i>');
            }
            if (this.matchInfo.IsFIFA) {
                html.push('<i class="icon-match icon-fifa tooltip-tipsy" original-title="FIFA"></i>');
            }
            html.push('</td>');
            return html.join('');
        }

        this.generateLeagueCell = function () {
            var html = [];
            html.push(this.generateOpeningTd('js-dataLeague'));
            html.push('<p>' + this.matchInfo.League + '</p>');
            var isHasMatchOfferOrNoHDP = !IsNullOrEmpty(this.matchInfo.NoOfHDP) || this.matchInfo.MatchOffers.length;
            if (isHasMatchOfferOrNoHDP) {
                html.push('<p>');
                var noOfHDP = this.matchInfo.NoOfHDP ? this.matchInfo.NoOfHDP.trim().replace(' ', '') : '';
                if (noOfHDP) {
                    html.push('<span class="badge-label badge-hdp-' + this.matchInfo.NoOfHDPId + '">' + noOfHDP + '</span>');
                }

                this.matchInfo.MatchOffers.forEach(function (element) {
                    html.push('<span class="badge-label match-offer">' + element.MatchOfferValue + '</span>');
                });

                html.push('</p>');
            }
            html.push('</td>');
            return html.join('');
        }

        this.generateMatchCell = function (popupClassName) {
            var html = [];
            html.push(this.generateOpeningTd('js-dataMatchName'));
            const isAllowedToViewDetail = common.hasDetailInformationPermission() && this.matchInfo.IsAllowedToViewDetail;
            if (isAllowedToViewDetail) {
                html.push('<a class="match-link js-openModalLiveDetail" href="javascript:;" match-id="' + this.matchInfo.OWPMatchId + '">');
            } else {
                html.push('<a>');
            }
            html.push('<p class="home-team">' + this.matchInfo.Home + '</p>');
            html.push('<p class="away-team">' + this.matchInfo.Away + '</p>');
            html.push('</a>');

            if (this.matchInfo.MatchCode != null) {
                html.push('<p class="match-code">' + this.matchInfo.MatchCode + '</p>');
            }
            if (this.matchInfo.TeamRound != null) {
                html.push('<p class="team-round">' + this.matchInfo.TeamRound + '</p>');
            }
            html.push('<div class="list-button">');
            if (this.matchInfo.AdminSiteId > 0) {
                var dropdownId = 'dropdown' + newGuid();
                html.push(this.generateIconTrading(dropdownId, popupClassName));
                html.push(this.generateTradingPopup(this.matchInfo.AdminSiteId, dropdownId, this.matchInfo.IsFantasy, this.matchInfo.MinorBettypeId));
            }

            html.push(this.generateCopyIcon(this.matchInfo));
            html.push('</div>');
            html.push('</td>');
            return html.join('');
        }

        this.generateTicketCell = function () {
            let html = []
            var id = 'dropdown' + newGuid();
            html.push(this.generateOpeningTd('center js-ticketCell'));

            if (matchInfo.TicketInfos.length > 0) {
                html.push(this.generateTicketPopup(id, this.matchInfo));
            }
            else {
                let buttonCreate = `<svg onClick="handleOpenCreateTicketByMatchId({OWPMatchId:${matchInfo.OWPMatchId},League:\'${matchInfo.League} '\,Home:'${matchInfo.Home}',Away:'${matchInfo.Away}',EventDateTime:'${matchInfo.EventDateTime}'})" class="mdi-icon icon-plus" width="24" height="24" fill="currentColor" viewBox="0 0 24 24"><path d="M17,13H13V17H11V13H7V11H11V7H13V11H17M12,2C6.48,2 2,6.48 2,12C2,17.52 6.48,22 12,22C17.52,22 22,17.52 22,12C22,6.48 17.52,2 12,2Z"></path></svg><br />`
                ticketPermission.isHost && html.push(buttonCreate);
            }
            return html.join('');
        }

        this.generateTicketPopup = function (dropdownId, match) {
            let popupHtml = [];
            let dateString = (m) => m.getUTCFullYear() + "/" + (m.getUTCMonth() + 1) + "/" + m.getUTCDate() + " " + m.toLocaleString('en-US', {
                hour: 'numeric',
                minute: 'numeric',
                hour12: true
            });
            let isReadAll = match.TicketInfos.every(x => x.IsRead) ? 'icon-message' : 'icon-unread-message';
            const tableTicket = match.TicketInfos.reduce((s, x) => {
                let isRead = x.IsRead ? "icon-message" : "icon-unread-message";
                return s.concat(`<tr>
                    <td>
                        <svg onClick=handleOpenMessage({ticketId:${x.TicketId},ticketType:4}) id="message-${x.TicketId}" class="mdi-icon ${isRead}" width="24" height="24" fill="icon-message" viewBox="0 0 24 24" >
                            <path d="M20,2C21.1,2 22,2.9 22,4V16C22,17.1 21.1,18 20,18H6L2,22V4C2,2.89 2.9,2 4,2H20M4,4V17.17L5.17,16H20V4H4M6,7H18V9H6V7M6,11H15V13H6V11Z"></path>
                        </svg>
                    </td>
                    <td>${x.TicketId}</td>
                    <td>
                        <div class="line-subtype" id="subtype-2934">${(x.SubtypeName) || ''}</div>
                    </td>
                    <td>
                        <div class="line-message">
                            <span class="username">${x.LatestUsername}</span>
                            <span class="time"> - ${dateString(new Date(x.LatestCreatedDate))}: </span> ${x.LatestMessage}
                        </div>
                    </td>
                </tr>`)
            }, '');
            let buttonCreate = `<div class="button"><button class="button-submit" onClick="handleOpenCreateTicketByMatchId({OWPMatchId:${match.OWPMatchId},League:\'${match.League} '\,Home:'${match.Home}',Away:'${match.Away}',EventDateTime:'${match.EventDateTime}'})">Add Ticket</button></div>`
            popupHtml.push(`<div id=${dropdownId} class= "dropdown-content tickettable">`);
            ticketPermission.isHost && popupHtml.push(buttonCreate);
            popupHtml.push(`<table class="ticket-list-table">
             ${tableTicket}
            </table >`);
            popupHtml.push('</div>');
            popupHtml.push(`<button class="dropdown-button" data-activates=${dropdownId}> <svg class="mdi-icon ${isReadAll}" width="24" height="24" fill="icon-unread-message" viewBox="0 0 24 24"><path d="M20,2C21.1,2 22,2.9 22,4V16C22,17.1 21.1,18 20,18H6L2,22V4C2,2.89 2.9,2 4,2H20M4,4V17.17L5.17,16H20V4H4M6,7H18V9H6V7M6,11H15V13H6V11Z"></path></svg> </button><br />`);
            return popupHtml.join('');
        }

        this.generateCopyIcon = function (matchInfo) {
            const kickOffTime = 'K.O Time: ' + matchInfo.KickOffTime;
            const league = 'League: ' + matchInfo.League;
            const match = 'Match: ' + matchInfo.Home + ' -vs- ' + matchInfo.Away;
            const matchId = matchInfo.AdminSiteId
                ? 'Match ID: ' + matchInfo.AdminSiteId
                : 'Match ID: ';

            const iptv = matchInfo.IPTVCopy
                ? 'TV/IPTV: ' + matchInfo.IPTVCopy
                : 'TV/IPTV: ';
            const provider = matchInfo.Provider
                ? 'Provider: ' + matchInfo.Provider
                : 'Provider: ';
            return '<span>\
           <button onClick="executeCopy(\'' + kickOffTime + '\',\
            \'' + league + '\',\
            \'' + match + '\',\
            \'' + matchId + '\',\
            \'' + iptv + '\',\
            \'' + provider + '\')"\
            class="copy-btn btn btn-primary" owp-match-id="' + matchInfo.OWPMatchId + '"><span class="icon-content_copy"></span></button>\
            </span>';
        }

        this.generateRemarkCell = function () {
            var html = [];
            html.push(this.generateOpeningTd('vertical-align-top'));
            html.push('<div class="js-content-remark">');
            var remark = this.matchInfo.Remark;
            var hasRemark = !isNullOrWhitespace(remark);
            if (hasRemark) {
                html.push('<p>' + common.HtmlEncode(remark) + ',</p>');
            }

            var round = this.matchInfo.Round;
            var hasRound = !isNullOrWhitespace(round);
            var division = this.matchInfo.Division;
            var hasDivision = !isNullOrWhitespace(division);
            if (hasRound) {
                html.push('<p><b>Round</b>: ' + common.HtmlEncode(round) + (hasDivision ? ',' : '') + '</p>');
            }
            if (hasDivision) {
                html.push('<p><b>Division</b>: ' + common.HtmlEncode(division) + '</p>');
            }
            html.push('</div>');
            html.push('</td>');
            return html.join('');
        }

        this.generateNoteCell = function () {
            var html = [];
            html.push(this.generateOpeningTd('center'));
            if (this.matchInfo.TotalNotes > 0) {
                html.push('<div class="block-note">');
                html.push('<i class="icon mdi mdi-comment icon-note js-viewMatchNote tooltip-tipsy" data-owpmatchid="' + this.matchInfo.OWPMatchId + '" original-title="Note"></i>');
                html.push('</div>');
                html.push('<div class="js-matchNotes" data-owpmatchid="' + this.matchInfo.OWPMatchId + '" style="display: none;">');
                for (var i = 0; i < this.matchInfo.MatchNotes.length; i++) {
                    var note = this.matchInfo.MatchNotes[i];
                    html.push('<p class="note-content"  style="' + (!note.IsViewed ? 'font-weight: bold;' : '') + '">');

                    html.push('<span><span class="user">&#64;' + note.Username + ' </span><span>' + note.Note + '</span></span>');

                    var displayLastUpdated = '';
                    if (note.LastUpdated) {
                        displayLastUpdated = moment(note.LastUpdated, 'YYYY-MM-DDTHH:mm:ss').format('MM/DD/YYYY hh:mm A');
                    }

                    html.push('<span>' + displayLastUpdated + '</span>');

                    html.push('</p >');
                }
                html.push('</div>');
                html.push('<div class="js-dataMatchNotes" style="display: none;">');
                for (var i = 0; i < this.matchInfo.MatchNotes.length; i++) {
                    var note = this.matchInfo.MatchNotes[i];

                    html.push('<p>' + note.Note + '</p>');
                }
                html.push('</div>');
            } else {
                html.push('<div class="js-dataMatchNotes" style="display: none;"></div>');
            }
            html.push('</td>');
            return html.join('');
        }

        this.generateLACell = function () {
            var html = [];
            html.push(this.generateOpeningTd('js-dataLA'));
            html.push(this.generateAssignments(this.matchInfo.Assignment.LA));
            html.push(this.generateOtherAssignments(this.matchInfo.Assignment.LAOthers));
            html.push('</td>');
            return html.join('');
        }

        this.generateTVCell = function () {
            function generateTV(televisions) {
                var html = [];
                for (var i = 0; i < televisions.length; i++) {
                    var television = televisions[i];
                    html.push('<span style="color: ' + television.Color + '">' + television.TVCode + '</span>');
                }
                return html.join(', ');
            }

            var html = [];
            html.push(this.generateOpeningTd('js-dataTVIPTV vertical-align-top'));
            html.push('<div class="dataTVIPTV-content">');
            var televisionHtml = generateTV(this.matchInfo.Televisions);
            if (televisionHtml) {
                html.push(televisionHtml);
            }

            var channel = this.matchInfo.Channel;
            if (!isNullOrWhitespace(channel)) {
                html.push('<div class="icon-bottom tv">');
                var dropdownId = 'dropdown' + newGuid();
                html.push('<span class="icon-channel has-info dropdown-button tooltip-tipsy" data-activates="' + dropdownId + '" original-title="Channel"></span>');
                html.push('<ul id="' + dropdownId + '" class="dropdown-content"><li><a href="javascript:;">' + channel + '</a></li></ul>');
                html.push('</div>');
            }

            html.push('</div>');
            html.push('</td>');
            return html.join('');
        }

        this.generateProviderCell = function () {
            var html = [];
            html.push(this.generateOpeningTd('js-dataProvider vertical-align-top'));
            html.push('<div class="dataProvider-content">');
            html.push('<div class="js-parent-provider">');
            var provider = this.matchInfo.Provider;
            if (!isNullOrWhitespace(provider)) {
                html.push('<p>' + provider + '</p>');
            }

            var watchBet = this.matchInfo.WatchBet;
            if (!isNullOrWhitespace(watchBet)) {
                html.push('<p class="wb">' + watchBet + '</p>');
            }

            var phone = this.matchInfo.Phone;
            if (!isNullOrWhitespace(phone)) {
                html.push('<p class="phone"><i class="icon mdi mdi-phone"></i>' + phone + '</p>');
            }

            html.push('</div>');
            var otherProvider = this.matchInfo.OtherProvider;
            if (!isNullOrWhitespace(otherProvider)) {
                html.push('<div class="icon-bottom">');
                var dropdownId = 'dropdown' + newGuid();
                html.push('<i class="icon mdi mdi-format-list-bulleted icon-list-provider dropdown-button tooltip-tipsy" data-activates="' + dropdownId + '" original-title="Other Provider List"></i>');
                html.push('<ul id="' + dropdownId + '" class="dropdown-content">');

                html.push('<li><a href="javascript:;">' + otherProvider + '</a></li>');
                html.push('</ul>');
                html.push('</div>');
            }

            html.push('</div>');
            html.push('</td>');
            return html.join('');
        }

        this.generateAssignments = function (assignments) {
            var html = [];
            for (var i = 0; i < assignments.length; i++) {
                html.push('<p>');
                var assignment = assignments[i];
                if (!assignment.IsCurrentUser && assignment.IsHasContactInfo) {
                    var dropdownId = 'dropdown' + newGuid();
                    html.push(this.generateUserNameHasContact(dropdownId, assignment.Username, assignment.Username));
                    html.push(this.generateContactInfo(assignment, dropdownId));
                } else {
                    html.push(this.generateNormalUser(assignment.Username, assignment.Username, assignment.IsCurrentUser));
                }

                html.push('</p>');
            }
            return html.join('');
        }

        this.generateOtherAssignments = function (assignments) {
            var html = [];
            for (var i = 0; i < assignments.length; i++) {
                html.push('<p>');
                html.push('<span class="other-assignee">');
                var assignment = assignments[i];
                if (!assignment.IsCurrentUser && assignment.IsHasContactInfo) {
                    var dropdownId = 'dropdown' + newGuid();
                    html.push(assignment.AssignmentNote.replace('#<user>', this.generateUserNameHasContact(dropdownId, assignment.Username, assignment.Username)));
                    html.push(this.generateContactInfo(assignment, dropdownId));
                } else {
                    html.push(assignment.AssignmentNote.replace('#<user>', this.generateNormalUser(assignment.Username, assignment.Username, assignment.IsCurrentUser)));
                }

                html.push('</span>');
                html.push('</p>');
            }
            return html.join('');
        }

        this.generateIconTrading = function (dropdownId, popupClassName) {
            return (
                '<span class="assignee assignee-trader ' + popupClassName + '"> \
                    <a class="dropdown-button js-traderName" href="javascript:;" data-activates="' + dropdownId + '"><span class="icon-link"></span></a> \
                </span>'
            )
        }

        this.generateTradingPopup = function (matchId, dropdownId, isFantasy, minorBettypeId) {
            let popupHtml = [];
            popupHtml.push('<ul id = "' + dropdownId + '" class="dropdown-content">');
            popupHtml.push(this.generateLaControllerItem(matchId, dropdownId));
            popupHtml.push(this.generateTradingItem(dropdownId));
            if (isFantasy) {
                popupHtml.push(this.generateFantasySubItem(matchId, dropdownId));
            } else {
                popupHtml.push(this.generateOtherMatchSubItem(matchId, dropdownId, minorBettypeId));
            }
            popupHtml.push('</ul>');
            return popupHtml.join('');
        }

        this.generateLaControllerItem = function (matchId, dropdownId) {
            var laPageUrl = 'http://live.oneops.net:805/Live/OddsControl.aspx?matchid=' + matchId + '&type=s';
            return (
                '<li>\
                        <a href='+ laPageUrl + ' rel = "noopener noreferrer" target = "_blank">LA Controller</a>\
                    </li>'
            )
        }

        this.generateTradingItem = function (dropdownId) {
            return '<li>\
                    <span>Live Trading</span>\
                </li>';
        }

        this.generateFantasySubItem = function (matchId, dropdownId) {
            var tradingPageUrl = 'http://live.oneops.net/FantasyRobot/FantasyRobotControl.aspx?MatchId=' + matchId;
            return '<li>\
                            <a href='+ tradingPageUrl + ' rel = "noopener noreferrer" target= "_blank">Fantasy</a>\
                        </li>'
        }

        this.generateOtherMatchSubItem = function (matchId, dropdownId, minorBettypeId) {
            const cornersMatchIds = [-3, -4];
            const isCornersMatch = cornersMatchIds.indexOf(minorBettypeId) >= 0;
            var tradingPageMlUrl = "http://live.oneops.net/TradingAutomation/MLRobotControl.aspx?MatchId=" + matchId;
            var tradingPageRt2Url = "http://live.oneops.net/RobotTrader2/RobotTraderController.aspx?MatchId=" + matchId;
            var tradingPageNoOfCornersRt3Url = "http://live.oneops.net/RobotTrader3/Robot3ChildMatchControl.aspx?MatchId=" + matchId + "&rbtVer=7";
            var tradingPageRt3Url = "http://live.oneops.net/RobotTrader3/RobotTrader3Control.aspx?MatchId=" + matchId;
            var tradingPageStmUrl = "http://live.oneops.net/FutsalRobot/FutsalRobotControl.aspx?MatchId=" + matchId;
            var tradingPageLsport = 'http://live.oneops.net/TradingAutomation/MLRobotLSportControl.aspx?MatchId=' + matchId;


            const rt3Url = minorBettypeId === -3 ? tradingPageNoOfCornersRt3Url : tradingPageRt3Url;
            const tradingMlElement = !isCornersMatch ?
                '<li>\
                    <a href='+ tradingPageMlUrl + ' rel = "noopener noreferrer" target = "_blank">ML</a>\
                </li>' : '';
            const tradingStmElement = !isCornersMatch ?
                '<li>\
                    <a href=' + tradingPageStmUrl + ' rel = "noopener noreferrer" target = "_blank">STM</a>\
                </li>' : '';

            return tradingMlElement +
                '<li>\
                    <a href='+ tradingPageRt2Url + ' rel = "noopener noreferrer" target = "_blank">RT2</a>\
                </li>\
                <li>\
                    <a href=' + rt3Url + ' rel = "noopener noreferrer" target = "_blank">RT3</a>\
                </li>'
                + tradingStmElement
                +
                '<li>\
                    <a href=' + tradingPageLsport + ' rel="noopener noreferrer" target="_blank">Lsport</a>\
                </li>'

            '</ul>'
        }

        this.generateUserNameHasContact = function (dropdownId, username, nickname) {
            return (
                '<span class="assignee"> \
                    <a class="dropdown-button js-traderName" href="javascript:;" data-activates="' + dropdownId + '">' + nickname + '</a> \
                </span>\
                <span style="display:none">' + username + '</span>'
            );
        }

        this.generateContactInfo = function (assignment, dropdownId) {
            var html = [];
            html.push('<ul id="' + dropdownId + '" class="dropdown-content">');
            if (!isNullOrWhitespace(assignment.Skype)) {
                var skypeChat = "skype:" + assignment.Skype + "?chat";
                html.push(
                    '<li> \
                        <a class="link-skype" href="' + skypeChat + '"> \
                            <i class="mdi mdi-skype icon-skype"></i> \
                            <span class="skype-value">' + assignment.Skype + '</span> \
                        </a> \
                    </li>'
                );
            }

            var extension = assignment.Extension;
            if (!isNullOrWhitespace(extension)) {
                html.push(
                    '<li> \
                        <a href="javascript:;"> \
                            <i class="mdi mdi-deskphone icon-phone"></i> \
                            <span class="extension-value">' + extension + '</span> \
                        </a> \
                    </li>'
                );
            }

            var phone = assignment.Phone;
            if (!isNullOrWhitespace(phone)) {
                html.push(
                    '<li> \
                        <a href="javascript:;"> \
                            <i class="mdi mdi-cellphone-iphone icon-phone"></i> \
                            <span class="phone-value">' + phone + '</span> \
                        </a> \
                    </li>'
                );
            }

            html.push('</ul>');
            return html.join('');
        }

        this.generateNormalUser = function (username, nickname, isCurrentUser) {
            return (
                '<span class="' + (isCurrentUser ? 'badge-label my-assign' : 'assignee') + ' js-traderName">' + nickname + '</span> \
                <span style="display:none">' + username + '</span>'
            );
        }

        this.generateMaxbet = function (maxbet) {
            if (maxbet === "") {
                return ""
            } else {
                return '<span class="maxbet tooltip-tipsy" original-title="Maxbet/Spread of MBC">' + maxbet + '</span>';
            }
        }


        this.generate = function (popupClassName) {
            var html = [];
            var rowClass = this.getRowClass();
            var openingTR = '<tr class="' + rowClass + '" data-row-group="' + this.index + '">';
            var hdpRowspan = this.rowspan === 2 ? 1 : 2;
            var handicapMbcToDisplay = this.matchInfo.MatchSetting.Handicap.MaxbetSpreadToDisplay;
            var handicap1stMbcToDisplay = this.matchInfo.MatchSetting.Handicap1st.MaxbetSpreadToDisplay;
            var oneXTwoMbcToDisplay = this.matchInfo.MatchSetting.OneXTwo.MaxbetSpreadToDisplay;
            var overUnderMbcToDisplay = this.matchInfo.MatchSetting.OverUnder.MaxbetSpreadToDisplay;
            var overUnder1stMbcToDisplay = this.matchInfo.MatchSetting.OverUnder1st.MaxbetSpreadToDisplay;
            var oneXTwo1stMbcToDisplay = this.matchInfo.MatchSetting.OneXTwo1st.MaxbetSpreadToDisplay;

            html.push(openingTR);
            html.push(this.generateNOCell());
            html.push(this.generateKOTimeCell());
            html.push(this.generateLeagueCell());
            html.push(this.generateMatchCell(popupClassName));
            html.push(this.generateTicketCell())
            html.push(this.generateRemarkCell());
            if (hasReadNotePermission) {
                html.push(this.generateNoteCell());
            }

            html.push('<td rowspan="' + hdpRowspan + '" class="bg-hdpou center">HDP</td>');
            html.push('<td rowspan="' + hdpRowspan + '" class="bg-maxbet js-dataMaxbetHDPOU ">')
            var htmlMbcHandicap = this.generateMaxbet(handicapMbcToDisplay);
            if (htmlMbcHandicap !== "") {
                html.push(htmlMbcHandicap);
            }
            html.push('</td>')
            html.push(
                '<td rowspan="' + hdpRowspan + '" class="js-dataFullTimeTrader"> \
                ' + this.generateAssignments(this.matchInfo.Assignment.Handicap) + ' \
                ' + this.generateOtherAssignments(this.matchInfo.Assignment.HandicapOthers) + ' \
                </td>'
            );
            html.push('<td rowspan="' + hdpRowspan + '" class="bg-maxbet js-dataFirstHalfTrader">');
            var htmlMbcHandicap1st = this.generateMaxbet(handicap1stMbcToDisplay);
            if (htmlMbcHandicap1st !== "") {
                html.push(htmlMbcHandicap1st);
            }
            html.push('</td>')
            html.push(
                '<td rowspan="' + hdpRowspan + '" class="js-dataFirstHalfTrader"> \
                ' + this.generateAssignments(this.matchInfo.Assignment.Handicap1st) + ' \
                ' + this.generateOtherAssignments(this.matchInfo.Assignment.Handicap1stOthers) + ' \
                </td>'
            );
            html.push('<td class="bg1x2 center">FT1X2</td>');
            html.push('<td class="bg-maxbet js-dataMaxbet1x2">');
            var htmlMbcOneXTwo = this.generateMaxbet(oneXTwoMbcToDisplay);
            if (htmlMbcOneXTwo !== "") {
                html.push(htmlMbcOneXTwo);
            }
            html.push('</td>')
            html.push(
                '<td class="js-dataSpecialProduct"> \
                ' + this.generateAssignments(this.matchInfo.Assignment.OneXTwo) + ' \
                ' + this.generateOtherAssignments(this.matchInfo.Assignment.OneXTwoOthers) + ' \
                </td>'
            );
            html.push(this.generateLACell());
            html.push(this.generateTVCell());
            html.push(this.generateProviderCell());
            html.push('</tr>');

            if (this.rowspan === 2) {
                html.push(openingTR);
                html.push('<td class="bg-hdpou center js-positionDisplayNotes" data-owpmatchid="' + this.matchInfo.OWPMatchId + '">OU</td>');
                html.push('<td class="bg-maxbet js-dataMaxbetHDPOU">');
                var htmlMbcOverUnder = this.generateMaxbet(overUnderMbcToDisplay);
                if (htmlMbcOverUnder !== "") {
                    html.push(htmlMbcOverUnder);
                }
                html.push('</td>')
                html.push(
                    '<td class="js-dataFullTimeTrader"> \
                    ' + this.generateAssignments(this.matchInfo.Assignment.OverUnder) + ' \
                    ' + this.generateOtherAssignments(this.matchInfo.Assignment.OverUnderOthers) + ' \
                    </td>'
                );
                html.push('<td class="bg-maxbet js-dataFirstHalfTrader">');
                var htmlMbcOverUnder1st = this.generateMaxbet(overUnder1stMbcToDisplay);
                if (htmlMbcOverUnder1st !== "") {
                    html.push(htmlMbcOverUnder1st);
                }
                html.push('</td>')
                html.push(
                    '<td class="js-dataFirstHalfTrader"> \
                    ' + this.generateAssignments(this.matchInfo.Assignment.OverUnder1st) + ' \
                    ' + this.generateOtherAssignments(this.matchInfo.Assignment.OverUnder1stOthers) + ' \
                    </td>'
                );
                html.push('<td class="bg1x2 center">HT1X2</td>');
                html.push('<td class="bg-maxbet js-dataMaxbet1x2">');
                var htmlMbcOneXTwo1st = this.generateMaxbet(oneXTwo1stMbcToDisplay);
                if (htmlMbcOneXTwo1st !== "") {
                    html.push(htmlMbcOneXTwo1st);
                }
                html.push('</td>')
                html.push(
                    '<td class="js-dataSpecialProduct"> \
                    ' + this.generateAssignments(this.matchInfo.Assignment.OneXTwo1st) + ' \
                    ' + this.generateOtherAssignments(this.matchInfo.Assignment.OneXTwo1stOthers) + ' \
                    </td>'
                );
                html.push('</tr>');
            }
            else {
                html.push(openingTR);
                html.push('<td class="bg1x2 center">HT1X2</td>');
                html.push('<td class="bg-maxbet js-dataMaxbet1x2">');
                var htmlMbcOneXTwo1st = this.generateMaxbet(oneXTwo1stMbcToDisplay);
                if (htmlMbcOneXTwo1st !== "") {
                    html.push(htmlMbcOneXTwo1st);
                }
                html.push('</td>')
                html.push(
                    '<td class="js-dataSpecialProduct"> \
                    ' + this.generateAssignments(this.matchInfo.Assignment.OneXTwo1st) + ' \
                    ' + this.generateOtherAssignments(this.matchInfo.Assignment.OneXTwo1stOthers) + ' \
                    </td>'
                );
                html.push('</tr>');

                var ouSpecificBetTypeGroupList = [{
                    IsMajorBetType: true,
                    BetTypeName: 'FM',
                    MaxbetSpread: this.matchInfo.MatchSetting.FastMarket.MaxbetSpreadToDisplay,
                    IsHasMaxbetSpread: isHasMaxbetSpread(this.matchInfo.MatchSetting.FastMarket),
                    MainAssignment: this.matchInfo.Assignment.FastMarket,
                    OthersAssignment: this.matchInfo.Assignment.FastMarketOthers
                }, {
                    IsMajorBetType: true,
                    BetTypeName: 'AIO',
                    MaxbetSpread: '',
                    IsHasMaxbetSpread: false,
                    MainAssignment: this.matchInfo.Assignment.AllInOne,
                    OthersAssignment: this.matchInfo.Assignment.AllInOneOthers
                }, {
                    IsMajorBetType: false,
                    BetTypeName: '15mins',
                    MaxbetSpread: this.matchInfo.MatchSetting.Specific15Mins.MaxbetSpreadToDisplay,
                    IsHasMaxbetSpread: isHasMaxbetSpread(this.matchInfo.MatchSetting.Specific15Mins),
                    MainAssignment: this.matchInfo.Assignment.Specific15Mins,
                    OthersAssignment: this.matchInfo.Assignment.Specific15MinsOthers
                }, {
                    IsMajorBetType: false,
                    BetTypeName: '10mins',
                    MaxbetSpread: this.matchInfo.MatchSetting.Specific10Mins.MaxbetSpreadToDisplay,
                    IsHasMaxbetSpread: isHasMaxbetSpread(this.matchInfo.MatchSetting.Specific10Mins),
                    MainAssignment: this.matchInfo.Assignment.Specific10Mins,
                    OthersAssignment: this.matchInfo.Assignment.Specific10MinsOthers
                }];
                ouSpecificBetTypeGroupList = $.grep(ouSpecificBetTypeGroupList, function (item) {
                    return item.MainAssignment.length > 0 || item.IsHasMaxbetSpread;
                });
                var ouRowspan = ouSpecificBetTypeGroupList.length;

                html.push(openingTR);
                html.push('<td rowspan="' + ouRowspan + '" class="bg-hdpou center js-positionDisplayNotes" data-owpmatchid="' + this.matchInfo.OWPMatchId + '">OU</td>');
                html.push('<td rowspan="' + ouRowspan + '" class="bg-maxbet js-dataMaxbetHDPOU">');
                var htmlMbcOverUnder = this.generateMaxbet(overUnderMbcToDisplay);
                if (htmlMbcOverUnder !== "") {
                    html.push(htmlMbcOverUnder);
                }
                html.push('</td>')
                html.push(
                    '<td rowspan="' + ouRowspan + '" class="js-dataFullTimeTrader"> \
                    ' + this.generateAssignments(this.matchInfo.Assignment.OverUnder) + ' \
                    ' + this.generateOtherAssignments(this.matchInfo.Assignment.OverUnderOthers) + ' \
                    </td>'
                );
                html.push('<td rowspan="' + ouRowspan + '" class="bg-maxbet js-dataFirstHalfTrader">');
                var htmlMbcOverUnder1st = this.generateMaxbet(overUnder1stMbcToDisplay);
                if (htmlMbcOverUnder1st !== "") {
                    html.push(htmlMbcOverUnder1st);
                }
                html.push('</td>')
                html.push(
                    '<td rowspan="' + ouRowspan + '" class="js-dataFirstHalfTrader"> \
                    ' + this.generateAssignments(this.matchInfo.Assignment.OverUnder1st) + ' \
                    ' + this.generateOtherAssignments(this.matchInfo.Assignment.OverUnder1stOthers) + ' \
                    </td>'
                );
                html.push('<td class="' + (ouSpecificBetTypeGroupList[0].IsMajorBetType ? 'bg1x2' : 'bg-mins') + ' center">' + ouSpecificBetTypeGroupList[0].BetTypeName + '</td>');
                html.push('<td class="bg-maxbet js-dataMaxbet1x2">');
                var htmlMbcBetTypesGroupList = this.generateMaxbet(ouSpecificBetTypeGroupList[0].MaxbetSpread);
                if (htmlMbcBetTypesGroupList !== "") {
                    html.push(htmlMbcBetTypesGroupList);
                }
                html.push('</td>')
                html.push(
                    '<td class="js-dataSpecialProduct"> \
                    ' + this.generateAssignments(ouSpecificBetTypeGroupList[0].MainAssignment) + ' \
                    ' + this.generateOtherAssignments(ouSpecificBetTypeGroupList[0].OthersAssignment) + ' \
                    </td>'
                );
                html.push('</tr>');

                for (var i = 1; i < ouSpecificBetTypeGroupList.length; i++) {
                    html.push(openingTR);
                    html.push('<td class="' + (ouSpecificBetTypeGroupList[i].IsMajorBetType ? 'bg1x2' : 'bg-mins') + ' center">' + ouSpecificBetTypeGroupList[i].BetTypeName + '</td>');
                    html.push('<td class="bg-maxbet js-dataMaxbet1x2">');
                    var htmlMbcOuSpecificBetTypeGroupList = this.generateMaxbet(ouSpecificBetTypeGroupList[i].MaxbetSpread);
                    if (htmlMbcOuSpecificBetTypeGroupList !== "") {
                        html.push(htmlMbcOuSpecificBetTypeGroupList);
                    }
                    html.push('</td>')
                    html.push(
                        '<td class="js-dataSpecialProduct"> \
                        ' + this.generateAssignments(ouSpecificBetTypeGroupList[i].MainAssignment) + ' \
                        ' + this.generateOtherAssignments(ouSpecificBetTypeGroupList[i].OthersAssignment) + ' \
                        </td>'
                    );
                    html.push('</tr>');
                }
            }

            return html.join('');
        }
    }

    return {
        Initialize: function () {
            $('.js-clearAllFilter').addClass('is-disabled');
            registerEvent();
            var filterId = parseInt(domElements.formFilter.find('#FilterId').val());

            if (filterId > 0) {
                domElements.applyBtn.trigger('click');
            }
        }
    };
}($);

$(document).ready(function () {
    eventInformation.Initialize();
});
