import { useState, useEffect } from 'react';
import AuthorizeService from '../../Authorize/Service';
import { FunctionKeys } from '../Constants';

const STORAGE_KEYS = {
  TOKEN: 'rc_token',
  USERNAME: 'rc_username',
  USER_ID: 'rc_userid'
};

const getStoredAuth = () => ({
  token: localStorage.getItem(STORAGE_KEYS.TOKEN),
  username: localStorage.getItem(STORAGE_KEYS.USERNAME),
  userId: localStorage.getItem(STORAGE_KEYS.USER_ID)
});

const clearStorage = () => {
  Object.values(STORAGE_KEYS).forEach(key => localStorage.removeItem(key));
};

const storeAuth = ({ token, username, userId }) => {
  localStorage.setItem(STORAGE_KEYS.TOKEN, token);
  localStorage.setItem(STORAGE_KEYS.USERNAME, username);
  localStorage.setItem(STORAGE_KEYS.USER_ID, userId);
};

const verifyToken = async (token, userId, chatUrl) => {
  try {
    const response = await fetch(`${chatUrl}/api/v1/me`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'X-Auth-Token': token,
        'X-User-Id': userId
      }
    });
    return response.ok;
  } catch (err) {
    console.error('Token verification failed:', err);
    return false;
  }
};

const loginToRocketChat = async (username, chatUrl) => {
  const response = await fetch(`${chatUrl}/api/v1/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      username,
      password: 'password'
    })
  });

  if (!response.ok) {
    throw new Error('Failed to log in to Rocket.Chat');
  }

  const { data } = await response.json();
  return { token: data.authToken, userId: data.userId };
};

const useRocketChat = () => {
  const [authToken, setAuthToken] = useState(null);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const authenticate = async () => {
      setLoading(true);
      try {
        const chatUrl = import.meta.env.VITE_CHAT_SEVER_URL;
        const policy = await AuthorizeService.authorize(
          FunctionKeys.RocketChat
        );

        if (!policy?.Username) {
          throw new Error('Rocket.Chat policy not found');
        }

        const { token, username, userId } = getStoredAuth();

        if (token && userId && username === policy.Username) {
          const isValid = await verifyToken(token, userId, chatUrl);
          if (isValid) {
            setAuthToken(token);
            return;
          }
          clearStorage();
        }

        const { token: newToken, userId: newUserId } = await loginToRocketChat(
          policy.Username,
          chatUrl
        );
        storeAuth({
          token: newToken,
          username: policy.Username,
          userId: newUserId
        });
        setAuthToken(newToken);
      } catch (err) {
        console.error(err);
        setError(err.message);
        clearStorage();
      } finally {
        setLoading(false);
      }
    };

    authenticate();
  }, []);

  return { authToken, error, loading };
};

export default useRocketChat;
