import Header from './Header';
import { fireEvent, render, screen } from '@testing-library/react';
import Notification from './Notification';

jest.mock('./Notification', () => jest.fn(() => 'NotificationFake'));
describe('Header Component', () => {
  const props = {
    title: 'Home page',
    username: 'superman',
    logoutLink: 'logout_link',
    notificationSetingLink: 'notification_link',
    quickSearchPolicy: {
      HasRead: true
    }
  };

  it('check handleVisibleSearchArea function when isVisible=true', async () => {
    render(<Header {...props} />);
    fireEvent.click(screen.getByText(/Search/i));

    expect(Notification).toBeCalled();
    expect(document.body.classList.contains('is-open-search')).toBe(true);
  });

  it('check handleVisibleSearchArea function when isVisible=false', () => {
    render(<Header {...props} />);
    fireEvent.click(screen.getByText(/Search/i));
    fireEvent.click(screen.getByText(/close/i));

    expect(document.body.classList.contains('is-open-search')).toBe(false);
  });

  it('check icon Close quick search onClick callback', () => {
    const { container } = render(<Header {...props} />);
    const iconClose = container.querySelector('div.cd-cover-layer');
    fireEvent.click(iconClose);

    expect(document.body.classList.contains('is-open-search')).toBe(false);
  });
});
