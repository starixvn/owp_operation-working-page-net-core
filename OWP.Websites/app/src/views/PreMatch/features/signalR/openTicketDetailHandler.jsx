import ToastrWrapper from '../../../components/ToastrWrapper';
import {
  getTicketDetail,
  getTicketSubTypes
} from '../../../Shared/chatBoardNew/chatBoardSlice';
import store from '../../app/store';
import { getDetailSegmentUrlByEventType } from '../../../Shared/TicketCenterPrematch/helper/helper';
import { backupGetIssueModel } from './signalRSlice';

const handleOpenTicketDetail = (ticketId, eventType) => {
  const { pagePolicy } = store.getState();
  const { permission } = pagePolicy;

  const requestDetailModel = {
    ticketId,
    actorId: permission.ActorId,
    detailSegmentUrl: getDetailSegmentUrlByEventType(eventType),
    eventType
  };
  store.dispatch(backupGetIssueModel(requestDetailModel));
  const actionResult = store.dispatch(getTicketDetail(requestDetailModel));
  store.dispatch(getTicketSubTypes(actionResult.payload.data.ticketType));
  if (getTicketDetail.rejected.match(actionResult)) {
    ToastrWrapper.showError(actionResult.payload);
  }
};

export { handleOpenTicketDetail };
