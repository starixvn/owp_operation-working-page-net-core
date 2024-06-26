import {
  HubConnectionBuilder,
  LogLevel,
  HttpTransportType
} from '@microsoft/signalr';

const configSignalR = hubName => {
  const connection = new HubConnectionBuilder()
    .withUrl(`/${hubName}`, {
      skipNegotiation: true,
      transport: HttpTransportType.WebSockets
    })
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Error)
    .build();
  (async () => {
    try {
      await connection.start();
      console.log('[Net core] SignalR Connected.');
    } catch (err) {
      console.log(err);
    }
  })();

  return connection;
};

export { configSignalR };
