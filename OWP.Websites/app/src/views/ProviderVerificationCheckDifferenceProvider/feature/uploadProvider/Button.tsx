import { useDispatch } from 'react-redux';
import { setBackStep, setNextStep } from './providerUploadSlice';

const BackButton = (props: { onClick?: Function }) => {
  const { onClick } = props;
  const dispatch = useDispatch();
  const handleClick = () => {
    onClick?.();
    dispatch(setBackStep());
  };

  return (
    <button className='btn-back' onClick={handleClick}>
      Back
    </button>
  );
};

const NextButton = (props: { onValidate?: Function }) => {
  //const dispatch = useDispatch();

  const { onValidate } = props;
  const handleClick = () => {
    if (!onValidate || onValidate()) {
      //dispatch(setNextStep());
      //dispatch(resetResponse());
      //dispatch(setResetValidationResult());
    }
  };

  return (
    <button className='btn-next' onClick={handleClick}>
      Next
    </button>
  );
};

const SubmitButton = (props: { onSubmit: Function }) => {
  const { onSubmit } = props;

  return (
    <button className='btn-submit' onClick={() => onSubmit()}>
      Submit
    </button>
  );
};

export { BackButton, NextButton, SubmitButton };
