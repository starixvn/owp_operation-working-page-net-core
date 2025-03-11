import { useDispatch } from 'react-redux';
import { setBackStep, setNextStep, setResetValidationResult } from './providerUploadSlice';
import { exportExcel } from '../../../Shared/common/ExportExcel/Helper';
import Utils from '../../../Shared/Utils';
import dayjs from 'dayjs';
import { useAppSelector } from '../../app/hooks';

const BackButton = (props: { onClick?: Function; }) => {
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

const NextButton = (props: { onValidate?: Function; }) => {
  const dispatch = useDispatch();
  const { onValidate } = props;
  const handleClick = () => {
    if (!onValidate || onValidate()) {
      dispatch(setNextStep());
      dispatch(setResetValidationResult());
    }
  };

  return (
    <button className='btn-next' onClick={handleClick}>
      Next
    </button>
  );
};

const SubmitButton = (props: { onSubmit: Function; }) => {
  const { onSubmit } = props;

  return (
    <button className='btn-submit' onClick={() => onSubmit()}>
      Submit
    </button>
  );
};

const ExportButton = () => {
  const { upload } = useAppSelector(
    state => state
  );
  const { sessionId } = upload.validationResponse.value;
  const { eventDate } = upload;
  const handleExport = async () => {
    const date = Utils.formatDate(eventDate);
    const params = `eventDate=${date}&sessionId=${sessionId}`;
    const url = `api/ProviderVerification/export/difference?${params}`;
    const fileName = `CheckDifferenceProviderId - ${dayjs(date).format('MMDDYYYY')}.xlsx`;

    exportExcel(url, fileName);
  };

  return (
    <button className='button-export' onClick={handleExport}>
      Export
    </button>
  );
};

export { BackButton, NextButton, SubmitButton, ExportButton };
