import { useCallback, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Loading from '../../../components/Loading';
import { RootState } from '../../app/storeNative';
import { queryProviderArrangement, setProviders } from '../ProviderArrangementSlice';
import { ProviderData } from '../TypeNative';
import SortListNative from './SortListNative';

const ProviderArrangementTable = () => {
  const { isLoading, providers, isEdit } = useSelector((state: RootState) => state.providerReducer);
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(queryProviderArrangement());
  }, [dispatch]);

  const setData = useCallback(
    (data: ProviderData[]) => {
      const newData = data.map((x, i: number) => {
        return {
          id: x.id,
          value: x.value,
          order: i + 1
        } as ProviderData;
      });
      dispatch(setProviders(newData));
    }, [dispatch]);

  if (isLoading) {
    return <Loading />;
  }

  return (
    <SortListNative
      isEdit={isEdit}
      data={providers}
      setData={setData}
    />
  );
};

export default ProviderArrangementTable;