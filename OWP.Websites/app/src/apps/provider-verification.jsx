import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import ProviderVerification from './../views/ProviderVerification/ProviderVerificationNative';
import '../scss/pages/_ProviderVerification.scss';
import '../scss/components/_chatboard.scss';
import '../scss/pages/_multipleFilter.scss';
import '../scss/pages/_popupEditor.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
  <Layout view={<ProviderVerification />} title='Provider Verification' />
);
