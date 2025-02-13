import { ChangeEvent, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Input } from 'reactstrap';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { RootState } from '../../app/storeNative';
import { validateFile } from '../../../Shared/uploadFile/validationHelper';
import { setFile, setIsUploadFMAndAIO } from '../slice/eventUploadSlice';
import { BackButton, NextButton } from './Button';

const UPLOAD_PLACEHOLDER = 'Upload file';

const AssignAndUpload = () => {
  const { isUploadFMAndAIO, eventDate } = useSelector(
    (state: RootState) => state.uploadReducer.request
  );
  const dispatch = useDispatch();

  const [textInput, setTextInput] = useState(UPLOAD_PLACEHOLDER);
  const [selectedFile, setSelectedFile] = useState<File>();

  const handleOnChange = async (event: ChangeEvent<HTMLInputElement>) => {
    setTextInput(UPLOAD_PLACEHOLDER);
    const file = event.target.files && event.target.files[0];
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
      Date: <b>{eventDate}</b>
      <div>Upload Event List with your assignment (max: 5MB)</div>
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
      <div className='checkbox'>
        <Input
          id='id'
          checked={isUploadFMAndAIO}
          type='checkbox'
          className='check-input'
          onChange={() => dispatch(setIsUploadFMAndAIO())}
        />
        <label className='label' htmlFor='id'>
          Upload FM & AIO&apos;s assignment
        </label>
      </div>
      <div className='wizard-buttons'>
        <BackButton />
        <NextButton onValidate={validateUploadFile} />
      </div>
    </div>
  );
};

export default AssignAndUpload;
