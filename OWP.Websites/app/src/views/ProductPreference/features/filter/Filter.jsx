import { useState, useEffect, useCallback } from 'react';
import Loading from '../../../components/Loading';
import { getProductPreference } from '../productPreference/productThunkNative';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import FilterRole from './FilterRole';
import FilterSection from './FilterSection';
import FilterLocation from './FilterLocation';
import FilterUser from './FilterUser';
import { useSelector } from 'react-redux';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { isEmpty } from 'lodash';

const buildAlertMessage = type => {
  ToastrWrapper.showError(
    <h4>Please select at least one option for {type}</h4>
  );
};

const Filter = () => {
  const [loading, setLoading] = useState(true);
  const { filterInfo } = useSelector(state => state.filter);
  const dispatch = useStableDispatch();

  useEffect(() => {
    (async () => {
      dispatch(getProductPreference());
      setLoading(false);
    })();
  }, [dispatch]);

  const handleSearch = useCallback(() => {
    const { RoleId, SectionId, LocationId } = filterInfo;
    if (isEmpty(RoleId)) {
      buildAlertMessage('Role');
      return;
    }
    if (isEmpty(SectionId)) {
      buildAlertMessage('Section');
      return;
    }
    if (isEmpty(LocationId)) {
      buildAlertMessage('Location');
      return;
    }
    dispatch(getProductPreference());
  }, [dispatch, filterInfo]);

  if (loading) {
    return <Loading />;
  }
  return (
    <div className='filter '>
      <FilterUser />
      <FilterRole />
      <FilterSection />
      <FilterLocation />
      <button className='button-submit' onClick={handleSearch}>
        Search
      </button>
    </div>
  );
};

export default Filter;
