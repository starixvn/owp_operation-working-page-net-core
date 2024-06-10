import NetCoreSignalR from '../../../Shared/NetCoreSignalR/NetCoreSignalR';
import { handleNewMessageIncome } from './newMessageIncomeHandler';
import { handleNewAddRecipient } from './newAddRecipientHandler';

const useSignalRWrapper = () => {
  const handlers = [
    {
      method: 'NotifyNewMessageIncome',
      handler: handleNewMessageIncome
    },
    {
      method: 'NotifyNewAddRecipient',
      handler: handleNewAddRecipient
    }
  ];

  return <NetCoreSignalR handlers={handlers} hubName='ticketCenterNotifyHub' />;
};

export default useSignalRWrapper;
