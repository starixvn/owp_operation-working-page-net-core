import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from './app/hookNative';
import Filter from './feature/filter/Filter';
import Table from './feature/table/Table';
import { getConfirmProvider } from './feature/table/tableThunk';
import usePermission from '../Shared/customHook/usePermission';
import { isEquals } from 'immutability-helper';
import { REQUEST_STATUS, FunctionKeys } from '../Shared/Constants';
import Loading from '../components/Loading';
import { RECIPIENT_TYPE } from '../Shared/createTicketNew/const/const';
import CreateTicketLGAView from '../Shared/createTicketNew/CreateTicketLGAView';
import TicketDetailBoard from '../Shared/chatBoardNew/TicketDetailBoard';

const ProviderVerificationAppNative = () => {
  const dispatch = useAppDispatch();
  const loadingConfirmProviderOption = useAppSelector(
    state => state.common.confirmProvider.loadingConfirmProviderOption
  );

  const pagePolicy = usePermission(
    FunctionKeys.ProviderVerification,
    'providerPolicy'
  );

  useEffect(() => {
    dispatch(getConfirmProvider());
  }, [dispatch]);

  const ticketCenterPermissionModel = usePermission(
    FunctionKeys.TicketCenter,
    'pagePolicy'
  );
  const { permission } = ticketCenterPermissionModel;
  const { Username, ActorId } = permission;

  const creatorInfo = {
    groupName: null,
    nickName: Username,
    recipientId: ActorId,
    recipientType: RECIPIENT_TYPE.INDIVIDUAL,
    userName: Username
  };

  if (
    isEquals(loadingConfirmProviderOption, REQUEST_STATUS.pending) ||
    !Username ||
    !pagePolicy.permission.Username
  ) {
    return <Loading />;
  }

  return (
    <>
      <div className='provider-verification'>
        <Filter />
        <Table />
      </div>
      <CreateTicketLGAView
        creatorInfo={creatorInfo}
        isProviderVerification={true}
      />
      <TicketDetailBoard />
    </>
  );
};

export default ProviderVerificationAppNative;
