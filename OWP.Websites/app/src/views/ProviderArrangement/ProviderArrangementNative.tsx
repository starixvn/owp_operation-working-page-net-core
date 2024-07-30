import { Provider } from 'react-redux';
import { OWPPage } from '../Shared/common/OWP/OPWPage';
import store from './app/storeNative';
import ActionHeader from './features/actionHeader/ActionHeader';
import ProviderArrangementTable from './features/table/ProviderArrangementTable';

const ProviderArrangement = () => {
  return (
    <Provider store={store}>
      <OWPPage className='product-arrangement'>
        <ActionHeader />
        <ProviderArrangementTable />
      </OWPPage>
    </Provider>
  );
};

export default ProviderArrangement;
