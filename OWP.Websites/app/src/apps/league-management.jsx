import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import App from '../views/LeagueManagement/App';
import '../scss/pages/_MaxbetManagement.scss';
import 'react-table-hoc-fixed-columns-ts/lib/styles.css';
import '../scss/pages/_multipleFilter.scss';
import { BrowserRouter } from 'react-router-dom';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <BrowserRouter>
    <Layout view={<App />} title='League Management' />
  </BrowserRouter>
);
