import { DragEvent } from 'react';
import FunctionDropdown from './components/FunctionDropdown.native';
import Describe from './components/Describe.native';
import RateFunction from './components/RateFunction';
import DragAndDrop from '../Shared/common/DragAndDrop/DragAndDrop';
import useStore from './app/store';
import {
  isImage,
  validateFileExtension,
  validateFileSize
} from '../Shared/chatBoardNew/helper/chatBoardHelper';
import ToastsWrapper from '../components/ToastrWrapper';
const Body = () => {
  const updateFeedbackInfo = useStore(state => state.updateFeedbackInfo);

  const handleSendFile = (event: DragEvent) => {
    if (event.dataTransfer.files) {
      const file = event.dataTransfer.files[0];
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

  return (
    <DragAndDrop onDrop={handleSendFile}>
      <div className='body'>
        <FunctionDropdown />
        <Describe />
        <RateFunction />
      </div>
    </DragAndDrop>
  );
};

export default Body;
