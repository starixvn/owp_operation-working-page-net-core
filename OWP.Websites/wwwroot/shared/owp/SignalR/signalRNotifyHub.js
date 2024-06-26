(function () {
    const hub = {
        method: 'notifyChangedInformationData',
        hubName: 'changedInfoNotifyHub'
    };

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(`/${hub.hubName}`)
        .configureLogging(signalR.LogLevel.Information)
        .build();
    async function start() {
        try {
            await connection?.start().then(() => {
                connection.on(hub.method, data => {
                    toastr.options = {
                        'newestOnTop': false,
                        "closeButton": true,
                        "timeOut": 0,
                        "extendedTimeOut": 5000,
                        "positionClass": "toast-bottom-right"
                    };
                    const message = data.message;

                    if (message.isImportant) {
                        const isBlink = message.isBlinking;
                        pushNotification(message);

                        var messageContent = messageBox.RenderOSAssigneeMessageContent(message.messageType, message.messageContent);
                        messageContent = messageBox.RenderPrematchNotification(message.messageType, messageContent);
                        messageContent = messageBox.RenderTicketCenterNotification(message.messageType, messageContent);
                        messageContent = messageBox.RenderAssigneeMessageContent(message.messageType, messageContent);
                        messageContent = '<p class="message-content">' + messageContent + '</p>';
                        messageContent = messageContent.replaceAll('&lt;', '<').replaceAll('&gt;', '>');
                        documentTitle.BlinkDocumentTitle(message);
                        switch (message.messageType) {
                            case 1:
                            case 8:
                                toastr.assigned(messageContent);
                                break;
                            case 2:
                            case 9:
                                toastr.removed(messageContent);
                                break;
                            case 6:
                            case 7:
                                toastr.cancelled(messageContent);
                                break;
                            case 101:
                            case 102:
                            case 105:
                            case 106:
                                toastr.assigned(messageContent);
                                break;
                            case 103:
                            case 104:
                                toastr.removed(messageContent);
                                break;
                            case 108:
                                toastr.cancelled(messageContent);
                                break;
                            case 201:
                            case 203:
                            case 204:
                            case 206:
                            case 207:
                            case 208:
                            case 211:
                            case 212:
                            case 213:
                            case 220:
                            case 221:
                            case 222:
                            case 224:
                                toastr.success(messageContent, undefined, undefined, isBlink);
                                break;
                            case 202:
                            case 205:
                            case 209:
                                toastr.error(messageContent, undefined, undefined, isBlink);
                                break;
                            case 210:
                                toastr.warning(messageContent, undefined, undefined, isBlink);
                                break;
                            default:
                                toastr.info(messageContent);
                                break;
                        }
                    }
                });
            });
        } catch (err) {
            setTimeout(start, 5000);
        }
    };

    connection.onclose(async () => {
        await start();
    });

    // Start the connection
    start();
})();