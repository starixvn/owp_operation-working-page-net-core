import Layout from './Layout';
import HeaderService from './Header/Service';
import AuthorizeService from './Authorize/Service';
import { setAxiosInterceptors } from './Shared/Service/ServiceCommon';
import { render, screen, act } from '@testing-library/react';
import { BrowserRouter as Router } from 'react-router-dom';
import Notification from './Header/Notification';


jest.mock('./Shared/Service/ServiceCommon');
jest.mock('./Header/Service');
jest.mock('./Authorize/Service');

const props = {
  title: 'layout ne',
  view: <h3>View</h3>
};
const header = {
  title: 'Home page',
  username: 'superman',
  logoutLink: 'logout_link',
  notificationSetingLink: 'notification_link',
  navigation: {
    items: [{ name: 'My Ticket' }, { name: 'OS live game assignment' }]
  }
};

describe('Test <Layout/>', () => {
  afterEach(() => {
    HeaderService.query.mockReset();
  });

  beforeEach(() => {
    HeaderService.query.mockResolvedValue(header);

    AuthorizeService.authorize.mockReturnValue({
      HasRead: true
    });

    setAxiosInterceptors.mockReturnValue({});
    jest
      .spyOn(Notification.prototype, 'initializeSignalRNetCore')
      .mockImplementation(() => { });
    jest
      .spyOn(Notification.prototype, 'initializeSignalR')
      .mockImplementation(() => { });

    jest
      .spyOn(Notification.prototype, 'queryMessages')
      .mockImplementation(() => { });
  });

  it('should render Loading when state.isLoading is true', () => {
    const mockComponentDidMount = jest
      .spyOn(Layout.prototype, 'componentDidMount')
      .mockImplementation(() => { });
    const { container } = render(<Layout title='' view={{ hihi: '' }} />);

    expect(container.getElementsByClassName('loading-process')).toHaveLength(1);

    mockComponentDidMount.mockRestore();
  });

  it('componentDidMount should setState header from HeaderService', async () => {
    await act(() => render(<Router>
      <Layout {...props} />
    </Router>));

    expect(screen.getByText(/My Ticket/i)).toBeInTheDocument();
    expect(screen.getByText(/OS live game assignment/i)).toBeInTheDocument();
    expect(HeaderService.query).toBeCalledTimes(1);
    expect(AuthorizeService.authorize).toBeCalledTimes(3);
  });
});
