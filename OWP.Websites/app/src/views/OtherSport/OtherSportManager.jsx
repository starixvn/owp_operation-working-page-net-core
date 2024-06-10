import { useEffect, useState } from 'react';
import { OWPBody, OWPHeader } from '../Shared/common/OWP/OPWPage';
import AssignmentUpload from './features/AssignmentUpload';
import store from './app/store';
import { Provider } from 'react-redux';
import OSTable from './features/OSTable/OSTable';
import FilterHeader from './features/FilterHeader/FilterHeader';
import getPermission from '../Shared/permission/getPermission';
import { FunctionKeys } from '../Shared/Constants';
import LastUpdated from './features/LastUpdate/LastUpdated';
import AutoRefresh from './features/FilterHeader/AutoRefresh';
import Loading from '../components/Loading';

const OtherSportManager = () => {
  const [policy, setPolicy] = useState({});
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    (async functionKey => {
      const policyResult = await getPermission(functionKey);
      setPolicy(policyResult);
      setLoading(false);
    })(FunctionKeys.OSLiveGameAssignment);
  }, []);

  if (loading) {
    return <Loading />;
  }

  return (
    <Provider store={store}>
      <OWPHeader className='other-sport'>
        <FilterHeader policy={policy} />
      </OWPHeader>
      <div className='upload-wrapper'>
        <LastUpdated />
      </div>
      <OWPBody className='other-sport'>
        <OSTable className='other-sport-table' />
      </OWPBody>
      <AutoRefresh />
    </Provider>
  );
};

export default OtherSportManager;
