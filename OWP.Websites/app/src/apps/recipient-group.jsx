import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import RecipientGroupAppNative from '../views/RecipientGroup/RecipientGroupAppNative';
import '../scss/pages/_RecipientGroup.scss';
import '../scss/components/_recipientSuggestion.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<RecipientGroupAppNative />} title='Recipient Group' />
);
