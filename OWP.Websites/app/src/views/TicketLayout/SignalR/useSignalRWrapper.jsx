import NetCoreSignalR from '../../Shared/NetCoreSignalR/NetCoreSignalR';
import handleNewMessageIncome from './handleNewMessageIncome';
import handleNewAddRecipient from './handleNewAddRecipient';
import handleOpenTicketDetail from './handleOpenTicketDetail';
import handleChangeTicketStatus from './changeStatusIncomeHandler';
import changeSubTypeIncomeHandler from './changeSubTypeIncomeHandler';
import changeMessageIconHandler from './changeMessageIconHandler';
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
      method: 'NotifyNewAddRecipient',
      handler: handleNewAddRecipient
    },
    {
      method: 'NotifyChangeStatusIncome',
      handler: handleChangeTicketStatus
    },
    {
      method: 'NotifyChangeSubTypeIncome',
      handler: changeSubTypeIncomeHandler
    },
    {
      method: 'NotifyUserOnClickToast',
      handler: handleOpenTicketDetail
    }
  ];

  return <NetCoreSignalR handlers={handlers} hubName='ticketCenterNotifyHub' />;
};

export default useSignalRWrapper;
