import { Provider } from 'react-redux';
import { OWPBody, OWPHeader, OWPPage } from '../Shared/common/OWP/OPWPage';
import store from './app/storeNative';
import Filter from './features/filters/Filter';
import ReportTable from './features/table/ReportTable';

const PrematchReport = () => {
  return (
    <Provider store={store}>
      <OWPPage className='prematch-report'>
        <OWPHeader className='prematch-report' />
        <OWPBody className='prematch-report'>
          <Filter />
          <ReportTable />
        </OWPBody>
      </OWPPage>
    </Provider>
  );
};
export default PrematchReport;
