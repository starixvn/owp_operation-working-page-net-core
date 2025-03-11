import { useState, ChangeEvent, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { BackButton, NextButton } from './Button';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { validateFile } from '../../../Shared/uploadFile/validationHelper';
import { setFile } from './providerUploadSlice';
import { TUploadProviderFile } from '../../type/typeNative';
import { setEventDate } from './providerUploadSlice';


const UPLOAD_PLACEHOLDER = 'Upload file';

const UploadProviderFile = (props: TUploadProviderFile) => {
  const [textInput, setTextInput] = useState(UPLOAD_PLACEHOLDER);
  const [selectedFile, setSelectedFile] = useState<File>();
  const dispatch = useDispatch();
  const date = new URL(window.location.href).searchParams.get("date");

  useEffect(() => {
    dispatch(setEventDate(date));
  }, []);

  const handleOnChange = async (event: ChangeEvent<HTMLInputElement>) => {
    setTextInput(UPLOAD_PLACEHOLDER);
    const file = event.target.files?.[0];
    file && setTextInput(file.name);
    file && setSelectedFile(file);
  };

  const validateUploadFile = () => {
    const message = validateFile(selectedFile);
    if (message) {
      ToastrWrapper.showError(message);
      return false;
    }

    dispatch(setFile(selectedFile));

    return true;
  };

  return (
    <div className='upload-file'>
      Date: <b>{date}</b>
      <div>Upload Agent file to compare (max: 5MB)</div>
      <div className='control-field'>
        <span className='text-input-file'>{textInput}</span>
        <label htmlFor='myFile' className='button-input-file'>
          Browse
        </label>
        <input
          type='file'
          id='myFile'
          name='filename'
          accept='.xlsx,.xls,.xlsm'
          onChange={handleOnChange}
        />
      </div>
      <div className='wizard-buttons'>
        <BackButton />
        <NextButton onValidate={validateUploadFile} />
      </div>
    </div>
  );
};

export default UploadProviderFile;
