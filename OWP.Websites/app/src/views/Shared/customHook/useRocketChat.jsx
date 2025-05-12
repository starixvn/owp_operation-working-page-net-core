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

  if (response.status === 401) {
    return {
      token: null,
      userId: null,
      hasError: true,
      errorMessage: 'Unauthorized',
      statusCode: response.status
    };
  }
  if (!response.ok) {
    return { token: null, userId: null, hasError: true };
  }

  const { data } = await response.json();
  return {
    token: data.authToken,
    userId: data.userId,
    hasError: false,
    statusCode: response.status
  };
};

const handleReLogin = async (policy, chatUrl) => {
  const { token, userId, statusCode, hasError, errorMessage } =
    await loginToRocketChat(policy.Username, policy.RawPassword, chatUrl);
  if (!hasError) {
    storeAuth({
      token,
      username: policy.Username,
      userId
    });

    return {
      token,
      statusCode,
      hasError,
      errorMessage
    };
  }
  return {
    hasError: true,
    statusCode: 401,
    errorMessage: 'Session is invalid. The login is being reprocessed...'
  };
};

const loginWithTokenFromStore = async (policy, chatUrl) => {
  const storedAuth = getStoredAuth();

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
      return {
        hasError: false,
        statusCode: 200,
        errorMessage: null,
        token: storedAuth.token
      };
    }
  }

  clearStorage();
  return {
    hasError: true,
    statusCode: 401,
    errorMessage: 'Session is invalid. The login is being reprocessed...'
  };
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

        let loginResult = await loginWithTokenFromStore(policy, chatUrl);
        if (!loginResult.hasError) {
          setAuthToken(loginResult.token);
          setError(null);
          return;
        }

        loginResult = await handleReLogin(policy, chatUrl);
        if (!loginResult.hasError) {
          setAuthToken(loginResult.token);
          setError(null);
          return;
        }
        setError(loginResult);
      } finally {
        setLoading(false);
      }
    };

    authenticate();
  }, [setAuthToken, setError]);

  return { authToken, error, loading };
};

export default useRocketChat;
