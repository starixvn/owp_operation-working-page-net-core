import { createRoot } from 'react-dom/client';
import SmallChattingWrapper from '../views/SmallChatting/SmallChattingWrapper';
import '../scss/components/_chatboard.scss';
import '../scss/components/_chatboard-small.scss';
import '../scss/base/_base.scss';
import '../scss/fonts/icon-OWP/icon.scss';
import '../scss/style.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(<SmallChattingWrapper></SmallChattingWrapper>);
