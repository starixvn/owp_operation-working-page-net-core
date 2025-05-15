import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import '../scss/pages/_ProviderStatistic.scss';
import ProviderStatisticNative from '../views/ProviderStatistic/ProviderStatisticNative';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<ProviderStatisticNative />} title='Provider Statistics' />
);
