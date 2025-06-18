import { Provider } from 'react-redux';
import { OWPBody } from '../Shared/common/OWP/OPWPage';
import Filter from './features/filter/Filter';
import store from './app/storeNative';
import React from 'react';

const TeamManagementNative = () => {
  return (
    <Provider store={store}>
      <div className='body-content'>
        <OWPBody className='team-management'>
          <Filter />
        </OWPBody>
      </div>
    </Provider>
  );
};
export default TeamManagementNative;
