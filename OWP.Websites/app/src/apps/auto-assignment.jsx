import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import AutoAssignment from '../views/AutoAssignment/AutoAssignment';
import '../scss/pages/_AutoAssignment.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(<Layout view={<AutoAssignment />} title='Auto Assignment' />);
