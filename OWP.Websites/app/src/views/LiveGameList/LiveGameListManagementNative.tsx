import { useEffect } from 'react';
import { OWPBody, OWPHeader } from '../Shared/common/OWP/OPWPage';
import Filter from './filterArea/Filter';
import { FunctionKeys } from '../Shared/Constants';
import usePermission from '../Shared/customHook/usePermission';
import CreateTicketBoardOther from '../Shared/createTicketNew/CreateTicketBoardOther';
import CreateTicketLGAView from '../Shared/createTicketNew/CreateTicketLGAView';
import CreateTicketLiveIncident from '../Shared/createTicketNew/CreateTicketLiveIncident';
import { RECIPIENT_TYPE } from './LiveGameListConst.json';
import TicketDetailBoard from '../Shared/chatBoardNew/TicketDetailBoard';
import Loading from '../components/Loading';
import { setUserId } from './table/tableSlice/tableSlice';
import { useDispatch } from 'react-redux';
import PopupDetail from './component/PopupDetail';

import { useSelector } from 'react-redux';
import { RootState } from './app/storeNative';
import LiveGameListTable from './table/LiveGameListTable';

type CreatorInfo = {
  groupName: null | string;
  nickName: null | string;
  recipientId: number;
  recipientType: number;
  userName: string;
};

const LiveGameListManagementNative = () => {
  const stableDispatch = useDispatch();

  const pagePermissionModel = usePermission(
    FunctionKeys.TicketCenter,
    'pagePolicy'
  );

  const { permission } = pagePermissionModel;
  const { Username, ActorId } = permission;

  useEffect(() => {
    stableDispatch(setUserId(permission.ActorId));
  });

  if (!Username) {
    return <Loading />;
  }

  const creatorInfo: CreatorInfo = {
    groupName: null,
    nickName: Username,
    recipientId: ActorId,
    recipientType: RECIPIENT_TYPE.INDIVIDUAL,
    userName: Username
  };

  return (
    <div className='live-game-list'>
      <OWPHeader className='live-game-list-header'>
        <Filter />
      </OWPHeader>
      <OWPBody className='live-game-list-body'>
        <LiveGameListTable />
        <CreateTicketLGAView creatorInfo={creatorInfo} />
        <CreateTicketBoardOther creatorInfo={creatorInfo} />
        <CreateTicketLiveIncident creatorInfo={creatorInfo} />
        <TicketDetailBoard />
        <PopupDetail />
      </OWPBody>
    </div>
  );
};

export default LiveGameListManagementNative;
