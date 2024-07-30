import store from './app/storeNative';
import { Provider } from 'react-redux';
import SmallTicketCenterManagementNative from './SmallTicketCenterManagementNative';
import useSignalRWrapper from './features/signalR/useSignalRWrapper';
import { SpinnerContainer } from '../components/Spinner';
const SmallTicketCenterAppNative = () => {
  return (
    <Provider store={store}>
      <SmallTicketCenterManagementNative />
      <SpinnerContainer />
      {useSignalRWrapper()}
    </Provider>
  );
};

export default SmallTicketCenterAppNative;
