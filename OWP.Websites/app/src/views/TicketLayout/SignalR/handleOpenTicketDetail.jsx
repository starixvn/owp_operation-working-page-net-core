import openTicketDetailHandler from '../../Shared/chatBoardNew/helper/openTicketDetailHandler';
import store from '../store';

const handleOpenTicketDetail = async (ticketId, eventType) => {
  const { pagePolicy } = store.getState();
  const { permission } = pagePolicy;

  await openTicketDetailHandler(
    ticketId,
    permission.ActorId,
    eventType,
    store.dispatch
  );
};

export default handleOpenTicketDetail;
