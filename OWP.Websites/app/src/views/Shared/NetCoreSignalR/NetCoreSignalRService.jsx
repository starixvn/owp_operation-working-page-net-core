import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

const configSignalR = hubName => {
  const url = `/${hubName}`;

  const connection = new HubConnectionBuilder()
    .withUrl(url)
    .configureLogging(LogLevel.Information)
    .withAutomaticReconnect()
    .build();

  return connection;
};

export { configSignalR };
