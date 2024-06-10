import MaxbetManagement from './components/MaxbetManagement';
import { Provider } from 'react-redux';
import store from './store/store';

const App = () => {
  return (
    <Provider store={store}>
      <MaxbetManagement />
    </Provider>
  );
};
export default App;
