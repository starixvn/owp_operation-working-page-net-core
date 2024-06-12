import { formatTime } from '../../../../DateTime/dateTimeHelper';
import PropTypes from 'prop-types';

const propTypes = {
  createdDate: PropTypes.string.isRequired,
  isFile: PropTypes.bool.isRequired,
  fileId: PropTypes.string,
  message: PropTypes.string.isRequired,
  className: PropTypes.string,
  URL_DOWNLOAD_FILE: PropTypes.string
};

const MessageContentsNative = props => {
  const { createdDate, fileId, isFile, message, className } = props;
  const getCreatedTime = () =>
    formatTime(createdDate, 'minute', 'hh:mm A DD/MM/YYYY');

  const getMessageContent = () => {
    return (
      <>
        {!fileId && <span>{message}</span>}
        {fileId && isFile && <span className={className}>{message}</span>}
        {fileId && !isFile && (
          <>
            <span className={`${className} not-available`}>{message}</span>
            <p>The file is no longer available for download</p>
          </>
        )}
      </>
    );
  };

  return { getCreatedTime, getMessageContent };
};

MessageContentsNative.propTypes = propTypes;
export default MessageContentsNative;
