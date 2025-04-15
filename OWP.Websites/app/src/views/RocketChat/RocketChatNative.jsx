import React, { useState, useEffect, useRef } from 'react';
import AuthorizeService from '../Authorize/Service';
import Loading from '../components/Loading';
import { FunctionKeys } from '../Shared/Constants';

export default function RocketChatNative() {
  const [authToken, setAuthToken] = useState(null);
  const [error, setError] = useState(null);
  const iframeRef = useRef(null);

  useEffect(() => {
    const fetchPolicyAndLoginToRC = async () => {
      const policy = await AuthorizeService.authorize(FunctionKeys.RocketChat);
      if (policy?.Username) {
        const response = await fetch('https://chat.nexdev.net/api/v1/login', {
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
      iframeRef.current.src = `https://chat.nexdev.net/home?resumeToken=${authToken}`;
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
}
