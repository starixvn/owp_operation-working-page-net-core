import { useDispatch, useSelector } from 'react-redux';
import { CheckboxMarkedCircleOutlineIcon, CloseCircleIcon } from 'mdi-react';
import { useEffect } from 'react';
import { RootState } from '../../app/storeNative';
import { BackButton, SubmitButton, ExportButton } from './Button';
import { setBackStep, setNextStep, setResetValidationResult, uploadProviderThunk, submitProviderThunk, resetResponse, setShowTable } from './providerUploadSlice';
import { getEvents } from './providerUploadServiceNative';
import ToastrWrapper from '../../../components/ToastrWrapper';

const Review = (props: TReview) => {
  const { validationResponse, isLoading, submitResponse, file, showTable } = useSelector((state: RootState) => state.upload);
  const { validationResults, isSuccess, value, exception } = validationResponse;
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(uploadProviderThunk(file));
    dispatch(setResetValidationResult());
    dispatch(resetResponse());
  }, []);

  useEffect(() => {
    if (exception) {
      ToastrWrapper.showError(exception);
    }
  }, [exception]);

  const submitUploadFile = async () => {
    if (value.originalRecordCount > 0) {
      handleSubmit();
      dispatch(setShowTable(true));
    } else {
      // TODO
    }

  };

  const handleSubmit = () => {
    const { sessionId } = validationResponse.value;
    dispatch(resetResponse());
    dispatch(submitProviderThunk({ sessionId }));

    //fake data
    dispatch(getEvents({
      eventDate: new Date(2025, 1, 6),
      onlyShowIsMismatchEvent: false
    }));
  };

  return (
    <>
      {isSuccess
        ? <div className='success-info'>
          <CheckboxMarkedCircleOutlineIcon size={20} />
          <span>
            There is no failed record in the total of {value.originalRecordCount} record(s)
          </span>
        </div>
        : !exception && <>
          <div className='error-info'>
            <CloseCircleIcon size={20} />
            <span>Number of failure(s): {validationResults.length}</span>
          </div>
        </>
      }
      <div className='wizard-buttons'>
        {showTable && <ExportButton />}

        <BackButton onClick={() => {
          dispatch(setResetValidationResult());
          dispatch(resetResponse());
          dispatch(setShowTable(false));
        }} />

        {!showTable && isSuccess && <SubmitButton onSubmit={submitUploadFile} />}
      </div>
    </>
  );
};

export default Review;
