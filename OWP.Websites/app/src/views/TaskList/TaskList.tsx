import { Provider } from 'react-redux';
import { OWPHeader, OWPPage, OWPBody } from '../Shared/common/OWP/OPWPage';
import store from './app/storeNative';
import TabSwitcher from './Navigation/TabSwitcher';
import useSignalRWrapper from './signalR/useSignalRWrapper';

const TaskList = () => {
  return (
    <Provider store={store}>
      <OWPPage className='tasklist'>
        <OWPHeader className='tasklist'>
          <div></div>
        </OWPHeader>
        <OWPBody className='tasklist'>
          <TabSwitcher />
        </OWPBody>
      </OWPPage>
      {useSignalRWrapper()}
    </Provider>
  );
};

export default TaskList;
