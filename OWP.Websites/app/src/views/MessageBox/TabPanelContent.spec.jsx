import {
  fireEvent,
  render,
  screen,
  waitFor,
  waitForElementToBeRemoved
} from '@testing-library/react';
import TabPanelContent from './TabPanelContent';

import Service from './Service';

jest.mock('./Service');

const responseData = {
  Messages: [
    {
      MessageId: 640729,
      IsViewed: false,
      MessageContent: 'superman edited',
      CreatedBy: 8,
      CreatedDate: '2019-01-14T05:42:40.59+07:00',
      ProfilePic: null,
      IsImportant: false,
      MessageType: 12,
      SystemMessageType: 1,
      TimeSpan: '01/14/2019 05:42 AM'
    },
    {
      MessageId: 640728,
      IsViewed: false,
      MessageContent:
        'superman edited an assignment of MT0A vs MT0B - bettype: No.of Corners (LA)\r\n- Old: #superman\r\n- New: ',
      CreatedBy: 8,
      CreatedDate: '2019-01-14T05:42:40.59+07:00',
      ProfilePic: null,
      IsImportant: false,
      MessageType: 12,
      SystemMessageType: 1,
      TimeSpan: '01/14/2019 05:42 AM'
    },
    {
      MessageId: 640727,
      IsViewed: false,
      MessageContent:
        'superman edited an assignment of MT0A vs MT0B - bettype: Last Booking (LA)\r\n- Old: #superman\r\n- New: ',
      CreatedBy: 8,
      CreatedDate: '2019-01-14T05:42:40.59+07:00',
      ProfilePic: null,
      IsImportant: false,
      MessageType: 12,
      SystemMessageType: 1,
      TimeSpan: '01/14/2019 05:42 AM'
    },
    {
      MessageId: 640726,
      IsViewed: false,
      MessageContent:
        'superman edited an assignment of MT0A vs MT0B - bettype: Next Corners (LA)\r\n- Old: #starix29\r\n- New: ',
      CreatedBy: 8,
      CreatedDate: '2019-01-14T05:42:40.59+07:00',
      ProfilePic: null,
      IsImportant: false,
      MessageType: 12,
      SystemMessageType: 1,
      TimeSpan: '01/14/2019 05:42 AM'
    },
    {
      MessageId: 640725,
      IsViewed: false,
      MessageContent:
        'superman edited an assignment of MT0A vs MT0B - bettype: Home Single Team O/U of Corners (LA)\r\n- Old: #starix21\r\n- New: ',
      CreatedBy: 8,
      CreatedDate: '2019-01-14T05:42:40.59+07:00',
      ProfilePic: null,
      IsImportant: false,
      MessageType: 12,
      SystemMessageType: 1,
      TimeSpan: '01/14/2019 05:42 AM'
    },
    {
      MessageId: 640724,
      IsViewed: false,
      MessageContent:
        'superman edited an assignment of MT0A vs MT0B - bettype: Away Single Team O/U of Corners (LA)\r\n- Old: #starix22\r\n- New: ',
      CreatedBy: 8,
      CreatedDate: '2019-01-14T05:42:40.59+07:00',
      ProfilePic: null,
      IsImportant: false,
      MessageType: 12,
      SystemMessageType: 1,
      TimeSpan: '01/14/2019 05:42 AM'
    },
    {
      MessageId: 640723,
      IsViewed: false,
      MessageContent:
        'superman edited an assignment of MT0A vs MT0B - bettype: 2H 1st Free Kick (LA)\r\n- Old: #starix7\r\n- New: ',
      CreatedBy: 8,
      CreatedDate: '2019-01-14T05:42:40.59+07:00',
      ProfilePic: null,
      IsImportant: false,
      MessageType: 12,
      SystemMessageType: 1,
      TimeSpan: '01/14/2019 05:42 AM'
    },
    {
      MessageId: 640722,
      IsViewed: false,
      MessageContent:
        'superman edited an assignment of MT0A vs MT0B - bettype: Last Free Kick (LA)\r\n- Old: #starix8\r\n- New: ',
      CreatedBy: 8,
      CreatedDate: '2019-01-14T05:42:40.59+07:00',
      ProfilePic: null,
      IsImportant: false,
      MessageType: 12,
      SystemMessageType: 1,
      TimeSpan: '01/14/2019 05:42 AM'
    },
    {
      MessageId: 640721,
      IsViewed: false,
      MessageContent:
        'superman edited an assignment of MT0A vs MT0B - bettype: Last Goal Kick (LA)\r\n- Old: #starix14\r\n- New: ',
      CreatedBy: 8,
      CreatedDate: '2019-01-14T05:42:40.59+07:00',
      ProfilePic: null,
      IsImportant: false,
      MessageType: 12,
      SystemMessageType: 1,
      TimeSpan: '01/14/2019 05:42 AM'
    },
    {
      MessageId: 640720,
      IsViewed: false,
      MessageContent:
        'superman edited an assignment of MT0A vs MT0B - bettype: Next Goal Kick (LA)\r\n- Old: #starix13\r\n- New: ',
      CreatedBy: 8,
      CreatedDate: '2019-01-14T05:42:40.59+07:00',
      ProfilePic: null,
      IsImportant: false,
      MessageType: 12,
      SystemMessageType: 1,
      TimeSpan: '01/14/2019 05:42 AM'
    }
  ],
  Keyword: '',
  PageSize: 10,
  PageNumber: 2,
  IsWatcher: null
};
describe('TabPanelContent Component Tests', () => {
  beforeEach(() => {
    Service.searchAll.mockReset();
  });
  it('should render <Search />', async () => {
    Service.searchAll.mockResolvedValue(responseData);
    const props = {
      tabId: '1',
      fetchOnLoad: '1',
      isWatcher: null
    };
    const component = render(<TabPanelContent {...props} />);
    await waitFor(() => {
      expect(component).toMatchSnapshot();
    });
  });

  it('test search component on render data ', async () => {
    const responseData = {
      Messages: [
        {
          MessageId: 640729,
          IsViewed: false,
          MessageContent: 'superman edited',
          CreatedBy: 8,
          CreatedDate: '2019-01-14T05:42:40.59+07:00',
          ProfilePic: null,
          IsImportant: false,
          MessageType: 12,
          SystemMessageType: 1,
          TimeSpan: '01/14/2019 05:42 AM'
        }
      ],
      Keyword: '',
      PageSize: 10,
      PageNumber: 2,
      IsWatcher: null
    };
    Service.searchAll.mockResolvedValue(responseData);
    const props = {
      tabId: '1',
      fetchOnLoad: '1',
      isWatcher: null
    };
    const { container } = render(<TabPanelContent {...props} />);
    const icon = container.querySelector(`[class="icon-loading"]`);
    expect(icon).toBeInTheDocument();
    await waitForElementToBeRemoved(icon);
    expect(screen.getByText('superman edited')).toBeInTheDocument();
  });

  it('test messageItem on Click ', async () => {
    Service.searchAll.mockResolvedValue(responseData);
    Service.updateStatus.mockResolvedValue({ msg: 'OK' });
    const props = {
      tabId: '1',
      fetchOnLoad: '1',
      isWatcher: null
    };
    const { container } = render(<TabPanelContent {...props} />);
    const icon = container.querySelector(`[class="icon-loading"]`);
    expect(icon).toBeInTheDocument();
    await waitForElementToBeRemoved(icon);
    fireEvent.click(screen.getByText('superman edited'));
    expect(screen.getByText('superman edited')).toBeInTheDocument();
    expect(Service.updateStatus).toBeCalledTimes(1);
  });

  it('test Pagination on onPageChanged Click ', async () => {
    Service.searchAll.mockResolvedValue(responseData);
    Service.updateStatus.mockResolvedValue({ msg: 'OK' });
    const props = {
      tabId: '1',
      fetchOnLoad: '1',
      isWatcher: null
    };
    const { container } = render(<TabPanelContent {...props} />);
    const icon = container.querySelector(`[class="icon-loading"]`);
    expect(icon).toBeInTheDocument();
    await waitForElementToBeRemoved(icon);
    fireEvent.click(screen.getByText('Next'));
    const iconSecond = container.querySelector(`[class="icon-loading"]`);
    await waitFor(() => {
      expect(iconSecond).toBeInTheDocument();
    });
  });

  it('test Pagination on submit Click ', async () => {
    Service.searchAll.mockResolvedValue(responseData);
    Service.updateStatus.mockResolvedValue({ msg: 'OK' });
    const props = {
      tabId: '1',
      fetchOnLoad: '1',
      isWatcher: null
    };
    const { container } = render(<TabPanelContent {...props} />);
    const inputComponent = container.querySelector('.search-field');
    fireEvent.change(inputComponent, { target: { value: 'Liam' } });
    fireEvent.keyPress(inputComponent, {
      key: 'Enter',
      code: 'Enter',
      charCode: 13
    });
    const iconSecond = container.querySelector(`[class="icon-loading"]`);
    await waitFor(() => {
      expect(iconSecond).toBeInTheDocument();
    });
  });
});
