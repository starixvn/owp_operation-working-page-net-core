import axios from 'axios';
import HeaderService from './Service';
import Utils from '../Shared/Utils';

jest.mock('axios');
jest.mock('../Shared/Utils');
Utils.resolveUrl.mockImplementation(url => 'rootURL_' + url);

const response = {
  data: {
    Username: 'sa',
    LogoutPath: 'logout_link',
    NotificationSetingPath: 'notification_link',
    Menus: [
      {
        Id: 571,
        Name: 'General',
        Path: '',
        Children: [
          {
            Id: 572,
            Name: 'Home',
            Path: '/Home',
            Children: null
          },
          {
            Id: 581,
            Name: 'User Management',
            Path: '/EmployeeManagement/UserManagement',
            Children: null
          },
          {
            Id: 619,
            Name: 'Working Shift Management',
            Path: '/EmployeeManagement/WorkingShiftManagement',
            Children: null
          }
        ]
      },
      {
        Id: 576,
        Name: 'Event Management',
        Path: '',
        Children: [
          {
            Id: 586,
            Name: 'Max Bet Management',
            Path: '/EventManagement/MaxBetManagement',
            Children: null
          },
          {
            Id: 589,
            Name: 'Odds Compiler',
            Path: '/EventManagement/OddsCompiler',
            Children: null
          }
        ]
      }
    ]
  }
};

describe('Header service', () => {
  beforeEach(() => {
    axios.get.mockReset();
  });

  it('query return expected data when response.data.Menus has Children', async () => {
    axios.get.mockResolvedValue(response);

    expect.assertions(4);
    const actual = await HeaderService.query('abc');

    expect(actual.username).toEqual(response.data.Username);
    expect(actual.logoutLink).toEqual('rootURL_' + response.data.LogoutPath);
    expect(actual.notificationSetingLink).toEqual(
      'rootURL_' + response.data.NotificationSetingPath
    );
    expect(actual.navigation).toMatchSnapshot();
  });

  it('query return expected data when response.data.Menus empty', async () => {
    response.data.Menus = [
      {
        Id: 571,
        Name: 'General',
        Path: '',
        Children: null
      }
    ];
    axios.get.mockResolvedValue(response);

    expect.assertions(4);
    const actual = await HeaderService.query();

    expect(actual.username).toEqual(response.data.Username);
    expect(actual.logoutLink).toEqual('rootURL_' + response.data.LogoutPath);
    expect(actual.notificationSetingLink).toEqual(
      'rootURL_' + response.data.NotificationSetingPath
    );
    expect(actual.navigation).toMatchSnapshot();
  });
});
