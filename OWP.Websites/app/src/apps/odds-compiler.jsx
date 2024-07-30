import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import OddsCompiler from '../views/OddsComplier/OddsCompiler';
import '../scss/pages/_OddsCompiler.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<OddsCompiler />} title='Odds Compiler' />
);
