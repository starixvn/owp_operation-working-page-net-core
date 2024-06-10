import { Provider } from 'react-redux';
import store from './app/storeNative';
import ProviderVerificationAppNative from './ProviderVerificationAppNative';
import useSignalRWrapper from './feature/signalR/useSignalRWrapper';

const ProviderVerificationNative = () => {
  return (
    <Provider store={store}>
      <ProviderVerificationAppNative />
      {useSignalRWrapper()}
    </Provider>
  );
};

export default ProviderVerificationNative;
