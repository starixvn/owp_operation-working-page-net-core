import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

const configSignalR = hubName => {
  const connection = new HubConnectionBuilder()
  .withUrl(`/${hubName}`, {
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
})
    .configureLogging(LogLevel.Error)
    .build();

  async function start() {
    try {
      await connection.start();
      console.log('[Net core] SignalR Connected.');
    } catch (err) {
      debugger
      console.log(err);
      //setTimeout(start, 5000);
    }
  }

  connection.onclose(async () => {
    await start();
  });

  return connection;
};

export { configSignalR };
