import { useEffect, useCallback } from 'react';
import { useDispatch, useSelector, useStore } from 'react-redux';
import { postData } from '../actions/action';
import Loading from '../../components/Loading';
import { SpecialProductConst } from '../constants/SpecialProductConst';
import { SPECIALPRODUCT_QUERY } from '../constants/action-type';
import { selectSpecialProductData } from '../selector/SelectorSpecialProduct';
import SpecialProductTable from './specialProducts/table/SpecialProductTable';
import NewCategoryButton from './specialProducts/table/NewCategoryButton';
import { buildQuerySimpleApi } from '../../Shared/Constants';

const SpecialProduct = () => {
  const dispatch = useDispatch();
  const selector = useSelector(selectSpecialProductData);
  const stableDispatch = useCallback(dispatch, [dispatch]);
  const store = useStore();
  const { policyData } = store.getState().policyReducer;
  const hasWrite = policyData.generalPolicy.HasWrite;

  const handleQuery = useCallback(() => {
    const model = {
      apiKey: SpecialProductConst.specialProductGetAllAPIKey
    };
    stableDispatch(
      postData(buildQuerySimpleApi(), model, SPECIALPRODUCT_QUERY)
    );
  }, [stableDispatch]);

  useEffect(() => {
    handleQuery();
  }, [handleQuery]);

  if (selector.specialProductPending) {
    return <Loading />;
  }

  return (
    <>
      <div>{hasWrite && <NewCategoryButton />}</div>
      <SpecialProductTable />
    </>
  );
};

export default SpecialProduct;
