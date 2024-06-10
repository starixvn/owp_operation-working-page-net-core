import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import UserManagement from './../views/UserManagement/UserManagement';
import '../scss/pages/_UserManagement.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(<Layout view={<UserManagement />} title='User Management' />);
