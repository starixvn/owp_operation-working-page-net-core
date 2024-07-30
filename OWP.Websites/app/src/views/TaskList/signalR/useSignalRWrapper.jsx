import { reloadTicketCellTable } from './signalRHelper';
import { handleChangeStatus } from './changeStatusIncomeHandler';
import { handleChangeSubtype } from './changeSubtypeIncomeHandler';
import { changeMessageIconHandler } from './changeMessageIconHandler';
import SignalRWrapper from '../../Shared/ticketEvent/signalR/signalRWrapper';

const useSignalRWrapper = () => {
  return (
    <SignalRWrapper
      reloadTicketCellTable={reloadTicketCellTable}
      handleChangeStatus={handleChangeStatus}
      handleChangeSubtype={handleChangeSubtype}
      changeMessageIconHandler={changeMessageIconHandler}
    />
  );
};

export default useSignalRWrapper;
