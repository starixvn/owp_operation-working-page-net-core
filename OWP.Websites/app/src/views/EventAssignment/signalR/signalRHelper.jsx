import store from '../app/store';
import { setDataSignalR } from '../reducer/tableReducer';
import { handleReload } from '../../Shared/ticketEvent/signalR/reLoadHelper';

const reloadTicketCellTable = (parentMatchId, actorId) => {
  handleReload(store, actorId, parentMatchId, setDataSignalR);
};

export { reloadTicketCellTable };
