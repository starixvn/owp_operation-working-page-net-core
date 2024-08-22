import ToastrWrapper from '../../components/ToastrWrapper';
import { onClickIssueModel } from '../../TicketCenter/features/signalR/signalRSlice';
import useStableDispatch from '../customHook/useStableDispatch';
import { getDetailSegmentUrlByEventType } from '../TicketCenterPrematch/helper/helper';
import {
  getTicketDetail,
  getTicketSubTypes
} from '../chatBoardNew/chatBoardThunkNative';
import usePermission from './usePermission';
import { FunctionKeys } from '../Constants';

const useOpenTicketDetailHook = async (
  ticketId,
  eventType,
  typeId,
  policyName = 'policy'
) => {
  const dispatch = useStableDispatch();
  const permission = usePermission(FunctionKeys.TicketCenter, policyName);

  const requestDetailModel = {
    ticketId,
    actorId: permission.ActorId,
    detailSegmentUrl: getDetailSegmentUrlByEventType(eventType)
  };
  dispatch(onClickIssueModel(requestDetailModel));
  dispatch(getTicketSubTypes(typeId));
  const actionResult = await dispatch(
    getTicketDetail({
      ...requestDetailModel,
      isQueryMessageInLastTwoDay: true
    })
  );
  if (getTicketDetail.rejected.match(actionResult)) {
    return ToastrWrapper.showError(actionResult.payload);
  }
};

export default useOpenTicketDetailHook;
