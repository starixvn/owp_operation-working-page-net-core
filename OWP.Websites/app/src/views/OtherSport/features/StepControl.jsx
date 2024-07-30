import CloudDownloadIcon from 'mdi-react/CloudDownloadIcon';
import CloudUploadIcon from 'mdi-react/CloudUploadIcon';
import EyeIcon from 'mdi-react/EyeIcon';
import StepControlNodeTemplate from './StepControlNodeTemplate';
import PropTypes from 'prop-types';

const propTypes = {
  currentStep: PropTypes.number.isRequired
};

const StepControl = props => {
  const { currentStep } = props;

  const buildPropTemplateNode = ({
    classNameLiElement,
    title
  }) => ({
    classNameLiElement,
    classNameDivElement: 'pearl-icon',
    title
  });

  const renderStepUploadAssignment = () => {
    const title = 'UPLOAD ASSIGNMENT(<5MB)';
    const propNode = buildPropTemplateNode({
      classNameLiElement: 'active',
      title
    });
    return <StepControlNodeTemplate
      {...propNode}>
      <CloudDownloadIcon size={30} />
    </StepControlNodeTemplate>;
  };

  const renderStepValidation = () => {
    const propNode = buildPropTemplateNode({
      classNameLiElement: currentStep >= 1
        ? 'active'
        : '',
      title: 'VALIDATION'
    });
    return <StepControlNodeTemplate
      {...propNode}>
      <CloudUploadIcon size={30} />
    </StepControlNodeTemplate>;
  };

  const renderStepReview = () => {
    const propNode = buildPropTemplateNode({
      classNameLiElement: currentStep >= 2
        ? 'active'
        : '',
      title: 'REVIEW'
    });
    return <StepControlNodeTemplate
      {...propNode}>
      <EyeIcon size={30} />
    </StepControlNodeTemplate>;
  };

  return (
    <ul className='wizard-steps'>
      {renderStepUploadAssignment()}
      {renderStepValidation()}
      {renderStepReview()}
    </ul>
  );
};

StepControl.propTypes = propTypes;

export default StepControl;
