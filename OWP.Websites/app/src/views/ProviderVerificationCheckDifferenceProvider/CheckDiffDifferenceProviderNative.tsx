import { Provider } from 'react-redux';
import store from './app/storeNative';
import ProviderUpload from './feature/uploadProvider/ProviderUpload';

const CheckDiffDifferenceProviderNative = () => {
  return (
    <div className='event-upload-container'>
      <Provider store={store}>
        <ProviderUpload />
      </Provider>
    </div>
  );
};

export default CheckDiffDifferenceProviderNative;
