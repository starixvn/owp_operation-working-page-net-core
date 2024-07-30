import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import ProductPreference from '../views/ProductPreference/ProductPreference';
import '../scss/pages/_ProductPreference.scss';

const container = document.getElementById('root');
const root = createRoot(container);
root.render(
  <Layout view={<ProductPreference />} title='Assignment Configuration' />
);