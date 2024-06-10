import { Provider } from 'react-redux';
import { OWPBody, OWPHeader, OWPPage } from '../Shared/common/OWP/OPWPage';
import store from './app/storeNative';
import TicketTypeRecipientTable from './features/table/TicketTypeRecipientTable';

const TicketTypeRecipientNative = () => {
  return (
    <Provider store={store}>
      <OWPPage className='recipient-group'>
        <OWPHeader className='recipient-group'></OWPHeader>
        <TicketTypeRecipientTable />
        <OWPBody className='recipient-group'></OWPBody>
      </OWPPage>
    </Provider>
  );
};

export default TicketTypeRecipientNative;
