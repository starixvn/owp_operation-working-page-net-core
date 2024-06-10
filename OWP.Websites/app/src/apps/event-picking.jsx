import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import EventPickingAppNative from '../views/EventPicking/EventPickingAppNative';
import '../scss/pages/_EventPicking.scss';
import '../scss/pages/_multipleFilter.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<EventPickingAppNative />} title='Event Picking New' />
);
