import NetCoreSignalR from '../../Shared/NetCoreSignalR/NetCoreSignalR';
import handleNewMessageIncome from './newMessageIncomeHandler';
import handleChangeTicketStatus from './changeStatusIncomeHandler';
import changeSubTypeIncomeHandler from './changeSubTypeIncomeHandler';
import { changeMessageIconHandler } from './changeMessageIconHandler';

const useSignalRWrapper = () => {
  const handlers = [
    {
      method: 'NotifyUpdateReactIconMessage',
      handler: changeMessageIconHandler
    },
    {
      method: 'NotifyNewMessageIncome',
      handler: handleNewMessageIncome
    },
    {
      method: 'NotifyChangeStatusIncome',
      handler: handleChangeTicketStatus
    },
    {
      method: 'NotifyChangeSubTypeIncome',
      handler: changeSubTypeIncomeHandler
    }
  ];

  return <NetCoreSignalR handlers={handlers} hubName='ticketCenterNotifyHub' />;
};

export default useSignalRWrapper;
