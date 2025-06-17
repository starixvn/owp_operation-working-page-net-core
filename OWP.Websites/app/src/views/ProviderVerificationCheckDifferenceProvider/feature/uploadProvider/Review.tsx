import { useDispatch } from 'react-redux';
import { CheckboxMarkedCircleOutlineIcon, CloseCircleIcon } from 'mdi-react';
import { useAppSelector } from '../../app/hooks';
import { useEffect } from 'react';
import { BackButton, SubmitButton, ExportButton } from './Button';
import { setResetValidationResult, uploadProviderThunk, submitProviderThunk, setShowTable } from './providerUploadSlice';
import { TReview } from '../../type/typeNative';

const Review = (props: TReview) => {

  const { upload } = useAppSelector(
    state => state
  );
  const { isSuccess, value, errors } = upload.validationResponse;
  const { validationResponse, file, showTable, eventDate } = upload;

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
    dispatch(submitProviderThunk({ sessionId: sessionId, date: eventDate }));
  };

  return (
    <>
      {isSuccess ? (
        <div className='success-info'>
          <CheckboxMarkedCircleOutlineIcon size={20} />
          <span>
            There is no failed record in the total of {value.originalRecordCount} record(s)
          </span>
        </div>
      ) : (
        errors?.length > 0 && (
          <div className="error-info">
            <CloseCircleIcon size={20} />
            <div className='error-info-text'>
              {errors.map((error, index) => (
                <span key={index}>
                  {error.code === 1003
                    ? error.description
                    : `${error.description} in row ${error.row}`}
                </span>
              ))}
            </div>
          </div>
        )
      )
      }
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
