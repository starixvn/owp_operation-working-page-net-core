import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import OsLeagueSetting from '../views/OsLeagueSetting/OsLeagueSetting';
import '../scss/pages/_OsLeagueSetting.scss';

const container = document.getElementById('root');

if (container) {
  const root = createRoot(container);
  root.render(<Layout view={<OsLeagueSetting />} title='Basketball Max Bet' />);
}
