import { Provider } from 'react-redux';
import { OWPBody } from '../Shared/common/OWP/OPWPage';
import store from './app/storeNative';
import Filter from './features/filter/Filter';
import Table from './features/table/Table';

const TeamManagementNative = () => {
  return (
    <Provider store={store}>
      <div className='body-content'>
        {(OWPBody as any)({
          className: 'team-management', children: (
            <>
              <Filter />
              <Table />
            </>
          )
        })}
      </div>
    </Provider>
  );
};
export default TeamManagementNative;
