import { createRef, ChangeEvent } from 'react';
import HighlightOffIcon from '@mui/icons-material/HighlightOff';
import { TextField } from '@mui/material';
import AttachFileIcon from '../../../assets/svg-element/AttachFileIcon';
import { BODY } from '../const.json';
import useStore from '../app/store';
import { CloseIcon, FileOutlineIcon } from 'mdi-react';
import {
  isImage,
  validateFileExtension,
  validateFileSize
} from '../../Shared/chatBoardNew/helper/chatBoardHelper';
import ToastsWrapper from '../../components/ToastrWrapper';
const Describe = () => {
  const { inputMessage, selectedFile, selectedImage } = useStore(
    state => state.feedBackInfo
  );
  const updateFeedbackInfo = useStore(state => state.updateFeedbackInfo);
  const inputFileRef = createRef<HTMLInputElement>();

  const handleFileInputChange = (event: ChangeEvent<HTMLInputElement>) => {
    if (event.target.files) {
      const file = event.target.files[0];
      if (!validateFileExtension(file.name)) {
        ToastsWrapper.showError('File extension not allowed!');
      } else if (!validateFileSize(file.size)) {
        ToastsWrapper.showError(
          "The file's size should not larger than 5MB. Try another one"
        );
      } else {
        isImage(file.name)
          ? updateFeedbackInfo('selectedImage', file)
          : updateFeedbackInfo('selectedFile', file);
      }
    }
  };

  const handleInputMessageChange = (event: ChangeEvent<HTMLInputElement>) => {
    updateFeedbackInfo('inputMessage', event.target.value);
  };

  const handlePaste = (event: ClipboardEvent) => {
    const items = (event.clipboardData || event.originalEvent.clipboardData)
      .items;
    for (const item of items) {
      if (item.type.indexOf('image') !== -1) {
        const blob = item.getAsFile();
        updateFeedbackInfo('selectedImage', blob);
        break;
      }
    }
  };

  const handleRemoveImage = () => {
    updateFeedbackInfo('selectedImage', null);
  };

  return (
    <div className='describe'>
      <div className='description'>
        <p className='title' style={{ marginBottom: '3px' }}>
          {BODY.DESCRIPTION.EN}
          <span className='required'> *</span>
        </p>
        <p className='title'>
          {BODY.DESCRIPTION.CN}
        </p>
      </div>
      <div className='chat-box'>
        <div>
          {selectedImage && (
            <div className='input-image'>
              <img src={URL.createObjectURL(selectedImage)} alt='Pasted' />
              <div className='icon-remove-div'>
                <HighlightOffIcon
                  fontSize='large'
                  onClick={handleRemoveImage}
                />
              </div>
            </div>
          )}

          <TextField
            inputRef={inputFileRef}
            variant='outlined'
            placeholder='Type a message...'
            onChange={handleInputMessageChange}
            value={inputMessage}
            autoFocus
            multiline
            onPaste={handlePaste}
            minRows={10}
            maxRows={25}
            inputProps={{ maxLength: 2000 }}
            spellCheck={false}
          />
        </div>
        <AttachFileIcon
          handleOnClick={() =>
            inputFileRef?.current && inputFileRef.current.click()
          }
        />

        <input
          ref={inputFileRef}
          hidden
          type='file'
          onChange={handleFileInputChange}
        />
      </div>
      {selectedFile && (
        <div className='file-list'>
          <div className='item item-0'>
            <div className='info-file'>
              <FileOutlineIcon className='icon-file' />
              <div>{selectedFile.name}</div>
              <div className='file-volume'>
                {(selectedFile.size / (1024 * 1024)).toFixed(2)} MB
              </div>
            </div>
            <CloseIcon
              className='icon-close'
              onClick={() => updateFeedbackInfo('selectedFile', null)}
            />
          </div>
        </div>
      )}
    </div>
  );
};

export default Describe;
