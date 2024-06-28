/*jshint strict: false*/
/* global  common */
/* exported documentTitleFactory */

var documentTitleFactory = (function ($) {
    var urls = {
        CountUnreadMessage: `${location.origin}/api/application/title/unread-message`
    };

    return {
        CountUnreadMessage: function () {
            return $.ajax({
                url: common.ResolveRootUrl(urls.CountUnreadMessage),
                dataType: 'json',
                data: {},
                method: 'GET',
                error: common.HandleAjaxError
            });
        }
    };
})(jQuery);