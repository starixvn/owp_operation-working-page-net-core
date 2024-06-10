import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { getCurrentDateGmtPlus8 } from '../../../Shared/DateTime/dateTimeHelper';
import { RootState } from '../../app/storeNative';
import { buildDateString } from '../helper/buildHelper';
import { setEventDate } from '../slice/eventUploadSlice';
import AssignAndUpload from './AssignAndUpload';
import GetEvent from './GetEvent';
import Review from './Review';
import StepControl from './StepControl';

const EventUploadContent = () => {
  const { currentStep } = useSelector((state: RootState) => state.uploadReducer);
  const defaultDate = getCurrentDateGmtPlus8();
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(setEventDate(buildDateString(defaultDate)));
  }, []);

  const wizardContent = [
    <GetEvent key={0} />,
    <AssignAndUpload key={1} />,
    <Review key={2} />
  ];

  return (
    <>
      <StepControl currentStep={currentStep} />
      {wizardContent[currentStep]}
    </>
  );
};

export default EventUploadContent;