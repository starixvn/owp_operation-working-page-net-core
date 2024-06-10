import { reloadTicketCellTable } from './signalRHelper';
import { handleChangeStatus } from './changeStatusIncomeHandler';
import { handleChangeSubtype } from './changeSubtypeIncomeHandler';
import SignalRWrapper from '../../Shared/ticketEvent/signalR/signalRWrapper';

const useSignalRWrapper = () => {
  return (
    <SignalRWrapper
      reloadTicketCellTable={reloadTicketCellTable}
      handleChangeStatus={handleChangeStatus}
      handleChangeSubtype={handleChangeSubtype}
    />
  );
};

export default useSignalRWrapper;
