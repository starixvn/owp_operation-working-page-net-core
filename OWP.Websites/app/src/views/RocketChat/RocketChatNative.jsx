import React, { useState, useEffect, useRef } from 'react';
import AuthorizeService from '../Authorize/Service';
import Loading from '../components/Loading';
import { FunctionKeys } from '../Shared/Constants';

export default function RocketChatNative() {
  const [authToken, setAuthToken] = useState(null);
  const [policy, setPolicy] = useState(null);
  const iframeRef = useRef(null);

  useEffect(() => {
    const fetchPolicy = async () => {
      try {
        const policy = await AuthorizeService.authorize(
          FunctionKeys.RocketChat
        );
        setPolicy(policy);
      } catch (error) {
        console.error('Error fetching authorization policy:', error);
      }
    };

    fetchPolicy();
  }, []);

  useEffect(() => {
    const loginRocketChat = async () => {
      try {
        const response = await fetch('https://chat.nexdev.net/api/v1/login', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ username: 'CCSABBY', password: 'password' })
        });

        if (!response.ok) {
          throw new Error('Failed to log in to Rocket.Chat');
        }

        const { data } = await response.json();
        setAuthToken(data.authToken);
      } catch (error) {
        console.error('Error logging in to Rocket.Chat:', error);
      }
    };

    loginRocketChat();
  }, []);

  useEffect(() => {
    if (authToken && iframeRef.current) {
      iframeRef.current.src = `https://chat.nexdev.net/home?resumeToken=${authToken}`;
    }
  }, [authToken]);

  if (!policy?.HasRead) {
    return <Loading />;
  }

  return (
    <iframe
      ref={iframeRef}
      width='100%'
      height='890'
      title='Rocket.Chat'
      style={{ border: 'none' }}
    />
  );
}
