import CloudDownloadIcon from 'mdi-react/CloudDownloadIcon';
import CloudUploadIcon from 'mdi-react/CloudUploadIcon';
import EyeIcon from 'mdi-react/EyeIcon';
import { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/storeNative';
import { useAppDispatch } from '../../app/hooks';
import { setStep, setShowTable } from './providerUploadSlice';

type PropTypes = {
  currentStep: number;
};

const STEP_0 = 0;
const STEP_1 = 1;
const STEP_2 = 2;

const StepControl = (props: PropTypes) => {
  const { showTable } = useSelector((state: RootState) => state.upload);
  const dispatch = useAppDispatch();
  const currentStep = props.currentStep;
  const [clickStep, setClickStep] = useState(currentStep);

  const handleClickStep = (step: number) => {
    setClickStep(step);
    if (step < STEP_2) {
      dispatch(setStep(step));
    }
  };

  useEffect(() => {
    if (showTable && clickStep < STEP_2) {
      dispatch(setShowTable(false));
    }
  }, [clickStep]);

  const renderStepNodeTemplate = (step: number) => {
    const className = currentStep > step || showTable ? 'active' : '';
    const title = [
      {
        title: 'GET AGENT PROVIDERID TEMPLATE',
        onClick: () => handleClickStep(STEP_0),
        icon: <CloudDownloadIcon size={24} />
      },
      {
        title: 'FULLFILE & UPLOAD',
        onClick: () => handleClickStep(STEP_1),
        icon: <CloudUploadIcon size={24} />
      },
      {
        title: 'REVIEW',
        onClick: () => handleClickStep(STEP_2),
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
