import { v4 } from 'uuid';
import { CloseIcon, FileOutlineIcon } from 'mdi-react';
import PropTypes from 'prop-types';
import { TooltipText } from '../../common/Tooltip';
const propTypes = {
  files: PropTypes.array,
  onRemove: PropTypes.func
};
const SelectedAttachments = props => {
  const { files, onRemove } = props;
  if (!files) {
    return null;
  }
  const renderFiles = () => {
    return files.map((file, index) => {
      const sizeInMB = (file.size / (1024 * 1024)).toFixed(2);
      return (
        <div key={`${v4()}${index}`} className={`item item-${index}`}>
          <div className='info-file'>
            <FileOutlineIcon className='icon-file' />
            <TooltipText
              innerClassName='file-name'
              rowIndex={index}
              toolTipContent={file.name}
              text={file.name}
            />
            <div className='file-volume'>{sizeInMB} MB</div>
          </div>
          <CloseIcon className='icon-close' onClick={() => onRemove(index)} />
        </div>
      );
    });
  };
  return <div className='file-list'>{renderFiles()}</div>;
};
SelectedAttachments.propTypes = propTypes;

export default SelectedAttachments;
