import store from './app/storeNative';
import { Provider } from 'react-redux';
import PopupManagement from './PopupManagement';
import useSignalRWrapper from './signalR/useSignalRWrapper';
import { SpinnerContainer } from './../components/Spinner';

export default function PopupAppNative() {
  return (
    <Provider store={store}>
      <PopupManagement></PopupManagement>
      <SpinnerContainer></SpinnerContainer>
      {useSignalRWrapper()}
    </Provider>
  );
}
