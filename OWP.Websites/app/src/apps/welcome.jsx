import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import Welcome from '../views/Welcome/Welcome';
import '../scss/pages/_Welcome.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(<Layout view={<Welcome />} title='Welcome' />);
