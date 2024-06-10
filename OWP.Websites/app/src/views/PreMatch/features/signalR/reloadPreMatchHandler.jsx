import {
  setNewMessage,
  setTicketStatus,
  updateSubTypeTicketDetailByTicketId,
  updateMessageIconRealTime
} from '../../../Shared/chatBoardNew/chatBoardSlice';
import store from '../../app/store';
import {
  reloadTicketInTable,
  reloadTicketCell
} from '../league/shiftSlice/shiftSlice';
import { throttle } from 'lodash';
import { URL_QUERY_TICKET_LIST } from '../../const/const';
import { get } from '../../ImportPrematchHelperNative';

const reloadTicketCellInTable = async (id, isTicketLeague) => {
  const model = store.getState();
  const { isEdit } = model.editor;
  const { permission } = model.policy;
  const getTicketList = async () => {
    const response = await get({
      url: URL_QUERY_TICKET_LIST,
      id,
      actorId: permission.ActorId,
      isTicketLeague
    });
    return response.data;
  };
  const submitThrottle = throttle(ticketList => {
    store.dispatch(reloadTicketCell({ id, isTicketLeague, ticketList }));
  }, 500);
  const ticketList = await getTicketList();

  !isEdit && submitThrottle(ticketList);
};

const reloadPreMatch = async (id, isTicketLeague, data) => {
  const { ticket, messageResponse } = data;
  const {
    policy: {
      permission: { ActorId }
    }
  } = store.getState();
  reloadTicketInPreMatchTableHandler({
    id,
    isTicketLeague,
    ticket: {
      EventType: ticket.eventType,
      IsRead: messageResponse.userId === ActorId,
      LatestCreatedDate: ticket.latestCreatedDate,
      LatestMessage: ticket.latestMessage,
      LatestUsername: ticket.latestUsername,
      OWPLeagueId: ticket.owpLeagueId,
      SubtypeId: ticket.subtypeId,
      SubtypeName: ticket.subtypeName,
      TicketId: ticket.ticketId,
      InfoSiteMatchId: ticket.infoSiteMatchId
    }
  });

  reloadOpenTicketHandler(messageResponse, ticket.ticketId);
};

const reloadTicketInPreMatchTableHandler = ({ id, isTicketLeague, ticket }) => {
  const model = store.getState();
  const { isEdit } = model.editor;
  !isEdit &&
    store.dispatch(
      reloadTicketInTable({
        id,
        isTicketLeague,
        ticket
      })
    );
};

const reloadOpenTicketHandler = (messageResponse, ticketId) => {
  const model = store.getState();
  const { isOpenPopup, ticketDetail } = model.chatBoard;

  isOpenPopup &&
    ticketDetail.ticketId === ticketId &&
    store.dispatch(setNewMessage(messageResponse));
};

const changeMessageIconHandler = (messageReactIcon, action, ticketId) => {
  const model = store.getState();
  const { isOpenPopup, ticketDetail } = model.chatBoard;
  ticketDetail.ticketId === ticketId &&
    isOpenPopup &&
    store.dispatch(updateMessageIconRealTime({ messageReactIcon, action }));
};

const reloadStatusOpenTicketHandler = async (ticketId, ticketStatus) => {
  const model = store.getState();
  const { isOpenPopup, ticketDetail } = model.chatBoard;
  const { ticketId: openTicketId } = ticketDetail;

  isOpenPopup &&
    openTicketId === ticketId &&
    store.dispatch(setTicketStatus(ticketStatus));
};

const reloadSubTypeOpenTicketHandler = async ticketInfo => {
  const model = store.getState();
  const { isOpenPopup } = model.chatBoard;

  isOpenPopup &&
    store.dispatch(updateSubTypeTicketDetailByTicketId(ticketInfo));
};

export {
  reloadOpenTicketHandler,
  reloadPreMatch,
  reloadTicketCellInTable,
  reloadStatusOpenTicketHandler,
  reloadSubTypeOpenTicketHandler,
  changeMessageIconHandler
};
