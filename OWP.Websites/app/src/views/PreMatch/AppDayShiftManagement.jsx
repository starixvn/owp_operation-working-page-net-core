import store from './app/store';
import { Provider } from 'react-redux';
import DayShiftManagement from './DayShiftManagement';
import { SignalRWrapper } from './ImportPrematchHelperNative';

const AppDayShiftManagement = () => {
  return (
    <Provider store={store}>
      <DayShiftManagement />
      <SignalRWrapper />
    </Provider>
  );
};
export default AppDayShiftManagement;
