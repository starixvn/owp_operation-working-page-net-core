/* File Created: September 14, 2016 */
/*jshint strict: false*/
/* global Namespace */
////'use strict';

if (!String.format) {
    String.format = function (format) {
        var args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] !== 'undefined'
                ? args[number]
                : match
                ;
        });
    };
}

function fanexOWPCommon() {
    var $this = this;

    $this.HasViewScorePermission = false;

    var messages = {
        NotFoundError: null,
        InternalError: null,
        AccessDeniedError: null
    };

    var urlCommon = {
        NotFoundError: 'Error/NotFound',
        InternalError: 'Error/InternalError',
        AccessDeniedError: 'Error/AccessDenied'
    };

    var domElements = {
        divMsgBox: null,
        divPopup: null
    };
    var cachedValues = {
        RootURL: ''
    };

    $this.Formats = {
        DateFormat: 'mm/dd/yy',
        MomentDateFormat: 'MM/DD/YYYY',
        IntegerFormat: '0,0',
        DecimalFormat: '0,0.00',
        DecimalFormat3Digits: '0,0.000',
        PercentFormat: '0,0.00%',
        MonthYearDateFormat: 'MM-yy',
        MonthFormat: 'MMM dd, yyyy',
        ShortDateFormat: 'MM/dd/yyyy'
    };

    $this.Constants = {
        PagerRanges: [50, 100, 200, 300, 500],
        PageSize: 50,
        NumOfVirtualRows: 100,
        VirtualThresholdNumber: 25
    };

    $this.HandleAjaxError = function (xhr, textStatus, errorThrown) {
        var url = urlCommon.InternalError;

        if (xhr.status === 401) {
            url = 'Auth?ReturnUrl=' + window.location.pathname;
            window.location = $this.ResolveRootUrl(url);
        } else if (xhr.status === 403) {
            url = urlCommon.AccessDeniedError;
        } else if (xhr.status === 404) {
            url = urlCommon.NotFoundError;
        } else if (xhr.status === 503) {
            url = 'Auth/Offline';
            window.location = $this.ResolveRootUrl(url);
        }
        else if(xhr.status=== 500)
        {
            if (xhr.responseJSON && xhr.responseJSON.Exception) {
                alert(xhr.responseJSON.Exception)
            }
            else if(xhr.responseText) {
                alert(xhr.responseText)
            }
            else {
                alert("Unknow error, please contact administrator!");
            }
        }
    };

    $this.ShowErrorMessage = function (message) {
        domElements.divMsgBox.showErr(message, { sticky: true, position: ['top: 40', 'center'] });
    };

    $this.ShowInfoMessage = function (message) {
        domElements.divMsgBox.showInfo(message, { duration: 3000, position: ['top: 40', 'center'] });
    };

    $this.ShowConfirmMessage = function (message, callbackFunc) {
        var options = {
            modal: true,
            closeBtn: false,
            header: true,
            css: 'custom-confirm'
        };

        callbackFunc = callbackFunc || function () { };
        domElements.divMsgBox.confirm(message, callbackFunc, options);
    };

    $this.CloseMsgBox = function () {
        domElements.divMsgBox.close();
    };

    // Show inline error message in the case of form or popup.
    $this.ShowInlineErrorMsg = function ($obj, message) {
        $obj.html(message);
        $obj.removeClass('success').removeClass('info').addClass('error').slideDown();
    };

    // Show inline success message in the case of form or popup.
    $this.ShowInlineSuccessMsg = function ($obj, message, autoHide, callbackFunc) {
        autoHide = autoHide === undefined ? true : autoHide;
        $obj.html(message);
        $obj.removeClass('error').removeClass('info').addClass('success').slideDown();

        if (autoHide) {
            setTimeout(function () {
                $this.HideInlineMsg($obj, callbackFunc);
            }, 3000);
        } else if (callbackFunc) {
            callbackFunc();
        }
    };

    // Show inline infomation message in the case of form or popup.
    $this.ShowInlineInfoMsg = function ($obj, message, autoHide, callbackFunc) {
        autoHide = autoHide === undefined ? true : autoHide;
        $obj.html(message);
        $obj.removeClass('error').removeClass('success').addClass('info').slideDown();

        if (autoHide) {
            setTimeout(function () {
                $this.HideInlineMsg($obj, callbackFunc);
            }, 3000);
        } else if (callbackFunc) {
            callbackFunc();
        }
    };

    // Hide inline message.
    $this.HideInlineMsg = function ($obj, callbackFunc) {
        callbackFunc = callbackFunc || function () { };
        $obj.removeClass('error').removeClass('info').slideUp(callbackFunc);
        $obj.html('');
    };

    $this.Modal = function (title, html, option) {
        domElements.divPopup.html(html).dialog({
            width: 'auto',
            modal: true,
            position: ['center', 160],
            title: title
        });

        if (option) {
            domElements.divPopup.dialog('option', option);
        }
    };

    $this.Popup = function (title, html, option) {
        domElements.divPopup.html(html).dialog({
            width: 'auto',
            modal: false,
            position: ['center', 160],
            title: title
        });

        if (option) {
            domElements.divPopup.dialog('option', option);
        }
    };

    $this.IsOpenPopup = function () {
        return domElements.divPopup.html().length > 0 && domElements.divPopup.dialog('isOpen');
    };

    $this.ClosePopup = function () {
        domElements.divPopup.dialog('close');
    };

    $this.DisableInput = function () {
        try { $.blockUI({ message: '' }); } catch (e) { }
    };

    $this.EnableInput = function () {
        try { $.unblockUI(); } catch (e) { }
    };

    $this.ResolveRootUrl = function (relativeUrl) {
        return cachedValues.RootURL + relativeUrl;
    };

    $this.NavigateToRoot = function (url) {
        window.location.replace($this.ResolveRootUrl(url));
    };

    $this.HtmlEncode = function (value) {
        return $('<div/>').text(value).html();
    };

    $this.HtmlDecode = function (value) {
        return $('<div/>').html(value).text();
    };

    $this.DownloadFile = function (url, parameters) {
        if (url && parameters) {
            var downloadUrl = url + '?';
            var iframe = $('body iframe');

            parameters = typeof parameters === 'string' ? parameters : $.param(parameters);
            downloadUrl += parameters;

            if (iframe.length === 0) {
                iframe = $('<iframe width="0" height="0" class="display-none"></iframe>');
                iframe.appendTo('body');
            }

            iframe.attr('src', downloadUrl);
        }
    };

    $this.GetNumWithLabel = function (number, nullDisplay) {
        if (number || number === 0) {
            return number;
        }
        else {
            return nullDisplay;
        }
    };

    $this.SetCookie = function (cname, cvalue, expiredays) {
        var expires = "";
        if (expiredays) {
            var date = new Date();
            date.setTime(date.getTime() + (expiredays * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        }

        document.cookie = cname + "=" + cvalue + expires + "; path=/";
    };

    $this.GetCookie = function (cname) {
        var nameEQ = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) === ' ')
                c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) === 0)
                return c.substring(nameEQ.length, c.length);
        }

        return null;
    };

    $this.EraseCookie = function (cname) {
        $this.SetCookie(cname, "", -1);
    };

    $this.modalInformationMessage = function (message) {
        $('#js-modalInformationMessage .js-message').html(message);

        $('#js-modalInformationMessage').openModal({
            dismissible: false,
            ready: function () {
                $(":input, a").attr("tabindex", "-1");
                $('.modal-action').focus();
            },
            complete: function () {
                $(":input, a").removeAttr("tabindex");
            }
        });
    };

    $this.modalWarningMessage = function (message) {
        $('#js-modalWarningMessage .js-message').html(message);

        $('#js-modalWarningMessage').openModal({
            dismissible: false,
            ready: function () {
                $(":input, a").attr("tabindex", "-1");
                $('.modal-action').focus();
            },
            complete: function () {
                $(":input, a").removeAttr("tabindex");
            }
        });
    };

    $this.modalErrorMessage = function (messages) {
        if ($.isArray(messages)) {
            if (messages.length > 1) {
                var ul = '<ul>';
                for (var i = 0; i < messages.length; i++) {
                    var li = '<li class="text-left">- ' + messages[i] + '</li>';
                    ul += li;
                }

                ul += '</ul>';

                $('#js-modalErrorMessage .js-message').html(ul);
            } else {
                $('#js-modalErrorMessage .js-message').html(messages[0]);
            }
        } else {
            $('#js-modalErrorMessage .js-message').html(messages);
        }

        $('#js-modalErrorMessage').openModal({
            dismissible: false,
            ready: function () {
                $(":input, a").attr("tabindex", "-1");
                $('.modal-action').focus();
            },
            complete: function () {
                $(":input, a").removeAttr("tabindex");
            }
        });
    };

    $this.hasDetailInformationPermission = function () {
        return $('#js-modalLiveDetail').length > 0;
    }

    $this.modalLiveDetail = function (matchId, callback) {
        var link = common.ResolveRootUrl('EventManagement/LiveDetail/Index/' + matchId);
        $('#js-modalLiveDetail iframe').attr('src', link);
        $('#js-modalLiveDetail').openModal({
            dismissible: false,
            onCloseStart: function () {
                console.log('onCloseStart');
                return false;
            },
        });
        function closeModal() {
            $('#js-modalLiveDetail').closeModal({
                complete: function () {
                    var updateResult = parseInt($('#js-modalLiveDetail iframe').attr('updated'));
                    if (updateResult > 0) {
                        $('#js-modalLiveDetail iframe').attr('updated', 0);
                        callback && callback();
                    }
                }
            });
        }

        $('#js-modalLiveDetail').find('.modal-close-x').on('click', function () {
            var editGeneralInfo = $('#js-modalLiveDetail iframe').attr('edit-general-info');
            var editMatchInfo = $('#js-modalLiveDetail iframe').attr('edit-match-info');
            var editTradingInfo = $('#js-modalLiveDetail iframe').attr('edit-trading-info');
            var editProviderInfo = $('#js-modalLiveDetail iframe').attr('edit-provider-info');

            var canClose = true;
            if ((editGeneralInfo && editGeneralInfo === '1') ||
                (editMatchInfo && editMatchInfo === '1') ||
                (editTradingInfo && editTradingInfo === '1') ||
                (editProviderInfo && editProviderInfo === '1')) {
                canClose = false;
            }
            if (canClose === false) {
                var msg = "Your unsaved changes will be lost. Are you sure?";

                Swal.fire({
                    text: msg,
                    type: 'question',
                    showCancelButton: true,
                    reverseButtons: true
                }).then((result) => {
                    if (result.value) {
                        closeModal();
                    }
                })
            }
            else {
                closeModal();
            }
        });
    }

    $this.InitPagination = function (el, totalItem, itemsPerPage, currentPage, onPageClick) {
        $(el).pagination({
            items: totalItem,
            itemsOnPage: itemsPerPage,
            currentPage: currentPage,
            displayedPages: 3,
            prevText: '<i class="icon mdi mdi-chevron-left"></i>',
            nextText: '<i class="icon mdi mdi-chevron-right"></i>',
            ellipsePageSet: false,
            onPageClick: function (pageNumber) {
                if (typeof onPageClick === 'function') {
                    onPageClick(pageNumber);
                }
            }
        });
    };

    $this.urlParam = function (name) {
        var results = new RegExp('[\?&]' + name + '=([^&#]*)')
            .exec(window.location.search);

        return (results !== null) ? results[1] || 0 : false;
    };

    $this.htmlEntities = function (str) {
        return String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    };
    
    $this.Initialize = function () {
        domElements.divPopup = $('#divPopup');
        domElements.divMsgBox = $.msgbox({
            message: '',
            type: 'err',
            modal: false,
            position: ['top:130', 'center'],
            theme: '',
            zIndex: 9999,
            useAnimation: false
        });

        messages.AccessDeniedError = $('#AccessDeniedError').val();
        messages.InternalError = $('#InternalError').val();
        messages.NotFoundError = $('#NotFoundError').val();

        cachedValues.RootURL = $('#RootURL').val();

        domElements.divPopup.on('dialogclose', function () {
            $this.CloseMsgBox();
        });

        $.ajaxSetup({
            headers: { '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val() }
        });
    };
}

var common = new fanexOWPCommon();

$(document).ready(function () {
    common.Initialize();
    // moment.tz.setDefault("America/Los_Angeles");
});