import { useDispatch, useSelector } from 'react-redux';
import { CheckboxMarkedCircleOutlineIcon } from 'mdi-react';
import { useEffect } from 'react';
import { RootState } from '../../app/storeNative';
import { BackButton, SubmitButton, ExportButton } from './Button';
import { setResetValidationResult, uploadProviderThunk, submitProviderThunk, setShowTable } from './providerUploadSlice';
import { TReview } from '../../type/typeNative';

const Review = (props: TReview) => {
  const { validationResponse, file, showTable } = useSelector((state: RootState) => state.upload);
  const { isSuccess, value } = validationResponse;
  const dispatch = useDispatch();

  useEffect(() => {
    if (file) {
      dispatch(uploadProviderThunk(file));
    }
    dispatch(setResetValidationResult());
  }, []);

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
    dispatch(submitProviderThunk({ sessionId: { sessionId }, date: new Date(2025, 1, 27) }));
  };

  return (
    <>
      {isSuccess && (
        <div className='success-info'>
          <CheckboxMarkedCircleOutlineIcon size={20} />
          <span>
            There is no failed record in the total of {value.originalRecordCount} record(s)
          </span>
        </div>
      )}
      <div className='wizard-buttons'>
        {showTable && <ExportButton />}

        <BackButton onClick={() => {
          dispatch(setResetValidationResult());
          dispatch(setShowTable(false));
        }} />

        {!showTable && isSuccess && <SubmitButton onSubmit={submitUploadFile} />}
      </div>
    </>
  );
};

export default Review;
