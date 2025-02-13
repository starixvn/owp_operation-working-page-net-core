import { useEffect } from 'react';
import StepControl from './StepControl';
import GetTemplate from './GetTemplate';
import UploadProviderFile from './UploadProviderFile';
import Review from './Review';
import { useAppSelector } from '../../app/hooks';

const ProviderUpload = () => {
  const currentStep = useAppSelector(state => state.upload.currentStep);

  useEffect(() => {
    //dispatch(setEventDate(buildDateString(defaultDate)));
  }, []);

  const wizardContent = [
    <GetTemplate key={0} />,
    <UploadProviderFile key={1} />,
    <Review key={2} />
  ];

  return (
    <>
      <StepControl currentStep={currentStep} />
      {wizardContent[currentStep]}
    </>
  );
};

export default ProviderUpload;
