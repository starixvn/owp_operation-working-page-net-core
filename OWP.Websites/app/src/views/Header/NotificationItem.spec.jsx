import { fireEvent, render, screen } from '@testing-library/react';
import { MessageTypes, SystemMessageTypes } from '../Message/Service';
import NotificationItem from './NotificationItem';
import CloudSyncIcon from 'mdi-react/CloudSyncIcon';
import AccountIcon from 'mdi-react/AccountIcon';
import BullhornIcon from 'mdi-react/BullhornIcon';

jest.mock('mdi-react/AccountIcon', () => jest.fn(() => 'AccountIcon'));
jest.mock('mdi-react/BullhornIcon', () => jest.fn(() => 'BullhornIcon'));
jest.mock('mdi-react/CloudSyncIcon', () => jest.fn(() => 'CloudSyncIcon'));
describe('Notification Item Component', () => {
  const dataItem = {
    RowNumber: 1,
    MessageId: 1,
    MessageContent: 'Hello',
    CreatedDate: '2018-12-07',
    IsImportant: true,
    IsViewed: false,
    MessageType: MessageTypes.AssignmentInsert
  };
  it('Render <NotificationItem data={} />"', done => {
    const props = {
      data: {}
    };
    const { container } = render(<NotificationItem {...props} />);
    expect(container).toMatchSnapshot();
    expect(container.querySelector('.noti-item')).toBeInTheDocument();
    done();
  });

  it('should render div with Message IsImportant True', done => {
    const props = {
      data: dataItem
    };

    const { container } = render(<NotificationItem {...props} />);
    process.nextTick(() => {
      expect(container.querySelector('.star-icon')).toBeInTheDocument();
      done();
    });
  });

  it('should renderMessageIcon Default', () => {
    const props = {
      data: dataItem
    };

    render(<NotificationItem {...props} />);

    expect(AccountIcon).toBeCalled();
  });

  it('should renderMessageIcon Notification', () => {
    const dataItemNew = dataItem;
    dataItemNew.SystemMessageType = SystemMessageTypes.Notification;
    const props = {
      data: dataItemNew
    };
    render(<NotificationItem {...props} />);

    expect(CloudSyncIcon).toBeCalled();
  });

  it('should renderMessageIcon Announcement', () => {
    const dataItemNew = dataItem;
    dataItemNew.SystemMessageType = SystemMessageTypes.Announcement;
    const props = {
      data: dataItemNew
    };

    render(<NotificationItem {...props} />);

    expect(BullhornIcon).toBeCalled();
  });
});
