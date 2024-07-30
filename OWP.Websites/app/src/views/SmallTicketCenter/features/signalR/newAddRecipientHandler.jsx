import { checkAllFilterCondition } from './signalRHelper';
import store from '../../app/storeNative';
import { appendTicketItemIntoDataSource } from '../table/slice/ticketSlice';

const handleNewAddRecipient = ticketInfo => {
  const { filter } = store.getState();
  const filterInfo = filter?.entities?.filterInfo;
  const isAppend = checkAllFilterCondition(filterInfo, ticketInfo);
  isAppend && store.dispatch(appendTicketItemIntoDataSource(ticketInfo));
};

export { handleNewAddRecipient };
