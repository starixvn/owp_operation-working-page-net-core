import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import AssigneeSuggestionList from './../views/LAKpi/LaKpiNative';
import '../scss/pages/_LAKpi.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(<Layout view={<AssigneeSuggestionList />} title='LA KPI' />);
