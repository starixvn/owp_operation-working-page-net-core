import { useState } from 'react';
import PropTypes from 'prop-types';
import { generateButton } from '../../Shared/ControlTemplateCommon';

const propTypes = {
  onClose: PropTypes.func.isRequired,
  onStepChange: PropTypes.func,
  onRenderContent: PropTypes.func.isRequired,
  onSubmit: PropTypes.func.isRequired,
  onCanNext: PropTypes.func.isRequired,
  onCanBack: PropTypes.func.isRequired,
  onCanSubmit: PropTypes.func.isRequired,
  onFinish: PropTypes.func.isRequired,
  defaultStep: PropTypes.number
};

const defaultProps = {
  onRenderStep: f => f,
  onStepChange: f => f,
  defaultStep: 0
};

const Wizard = props => {
  const {
    onSubmit,
    onClose,
    onStepChange,
    onRenderContent,
    onCanNext,
    onCanBack,
    onCanSubmit,
    onFinish,
    defaultStep
  } = props;
  const [currentStep, setCurrentStep] = useState(defaultStep);

  const handleNext = step => {
    const canNext = onStepChange(step);
    canNext && setCurrentStep(step + 1);
  };

  const handleSubmit = step => {
    const canNext = onSubmit(step);
    canNext && setCurrentStep(step + 1);
  };

  const renderCommand = () => {
    const propButtonBack = {
      className: 'btn-back',
      title: 'Back',
      onClick: () => setCurrentStep(currentStep - 1)
    };

    const propButtonNext = {
      className: 'btn-next',
      title: 'Next',
      onClick: () => handleNext(currentStep)
    };

    const propButtonSubmit = {
      className: 'btn-submit',
      title: 'Submit',
      onClick: () => handleSubmit(currentStep)
    };

    const propButtonOK = {
      className: 'btn-finish',
      title: 'OK',
      onClick: () => onClose(currentStep)
    };

    return (
      <>
        <div className='wizard-buttons'>
          {onCanBack(currentStep) && generateButton(propButtonBack)}
          {onCanNext(currentStep) && generateButton(propButtonNext)}
          {onCanSubmit(currentStep) && generateButton(propButtonSubmit)}
          {onFinish(currentStep) && generateButton(propButtonOK)}
        </div>
      </>
    );
  };

  const renderCloseButton = () => {
    return (
      <button
        className='btn-close'
        onClick={onClose}>
        X
      </button>
    );
  };

  return (
    <>
      <div className='wizard-overlay'>
        <div className='wizard-body'>
          {renderCloseButton()}
          {onRenderContent(currentStep)}
          {renderCommand()}
        </div>
      </div>
    </>
  );
};

Wizard.propTypes = propTypes;
Wizard.defaultProps = defaultProps;

export default Wizard;
