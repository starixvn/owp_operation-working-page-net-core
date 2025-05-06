import React, { useRef } from 'react';
import useRocketChat from '../Shared/customHook/useRocketChat';

const RocketChatNative = () => {
  const iframeRef = useRef(null);
  const { authToken, error } = useRocketChat();

  if (error) {
    return <div>{error}</div>;
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
        src={
          authToken
            ? `${
                import.meta.env.VITE_CHAT_SEVER_URL
              }/home?resumeToken=${authToken}`
            : ''
        }
      />
    </div>
  );
};

export default RocketChatNative;
