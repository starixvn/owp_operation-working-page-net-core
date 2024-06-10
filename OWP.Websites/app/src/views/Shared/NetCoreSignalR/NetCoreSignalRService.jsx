import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

const configSignalR = hubName => {
  const url =
    process.env.NODE_ENV === 'development'
      ? `/${hubName}`
      : `${window.netCoreRootUrl}/${hubName}`;

  const connection = new HubConnectionBuilder()
    .withUrl(url)
    .configureLogging(LogLevel.Information)
    .withAutomaticReconnect()
    .build();

  return connection;
};

export { configSignalR };
