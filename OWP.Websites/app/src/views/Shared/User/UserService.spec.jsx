import axios from 'axios';
import UserService from './UserService';
import { buildRequestURL } from '../../LiveDetail/buildUrlHelper';
jest.mock('axios');
const responseData = { data: 'ahihi' };
const users = [
  {
    Skype: 'ahihi',
    Extension: '998',
    Phone: '0113',
    UserId: 8,
    UserName: 'sa',
    NickName: 'superman',
    IsSecondAccount: false
  },
  {
    Skype: null,
    Extension: null,
    Phone: null,
    UserId: 33390427,
    UserName: 'SLAEVA',
    NickName: 'SLAEVA(N)',
    IsSecondAccount: false
  },
  {
    Skype: null,
    Extension: null,
    Phone: null,
    UserId: 5555555,
    UserName: 'satang',
    NickName: '',
    IsSecondAccount: false
  }
];

describe('User Service test', () => {
  beforeEach(() => {
    axios.get.mockReset();
  });

  it('return data when getAll is called', async () => {
    axios.get.mockResolvedValue(responseData);
    const data = await UserService.getAll();

    expect(data).toEqual(responseData.data);
    expect(axios.get).toBeCalledTimes(1);
    expect(axios.get).toBeCalledWith(buildRequestURL('user/all'));
  });

  it('return data when getSuggestionUsers is called', async () => {
    axios.get.mockResolvedValue(responseData);
    const model = { id: 22, userName: 'asd' };
    expect.assertions(3);
    const data = await UserService.getSuggestionUsers(model);

    expect(data).toEqual(responseData.data);
    expect(axios.get).toBeCalledTimes(1);
    expect(axios.get).toBeCalledWith(buildRequestURL('user/suggestion'), {
      params: model
    });
  });

  it('getSuggestion() return empty array when keyword is null', () => {
    const actual = UserService.getSuggestion(null);
    expect(actual).toEqual([]);
  });

  it('getSuggestion() return empty array when keyword not exists in localStorage users', () => {
    const mockStorageGetItem = jest
      .spyOn(Storage.prototype, 'getItem')
      .mockImplementation(() => JSON.stringify(users));
    const actual = UserService.getSuggestion('cc');

    expect(actual).toEqual([]);
    expect(mockStorageGetItem).toBeCalledTimes(1);
    expect(mockStorageGetItem).toBeCalledWith(
      'localStorageKey_DataSourceLocalUsers'
    );
    mockStorageGetItem.mockRestore();
  });

  it('getSuggestion() return array users when keyword exists in localStorage users', () => {
    const mockStorageGetItem = jest
      .spyOn(Storage.prototype, 'getItem')
      .mockImplementation(() => JSON.stringify(users));
    const actual = UserService.getSuggestion('sa');

    expect(actual).toEqual([
      {
        Extension: '998',
        NickName: 'superman',
        Phone: '0113',
        Skype: 'ahihi',
        UserId: 8,
        UserName: 'sa',
        IsSecondAccount: false
      },
      {
        Skype: null,
        Extension: null,
        Phone: null,
        UserId: 5555555,
        UserName: 'satang',
        NickName: '',
        IsSecondAccount: false
      }
    ]);
    mockStorageGetItem.mockRestore();
  });

  it('getInfoByUserName() return null when keyword is null', () => {
    const testCases = ['', null, undefined, '        '];
    expect.assertions(4);
    testCases.forEach(name => {
      const actual = UserService.getInfoByUserName(name);
      expect(actual).toEqual(null);
    });
  });

  it('getInfoByUserName() return undefined when keyword not exists in localStorage users', () => {
    const mockStorageGetItem = jest
      .spyOn(Storage.prototype, 'getItem')
      .mockImplementation(() => JSON.stringify(users));
    const actual = UserService.getInfoByUserName('cc');

    expect(actual).toEqual(undefined);
    expect(mockStorageGetItem).toBeCalledTimes(1);
    expect(mockStorageGetItem).toBeCalledWith(
      'localStorageKey_DataSourceLocalUsers'
    );
    mockStorageGetItem.mockRestore();
  });

  it('getInfoByUserName() return object when keyword exists in localStorage users', () => {
    const mockStorageGetItem = jest
      .spyOn(Storage.prototype, 'getItem')
      .mockImplementation(() => JSON.stringify(users));
    const testCases = ['sa', 'SA', 'sA'];
    expect.assertions(3);
    testCases.forEach(name => {
      const actual = UserService.getInfoByUserName(name);
      expect(actual).toEqual({
        Extension: '998',
        NickName: 'superman',
        Phone: '0113',
        Skype: 'ahihi',
        UserId: 8,
        UserName: 'sa',
        IsSecondAccount: false
      });
    });

    mockStorageGetItem.mockRestore();
  });

  it('getInfoByUserId() return null when userId is null', () => {
    const actual = UserService.getInfoByUserId(null);
    expect(actual).toEqual(null);
  });

  it('getInfoByUserId() return null when userId not exists in localStorage users', () => {
    const mockStorageGetItem = jest
      .spyOn(Storage.prototype, 'getItem')
      .mockImplementation(() => JSON.stringify(users));
    const actual = UserService.getInfoByUserId(999);

    expect(actual).toEqual(undefined);
    expect(mockStorageGetItem).toBeCalledTimes(1);
    expect(mockStorageGetItem).toBeCalledWith(
      'localStorageKey_DataSourceLocalUsers'
    );
    mockStorageGetItem.mockRestore();
  });

  it('getInfoByUserId() return object when userId exists in localStorage users', () => {
    const mockStorageGetItem = jest
      .spyOn(Storage.prototype, 'getItem')
      .mockImplementation(() => JSON.stringify(users));
    const actual = UserService.getInfoByUserId(8);

    expect(actual).toEqual({
      Extension: '998',
      NickName: 'superman',
      Phone: '0113',
      Skype: 'ahihi',
      UserId: 8,
      UserName: 'sa',
      IsSecondAccount: false
    });
    mockStorageGetItem.mockRestore();
  });

  it('getMentionsUsers() return empty array when keyword is null', () => {
    const actual = UserService.getMentionsUsers(null);
    expect(actual).toEqual([]);
  });

  it('getMentionsUsers() return empty array when keyword not exists in localStorage users', () => {
    const mockStorageGetItem = jest
      .spyOn(Storage.prototype, 'getItem')
      .mockImplementation(() => JSON.stringify(users));
    const actual = UserService.getMentionsUsers('cc');

    expect(actual).toEqual([]);
    expect(mockStorageGetItem).toBeCalledTimes(1);
    expect(mockStorageGetItem).toBeCalledWith(
      'localStorageKey_DataSourceLocalUsers'
    );
    mockStorageGetItem.mockRestore();
  });

  it('getMentionsUsers() return array when keyword exists in localStorage users', () => {
    const mockStorageGetItem = jest
      .spyOn(Storage.prototype, 'getItem')
      .mockImplementation(() => JSON.stringify(users));
    const actual = UserService.getMentionsUsers('sa');

    expect(actual).toEqual([
      {
        UserId: 8,
        UserName: 'sa',
        display: 'sa',
        id: 'sa',
        NickName: 'superman'
      },
      {
        UserId: 5555555,
        UserName: 'satang',
        display: 'satang',
        id: 'satang',
        NickName: ''
      }
    ]);
    mockStorageGetItem.mockRestore();
  });

  it('dataSourceLocalUsers() return data from service getAll', () => {
    axios.get.mockResolvedValue(responseData);
    const mockStorageSetItem = jest
      .spyOn(Storage.prototype, 'getItem')
      .mockImplementation(() => {});
    const actual = UserService.dataSourceLocalUsers();

    expect(mockStorageSetItem).toBeCalledTimes(1);

    expect(axios.get).toBeCalledTimes(1);
    expect(actual).toEqual(undefined);
    mockStorageSetItem.mockRestore();
  });
});
