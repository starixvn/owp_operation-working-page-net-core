/* global messageBoxFactory */
"use strict";

var messageBox = function ($) {
    var messageResources = {
        NoMessage: "No new message.",
        Loading: "Loading...",
        MessageType: {
            Notification: 1,
            Announcement: 2,
            Message: 3
        }
    };

    var isCompletedFetchAssigneeMessage = true;
    var isCompletedFetchWatcherMessage = true;
    var totalNotViewedAssigneeMessage = 0;
    var totalNotViewedWatcherMessage = 0;
    function encodeHTML(htmlText) {
        return $('<div />').text(htmlText).html();
    }
    function displayNumberNotViewedMessages() {
        var total = totalNotViewedAssigneeMessage + totalNotViewedWatcherMessage;
        if (total === 0) {
            $('.js-badge-notification').text('').hide();
        } else {
            var labelTotal = total + '';
            if (total > 99) {
                labelTotal = '99+';
            }
            $('.js-badge-notification').text(labelTotal).show();
        }
    }

    function displayUnseenMessageAssigneeTab(total) {
        if (total === 0) {
            $('.js-assignee-message-badge').text('').hide();
        } else {
            var labelTotal = total + '';
            $('.js-assignee-message-badge').text(labelTotal).show();
        }
    }

    function displayUnseenMessageWatcherTab(total) {
        if (total === 0) {
            $('.js-watcher-message-badge').text('').hide();
        } else {
            var labelTotal = total + '';
            $('.js-watcher-message-badge').text(labelTotal).show();
        }
    }

    function loadAssigneeMessages(numberMessage, isReload) {
        isCompletedFetchAssigneeMessage = false;
        var assigneeMessages = $('.js-assignee-messages');

        if (isReload) {
            assigneeMessages.empty();
        }
        var model = {
            SelectTop: numberMessage ? numberMessage : 0,
            IsWatcher: false
        };

        messageBoxFactory.GetListMessage(model).success(function (data) {
            totalNotViewedAssigneeMessage = data.TotalNotViewMessage;
            displayUnseenMessageAssigneeTab(totalNotViewedAssigneeMessage);
            displayNumberNotViewedMessages();

            if (data.Messages.length !== 0) {
                var dataLength = data.Messages.length;
                var messagesHtml = "";

                for (var i = 0; i < dataLength; i++) {
                    var message = data.Messages[i];
                    messagesHtml += renderAssigneeMessageItem(message);
                }

                assigneeMessages.append(messagesHtml);
            } else {
                if (isReload) {
                    assigneeMessages.append(renderSimpleMessage('No new message.'));
                }
            }
        }).complete(function () {
            isCompletedFetchAssigneeMessage = true;
        });
    }

    function loadWatcherMessages(numberMessage, isReload) {
        isCompletedFetchWatcherMessage = false;
        var watcherMessages = $('.js-watcher-messages');

        if (isReload) {
            watcherMessages.empty();
        }

        var model = {
            SelectTop: numberMessage,
            IsWatcher: true
        };

        messageBoxFactory.GetListMessage(model).success(function (data) {
            totalNotViewedWatcherMessage = data.TotalNotViewMessage;
            displayUnseenMessageWatcherTab(totalNotViewedWatcherMessage);
            displayNumberNotViewedMessages();

            if (data.Messages.length !== 0) {
                var dataLength = data.Messages.length;
                var messagesHtml = "";

                for (var i = 0; i < dataLength; i++) {
                    var message = data.Messages[i];

                    messagesHtml += renderWatcherMessageItem(message);
                }

                watcherMessages.append(messagesHtml);
            } else {
                if (isReload) {
                    watcherMessages.append(renderSimpleMessage('No new message.'));
                }
            }
        }).complete(function () {
            isCompletedFetchWatcherMessage = true;
        });
    }

    function updateViewedAssigneeMessage() {
        messageBoxFactory.UpdateMessagesStatus({
            IsWatcher: false
        }).success(function () {
            totalNotViewedAssigneeMessage = 0;
            displayUnseenMessageAssigneeTab(0);
            displayNumberNotViewedMessages();
        });
    }

    function updateViewedWatcherMessage() {
        messageBoxFactory.UpdateMessagesStatus({
            IsWatcher: true
        }).success(function () {
            totalNotViewedWatcherMessage = 0;
            displayUnseenMessageWatcherTab(0);
            displayNumberNotViewedMessages();
        });
    }

    function registerViewNotificationEvent() {
        $('.js-dropdown-notification').on('click', function () {
            //if ($('#dropdown-notification li').length > 11) {
            //    $('#dropdown-notification li:nth-child(11)').nextAll().remove();
            //    $('#dropdown-notification').scrollTop(0);
            //}

            if ($('.js-badge-notification').is(':visible')) {
                if ($('#assigneeMessageTab').is(':checked')) {
                    updateViewedAssigneeMessage();
                }

                if ($('#watcherMessageTab').is(':checked')) {
                    updateViewedWatcherMessage();
                }
            }
        });

        $('#assigneeMessageTab').on('click', function () {
            if (totalNotViewedAssigneeMessage !== 0) {
                updateViewedAssigneeMessage();
            }
        });

        $('#watcherMessageTab').on('click', function () {
            if (totalNotViewedWatcherMessage !== 0) {
                updateViewedWatcherMessage();
            }
        });
    }

    function renderAssigneeMessageItem(message) {
        var iconImportant = (message.IsImportant) ? '<i class="icon icon-star mdi mdi-star orange-text" aria-hidden="true"></i>' : '';

        var formattedMessageContent = message.MessageType < 100
            ? renderAssigneeMessageContent(message.MessageType, message.MessageContent)
            : renderOSAssigneeMessageContent(message.MessageType, message.MessageContent);
        formattedMessageContent = renderPrematchNotification(message.MessageType, formattedMessageContent);
        formattedMessageContent = renderTicketCenterNotification(message.MessageType, formattedMessageContent);

        formattedMessageContent = '<p class="message-content">' + formattedMessageContent + '</p>';

        return '<div class="noti-item">'
            + renderMessageIcon(message.SystemMessageType, message.ProfilePic)
            + '<div class="notification-content left">'
            + formattedMessageContent
            + '<p class="text-small">' + message.TimeAgo + '</p>'
            + '</div>'
            + iconImportant
            + '<input class="messageId" name="messageId" type="hidden" value="' + message.MessageId + '"></div>';
    }

    function renderTicketCenterNotification(messageType, messageContent) {
        switch (messageType) {
            case 220:
                messageContent = messageContent.replace('New ticket', '<span class="new-league">New ticket</span>');
                break;
            case 221:
            case 222:
                messageContent = messageContent.replace(
                    'New message',
                    '<span class="new-message-title">New message</span>'
                );
                break;
            case 224:
                messageContent = messageContent.replace(
                    'New attachment',
                    '<span class="new-attachment-title">New attachment</span>'
                );
                break;
        }
        return messageContent;
    }

    function renderAssigneeMessageContent(messageType, messageContent) {
        var escapedMessageContent = $("<div>").text(messageContent).html();

        switch (messageType) {
            case 1:
            case 8:
                escapedMessageContent = escapedMessageContent.replace('new ASSIGNED', '<span class="match-assigned">NEW ASSIGNED</span>');
                break;
            case 2:
            case 9:
                escapedMessageContent = escapedMessageContent.replace('REMOVED', '<span class="match-unassigned">REMOVED</span>');
                break;
            case 10:
                escapedMessageContent = escapedMessageContent.replace('EDITED', '<span class="match-edited">EDITED</span>');
                break;
            case 6:
            case 7:
                escapedMessageContent = escapedMessageContent
                    .replace('Cancel', '<span class="match-cancel">CANCEL</span>')
                    .replace('Postponed', '<span class="match-postponed">POSTPONED</span>')
                    .replace('Suspended', '<span class="match-suspended">SUSPENDED</span>');
                break;
        }

        return escapedMessageContent;
    }

    function renderOSAssigneeMessageContent(messageType, messageContent) {
        switch (messageType) {
            case 101:
            case 102:
                messageContent = messageContent.replace('NEW ASSIGNMENT', '<span class="match-assigned">NEW ASSIGNMENT</span>');
                break;
            case 103:
            case 104:
                messageContent = messageContent.replace('REMOVED', '<span class="match-unassigned">REMOVED</span>');
                break;
            case 105:
            case 106:
                messageContent = messageContent.replace('UPDATED ASSIGNEE', '<span class="match-assigned">UPDATED ASSIGNEE</span>');
                break;
            case 108:
                messageContent = messageContent
                    .replace('Canceled', '<span class="match-cancel">CANCELED</span>')
                    .replace('Postponed', '<span class="match-postponed">POSTPONED</span>')
                    .replace('Live', '<span class="match-live">LIVE</span>');
                break;
        }

        return messageContent
    }

    function renderPrematchNotification(messageType, messageContent) {
        switch (messageType) {
            case 201:
            case 204:
                messageContent = messageContent.replace('NEW ASSIGNMENT', '<span class="match-assigned">NEW ASSIGNMENT</span>');
                break;
            case 202:
            case 205:
                messageContent = messageContent.replace('REMOVED', '<span class="match-unassigned">REMOVED</span>');
                break;
            case 207:
                messageContent = messageContent.replace('New Add', '<span class="new-league">New Add</span>');
                break;
            case 208:
            case 209:
            case 210:
                messageContent = messageContent
                    .replace('ADD LIVE', '<span class="add-live">ADD LIVE</span>')
                    .replace('CANCEL LIVE', '<span class="cancel-live">CANCEL LIVE</span>')
                    .replace('Cancel', '<span class="match-cancel">CANCELED</span>')
                    .replace('Postponed', '<span class="match-postponed">POSTPONED</span>')
                    .replace('Live', '<span class="match-live">LIVE</span>')
                    .replace('Suspended', '<span class="match-suspended">SUSPENDED</span>')
                    .replace('MATCH STATUS', '<span class="match-status-change">MATCH STATUS</span>')
                    ;
                break;
        }
        return messageContent;
    }

    function renderMessageIcon(messageType, profilePic) {
        var result = '';

        switch (messageType) {
            case messageResources.MessageType.Notification:
                result = '<i class="icon icon-circle mdi mdi-cloud-sync white-text light-blue lighten-2" aria-hidden="true"></i>';
                break;
            case messageResources.MessageType.Announcement:
                result = '<i class="icon icon-circle mdi mdi-bullhorn blue lighten-2 white-text" aria-hidden="true"></i>';
                break;
            case messageResources.MessageType.Message:
                if (string.IsNullOrEmpty(profilePic)) {
                    result = '<i class="icon icon-circle mdi mdi-account teal white-text lighten-3" aria-hidden="true"></i>';
                }
                else {
                    result = '<i class="icon icon-circle" aria-hidden="true"><img src="' + profilePic + '" /></i>';
                }
                break;
            default:
                result = '<i class="icon icon-circle mdi mdi-cloud-sync white-text light-blue lighten-2" aria-hidden="true"></i>';
                break;
        }

        return result;
    }

    function renderSimpleMessage(message) {
        return '<div class="noti-item">' + message + '</div>';
    }

    function renderWatcherMessageItem(message) {
        return '<div class="noti-item">'
            + renderMessageIcon(message.SystemMessageType, message.ProfilePic)
            + '<div class="notification-content left">'
            + '<p class="message-content">' + encodeHTML(message.MessageContent) + '</p>'
            + '<p class="text-small">' + message.TimeAgo + '</p>'
            + '</div>'
            + '<input class="watcherMessageId" name="watcherMessageId" type="hidden" value="' + message.MessageId + '"></div>';
    }

    function initialize() {
        $('#dropdown-notification').on('click', function (e) {
            e.stopPropagation();
        });

        loadAssigneeMessages(10, true);
        messageBoxFactory.GetFunctionPolicy('NotificationSetting_MySectionMessage').success(function (data) {
            if (data.HasRead) {
                loadWatcherMessages(10, true);
            } else {
                $('.js-watcher-tab-label').hide();
                $('.js-watcher-messages').hide();
            }
        });

        registerViewNotificationEvent();
    }

    return {
        Initialize: initialize,
        LoadAssigneeMessages: loadAssigneeMessages,
        LoadWatcherMessages: loadWatcherMessages,
        RenderAssigneeMessageContent: renderAssigneeMessageContent,
        RenderOSAssigneeMessageContent: renderOSAssigneeMessageContent,
        RenderPrematchNotification: renderPrematchNotification,
        RenderTicketCenterNotification: renderTicketCenterNotification
    };
}($);

$(document).ready(function () {
    var isIFrame = $('.body-content').attr('data-isiframe') === 'True';
    if (!isIFrame) {
        messageBox.Initialize();
    }
});