import { useEffect } from 'react';
import { OWPBody, OWPHeader, OWPPage } from '../../Shared/common/OWP/OPWPage';

import {
  getTicketDetail,
  getTicketSubTypes
} from '../../Shared/chatBoardNew/chatBoardThunkNative';
import TicketDetailBoard from '../../Shared/chatBoardNew/TicketDetailBoard';
import { FunctionKeys } from '../../Shared/Constants';
import CreateTicketBoardOther from '../../Shared/createTicketNew/CreateTicketBoardOther';
import CreateTicketEventChecking from '../../Shared/createTicketNew/CreateTicketEventChecking';
import CreateTicketLiveIncident from '../../Shared/createTicketNew/CreateTicketLiveIncident';
import TicketLogWorkCreator from '../../Shared/createTicketNew/TicketLogWork/TicketLogWorkCreator';
import { useAppDispatch } from '../app/hooksNative';
import { RECIPIENT_TYPE } from '../const/const.json';
import HeaderNative from '../features/header/HeaderNative';
import { onClickIssueModel } from '../features/signalR/signalRSlice';
import TicketCenterTable from '../features/Table/TicketCenterTable';
import { getDetailSegmentUrlByEventType } from '../Helper/TicketCenterHelper';
import { usePermission } from '../ImportTicketCenterHelperNative';
import store from '../app/storeNative';

const { INDIVIDUAL } = RECIPIENT_TYPE;
const MyTicketTabNative = () => {
  const dispatch = useAppDispatch();
  const searchParams = new URLSearchParams(document.location.search);
  const actorId = searchParams.get('actorId');
  const eventType = searchParams.get('eventType');
  const ticketId = searchParams.get('ticketId');

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

  const {permission}= store.getState().pagePolicy;
  
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

export default MyTicketTabNative;
