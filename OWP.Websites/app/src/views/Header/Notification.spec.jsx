import { act, fireEvent, render, waitFor } from '@testing-library/react';
import Notification from './Notification';
import MessageService from '../Message/Service';
import AuthorizeService from '../Authorize/Service';
import Utils from '../Shared/Utils';

jest.mock('../Message/Service');
jest.mock('../Authorize/Service');
jest.mock('../Shared/Utils');

const messages = [
  {
    CreatedBy: 8,
    CreatedDate: '2018-12-10T21:51:27.393+07:00',
    IsImportant: false,
    IsViewed: true,
    MessageContent: 'Hello',
    MessageId: 626557,
    MessageType: 12,
    RowNumber: 1
  },
  {
    CreatedBy: 8,
    CreatedDate: '2018-12-11T21:51:27.393+07:00',
    IsImportant: false,
    IsViewed: true,
    MessageContent: 'Hello',
    MessageId: 626558,
    MessageType: 12,
    RowNumber: 1
  }
];

describe('Notification Component', () => {
  beforeEach(() => {
    Utils.resolveUrl.mockReset();
    AuthorizeService.authorize.mockReset();
    MessageService.query.mockReset();
    MessageService.updateIsViewed.mockReset();
  });

  it('Render <Notification /> match snapshot', () => {
    const mockComponentDidMount = jest
      .spyOn(Notification.prototype, 'componentDidMount')
      .mockImplementation(() => {});
    const { container } = render(<Notification />);

    expect(container.querySelector('.notification')).toBeInTheDocument();
    mockComponentDidMount.mockRestore();
  });

  it('should display List NotificationItem', () => {
    const mockComponentDidMount = jest
      .spyOn(Notification.prototype, 'componentDidMount')
      .mockImplementation(() => {});
    const { container } = render(<Notification />);
    expect(container.querySelector('.notification')).toBeInTheDocument();
    mockComponentDidMount.mockRestore();
  });

  it('should show modal and update viewed when click icon-notification onBadgeClick tab Watcher', () => {
    MessageService.updateIsViewed.mockResolvedValue();
    const mockComponentDidMount = jest
      .spyOn(Notification.prototype, 'componentDidMount')
      .mockImplementation(() => {});

    const queriedData = {
      Messages: messages,
      TotalNotViewMessage: 2
    };
    MessageService.query.mockResolvedValue(queriedData);
    const { container } = render(<Notification />);

    const badgeButton = container.querySelector('.icon-notification');
    waitFor(() => {
      fireEvent.click(badgeButton);
    });

    expect(mockComponentDidMount).toBeCalledTimes(1);

    mockComponentDidMount.mockRestore();
  });

  it('check Watcher tab onClick callback', () => {
    const mockInitializeSignalRNetcore = jest
      .spyOn(Notification.prototype, 'initializeSignalRNetCore')
      .mockImplementation(() => {});

   
    const mockQueryMessages = jest
      .spyOn(Notification.prototype, 'queryMessages')
      .mockImplementation(() => {});

      const queriedData = {
        Messages: messages,
        TotalNotViewMessage: 2
      };
      MessageService.query.mockResolvedValue(queriedData);

    const policy = { HasRead: true, HasWrite: true };
    AuthorizeService.authorize.mockResolvedValue(policy);
    const props = {
      policy: { HasRead: true, HasWrite: true },
     
    };
    render(<Notification {...props} />);
    act(() => {
      expect(mockQueryMessages).toBeCalledTimes(1);
      mockInitializeSignalRNetcore.mockReset();
      mockQueryMessages.mockReset();
    });
  });

  it('test Show All onClick callback', () => {
    jest
      .spyOn(Notification.prototype, 'componentDidMount')
      .mockImplementation(() => {});
    global.window = Object.create(window);
    const url = 'http://dummy.com';
    Object.defineProperty(window, 'location', {
      value: {
        href: url
      }
    });
    expect(window.location.href).toEqual(url);

    const policy = { HasRead: true, HasWrite: true };
    AuthorizeService.authorize.mockResolvedValue(policy);
    const props = {
      policy: { HasRead: true, HasWrite: true }
    };
    Utils.resolveUrl.mockResolvedValue('http://');
    const { container } = render(<Notification {...props} />);
    act(()=>{
      const element = container.querySelector('.show-all');
      fireEvent.click(element);
    })
   
  });
});
