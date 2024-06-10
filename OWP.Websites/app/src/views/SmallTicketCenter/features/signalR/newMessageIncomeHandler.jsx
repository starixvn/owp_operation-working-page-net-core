import store from '../../app/storeNative';
import { updateTicketItemIntoDataSource } from '../table/slice/tickerSlice';

const handleNewMessageIncome = (newMsgInfo, ticketInfo) => {
  const { filter } = store.getState();
  const filterInfo = filter?.entities?.filterInfo;
  store.dispatch(updateTicketItemIntoDataSource({ ticketInfo, filterInfo }));
};

export { handleNewMessageIncome };
