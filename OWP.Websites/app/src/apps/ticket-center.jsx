import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import TicketCenterAppNative from '../views/TicketCenter/TicketCenterAppNative';
import '../scss/pages/_TicketCenter.scss';
import '../scss/components/_chatboard.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout
    view={<TicketCenterAppNative />}
    title='My Tickets'
    isUseTicketLayout={false}
  />
);
