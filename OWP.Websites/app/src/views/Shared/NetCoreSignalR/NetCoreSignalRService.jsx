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
    .configureLogging(LogLevel.Information)
    .build();
  (async () => {
    await connection.start();
  })();

  return connection;
};

export { configSignalR };
