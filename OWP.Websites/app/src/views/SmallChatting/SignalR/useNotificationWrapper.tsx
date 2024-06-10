import NetCoreSignalR from '../../Shared/NetCoreSignalR/NetCoreSignalR';
import notifyChangedInformationData from './notifyChangedInformationData';

const useNotificationWrapper = () => {
  const handlers = [
    {
      method: 'notifyChangedInformationData',
      handler: notifyChangedInformationData
    }
  ];

  return <NetCoreSignalR handlers={handlers} hubName='changedInfoNotifyHub' />;
};

export default useNotificationWrapper;
