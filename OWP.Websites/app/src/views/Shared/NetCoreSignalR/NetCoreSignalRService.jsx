import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { baseApiUrl } from '../Constants';

const configSignalR = hubName => {
  const url = `${baseApiUrl}/${hubName}`;

  const connection = new HubConnectionBuilder()
    .withUrl(url)
    .configureLogging(LogLevel.Information)
    .withAutomaticReconnect()
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

  return connection;
};

export { configSignalR };
