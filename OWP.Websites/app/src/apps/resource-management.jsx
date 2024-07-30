import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import ResourceManagement from '../views/ResourceManagement/ResourceManagement';

import '../scss/style.scss';
import '../scss/pages/_ResourceManagement.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<ResourceManagement />} title='Resource Management' />
);
