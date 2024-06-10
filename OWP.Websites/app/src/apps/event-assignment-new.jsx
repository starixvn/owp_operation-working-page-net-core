import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import EventAssignmentAppNative from '../views/EventAssignmentNew/EventAssignmentAppNative';
import '../scss/components/_chatboard.scss';
import '../views/components/Assignment/assignment.scss';
import '../scss/pages/_EventAssignmentNew.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout
    view={<EventAssignmentAppNative />}
    title='Live Game Assignment New'
  />
);
