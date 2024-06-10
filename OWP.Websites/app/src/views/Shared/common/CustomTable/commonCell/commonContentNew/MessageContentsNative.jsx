import Linkify from 'react-linkify';
import { formatTime } from '../../../../DateTime/dateTimeHelper';
import PropTypes from 'prop-types';
import { isImage } from '../../../../chatBoardNew/helper/chatBoardHelper';
import {
  NET_CORE_FILE_UPLOAD,
  NET_FW_FILE_UPLOAD
} from '../../../../../Shared/Constants';
import { URL } from '../../../../../Shared/chatBoardNew/const/index.json';

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

  const handleOnClick = () => {
    const basicUrl =
      process.env.NODE_ENV === 'development'
        ? `${NET_CORE_FILE_UPLOAD}/${URL.DOWNLOAD_FILE}`
        : `${window.netCoreRootUrl}/${NET_FW_FILE_UPLOAD}/${URL.DOWNLOAD_FILE}`;

    const url = `${basicUrl}?fileId=${fileId}`;
    if (isImage(message)) {
      window.open(url);
    } else {
      window.location.href = url;
    }
  };

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
