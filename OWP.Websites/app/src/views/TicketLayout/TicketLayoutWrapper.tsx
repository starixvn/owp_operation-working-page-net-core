import { Provider } from 'react-redux';
import TicketDetailLayout from './TicketDetailLayout';
import '../../scss/components/_chatboard.scss';
import useSignalRWrapper from './SignalR/useSignalRWrapper';
import store from './store';

const TicketLayoutWrapper = () => {
  return (
    <Provider store={store}>
      <TicketDetailLayout />
      {useSignalRWrapper()}
    </Provider>
  );
};
export default TicketLayoutWrapper;
