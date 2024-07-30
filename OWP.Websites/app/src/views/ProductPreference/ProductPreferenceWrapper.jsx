import { useEffect } from 'react';
import store from './app/storeNative';
import { Provider, useSelector } from 'react-redux';
import Filter from './features/filter/Filter';
import { OWPPage, OWPHeader, OWPBody } from '../Shared/common/OWP/OPWPage';
import ProductPreferenceTable from './features/productPreference/table/ProductPreferenceTable';
import LastUpdate from './features/lastUpdate/LastUpdate';
import usePermission from '../Shared/customHook/usePermission';
import { functionKey } from './Const.json';
import { REQUEST_STATUS } from '../Shared/Constants';
import { isEqual } from 'lodash';
import Loading from '../components/Loading';
import useStableDispatch from '../Shared/customHook/useStableDispatch';
import { getDropdownOption } from './features/filter/filterThunk';

const ProductPreferenceWrapper = () => {
  const loading = useSelector(state => state.filter.loading);

  const modelPermission = usePermission(functionKey);
  const dispatch = useStableDispatch();

  const { permission } = modelPermission;
  useEffect(() => {
    dispatch(getDropdownOption());
  }, [dispatch]);

  if (!permission.Username || isEqual(loading, REQUEST_STATUS.pending)) {
    return <Loading />;
  }
  return (
    <Provider store={store}>
      <OWPPage className='product-assignment'>
        <OWPHeader className='product-assignment'>
          <Filter />
          <LastUpdate />
        </OWPHeader>
        <OWPBody className='product-assignment'>
          <ProductPreferenceTable />
        </OWPBody>
      </OWPPage>
    </Provider>
  );
};

export default ProductPreferenceWrapper;
