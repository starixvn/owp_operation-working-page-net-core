import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import AppDayShiftManagement from '../views/PreMatch/AppDayShiftManagement';
import '../scss/pages/_DayShiftAssignment.scss';
import '../scss/components/_chatboard.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout
    view={<AppDayShiftManagement />}
    title='DayShift Assignment'
  />
);
