import { Provider } from 'react-redux';
import LeagueNameFilter from './filter/LeagueNameFilter';
import LeagueInfoTable from './table/LeagueInfoTable';
import store from './app/storeNative';
import TVListCustom from './filter/TVListCustom';

const LeagueInfoManagement = () => {
  return (
    <>
      <TVListCustom></TVListCustom>
      <Provider store={store}>
        <LeagueNameFilter />
        <LeagueInfoTable />
      </Provider>
    </>
  );
};
export default LeagueInfoManagement;
