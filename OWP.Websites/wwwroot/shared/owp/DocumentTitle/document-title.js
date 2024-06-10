/* global messageBoxFactory */
"use strict";
let blinkTitleInterval = null;
const originalTitle = 'Operation Working Page';
var documentTitle = function ($) {
    function displayMessageUnread(totalMessageUnread) {
        let favicon = new Favico({
            animation: 'none',
            type: 'circle'
        });
        if (totalMessageUnread) {
            favicon.badge(totalMessageUnread);
        }
    }

    function buildMessageUnread(totalMessageUnread) {
        let messageDisplay =  null;
        if (totalMessageUnread > 99) {
            messageDisplay = "99+";
        }
        if (totalMessageUnread > 0 && totalMessageUnread <= 99) {
            messageDisplay = totalMessageUnread;
        }
        return messageDisplay;
    }

    function executeBlink(message) {
        if ([222, 224].some(id => id === message.messageType) && message.isNewMessage) {

            blinkTitleInterval = setInterval(function () {
                document.title = originalTitle === document.title
                    ? "New message"
                    : originalTitle;
            }, 1000);
        }
    }
    function blinkDocumentTitle(message) {
        clearInterval(blinkTitleInterval);
        
        const totalMessageUnread = buildMessageUnread(message.totalMessageUnread);

        displayMessageUnread(totalMessageUnread);

        executeBlink(message);

    };
    function countUnreadMessage() {
        documentTitleFactory
            .CountUnreadMessage()
            .success(function (data) {
                var favicon = new Favico({
                    animation: 'none',
                    type: 'circle'
                });
                const totalMessageUnread = buildMessageUnread(data);
                if (totalMessageUnread) {
                    favicon.badge(totalMessageUnread);
                }
            });
    }

    return {
        BlinkDocumentTitle: blinkDocumentTitle,
        CountUnreadMessage: countUnreadMessage
    };
}($);

document.addEventListener('visibilitychange', function () {
    if (document.visibilityState === 'visible') {
        clearInterval(blinkTitleInterval);
        document.title = originalTitle;
    }
});

