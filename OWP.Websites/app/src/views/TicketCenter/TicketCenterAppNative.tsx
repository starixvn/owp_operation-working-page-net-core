import store from './app/storeNative';
import { Provider } from 'react-redux';
import TicketCenterManagementNative from './TicketCenterManagementNative';
import useSignalRWrapper from './features/signalR/useSignalRWrapper';

const TicketCenterAppNative = () => {
  return (
    <Provider store={store}>
      <TicketCenterManagementNative />
      {useSignalRWrapper()}
    </Provider>
  );
};

export default TicketCenterAppNative;
