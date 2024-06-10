import { CheckboxMarkedCircleOutlineIcon, CloseCircleIcon } from 'mdi-react';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Loading from '../../../components/Loading';
import { RootState } from '../../app/storeNative';
import { setBackStep, setResetValidationResult, uploadEventThunk, submitEventThunk, resetResponse } from '../slice/eventUploadSlice';
import { BackButton, SubmitButton } from './Button';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { noEventWarningMessage, noRecommendWarningMessage, toastOptions } from '../../Const';
import { MessageResponse } from '../../TypeNative';

const ROW_OFFSET = 2;

const Review = () => {
  const { request, validationResponse, isLoading, submitResponse } = useSelector((state: RootState) => state.uploadReducer);
  const { validationResults, isSuccessful, recordCount, exception } = validationResponse;
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(uploadEventThunk(request));
    dispatch(setResetValidationResult());
    dispatch(resetResponse());
  }, []);

  useEffect(() => {
    if (exception) {
      ToastrWrapper.showError(exception);
    }
  }, [exception]);

  useEffect(() => {
    if (isSuccessful) {
      const { isSuccessful: isSubmitSuccessful, message } = submitResponse;
      if (!isSubmitSuccessful && message) {
        ToastrWrapper.showError(submitResponse.message);
      }
      if (isSubmitSuccessful) {
        dispatch(setBackStep());
        ToastrWrapper.showInfo(submitResponse.message);
      }
    }
  }, [submitResponse, isSuccessful, dispatch]);

  const submitAssignment = async () => {
    if (recordCount > 0) {
      handleSubmit();
    } else {
      const confirm = await ToastrWrapper.show({ ...toastOptions, html: noEventWarningMessage });
      if (confirm.value) {
        const noRecommendConfirm = await ToastrWrapper.show({ ...toastOptions, html: noRecommendWarningMessage });
        if (noRecommendConfirm.value) {
          handleSubmit();
        }
      }
    }
  };

  const handleSubmit = () => {
    const { sessionId } = validationResponse;
    const { eventDate, isUploadFMAndAIO } = request;
    dispatch(resetResponse());
    dispatch(submitEventThunk({ sessionId, eventDate, isUploadFMAndAIO }));
  };

  if (isLoading) {
    return <Loading />;
  }

  return (
    <div>
      {isSuccessful
        ? <div className='success-info'>
          <CheckboxMarkedCircleOutlineIcon size={20} />
          <span>
            There is no failed record in the total of {recordCount} record(s)
          </span>
        </div>
        : !exception && <>
          <div className='error-info'>
            <CloseCircleIcon size={20} />
            <span>Number of failure(s): {validationResults.length}</span>
          </div>
          <div className='tableFixHead'>
            <table className='table'>
              <thead>
                <tr>
                  <th>Row</th>
                  <th>Message</th>
                </tr>
              </thead>
              <tbody>
                {validationResults.map((result: MessageResponse, index: number) =>
                  <tr key={index}>
                    <td><span>{result.row + ROW_OFFSET}</span></td>
                    <td><span>{result.message}</span></td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </>
      }
      <div className='wizard-buttons'>
        <BackButton onClick={() => {
          dispatch(setResetValidationResult());
          dispatch(resetResponse());
        }} />
        {isSuccessful && <SubmitButton onSubmit={submitAssignment} />}
      </div>
    </div>
  );
};

export default Review;