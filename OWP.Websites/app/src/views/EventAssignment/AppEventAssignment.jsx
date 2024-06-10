import store from './app/store';
import { Provider } from 'react-redux';
import EventAssignment from './EventAssignment';
import useSignalRWrapper from './signalR/useSignalRWrapper';

const AppEventAssignment = () => {
  return (
    <Provider store={store}>
      <EventAssignment />
      {useSignalRWrapper()}
    </Provider>
  );
};
export default AppEventAssignment;
