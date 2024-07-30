import { useEffect } from 'react';
import { OWPPage, OWPHeader, OWPBody } from '../Shared/common/OWP/OPWPage';
import HeaderNative from './features/header/HeaderNative';
import TicketCenterTable from './features/Table/TicketCenterTable';
import { openTicketCenterIssueURL } from '../Message/Service';
import { FunctionKeys } from '../Shared/Constants';
import Loading from '../components/Loading';
import { usePermission } from './ImportTicketCenterHelperNative';
import { RECIPIENT_TYPE } from './const/const.json';
import TicketDetailBoard from '../Shared/chatBoardNew/TicketDetailBoard';
import CreateTicketLiveIncident from '../Shared/createTicketNew/CreateTicketLiveIncident';
import CreateTicketEventChecking from '../Shared/createTicketNew/CreateTicketEventChecking';
import CreateTicketBoardOther from '../Shared/createTicketNew/CreateTicketBoardOther';
import TicketLogWorkCreator from './../Shared/createTicketNew/TicketLogWork/TicketLogWorkCreator';
import { post } from '../Shared/Service/Service';
import store from './app/storeNative';
import { useAppDispatch } from './app/hooksNative';
import {
  getTicketDetail,
  getTicketSubTypes
} from '../Shared/chatBoardNew/chatBoardThunkNative';
import { onClickIssueModel } from './features/signalR/signalRSlice';
import { getDetailSegmentUrlByEventType } from './Helper/TicketCenterHelper';

const { INDIVIDUAL } = RECIPIENT_TYPE;
const TicketCenterManagementNative = () => {
  const dispatch = useAppDispatch();
  const searchParams = new URLSearchParams(document.location.search);
  const actorId = searchParams.get('actorId');
  const eventType = searchParams.get('eventType');
  const ticketId = searchParams.get('ticketId');

  const pagePermissionModel = usePermission(
    FunctionKeys.TicketCenter,
    'pagePolicy'
  );
  usePermission(FunctionKeys.DeleteTickets, 'deletePolicy');

  useEffect(() => {
    (async () => {
      if (actorId && eventType && ticketId) {
        const requestDetailModel = {
          ticketId: parseInt(ticketId),
          actorId: parseInt(actorId),
          detailSegmentUrl: getDetailSegmentUrlByEventType(eventType),
          eventType: parseInt(actorId)
        };
        dispatch(onClickIssueModel(requestDetailModel));
        const actionResult = await dispatch(
          getTicketDetail(requestDetailModel)
        );

        dispatch(getTicketSubTypes(actionResult.payload.data.ticketType));
        if (getTicketDetail.rejected.match(actionResult)) {
          ToastrWrapper.showError(actionResult.payload);
        }
      }
    })();
  }, []);

  const { permission } = pagePermissionModel;

  if (!permission.Username) {
    return <Loading />;
  }

  const creatorInfo = {
    groupName: null,
    nickName: permission.Username,
    recipientId: permission.ActorId,
    recipientType: INDIVIDUAL,
    userName: permission.Username
  };
  return (
    <div className='ticket-center'>
      <OWPPage className='Event-Tickets'>
        <OWPHeader className='tickets-tab'>
          <HeaderNative />
        </OWPHeader>

        <OWPBody className='tickets-tab'>
          <div className='table-EventChecking'>
            <TicketCenterTable />
          </div>
          <div className='popup-ChatBoard'>
            <TicketDetailBoard />
          </div>
          <CreateTicketEventChecking creatorInfo={creatorInfo} />
          <CreateTicketLiveIncident creatorInfo={creatorInfo} />
          <CreateTicketBoardOther creatorInfo={creatorInfo} />
          <TicketLogWorkCreator creatorInfo={creatorInfo} />
        </OWPBody>
      </OWPPage>
    </div>
  );
};

export default TicketCenterManagementNative;
