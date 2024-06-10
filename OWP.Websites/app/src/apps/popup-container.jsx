import { createRoot } from 'react-dom/client';
import PopupContainer from '../views/PopupContainer/PopupAppNative';
import '../scss/components/_chatboard.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(<PopupContainer />);
