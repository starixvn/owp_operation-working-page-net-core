import LAReportManagement from './components/LAReportManagement';
import { Provider } from 'react-redux';
import store from './store/store';

const App = () => {
  return (
    <Provider store={store}>
      <LAReportManagement />
    </Provider>
  );
};

export default App;
