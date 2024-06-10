import { Provider } from 'react-redux';
import { OWPBody, OWPHeader, OWPPage } from '../Shared/common/OWP/OPWPage';
import Filter from './features/filter/Filter';
import ReportTable from './features/table/ReportTable';
import store from './app/storeNative';

const MatchCountLiveReport = () => {
  return (
    <Provider store={store}>
      <OWPPage className='match-count-report'>
        <OWPHeader className='match-count-report' />
        <OWPBody className='match-count-report'>
          <Filter />
          <ReportTable />
        </OWPBody>
      </OWPPage>
    </Provider>
  );
};
export default MatchCountLiveReport;
