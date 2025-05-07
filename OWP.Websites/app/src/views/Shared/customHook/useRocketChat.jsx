import { useState, useEffect } from 'react';
import AuthorizeService from '../../Authorize/Service';
import { FunctionKeys } from '../Constants';
import CryptoJS from 'crypto-js';

const STORAGE_KEYS = {
  TOKEN: 'rc_token',
  USERNAME: 'rc_username',
  USER_ID: 'rc_userid',
  EXPIRY: 'rc_expiry'
};

const getEncryptionKey = () => {
  const domain = window.location.hostname;
  return CryptoJS.SHA256(
    `${domain}_${import.meta.env.VITE_CHAT_SEVER_URL}`
  ).toString();
};

const encryptData = data => {
  const key = getEncryptionKey();
  return CryptoJS.AES.encrypt(JSON.stringify(data), key).toString();
};

const decryptData = encryptedData => {
  try {
    const key = getEncryptionKey();
    const bytes = CryptoJS.AES.decrypt(encryptedData, key);
    return JSON.parse(bytes.toString(CryptoJS.enc.Utf8));
  } catch (error) {
    console.error('Decryption failed:', error);
    return null;
  }
};

const getStoredAuth = () => {
  const encryptedExpiry = localStorage.getItem(STORAGE_KEYS.EXPIRY);
  if (!encryptedExpiry) {
    return null;
  }

  const expiry = decryptData(encryptedExpiry);
  if (!expiry || new Date().getTime() > parseInt(expiry)) {
    clearStorage();
    return null;
  }

  const encryptedToken = localStorage.getItem(STORAGE_KEYS.TOKEN);
  const encryptedUsername = localStorage.getItem(STORAGE_KEYS.USERNAME);
  const encryptedUserId = localStorage.getItem(STORAGE_KEYS.USER_ID);

  if (!encryptedToken || !encryptedUsername || !encryptedUserId) {
    clearStorage();
    return null;
  }

  return {
    token: decryptData(encryptedToken),
    username: decryptData(encryptedUsername),
    userId: decryptData(encryptedUserId)
  };
};

const clearStorage = () => {
  Object.values(STORAGE_KEYS).forEach(key => localStorage.removeItem(key));
};

const storeAuth = ({ token, username, userId }) => {
  const expiry = new Date().getTime() + 24 * 60 * 60 * 1000;
  localStorage.setItem(STORAGE_KEYS.TOKEN, encryptData(token));
  localStorage.setItem(STORAGE_KEYS.USERNAME, encryptData(username));
  localStorage.setItem(STORAGE_KEYS.USER_ID, encryptData(userId));
  localStorage.setItem(STORAGE_KEYS.EXPIRY, encryptData(expiry.toString()));
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

const loginToRocketChat = async (username, password, chatUrl) => {
  const response = await fetch(`${chatUrl}/api/v1/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      username,
      password
    })
  });

  if (!response.ok) {
    throw new Error('Failed to log in to Rocket.Chat');
  }

  const { data } = await response.json();
  return { token: data.authToken, userId: data.userId };
};

const handleReLogin = async (policy, chatUrl) => {
  const { token: newToken, userId: newUserId } = await loginToRocketChat(
    policy.Username,
    policy.RawPassword,
    chatUrl
  );
  storeAuth({
    token: newToken,
    username: policy.Username,
    userId: newUserId
  });
  return newToken;
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
        debugger;
        if (!policy?.Username) {
          throw new Error('Rocket.Chat policy not found');
        }

        const storedAuth = getStoredAuth();
        if (!storedAuth) {
          setError('Phiên làm việc đã hết hạn. Đang đăng nhập lại...');
          const newToken = await handleReLogin(policy, chatUrl);
          setAuthToken(newToken);
          setError(null);
          return;
        }

        if (
          storedAuth?.token &&
          storedAuth?.userId &&
          storedAuth?.username === policy.Username
        ) {
          const isValid = await verifyToken(
            storedAuth.token,
            storedAuth.userId,
            chatUrl
          );
          if (isValid) {
            setAuthToken(storedAuth.token);
            return;
          }
          setError('Token không hợp lệ. Đang đăng nhập lại...');
          clearStorage();
        }

        const newToken = await handleReLogin(policy, chatUrl);
        setAuthToken(newToken);
        setError(null);
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
