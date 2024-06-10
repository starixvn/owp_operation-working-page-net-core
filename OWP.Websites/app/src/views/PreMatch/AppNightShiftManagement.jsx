import store from './app/store';
import { Provider } from 'react-redux';
import NightShiftManagement from './NightShiftManagement';
import { SignalRWrapper } from './ImportPrematchHelperNative';

const AppNightShiftManagement = () => {
  return (
    <Provider store={store}>
      <NightShiftManagement />
      <SignalRWrapper />
    </Provider>
  );
};
export default AppNightShiftManagement;
