import store from './app/storeNative';
import { Provider } from 'react-redux';
import LiveGameListManagementNative from './LiveGameListManagementNative';
import useSignalRWrapper from './signalR/useSignalRWrapper';

const TicketCenterAppNative = () => {
  return (
    <Provider store={store}>
      <LiveGameListManagementNative />
      {useSignalRWrapper()}
    </Provider>
  );
};

export default TicketCenterAppNative;
