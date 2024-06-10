import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import ProviderArrangement from '../views/ProviderArrangement/ProviderArrangementNative';
import '../scss/pages/_ProviderArrangement.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<ProviderArrangement />} title='Provider Arrangement' />
);
