import { createRoot } from 'react-dom/client';
import '../scss/pages/_AccountSetting.scss';
import AccountSetting from '../views/AccountSetting/AccountSetting';
import Layout from '../views/Layout';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(<Layout view={<AccountSetting />} title='Account Setting' />);
