import { onClickIssueModel } from '../../../TicketCenter/features/signalR/signalRSlice';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { getDetailSegmentUrlByEventType } from '../../TicketCenterPrematch/helper/helper';
import {
  getTicketDetail,
  getTicketSubTypes
} from '../../chatBoardNew/chatBoardSlice';

const openTicketDetailHandler = async (
  ticketId,
  actorId,
  eventType,
  dispatch
) => {
  const requestDetailModel = {
    ticketId,
    actorId,
    detailSegmentUrl: getDetailSegmentUrlByEventType(eventType),
    eventType
  };

  dispatch(onClickIssueModel(requestDetailModel));
  const actionResult = await dispatch(
    getTicketDetail({
      ...requestDetailModel,
      isQueryMessageInLastTwoDay: true
    })
  );
  dispatch(getTicketSubTypes(actionResult.payload.data.ticketType));

  if (getTicketDetail.rejected.match(actionResult)) {
    ToastrWrapper.showError(actionResult.payload);
  }
};

export default openTicketDetailHandler;
