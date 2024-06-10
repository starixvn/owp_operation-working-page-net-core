import { createRef, useState } from 'react';
import { useSelector } from 'react-redux';
import { validExtension } from '../const/index.json';
import { setModelQuoteMessage } from '../chatBoardSlice';
import useStableDispatch from '../../customHook/useStableDispatch';
import PropTypes from 'prop-types';
import SendIcon from '@mui/icons-material/Send';
import HighlightOffIcon from '@mui/icons-material/HighlightOff';
import AttachFileIcon from '../../../../assets/svg-element/AttachFileIcon';
import QuoteIcon from '../../../../assets/svg-element/QuoteIcon';
import { formatTime } from '../../DateTime/dateTimeHelper';
import Comment from './Comment';

const InputMessage = ({
  handleSendFile,
  handleSendMessage
}) => {

  const dispatch = useStableDispatch();
  const inputFileRef = createRef();
  const [textFieldValue, setTextFieldValue] = useState('');
  const [selectedImage, setSelectedImage] = useState(null);
  const { inputMessage, modelQuoteMessage, isBlinkTitle } = useSelector(
    state => state.chatBoard
  );
  const { message, messageId, username, createdDate } = modelQuoteMessage;
  
  const handleFileInputChange = async event => {
    const file = event.target.files[0];
    await handleSendFile(event, file);
  };

  const sendMessage = async event => {
    if (textFieldValue && selectedImage) {
      setTextFieldValue('');
      await handleSendFile(event, selectedImage);
      setSelectedImage(null);
    }
    await handleSendMessage(event);
  };

  return (
    <div className='chat-box'>
        {textFieldValue && (
          <div className='input-image'>
            <img src={textFieldValue} alt='Pasted' />
            <div className='icon-remove-div'>
              <HighlightOffIcon fontSize='large' onClick={()=>setTextFieldValue('')} />
            </div>
          </div>
        )}
        {messageId && (
          <div className='quote-text-box'>
              <div className='icon-wrapper'>
                <div className='icon-quote'>
                  <QuoteIcon />
                </div>
                <div className='icon-remove'>
                  <HighlightOffIcon
                    fontSize='large'
                    onClick={()=>dispatch(setModelQuoteMessage({}))}
                  />
                </div>
              </div>
              <span className='quote-message-text-box'>{message}</span>
              <span className='quote-info-text-box'>
                {username},{' '}
                {formatTime(createdDate, 'minute', 'DD MMM YYYY hh:mm A')}
              </span>
          </div>
        )}
        <Comment
          inputMessage={inputMessage}
          isBlinkTitle={isBlinkTitle}
          onSetTextFieldValue={setTextFieldValue}
          onSetSelectedImage={setSelectedImage}
          onSendMessage={sendMessage}
        />

      <AttachFileIcon handleOnClick={() => inputFileRef.current.click()} />
      <SendIcon className='icon-send' size='24' onClick={sendMessage} />
      <input
        accept={validExtension}
        ref={inputFileRef}
        hidden
        type='file'
        onChange={handleFileInputChange}
      />
    </div>
  );
};
InputMessage.propTypes = {
  handleSendFile: PropTypes.func,
  handleSendMessage: PropTypes.func
};

export default InputMessage;
