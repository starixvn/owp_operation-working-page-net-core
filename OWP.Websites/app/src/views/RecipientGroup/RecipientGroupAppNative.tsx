import store from './app/storeNative';
import { Provider } from 'react-redux';
import { OWPPage, OWPBody, OWPHeader } from '../Shared/common/OWP/OPWPage';
import Filter from './features/filter/Filter';
import GroupTable from './features/groupTable/GroupTable';

const RecipientGroupAppNative = () => {
  return (
    <Provider store={store}>
      <OWPPage className='recipient-group'>
        <OWPHeader className='recipient-group'>
          <Filter />
        </OWPHeader>
        <OWPBody className='recipient-group'>
          <GroupTable />
        </OWPBody>
      </OWPPage>
    </Provider>
  );
};

export default RecipientGroupAppNative;
