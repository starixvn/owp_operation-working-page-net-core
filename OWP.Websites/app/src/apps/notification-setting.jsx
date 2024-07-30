import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import Notification from '../views/Notification/Notification';

import '../scss/style.scss';
import '../scss/pages/_NotificationSetting.scss';


const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<Notification />} title='Notification Setting' />
);
