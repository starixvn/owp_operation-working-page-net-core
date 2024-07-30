import store from '../../app/storeNative';
import {
  checkAllFilterCondition,
  handleCorrectFilter,
  handleInCorrectFilter
} from './signalRHelper';
const changeSubTypeIncomeHandler = ticketInfo => {
  const model = store.getState();
  const { isOpenPopup } = model.chatBoard;
  const { filterModel } = model.signalR;
  const { entities } = model.eventChecking;
  const isCorrectFilter = checkAllFilterCondition(filterModel, ticketInfo);
  const elementByTicketId = entities.find(
    x => x.ticketId === ticketInfo.ticketId
  );
  isCorrectFilter
    ? handleCorrectFilter(elementByTicketId, ticketInfo, isOpenPopup)
    : handleInCorrectFilter(ticketInfo);
};

export default changeSubTypeIncomeHandler;
