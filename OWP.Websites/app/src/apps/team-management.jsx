import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import '../scss/pages/_TeamManagement.scss';
import TeamManagementNative from '../views/TeamManagement/TeamManagementNative';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(<Layout view={<TeamManagementNative />} title='Team Management' />);
