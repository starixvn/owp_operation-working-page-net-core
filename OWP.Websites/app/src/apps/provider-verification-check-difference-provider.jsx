import { createRoot } from 'react-dom/client';
import Layout from '../views/Layout';
import CheckDiffDifferenceProviderNative from './../views/ProviderVerificationCheckDifferenceProvider/CheckDiffDifferenceProviderNative';
import '../scss/components/_chatboard.scss';
import '../scss/pages/_ProviderVerificationCheckDifferenceProvider.scss';
import '../scss/pages/_multipleFilter.scss';

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
    <Layout
        view={<CheckDiffDifferenceProviderNative />}
        title='Check Difference Provider'
    />
);
