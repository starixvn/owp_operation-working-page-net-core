import { useDispatch } from 'react-redux';
import { saveAs } from "file-saver";
import axios, { AxiosResponse } from 'axios';
import { setBackStep, setNextStep, setResetValidationResult } from './providerUploadSlice';

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
  const handleExport = async () => {
    const url = "/api/ProviderVerification/";
    const fileName = "CheckDifferenceProviderId.xlsx";
    const options = {
      url,
      method: 'GET',
      headers: { 'Accept': 'application/json' },
      responseType: 'blob' as const
    };
    const response: AxiosResponse<Blob> = await axios.request(options);
    const fileUrl = window.URL.createObjectURL(new Blob([response.data]));
    saveAs(fileUrl, fileName);
  };

  return (
    <button className='button-export' onClick={handleExport}>
      Export
    </button>
  );
};

export { BackButton, NextButton, SubmitButton, ExportButton };
