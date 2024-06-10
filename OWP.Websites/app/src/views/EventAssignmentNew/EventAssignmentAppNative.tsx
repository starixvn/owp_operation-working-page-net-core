import { Provider } from 'react-redux';
import store from './app/store';
import EventAssignment from './EventAssignment';
import useSignalRWrapper from './signalR/useSignalRWrapper';
const EventAssignmentAppNative = () => {
  return (
    <Provider store={store}>
      <EventAssignment />
      {useSignalRWrapper()}
    </Provider>
  );
};
export default EventAssignmentAppNative;
