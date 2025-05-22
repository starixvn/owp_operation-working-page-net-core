import { Provider } from 'react-redux';
import { OWPBody } from '../Shared/common/OWP/OPWPage';
import Filter from './features/filter/Filter';
import store from './app/storeNative';

const ProviderStatisticNative = () => {
  return (
    <Provider store={store}>
      <div className='body-content'>
        <OWPBody className='provider-statistics'>
          <Filter />
        </OWPBody>
      </div>
    </Provider>
  );
};
export default ProviderStatisticNative;
