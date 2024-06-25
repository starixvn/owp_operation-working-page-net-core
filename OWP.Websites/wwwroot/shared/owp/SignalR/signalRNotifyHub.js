(function () {
    const hub = {
        method: 'notifyChangedInformationData',
        hubName: 'changedInfoNotifyHub'
    };

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(`/${hub.hubName}`, {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
        })
        .configureLogging(signalR.LogLevel.Error)
        .build();

    async function start() {
        try {
            await connection.start();
            console.log('[Net FW] SignalR Connected.');
        } catch (err) {
            console.log(err);
            //setTimeout(start, 5000);
        }
    }

    connection.onclose(async () => {
        await start();
    });

    start();
})();