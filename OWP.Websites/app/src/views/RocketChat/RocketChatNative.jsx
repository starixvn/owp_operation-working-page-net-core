import React, { useState, useEffect, useRef } from 'react';
import AuthorizeService from '../Authorize/Service';
import { FunctionKeys } from '../Shared/Constants';
import { PROD_URL, LOCAL_URL } from './Const.json';

const RocketChatNative = () => {
  const [authToken, setAuthToken] = useState(null);
  const [error, setError] = useState(null);
  const iframeRef = useRef(null);

  useEffect(() => {
    const fetchPolicyAndLoginToRC = async () => {
      const policy = await AuthorizeService.authorize(FunctionKeys.RocketChat);
      const chatUrl =
        process.env.NODE_ENV === 'production' ? PROD_URL : LOCAL_URL;
      if (policy?.Username) {
        const response = await fetch(`${chatUrl}/api/v1/login`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            username: policy.Username,
            password: 'password'
          })
        });

        if (!response.ok) {
          setError('Failed to log in to Rocket.Chat');
        }

        const { data } = await response.json();
        setAuthToken(data.authToken);
      } else {
        setError('Policy not found');
      }
    };

    fetchPolicyAndLoginToRC();
  }, []);

  useEffect(() => {
    if (authToken && iframeRef.current) {
      const chatUrl =
        process.env.NODE_ENV === 'production' ? PROD_URL : LOCAL_URL;
      iframeRef.current.src = `${chatUrl}/home?resumeToken=${authToken}`;
    }
  }, [authToken]);

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
    />
  );
};

export default RocketChatNative;
