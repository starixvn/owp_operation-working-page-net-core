import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import AppEventAssignment from '../views/EventAssignment/AppEventAssignment';
import '../scss/components/_chatboard.scss';
import '../views/components/Assignment/assignment.scss';
import '../scss/pages/_EventAssignment.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<AppEventAssignment />} title='Live Game Assignment' />
);
