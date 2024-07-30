import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import MessageBox from '../views/MessageBox/MessageBox';

import '../scss/style.scss';
import '../scss/pages/_MessageBox.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<MessageBox />} title='Message Management' />
);
