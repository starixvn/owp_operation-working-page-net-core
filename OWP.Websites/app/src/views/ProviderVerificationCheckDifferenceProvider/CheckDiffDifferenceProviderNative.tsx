import { Provider } from 'react-redux';
import store from './app/storeNative';
import CheckDiffDifferenceProviderAppNative from './CheckDiffDifferenceProviderAppNative';

const CheckDiffDifferenceProviderNative = () => {

  return (
    <Provider store={store}>
      <CheckDiffDifferenceProviderAppNative />
    </Provider>
  );
};

export default CheckDiffDifferenceProviderNative;