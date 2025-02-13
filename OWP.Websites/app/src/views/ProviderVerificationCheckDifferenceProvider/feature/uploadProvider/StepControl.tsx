import CloudDownloadIcon from 'mdi-react/CloudDownloadIcon';
import CloudUploadIcon from 'mdi-react/CloudUploadIcon';
import EyeIcon from 'mdi-react/EyeIcon';
import { useAppDispatch } from '../../app/hooks';
import { setStep } from './providerUploadSlice';

type PropTypes = {
  currentStep: number;
};
const STEP_0 = 0;
const STEP_1 = 1;
const STEP_2 = 2;

const StepControl = (props: PropTypes) => {
  const dispatch = useAppDispatch();
  const { currentStep } = props;

  const renderStepNodeTemplate = (step: number) => {
    const className = currentStep > step ? 'active' : '';
    const title = [
      {
        title: 'GET AGENT PROVIDERID TEMPLATE',
        onClick: () => dispatch(setStep(STEP_0)),
        icon: <CloudDownloadIcon size={24} />
      },
      {
        title: 'FULLFILE & UPLOAD',
        onClick: () => dispatch(setStep(STEP_1)),
        icon: <CloudUploadIcon size={24} />
      },
      {
        title: 'REVIEW',
        icon: <EyeIcon size={24} />
      }
    ];

    const handleClick = () => {
      const onClick = title[step].onClick;
      onClick && onClick();
    };

    return (
      <li className={className} role='tab'>
        <div className='pearl-icon' onClick={handleClick}>
          {title[step].icon}
        </div>
        <div className='pearl-title' onClick={handleClick}>
          {title[step].title}
        </div>
      </li>
    );
  };

  return (
    <ul className='wizard-steps'>
      {renderStepNodeTemplate(STEP_0)}
      {renderStepNodeTemplate(STEP_1)}
      {renderStepNodeTemplate(STEP_2)}
    </ul>
  );
};

export default StepControl;
