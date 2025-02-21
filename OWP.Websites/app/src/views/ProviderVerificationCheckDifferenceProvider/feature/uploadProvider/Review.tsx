import { useDispatch, useSelector } from 'react-redux';
import { CheckboxMarkedCircleOutlineIcon } from 'mdi-react';
import { useEffect } from 'react';
import { RootState } from '../../app/storeNative';
import { BackButton, SubmitButton } from './Button';
import { setBackStep, setResetValidationResult, uploadProviderThunk, submitProviderThunk, resetResponse } from './providerUploadSlice';

const Review = (props: TReview) => {
  const { validationResponse, isLoading, submitResponse, file } = useSelector((state: RootState) => state.upload);
  const { validationResults, isSuccessful, recordCount, exception } = validationResponse;
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(uploadProviderThunk(file));
    dispatch(setResetValidationResult());
    dispatch(resetResponse());
  }, []);

  const submitUploadFile = async () => {
    if (recordCount > 0) {
      handleSubmit();
    } else {
      //TODO
    }
  };

  const handleSubmit = () => {
    const { sessionId } = validationResponse;
    dispatch(resetResponse());
    dispatch(submitProviderThunk({ sessionId }));
  };

  return (
    <div>
      <div className='success-info'>
        <CheckboxMarkedCircleOutlineIcon size={20} />
        <span>Successful</span>
      </div>
      <div className='wizard-buttons'>
        <BackButton onClick={() => {
          dispatch(setResetValidationResult());
          dispatch(resetResponse());
        }} />
        {/* {isSuccessful && <SubmitButton onSubmit={submitFile} />} */}
        <SubmitButton onSubmit={submitUploadFile} />
      </div>
    </div>
  );
};

export default Review;
