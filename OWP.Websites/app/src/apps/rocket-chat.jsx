import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import RocketChat from '../views/RocketChat/RocketChatNative';
import '../scss/style.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(<Layout view={<RocketChat />} title='Rocket Chat' />);
