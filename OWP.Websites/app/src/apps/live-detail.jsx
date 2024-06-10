import { createRoot } from 'react-dom/client';
import IFrame from '../views/IFrame';
import DetailInformation from '../views/LiveDetail/LiveDetail';
import '../scss/pages/_LiveDetails.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <IFrame view={<DetailInformation />} title='Details Information' />
);
