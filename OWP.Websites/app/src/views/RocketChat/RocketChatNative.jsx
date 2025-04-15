import React, { useRef } from 'react';
import useRocketChat from '../Shared/customHook/useRocketChat';

const RocketChatNative = () => {
  const iframeRef = useRef(null);
  const { authToken, error } = useRocketChat();

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <iframe
      ref={iframeRef}
      width='100%'
      height='890'
      title='Rocket.Chat'
      allow='microphone'
      style={{ border: 'none' }}
      src={
        authToken
          ? `${
              import.meta.env.VITE_CHAT_SEVER_URL
            }/home?resumeToken=${authToken}`
          : ''
      }
    />
  );
};

export default RocketChatNative;
