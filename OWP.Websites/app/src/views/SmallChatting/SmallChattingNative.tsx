import usePermission from '../Shared/customHook/usePermission';
import { FunctionKeys } from '../Shared/Constants';
import TicketDetailBoard from '../Shared/chatBoardNew/TicketDetailBoard';
import Loading from '../components/Loading';
import { getDetailSegmentUrlByEventType } from '../Shared/TicketCenterPrematch/helper/helper';
import { useAppDispatch } from './app/hooks';
import {
  getTicketDetail,
  getTicketSubTypes
} from '../Shared/chatBoardNew/chatBoardThunkNative';
import { useEffect } from 'react';
import ToastrWrapper from '../components/ToastrWrapper';
import { setIsDisplayCloseButton } from '../Shared/chatBoardNew/chatBoardSlice';
import BlinkDocumentTitle from './BlinkDocumentTitle';

const SmallChatting = () => {
  const dispatch = useAppDispatch();
  const searchParams = new URLSearchParams(document.location.search);
  const actorId = searchParams.get('actorId');
  const eventType = searchParams.get('eventType');
  const ticketId = searchParams.get('ticketId');
  const { permission } = usePermission(FunctionKeys.TicketCenter, 'pagePolicy');

  useEffect(() => {
    (async () => {
      const requestDetailModel = {
        ticketId,
        actorId,
        detailSegmentUrl: getDetailSegmentUrlByEventType(eventType),
        eventType
      };
      const actionResult = await dispatch(getTicketDetail(requestDetailModel));
      dispatch(setIsDisplayCloseButton(false));
      dispatch(getTicketSubTypes(actionResult.payload.data.ticketType));
      if (getTicketDetail.rejected.match(actionResult)) {
        ToastrWrapper.showError(actionResult.payload);
      }
    })();
  }, []);

  if (!permission.Username) {
    return <Loading></Loading>;
  }

  return (
    <div className='popup-ChatBoard'>
      <TicketDetailBoard isSmallBoard={true} />
      <BlinkDocumentTitle />
    </div>
  );
};
export default SmallChatting;
