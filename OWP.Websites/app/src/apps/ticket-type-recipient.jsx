import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import TicketTypeRecipientNative from '../views/TicketTypeRecipient/TicketTypeRecipientNative';
import '../scss/pages/_TicketTypeRecipient.scss';
import '../scss/components/_recipientSuggestion.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<TicketTypeRecipientNative />} title='Ticket Type Settings' />
);
