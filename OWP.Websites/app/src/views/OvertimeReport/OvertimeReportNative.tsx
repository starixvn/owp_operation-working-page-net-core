import { Provider } from 'react-redux';
import { OWPBody, OWPHeader, OWPPage } from '../Shared/common/OWP/OPWPage';
import store from './app/storeNative';
import NavigationTab from './features/navigationTab/Tab';

const OvertimeReport = () => {
  return (
    <Provider store={store}>
      <OWPPage className='ot-report'>
        <OWPHeader className='ot-report' />
        <OWPBody className='ot-report'>
          <NavigationTab />
        </OWPBody>
      </OWPPage>
    </Provider>
  );
};

export default OvertimeReport;
