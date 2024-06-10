import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import PrematchReport from '../views/PrematchReport/PrematchReportNative';
import 'react-table-hoc-fixed-columns-ts/lib/styles.css';
import '../scss/pages/_PrematchReport.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<PrematchReport />} title='Prematch Report' />
);
