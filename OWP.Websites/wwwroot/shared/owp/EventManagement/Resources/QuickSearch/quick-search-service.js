/* global quickSearchFactory */
/* exported quickSearchService */
'use strict';

var quickSearchService = function ($) {
    return {
        GetSearchSuggestion: function (param, cb) {
            return quickSearchFactory.GetSearchSuggestion(param).success(cb);
        }
    };
}(jQuery);