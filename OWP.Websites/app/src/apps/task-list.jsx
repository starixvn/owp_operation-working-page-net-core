import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import TaskList from '../views/TaskList/TaskList';
import '../scss/pages/_TaskList.scss';
import '../scss/components/_chatboard.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(<Layout view={<TaskList />} title='Task List' />);
