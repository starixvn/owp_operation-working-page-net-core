import { useState, useEffect } from 'react';
import AuthorizeService from '../../Authorize/Service';
import { FunctionKeys } from '../Constants';
import CryptoJS from 'crypto-js';
import { getServerUrl } from '../RcChat/Service';

const STORAGE_KEYS = {
  TOKEN: 'rc_token',
  USERNAME: 'rc_username',
  USER_ID: 'rc_userid',
  EXPIRY: 'rc_expiry'
};

const getEncryptionKey = async () => {
  const domain = window.location.hostname;
  const chatUrl = await getServerUrl();
  return CryptoJS.SHA256(`${domain}_${chatUrl}`).toString();
};

const encryptData = async data => {
  const key = await getEncryptionKey();
  return CryptoJS.AES.encrypt(JSON.stringify(data), key).toString();
};

const decryptData = async encryptedData => {
  try {
    const key = await getEncryptionKey();
    const bytes = CryptoJS.AES.decrypt(encryptedData, key);
    return JSON.parse(bytes.toString(CryptoJS.enc.Utf8));
  } catch (error) {
    console.info('Decryption failed:', error);
    return null;
  }
};

const getStoredAuth = async () => {
  const encryptedExpiry = localStorage.getItem(STORAGE_KEYS.EXPIRY);
  if (!encryptedExpiry) {
    return null;
  }

  const expiry = await decryptData(encryptedExpiry);
  if (!expiry || new Date().getTime() > parseInt(expiry, 10)) {
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
    token: await decryptData(encryptedToken),
    username: await decryptData(encryptedUsername),
    userId: await decryptData(encryptedUserId)
  };
};

const clearStorage = () => {
  Object.values(STORAGE_KEYS).forEach(key => localStorage.removeItem(key));
};

const storeAuth = async ({ token, username, userId }) => {
  const expiry = new Date().getTime() + 24 * 60 * 60 * 1000;
  localStorage.setItem(STORAGE_KEYS.TOKEN, await encryptData(token));
  localStorage.setItem(STORAGE_KEYS.USERNAME, await encryptData(username));
  localStorage.setItem(STORAGE_KEYS.USER_ID, await encryptData(userId));
  localStorage.setItem(
    STORAGE_KEYS.EXPIRY,
    await encryptData(expiry.toString())
  );
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
    console.info('Token verification failed:', err);
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
    return {
      token: null,
      userId: null,
      hasError: true,
      statusCode: response.status
    };
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
    await storeAuth({
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
    statusCode,
    errorMessage,
    token: null
  };
};

const loginWithTokenFromStore = async (policy, chatUrl) => {
  const storedAuth = await getStoredAuth();

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
  const [chatUrl, setChatUrl] = useState(null);
  useEffect(() => {
    const authenticate = async () => {
      setLoading(true);
      try {
        const chatUrl = await getServerUrl();
        const policy = await AuthorizeService.authorize(
          FunctionKeys.RocketChat
        );

        let loginResult = await loginWithTokenFromStore(policy, chatUrl);
        if (!loginResult.hasError) {
          setAuthToken(loginResult.token);
          setError(null);
          setChatUrl(chatUrl);
          return;
        }

        loginResult = await handleReLogin(policy, chatUrl);
        if (!loginResult.hasError) {
          setAuthToken(loginResult.token);
          setError(null);
          setChatUrl(chatUrl);
          return;
        }
        setError(loginResult);
        setChatUrl(chatUrl);
      } finally {
        setLoading(false);
      }
    };

    authenticate();
  }, [setAuthToken, setError]);

  return { authToken, error, loading, chatUrl };
};

export default useRocketChat;
