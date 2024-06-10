import Linkify from 'react-linkify';
import { formatTime } from '../../DateTime/dateTimeHelper';
import { isImage } from '../helper/chatBoardHelper';
import { URL } from '../const/index.json';
import {
  NET_CORE_FILE_UPLOAD,
  NET_FW_FILE_UPLOAD
} from '../../../Shared/Constants';
import ExcelIcon from '../../../../assets/svg-element/ExcelIcon';
import PdfIcon from '../../../../assets/svg-element/PdfIcon';
import WordIcon from '../../../../assets/svg-element/WordIcon';
import PptIcon from '../../../../assets/svg-element/PptIcon';
import AttachFileIcon from '../../../../assets/svg-element/AttachFileIcon';
import QuoteIcon from '../../../../assets/svg-element/QuoteIcon';
import ImageDisplay from './ImageDisplay';
import EmojiDisplay from './EmojiDisplay';

const MessageContents = ({
  createdDate = '',
  fileId = '',
  messageContent = '',
  quoteMessageId = null,
  quoteMessage = '',
  quoteUsername = '',
  quoteCreatedDate = '',
  imageFileName,
  messageReactIcons = [],
  ticketId = null
}) => {
  const getCreatedTime = () => formatTime(createdDate, 'minute', 'hh:mm A');
  const handleOnClick = () => {
    const basicUrl =
      process.env.NODE_ENV === 'development'
        ? `${NET_CORE_FILE_UPLOAD}/${URL.DOWNLOAD_FILE}`
        : `${window.netCoreRootUrl}/${NET_FW_FILE_UPLOAD}/${URL.DOWNLOAD_FILE}`;

    const url = `${basicUrl}?fileId=${fileId}`;

    if (isImage(messageContent)) {
      window.open(url);
    } else {
      window.location.href = url;
    }
  };

  const getFileIcon = () => {
    const extension = messageContent
      .split('.')
      .pop()
      .toLocaleLowerCase()
      .trim();
    switch (extension) {
      case 'pdf':
        return <PdfIcon />;
      case 'doc':
      case 'docx':
      case 'odt':
        return <WordIcon />;
      case 'xlsx':
      case 'xls':
      case 'xlsm':
        return <ExcelIcon />;
      case 'ppt':
      case 'pptx':
      case 'pps':
      case 'ppsx':
        return <PptIcon />;
      case 'png':
      case 'jpg':
      case 'jpeg':
      case 'gif':
      case 'bmp':
        return <></>;
      default:
        return <AttachFileIcon />;
    }
  };

  const buildMessageContent = () => {
    return (
      <>
        {quoteMessageId > 0 && (
          <>
            <QuoteIcon />
            <span className='quote-message'>
              {quoteMessage ?? 'This message has been deleted'}
            </span>
            {quoteMessage && (
              <span className='quote-info'>
                {quoteUsername},{' '}
                {formatTime(quoteCreatedDate, 'minute', 'DD MMM YYYY hh:mm A')}
              </span>
            )}
          </>
        )}

        <span
          className={`message-content ${quoteMessageId ? 'have-quote' : ''}`}>
          {messageContent}
        </span>
      </>
    );
  };

  const getMessageContent = () => {
    return (
      <div>
        {!fileId && (
          <p className='message-div'>
            <Linkify properties={{ target: '_blank' }}>
              {buildMessageContent()}
            </Linkify>
          </p>
        )}
        {fileId && (
          <div className='file-div' onClick={handleOnClick}>
            {getFileIcon()}
            {imageFileName ? (
              <ImageDisplay imageFileName={imageFileName}></ImageDisplay>
            ) : (
              <span className='is-file'>{messageContent}</span>
            )}
          </div>
        )}
        <div className='icons'>
          <EmojiDisplay
            messageReactIcons={messageReactIcons || []}
            ticketId={ticketId}></EmojiDisplay>
        </div>
      </div>
    );
  };

  return { getCreatedTime, getMessageContent };
};

export default MessageContents;
