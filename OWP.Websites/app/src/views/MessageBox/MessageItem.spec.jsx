import { fireEvent, render } from '@testing-library/react';
import MessageItem from './MessageItem';

describe('MessageItem Component Tests', () => {
  it('should render <MessageItem /> is Viewed false', () => {
    const props = {
      message: {
        SystemMessageType: 2,
        MessageContent: 'a',
        IsImportant: false,
        TimeSpan: '02/14/2019 10:05 AM',
        IsViewed: false
      }
    };
    const { container } = render(<MessageItem {...props} />);
    expect(container.querySelector('.unread')).toBeInTheDocument();
  });

  it('should render <MessageItem /> is Viewed true', () => {
    const props = {
      message: {
        SystemMessageType: 5,
        MessageContent: 'a',
        IsImportant: false,
        TimeSpan: '02/14/2019 10:05 AM',
        IsViewed: true
      }
    };
    const { container } = render(<MessageItem {...props} />);
    expect(container.querySelector('.unread')).toBeNull();
  });

  it('test div onClick update read status', () => {
    const props = {
      onClick: jest.fn(),
      message: {
        SystemMessageType: 1,
        MessageContent: 'a',
        IsImportant: false,
        TimeSpan: '02/14/2019 10:05 AM',
        IsViewed: false
      }
    };
    const { container } = render(<MessageItem {...props} />);

    const divMessageItem = container.querySelector('.message-item');
    fireEvent.click(divMessageItem);
    expect(container.querySelector('.unread')).toBeNull();
    expect(divMessageItem).toBeInTheDocument();
  });
});
