import { createRef } from 'react';
import PropTypes from 'prop-types';
import DragAndDrop from '../../common/DragAndDrop/DragAndDrop';
import { validExtension } from '../../chatBoardNew/const/index.json';
import ToastrWrapper from '../../../components/ToastrWrapper';
import ValidateHelper from './ValidateHelper';
import { cloneDeep } from 'lodash';
import SelectedAttachments from './SelectedAttachments';
import ErrorTable from './ErrorTable';

const propTypes = {
  setSelectedFiles: PropTypes.func,
  selectedFiles: PropTypes.array
};
const Attachments = props => {
  const { selectedFiles, setSelectedFiles } = props;
  const inputFileRef = createRef();

  const validateFile = files => {
    const { validatePipe } = ValidateHelper();
    const { isValid, errorList } = validatePipe(
      files,
      files.length + selectedFiles.length
    );
    if (isValid) {
      setSelectedFiles(oldArray => [...oldArray, ...files]);
    } else {
      ToastrWrapper.showError(<ErrorTable errors={errorList} />);
    }
  };

  const handleOnChange = event => {
    const arrayFile = Array.from(event.target.files);
    validateFile(arrayFile);
  };

  const handleDropFile = event => {
    const arrayFile = Array.from(event.dataTransfer.files);
    validateFile(arrayFile);
  };

  const handleRemoveSelectedFile = index => {
    const newSelectFiles = cloneDeep(selectedFiles);
    newSelectFiles.splice(index, 1);
    setSelectedFiles(newSelectFiles);
  };

  return (
    <div className='form-createTicket'>
      <div className='left-form'>Attach File:</div>
      <div className='right-form'>
        <DragAndDrop onDrop={handleDropFile} className='drop-create-ticket'>
          <div className='drag-able box-drag-able'>
            <button
              className='drag-able button-choose-file'
              onClick={() => inputFileRef.current.click()}>
              Choose File
            </button>
            <div className='drag-able text-drag'>
              Or drag and drop file here
            </div>
          </div>
        </DragAndDrop>
        <input
          accept={validExtension}
          ref={inputFileRef}
          type='file'
          multiple
          hidden
          onChange={handleOnChange}
          aria-label='file-input'
        />
        <SelectedAttachments
          files={selectedFiles}
          onRemove={handleRemoveSelectedFile}
        />
      </div>
    </div>
  );
};

Attachments.propTypes = propTypes;
export default Attachments;
