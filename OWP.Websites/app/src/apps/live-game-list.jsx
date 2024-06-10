import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import LiveGameListAppNative from '../views/LiveGameList/LiveGameListAppNative';
import '../scss/pages/_LiveGameList.scss';
import '../views/components/Assignment/assignment.scss';
import '../scss/components/_chatboard.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(<Layout view={<LiveGameListAppNative />} title='Live Game List' />);
