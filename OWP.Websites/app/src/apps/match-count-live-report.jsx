import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import MatchCountLiveReport from '../views/MatchCountLiveReport/MatchCountLiveReportNative';
import 'react-table-hoc-fixed-columns-ts/lib/styles.css';
import '../scss/pages/_MatchCountLiveReport.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<MatchCountLiveReport />} title='Match Count Live Report' />
);
