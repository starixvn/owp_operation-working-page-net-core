import { Provider } from 'react-redux';
import LeagueNameFilter from './filter/LeagueNameFilter';
import LeagueInfoTable from './table/LeagueInfoTable';
import store from './app/storeNative';
import TVListCustom from './filter/TVListCustom';
import AddLeagueSettingButton from './filter/AddLeagueSettingButton';

const LeagueInfoManagement = () => {
  return (
    <>
      <TVListCustom></TVListCustom>
      <Provider store={store}>
        <LeagueNameFilter />
        <AddLeagueSettingButton />
        <LeagueInfoTable />
      </Provider>
    </>
  );
};
export default LeagueInfoManagement;
