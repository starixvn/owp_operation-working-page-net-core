import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import OvertimeReport from '../views/OvertimeReport/OvertimeReportNative';
import 'react-table-hoc-fixed-columns-ts/lib/styles.css';
import '../scss/pages/_OvertimeReport.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<OvertimeReport />} title='Overtime Report' />
);
