import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import LstReport from '../views/LSTReport/LSTReport';
import '../scss/pages/_LSTReport.scss';


const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<LstReport />} title='Trader Report' />
);
