import axios from 'axios';
import { find, map, filter } from 'lodash';
import { buildRequestURL } from '../../LiveDetail/buildUrlHelper';

const getAll = async () => {
  const response = await axios.get(buildRequestURL('user/all'));
  return response.data;
};

const dataSourceUsersFromServer = () => {
  (async () => {
    const data = await getAll();
    const userMainAccounts = data.filter(x => !x.IsSecondAccount);
    const localStorageKey = 'localStorageKey_DataSourceLocalUsers';
    localStorage.setItem(localStorageKey, JSON.stringify(userMainAccounts));
    return userMainAccounts;
  })();
};

const dataSourceLocalUsersWithCache = () => {
  const localStorageKey = 'localStorageKey_DataSourceLocalUsers';
  const localDataSource = localStorage.getItem(localStorageKey);
  if (localDataSource) {
    return JSON.parse(localDataSource);
  }

  return dataSourceUsersFromServer();
};

const dataSourceLocalUsers = () => dataSourceLocalUsersWithCache();

const getSuggestionUsers = async model => {
  const response = await axios.get(buildRequestURL('user/suggestion'), {
    params: model
  });

  return response.data;
};

const getSuggestion = keyword => {
  if (keyword) {
    const lowerCaseKeyword = keyword.toLowerCase().trim();
    const users = dataSourceLocalUsers();
    return filter(users, user => {
      return (
        (user.IsSecondAccount === false &&
          user.UserName?.toLowerCase()?.includes(lowerCaseKeyword)) ||
        user.NickName?.toLowerCase()?.includes(lowerCaseKeyword)
      );
    });
  }

  return [];
};

const getInfoByUserName = name => {
  const nameTransform = name ? name.toLowerCase().trim() : name;
  if (nameTransform) {
    const users = getSuggestion(nameTransform);
    return find(users, user => {
      return (
        user.IsSecondAccount === false &&
        (user.UserName.toLowerCase() === nameTransform ||
          (user.NickName && user.NickName.toLowerCase() === nameTransform))
      );
    });
  }
  return null;
};

const getInfoByUserId = userId => {
  if (userId) {
    const users = dataSourceLocalUsers();
    return find(users, { UserId: userId });
  }

  return null;
};

const getMentionsUsers = keyword => {
  const queryUsers = getSuggestion(keyword);

  return map(queryUsers, user => {
    return {
      UserId: user.UserId,
      UserName: user.UserName,
      NickName: user.NickName,
      id: user.UserName,
      display: user.UserName
    };
  });
};

const getUseSuggesionFromCache = () => true;

const getUserFromCached = async () => {
  const localStorageKey = 'localStorageKey_DataSourceLocalUsers';
  const localDataSource = localStorage.getItem(localStorageKey);
  if (localDataSource) {
    return JSON.parse(localDataSource);
  }
  const data = await getAll();
  return data.filter(x => !x.IsSecondAccount);
};

const UserService = {
  getAll,
  getSuggestionUsers,
  getSuggestion,
  dataSourceLocalUsers,
  dataSourceUsersFromServer,
  getMentionsUsers,
  getInfoByUserId,
  getInfoByUserName,
  getUseSuggesionFromCache,
  getUserFromCached
};

export default UserService;
