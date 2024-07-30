/* global common, quickSearchService, msgNoSearchResults */
'use strict';

var quickSearch = function ($) {
    var domElements = {
        txtSearchKeywords: $("#txtSearchKeywords"),
        ulSearchSuggestion: $("#ulSearchSuggestion")
    };

    var delayRequestTimer;
    var isCompleteFetchData = true;
    let matchSuggestion;

    function getSearchSuggestion(isReload) {
        var keywords = domElements.txtSearchKeywords.val();
        var isNumericKeywords = $.isNumeric(keywords)

        if (isNumericKeywords || keywords.length >= 2) {
            if (isReload) {
                domElements.ulSearchSuggestion.empty();
                $('.loading-process').css("display", "inline");
            }

            var criteria = {
                SelectTop: 15,
                LastOWPMatchId: isReload ? null : $('#ulSearchSuggestion li:last a.js-openModalLiveDetail').attr("match-id"),
                SearchKeywords: keywords,
                IsNumericKeywords: isNumericKeywords,
                IsReload: isReload
            };

            fetchSearchResults(criteria);
        } else {
            domElements.ulSearchSuggestion.empty();
        }
    }

    function fetchSearchResults(criteria) {
        isCompleteFetchData = false;

        quickSearchFactory.GetSearchSuggestion(criteria).success(function (results) {
            if (criteria.SearchKeywords === domElements.txtSearchKeywords.val()) {
                if (results.length > 0) {
                    generateSearchSuggestion(results, criteria.SearchKeywords);
                } else {
                    if (criteria.IsReload) {
                        generateNoResults();
                    }
                }
            }
        }).complete(function () {
            $('.loading-process').css("display", "none");
            isCompleteFetchData = true;
        });
    }

    function generateSearchSuggestion(data, keyword) {
        matchSuggestion = data;
        var totalItems = data.length,
            htmlStr = '';

        for (var i = 0; i < totalItems; i++) {
            var eventInfo = data[i];
            var eventDate = moment.parseZone(eventInfo.EventDate).format('MMM DD, YYYY');
            var eventKoTime = moment.parseZone(eventInfo.EventDate).format('hh:mm A');
            var isAllowedToViewDetail = common.hasDetailInformationPermission() && eventInfo.IsAllowedToViewDetail;
            var todayMatchNo = getMatchNoText(eventInfo.IsToday, eventInfo.MatchNo, keyword);
            var allAssignee = generateAllAssignee(eventInfo, keyword);
            var itemHtmlStr =
                    '<li>' +
                        '<a class="cd-nowrap ' + (!isAllowedToViewDetail ? 'cursordefault' : 'js-openModalLiveDetail') + '" href="javascript:;" match-id="' + eventInfo.OWPMatchId + '">' +
                            '<p> <time class="default-highlight">' + eventDate + '</time> </p>' +
                            '<p> ' +
                                todayMatchNo +
                                'K.O <span class="default-highlight">' + eventKoTime + '</span>' +
                                highlight(eventInfo.LeagueName, keyword) +
                            '</p>' +
                            '<span class="default-highlight">' + highlight(eventInfo.Event, keyword) + '</span>' +
                            '<div> ' +
                                (eventInfo.IsHasAssignee ? '<p class="assignee">' + allAssignee + '</p>' : '') +
                            '</div>' +
                        '</a>' +
                        '<button class="copy-btn btn btn-primary" match-id="' + eventInfo.OWPMatchId + '"><span class="icon-content_copy"></span></button>' +
                        '<div class="match-info" style="display:none">' +
                            eventDate + '\n' +
                            (eventInfo.IsToday && eventInfo.MatchNo.length > 0 ? 'Match No. ' + eventInfo.MatchNo + '\t' : '') + 'K.O ' + eventKoTime + '\t' + eventInfo.LeagueName + '\n' +
                            eventInfo.Event +
                            (allAssigneeInString ? '\n' + allAssigneeInString(eventInfo) : '') +
                        '</div>' +
                    '</li>';

            htmlStr += itemHtmlStr;
        }

        domElements.ulSearchSuggestion.append(htmlStr);
    }

    function allAssigneeInString(data) {
        var assignee = ''

        if (data.MainTrader) {
            assignee += data.MainTrader.PositionName + ':' + data.MainTrader.AssigneesNameConjunction;
        }

        if (data.OtherTrader) {
            assignee += ', ' + data.OtherTrader.PositionName + ':' + data.OtherTrader.AssigneesNameConjunction;
        }

        if (data.MainLA) {
            assignee += (data.MainTrader || data.OtherTrader ? ', ' : '') + data.MainLA.PositionName + ':' + data.MainLA.AssigneesNameConjunction;
        }

        if (data.OtherLA) {
            assignee += ', ' + data.OtherLA.PositionName + ':' + data.OtherLA.AssigneesNameConjunction;
        }

        return assignee;
    }

    function getMatchNoText(isToday, matchNo, keyword) {
        if (isToday) {
            return 'Match No. <span class="default-highlight">' + highlight(matchNo, keyword) + '</span>';
        }

        return '';
    }

    function highlight(text, keyword) {
        var result = text;
        var textInLower = text.toLowerCase();
        var keywordInLower = keyword.toLowerCase();
        if (textInLower.includes(keywordInLower)) {
            var startIndexOfKeyword = textInLower.indexOf(keywordInLower);
            var keywordResultInText = text.substr(startIndexOfKeyword, keyword.length);
            result = text.split(keywordResultInText).join('<span class="highlight">' + keywordResultInText + '</span>');
        }

        return result;
    }

    function generateAllAssignee(data, keyword) {
        var htmlStr = '';

        if (data.MainTrader) {
            htmlStr += generateAssignee(data.MainTrader, keyword);
        }

        if (data.OtherTrader) {
            htmlStr += generateAssignee(data.OtherTrader, keyword);
        }

        if (data.MainLA) {
            htmlStr += generateAssignee(data.MainLA, keyword);
        }

        if (data.OtherLA) {
            htmlStr += generateAssignee(data.OtherLA, keyword);
        }

        return htmlStr;
    }

    function generateAssignee(data, keyword) {
        return '<span class="assignee-item">'
            + '<span class="position"><span>' + data.PositionName + ':</span> </span> <span class="name">' + highlight(data.AssigneesNameConjunction, keyword)
            + '</span></span>';
    }

    function generateNoResults() {
        domElements.ulSearchSuggestion.append('<li class="no-results"><i class="mdi mdi-emoticon-sad icon"></i>' + msgNoSearchResults + '</li>');
    }

    function handlePageEvents() {
        domElements.txtSearchKeywords.on('input', function () {
            clearTimeout(delayRequestTimer);
            delayRequestTimer = null;
            delayRequestTimer = setTimeout(function () {
                getSearchSuggestion(true);
            }, 300);
        });

        domElements.txtSearchKeywords.keypress(function (event) {
            if (event.keyCode === 10 || event.keyCode === 13) {
                event.preventDefault();
            }
        });

        domElements.ulSearchSuggestion.on('click', '.copy-btn', function () {
            const owpMatchId = $(this).attr('match-id');
            const matchInfo = matchSuggestion.find(x => x.OWPMatchId == owpMatchId);
            const koTime = 'K.O Time: ' + moment.parseZone(matchInfo.EventDate).format('hh:mm A');
            const league = 'League: ' + matchInfo.LeagueName;
            const match = 'Match: ' + matchInfo.Event;
            const matchId = matchInfo.AdminSiteId
                ? 'Match ID: ' + matchInfo.AdminSiteId
                : 'Match ID: ';
            const iptv = matchInfo.IPTVCopy
                ? 'TV/IPTV: ' + matchInfo.IPTVCopy
                : 'TV/IPTV: ';
            const provider = matchInfo.Provider
                ? 'Provider: ' + matchInfo.Provider
                : 'Provider: ';
            executeCopy(koTime, league, match, matchId, iptv, provider);
          
        });

        domElements.ulSearchSuggestion.on('click', '.js-openModalLiveDetail', function () {
            var matchId = $(this).attr('match-id');
            common.modalLiveDetail(matchId);
        });

        domElements.ulSearchSuggestion.on('scroll', function () {
            if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
                if (isCompleteFetchData) {
                    getSearchSuggestion(false);
                }
            }
        });
    }

    return {
        Initialize: function () {
            handlePageEvents();
        }
    };
}(jQuery);

$(function () {
    quickSearch.Initialize();
});