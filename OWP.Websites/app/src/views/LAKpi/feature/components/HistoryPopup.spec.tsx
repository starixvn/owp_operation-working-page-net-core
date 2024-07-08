import { render, waitFor } from '@testing-library/react';
import HistoryPopup from './HistoryPopup';
import { getLogHistory } from '../../service';

jest.mock('../../service');

describe('HistoryPopup component', () => {
  const mockLogs = [
    {
      modifyDate: '2023-01-01',
      modifyBy: 'John Doe',
      oldValue: 'Old Value 1',
      newValue: 'New Value 1'
    },
    {
      modifyDate: '2023-01-02',
      modifyBy: 'Jane Smith',
      oldValue: 'Old Value 2',
      newValue: 'New Value 2'
    }
  ];

  beforeEach(() => {
    getLogHistory.mockResolvedValue(mockLogs);
  });

  it('renders with correct header and table columns', async () => {
    const handleClosePopup = jest.fn();
    const userId = 123;
    const userName = 'John Doe';

    const { getByText, getByTestId } = render(
      <HistoryPopup
        handleClosePopup={handleClosePopup}
        userId={userId}
        userName={userName}
      />
    );

    await waitFor(() => {
      expect(
        getByText(`Target Update History - ${userName}`)
      ).toBeInTheDocument();
      expect(getByText('Date (GMT+8)')).toBeInTheDocument();
      expect(getByText('Modified by')).toBeInTheDocument();
      expect(getByText('Old Value')).toBeInTheDocument();
      expect(getByText('New Value')).toBeInTheDocument();

      expect(getByText(mockLogs[0].modifyDate)).toBeInTheDocument();
      expect(getByText(mockLogs[0].modifyBy)).toBeInTheDocument();
      expect(getByText(mockLogs[0].oldValue)).toBeInTheDocument();
      expect(getByText(mockLogs[0].newValue)).toBeInTheDocument();
    });
  });
});
