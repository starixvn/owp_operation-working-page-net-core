import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import OtherSportManager from '../views/OtherSport/OtherSportManager';
import '../scss/pages/_OtherSport.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<OtherSportManager />} title='OS Live Game Assignment' />
);
