import store from '../app/storeNative';
import { setDataSignalR } from '../table/tableSlice/tableSlice';
import { handleReload } from '../../Shared/ticketEvent/signalR/reLoadHelper';

const reloadTicketCellTable = (parentMatchId, actorId) => {
  handleReload(store, actorId, parentMatchId, setDataSignalR);
};

export { reloadTicketCellTable };
