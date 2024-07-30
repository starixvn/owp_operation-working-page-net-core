import { Suspense } from 'react';
import PropTypes from 'prop-types';
import {
  acceptedFileFormat,
  noFileIsSelected,
  fileExceed5Mb
} from './stepWizardValidator';
import validateFileThunk from './validationThunkNative';
import useStableDispatch from '../../Shared/customHook/useStableDispatch';
import { setErrorMessage, setWarningMessage } from './fileLoaderSlice';
import submitThunk from './submitThunkNative';
import { useStore, useSelector } from 'react-redux';
import Loading from '../../components/Loading';
import LazyFileLoader from '../lazyExport/LazyFileLoader';
import LazyValidation from '../lazyExport/LazyValidation';
import LazyStepControl from '../lazyExport/LazyStepControl';
import LazyWizardLoader from '../lazyExport/LazyWizardLoader';
import {
  getCurrentDateGmtPlus8,
  addDays,
  buildDateString
} from '../../Shared/DateTime/dateTimeHelper';
const propTypes = {
  onClose: PropTypes.func.isRequired
};

let file;
const WizardWrapper = ({ onClose }) => {
  const dispatch = useStableDispatch();
  const store = useStore();
  const { fromDate, toDate } = useSelector(state => state.fileLoader);
  const dateNow = getCurrentDateGmtPlus8();
  const yesterday = buildDateString(addDays(dateNow, -1));

  const handleFileChange = fileInfo => {
    file = fileInfo;
  };

  const fileInfoIsValid = () => {
    const validateExistsFileResult = noFileIsSelected(file);
    if (validateExistsFileResult.hasError) {
      dispatch(setErrorMessage({ ...validateExistsFileResult }));
      return false;
    }

    const validateFileNotSupportResult = acceptedFileFormat(file);
    if (validateFileNotSupportResult.hasError) {
      dispatch(setErrorMessage({ ...validateFileNotSupportResult }));
      return false;
    }
    const validateFileExceed5MbResult = fileExceed5Mb(file);
    if (validateFileExceed5MbResult.hasError) {
      dispatch(setErrorMessage({ ...validateFileExceed5MbResult }));
      return false;
    }
    if (dateNow.getHours() >= 12 && fromDate === yesterday) {
      dispatch(
        setWarningMessage({
          hasWarning: true,
          messageWarning:
            'If you choose yesterday, the data of matches uploaded before shall be deleted'
        })
      );
    }
    return true;
  };

  const validateFileContent = () => {
    (async () => {
      const validationResult = await dispatch(
        validateFileThunk({ file, fromDate, toDate })
      );
      return validationResult.isSuccessful;
    })();
  };

  const wizardContent = [
    {
      validate: fileInfoIsValid,
      component: <LazyFileLoader onFileChange={handleFileChange} />
    },
    {
      validate: validateFileContent,
      component: <LazyValidation onClose={onClose} />
    },
    {
      validate: () => true,
      component: <LazyValidation onClose={onClose} />
    }
  ];

  const renderContentWizard = currentStep => {
    return (
      <>
        <LazyStepControl currentStep={currentStep} />
        {wizardContent[currentStep].component}
      </>
    );
  };

  const handleStepChange = nextStep => {
    const canNext = wizardContent[nextStep].validate();
    if (canNext) {
      wizardContent[nextStep + 1].validate();
    }
    return !!canNext;
  };

  const handleSubmit = () => {
    const state = store.getState();
    const { isSuccessful, sessionId } = state.validation.validationResult;

    isSuccessful && dispatch(submitThunk({ sessionId, fromDate, toDate }));
    return isSuccessful;
  };

  const totalStep = wizardContent.length;
  return (
    <Suspense fallback={<Loading />}>
      <LazyWizardLoader
        defaultStep={0}
        onRenderContent={renderContentWizard}
        onClose={onClose}
        onStepChange={handleStepChange}
        onSubmit={handleSubmit}
        onCanNext={currentStep => currentStep < totalStep - 2}
        onCanBack={currentStep =>
          currentStep > 0 && currentStep <= totalStep - 2
        }
        onCanSubmit={currentStep => currentStep === totalStep - 2}
        onFinish={currentStep => currentStep > totalStep - 2}
      />
    </Suspense>
  );
};

WizardWrapper.propTypes = propTypes;
export default WizardWrapper;
