import { Provider } from 'react-redux';
import FilterSection from './table/FilterSection';
import LeagueInfoTable from './table/LeagueInfoTable';
import store from './app/storeNative';
import TVListCustom from './filter/TVListCustom';

const LeagueInfoManagement = () => {
  return (
    <>
      <TVListCustom></TVListCustom>
      <Provider store={store}>
        <FilterSection />
        <LeagueInfoTable />
      </Provider>
    </>
  );
};
export default LeagueInfoManagement;
