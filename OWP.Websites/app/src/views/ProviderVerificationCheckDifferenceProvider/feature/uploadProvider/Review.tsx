import { useDispatch, useSelector } from 'react-redux';
import { CheckboxMarkedCircleOutlineIcon } from 'mdi-react';
import { RootState } from '../../app/storeNative';
import { BackButton, SubmitButton } from './Button';

const Review = (props: TReview) => {
  const { validationResponse, isLoading, submitResponse } = useSelector((state: RootState) => state.upload);
  const { validationResults, isSuccessful, recordCount, exception } = validationResponse;

  const submitFile = async () => {
    // if (recordCount > 0) {
    //   handleSubmit();
    // } else {
    //   const confirm = await ToastrWrapper.show({ ...toastOptions, html: noEventWarningMessage });
    //   if (confirm.value) {
    //     const noRecommendConfirm = await ToastrWrapper.show({ ...toastOptions, html: noRecommendWarningMessage });
    //     if (noRecommendConfirm.value) {
    //       handleSubmit();
    //     }
    //   }
    // }
  };

  return (
    <div style={{ justifyContent: "space-between" }}>
      <div className='success-info'>
          <CheckboxMarkedCircleOutlineIcon size={20} />
          <span>Successful</span>
        </div>
      <div className='wizard-buttons'>
        <BackButton onClick={() => {
          // dispatch(setResetValidationResult());
          // dispatch(resetResponse());
        }} />
        {/* {isSuccessful && <SubmitButton onSubmit={submitFile} />} */}
        <SubmitButton onSubmit={submitFile} />
      </div>
    </div>
  );
};

export default Review;
