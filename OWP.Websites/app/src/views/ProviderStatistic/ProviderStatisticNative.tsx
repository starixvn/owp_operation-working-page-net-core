import { Provider } from 'react-redux';
import { OWPBody, OWPPage } from '../Shared/common/OWP/OPWPage';
import Filter from './features/filter/Filter';
import store from './app/storeNative';

const ProviderStatisticNative = () => {
  return (
    <Provider store={store}>
      <OWPPage>
        <OWPBody className='provider-statistic'>
          <Filter />
        </OWPBody>
      </OWPPage>
    </Provider>
  );
};
export default ProviderStatisticNative;
