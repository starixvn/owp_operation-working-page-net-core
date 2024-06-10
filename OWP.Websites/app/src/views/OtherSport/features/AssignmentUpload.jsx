import { useState, lazy, Suspense } from 'react';
import Loading from '../../components/Loading';

const WizardWrapper = lazy(() => import('./WizardWrapper'));

const AssignmentUpload = () => {
  const [canDisplayWizard, setCanDisplayWizard] = useState(false);

  return (
    <>
      <button
        className='button-upload-other-sport'
        onClick={() => setCanDisplayWizard(true)}>
        Upload assignment
      </button>
      {canDisplayWizard && (
        <Suspense fallback={<Loading />}>
          <WizardWrapper
            className='wizard-container'
            onClose={() => setCanDisplayWizard(false)}
          />
        </Suspense>
      )}
    </>
  );
};

export default AssignmentUpload;
