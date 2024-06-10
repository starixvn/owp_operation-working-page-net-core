import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import '../scss/pages/_EventUpload.scss';
import EventUploadAppNative from '../views/EventUpload/EventUploadAppNative';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(<Layout view={<EventUploadAppNative />} title='Event Upload' />);
