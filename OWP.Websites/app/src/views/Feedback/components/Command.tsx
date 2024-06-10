import useStore from '../app/store';
import ToastrWrapper from '../../components/ToastrWrapper';

const Command = () => {
  const postFeedback = useStore(state => state.postFeedback);
  const updateFeedbackInfo = useStore(state => state.updateFeedbackInfo);
  const validateBeforeCreate = useStore(state => state.validateBeforeCreate);

  const handleOnClick = () => {
    const result = validateBeforeCreate();
    if (!result.valid) {
      ToastrWrapper.showError(result.message);
      return;
    }
    postFeedback();
    resetPropDescribe();
  };

  const resetPropDescribe = () => {
    updateFeedbackInfo('inputMessage', '');
    updateFeedbackInfo('selectedFile', null);
    updateFeedbackInfo('selectedImage', null);
  };

  return (
    <div className='command'>
      <button className='submit' onClick={handleOnClick}>
        Submit
      </button>
    </div>
  );
};

export default Command;
