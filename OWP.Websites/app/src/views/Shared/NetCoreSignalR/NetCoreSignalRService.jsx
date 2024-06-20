import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { baseApiUrl } from '../Constants';

const configSignalR = hubName => {
  const url = `${baseApiUrl}/${hubName}`;

  const connection = new HubConnectionBuilder()
    .withUrl(url)
    .configureLogging(LogLevel.Information)
    .withAutomaticReconnect()
    .build();

  return connection;
};

export { configSignalR };
