import store from './app/storeNative';
import { Provider } from 'react-redux';
import useSignalRWrapper from './features/signalR/useSignalRWrapper';
import TabSwitcher from './navigation/TabSwitcher';

const TicketCenterAppNative = () => {
  return (
    <Provider store={store}>
      <TabSwitcher />
      {useSignalRWrapper()}
    </Provider>
  );
};

export default TicketCenterAppNative;
