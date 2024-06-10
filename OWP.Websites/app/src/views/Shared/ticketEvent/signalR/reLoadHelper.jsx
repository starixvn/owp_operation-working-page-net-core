import { getTicketDetail } from '../../chatBoardNew/chatBoardSlice';

const handleReload = (store, actorId, parentMatchId, setData) => {
  const model = store.getState();
  const { issueModel } = model.signalREventTicket;
  const { isOpenPopup } = model.chatBoard;
  store.dispatch(setData({ actorId, parentMatchId }));
  isOpenPopup && store.dispatch(getTicketDetail(issueModel));
};

const handleReloadEventAssignment = (
  store,
  actorId,
  userId,
  parentMatchId,
  setData
) => {
  const model = store.getState();
  const { issueModel } = model.signalREventTicket;
  const { isOpenPopup } = model.chatBoard;
  store.dispatch(setData({ actorId, parentMatchId, userId }));
  isOpenPopup && store.dispatch(getTicketDetail(issueModel));
};

const handleReloadTicketDetail = store => {
  const model = store.getState();
  const { issueModel } = model.signalREventTicket;
  const { isOpenPopup } = model.chatBoard;
  isOpenPopup && store.dispatch(getTicketDetail(issueModel));
};

export { handleReload, handleReloadEventAssignment, handleReloadTicketDetail };
