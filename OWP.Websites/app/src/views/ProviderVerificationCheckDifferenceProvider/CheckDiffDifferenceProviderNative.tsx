import { Provider } from 'react-redux';
import store from './app/storeNative';
import CheckDiffDifferenceProviderAppNative from './CheckDiffDifferenceProviderAppNative';
import useSignalRWrapper from './feature/signalR/useSignalRWrapper';

const CheckDiffDifferenceProviderNative = () => {

  return (
    <Provider store={store}>
      <CheckDiffDifferenceProviderAppNative />
      {useSignalRWrapper()}
    </Provider>
  );
};

export default CheckDiffDifferenceProviderNative;