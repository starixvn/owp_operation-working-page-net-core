import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import App from '../views/LAReport/App';
import '../scss/pages/_LAReport.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<App />} title='LA report' />
);
