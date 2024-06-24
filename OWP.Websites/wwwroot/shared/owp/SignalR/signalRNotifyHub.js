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
            await connection.start();
            console.log('SignalR Connected.');
        } catch (err) {
            console.log(err);
            setTimeout(start, 5000);
        }
    }

    connection.onclose(async () => {
        await start();
    });

    start();
})();