import { createRoot } from 'react-dom/client';
import SmallTicketCenterAppNative from '../views/SmallTicketCenter/SmallTicketCenterAppNative';
import '../scss/base/_base.scss';
import '../scss/fonts/icon-OWP/icon.scss';
import '../scss/style.scss';
import '../scss/pages/_SmallTicketCenter.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(<SmallTicketCenterAppNative />);
