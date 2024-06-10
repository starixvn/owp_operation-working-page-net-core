import { Provider } from 'react-redux';
import store from './app/store';
import SmallChatting from './SmallChatting';
import useNotificationWrapper from './SignalR/useNotificationWrapper';
import { SpinnerContainer } from '../components/Spinner';
import useSignalRWrapper from './SignalR/useSignalRWrapper';

const SmallChattingWrapper = () => {
  return (
    <div>
      <Provider store={store}>
        <SmallChatting />
        <SpinnerContainer />
        {useNotificationWrapper()}
        {useSignalRWrapper()}
      </Provider>
    </div>
  );
};

export default SmallChattingWrapper;
