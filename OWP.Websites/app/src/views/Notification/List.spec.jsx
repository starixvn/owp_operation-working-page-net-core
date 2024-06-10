import { render, fireEvent, screen, waitFor } from '@testing-library/react';
import List from './List';
import NotificationService from './Service';

jest.mock('./Service');

const mockData = {
  Assignees: [
    { NotificationSettingId: 1, Name: 'Setting 1', Value: 'True' },
    { NotificationSettingId: 2, Name: 'Setting 2', Value: 'False' }
  ],
  Watchers: [
    { NotificationSettingId: 3, Name: 'Setting 3', Value: 'True' },
    { NotificationSettingId: 4, Name: 'Setting 4', Value: 'False' }
  ]
};

describe('List component', () => {
  beforeEach(() => {
    NotificationService.getByCurrentUser.mockResolvedValue(mockData);
  });

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('renders loading message while data is being fetched', async () => {
    NotificationService.getByCurrentUser.mockResolvedValueOnce(
      new Promise(() => {})
    );
    render(<List policy={{ HasRead: true }} />);
    expect(screen.getByText('Loading...')).toBeInTheDocument();
    await waitFor(() =>
      expect(NotificationService.getByCurrentUser).toHaveBeenCalledTimes(1)
    );
  });

  it('renders Assignees and Watchers tabs', async () => {
    render(<List policy={{ HasRead: true }} />);
    expect(screen.getByText('My Message')).toBeInTheDocument();
    expect(screen.getByText('My Section Message')).toBeInTheDocument();
    await waitFor(() =>
      expect(NotificationService.getByCurrentUser).toHaveBeenCalledTimes(1)
    );
  });

  it('clicks on Assignees tab and checks a checkbox', async () => {
    render(<List policy={{ HasRead: true }} />);
    await waitFor(() =>
      expect(NotificationService.getByCurrentUser).toHaveBeenCalledTimes(1)
    );
    fireEvent.click(screen.getByText('My Message'));
    expect(screen.getByText('Setting 1')).toBeInTheDocument();
    expect(screen.getByText('Setting 2')).toBeInTheDocument();
    fireEvent.click(screen.getByTestId('notification-2-checkbox'));
    expect(NotificationService.update).toHaveBeenCalledWith({
      NotificationSettingId: '2',
      Value: 'True'
    });
  });

  it('clicks on Watchers tab and checks a checkbox', async () => {
    render(<List policy={{ HasRead: true }} />);
    await waitFor(() =>
      expect(NotificationService.getByCurrentUser).toHaveBeenCalledTimes(1)
    );
    fireEvent.click(screen.getByText('My Section Message'));
    expect(screen.getByText('Setting 3')).toBeInTheDocument();
    expect(screen.getByText('Setting 4')).toBeInTheDocument();
    fireEvent.click(screen.getByTestId('notification-4-checkbox'));
    expect(NotificationService.update).toHaveBeenCalledWith({
      NotificationSettingId: '4',
      Value: 'True'
    });
  });
});
