import React, { useRef } from 'react';
import useRocketChat from '../Shared/customHook/useRocketChat';
import LoginRocketChatFailIcon from '../../assets/svg-element/LoginRocketChatFailIcon';

const RocketChatNative = () => {
  const iframeRef = useRef(null);
  const { authToken, error, chatUrl } = useRocketChat();

  if (error?.hasError && error?.statusCode === 401) {
    return (
      <div className='rocket-chat-native-container' style={{}}>
        <div className='body' style={{}}>
          <div className='body-content' style={{}}>
            <LoginRocketChatFailIcon />
            <div className='title' style={{}}>
              Oooops!!
            </div>
            <div className='description' style={{}}>
              Failed to log in to OWP Chat.
            </div>
            <div className='description' style={{}}>
              Please reset your password and log in to OWP again.
            </div>
            <div className='command' style={{}}>
              <a className='button' style={{}} href='/Auth/ChangePassword'>
                Change password
              </a>
            </div>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div style={{ height: 'calc(100vh - 55px)', width: '100%' }}>
      <iframe
        ref={iframeRef}
        width='100%'
        height='100%'
        title='Rocket.Chat'
        allow='microphone; clipboard-write; clipboard-read'
        style={{ border: 'none' }}
        src={authToken ? `${chatUrl}/home?resumeToken=${authToken}` : ''}
      />
    </div>
  );
};

export default RocketChatNative;
