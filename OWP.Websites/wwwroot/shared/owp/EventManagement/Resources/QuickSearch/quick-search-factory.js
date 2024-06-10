/* global common */
/* exported quickSearchFactory */
'use strict';

var quickSearchFactory = function ($) {
    var urls = {
        GetSearchSuggestion: "api/application/quicksearch",
    };

    return {
        GetSearchSuggestion: function (criteria) {

            return $.ajax({
                url: common.ResolveRootUrl(urls.GetSearchSuggestion),
                data: criteria,
                method: 'POST',
                error: common.HandleAjaxError
            });
        }
    };
}(jQuery);