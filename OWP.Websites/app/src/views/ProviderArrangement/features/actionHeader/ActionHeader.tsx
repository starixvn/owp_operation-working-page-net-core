import { useEffect, useRef, useState } from 'react';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/storeNative';
import {
  changeIsEdit,
  queryProviderArrangement,
  updateProviderArrangement,
  queryLogProviderArrangement
} from '../ProviderArrangementSlice';
import { UncontrolledTooltip } from 'reactstrap';
import LogHistoryPopup from '../logHistory/LogHistoryPopup';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import LogHistoryIcon from '../../../../assets/svg-element/LogHistoryIcon';
import { isEqual } from 'lodash';
import getPermission from '../../../Shared/permission/getPermission';
import { FunctionKeys } from '../../../Shared/Constants';
import Loading from '../../../components/Loading';

const ActionHeader = () => {
  const ref = useRef(null);
  const dispatch = useStableDispatch();
  const [isOpenStatusHistory, setIsOpenStatusHistory] = useState(false);
  const { isEdit, providers, originalProviders } = useSelector(
    (state: RootState) => state.providerReducer
  );
  const [policy, setPolicy] = useState({ HasWrite: false });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    (async functionKey => {
      const policyResult = await getPermission(functionKey);
      setPolicy(policyResult);
      setLoading(false);
    })(FunctionKeys.ProviderArrangement);
  }, []);

  const { HasWrite } = policy;
  if (loading) {
    return <Loading />;
  }

  const handleBackViewMode = () => {
    dispatch(changeIsEdit());
    dispatch(queryProviderArrangement());
  };
  const handleCloseStatusHistory = () => setIsOpenStatusHistory(false);
  const handleOpenStatusHistory = async () => {
    setIsOpenStatusHistory(true);
    dispatch(queryLogProviderArrangement());
  };
  const handleSave = () => {
    if (!isEqual(providers, originalProviders)) {
      const model = {
        providers
      };
      dispatch(updateProviderArrangement(model));
    }
    handleBackViewMode();
  };
  return (
    <div className='action-header'>
      <div className='log-btn' onClick={handleOpenStatusHistory}>
        <div className='log-icon' ref={ref}>
          <LogHistoryIcon width={24} height={24} />
        </div>
        <UncontrolledTooltip placement='right' target={ref}>
          Log History
        </UncontrolledTooltip>
        {isOpenStatusHistory && (
          <LogHistoryPopup handleClosePopup={handleCloseStatusHistory} />
        )}
      </div>
      {HasWrite && (
        <div className='right-btn'>
          {!isEdit ? (
            <button className='btn-edit' onClick={handleBackViewMode}>
              Edit
            </button>
          ) : (
            <>
              <button className='btn-cancel' onClick={handleBackViewMode}>
                Cancel
              </button>
              <button className='btn-save' onClick={handleSave}>
                Save
              </button>
            </>
          )}
        </div>
      )}
    </div>
  );
};

export default ActionHeader;
