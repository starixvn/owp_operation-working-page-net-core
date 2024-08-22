import { useEffect } from 'react';
import Loading from '../components/Loading';
import openTicketDetailHandler from '../Shared/chatBoardNew/helper/openTicketDetailHandler';
import TicketDetailBoard from '../Shared/chatBoardNew/TicketDetailBoard';
import { FunctionKeys } from '../Shared/Constants';
import usePermission from '../Shared/customHook/usePermission';
import { useAppDispatch } from './app/hooks';
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
      await openTicketDetailHandler(
        ticketId,
        actorId,
        eventType,
        dispatch
      );
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
