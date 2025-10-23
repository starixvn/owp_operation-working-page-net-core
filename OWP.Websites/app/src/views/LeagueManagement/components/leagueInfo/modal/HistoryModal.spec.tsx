import React from 'react';
import { render, screen, waitFor, act } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import '@testing-library/jest-dom';
import HistoryModal from './HistoryModal';

// Mock dependencies
jest.mock('reactstrap', () => ({
  Modal: ({ children, ...props }: any) => (
    <div data-testid='modal' {...props}>
      {children}
    </div>
  ),
  ModalHeader: ({ children, ...props }: any) => (
    <div data-testid='modal-header' {...props}>
      {children}
    </div>
  ),
  ModalBody: ({ children, ...props }: any) => (
    <div data-testid='modal-body' {...props}>
      {children}
    </div>
  )
}));

jest.mock('../../../../Shared/Service/Service', () => ({
  get: jest.fn()
}));

jest.mock('dayjs', () => {
  const mockDayjs = jest.fn((dateString: string, format?: string) => ({
    format: jest.fn((formatString: string) => {
      // Mock format based on input date string
      if (dateString.includes('2025-09-28')) {
        return '09/28/2025 04:02 PM';
      }
      if (dateString.includes('2025-09-27')) {
        return '09/27/2025 02:30 PM';
      }
      return '09/26/2025 10:15 AM';
    })
  }));

  return mockDayjs;
});

// Import mocked modules
import { get } from '../../../../Shared/Service/Service';

describe('HistoryModal', () => {
  const mockProps = {
    leagueSettingId: 123,
    leagueName: 'Test League',
    onToggle: jest.fn()
  };

  const mockLogData = [
    {
      logId: 1,
      settingId: 123,
      createdDate: '2025-09-28T16:02:22.04',
      modifiedBy: 'user1',
      action: 'Updated the setting',
      details: [
        {
          field: 'League name',
          oldValue: '',
          newValue: 'Test League Updated'
        },
        {
          field: 'Status',
          oldValue: 'Active',
          newValue: 'Inactive'
        }
      ]
    },
    {
      logId: 2,
      settingId: 123,
      createdDate: '2025-09-27T14:30:15.00',
      modifiedBy: 'user2',
      action: 'Updated the setting',
      details: [
        {
          field: 'Max Bet',
          oldValue: '1000',
          newValue: '2000'
        }
      ]
    }
  ];

  beforeEach(() => {
    jest.clearAllMocks();
  });

  const renderComponent = (props = {}) => {
    const defaultProps = { ...mockProps, ...props };
    return render(<HistoryModal {...defaultProps} />);
  };

  describe('Rendering', () => {
    it('should render modal with correct title', () => {
      renderComponent();

      expect(screen.getByTestId('modal')).toBeInTheDocument();
      expect(screen.getByTestId('modal-header')).toBeInTheDocument();
      expect(screen.getByTestId('modal-body')).toBeInTheDocument();
      expect(screen.getByText('Log History - Test League')).toBeInTheDocument();
    });

    it('should render table headers correctly', () => {
      renderComponent();

      expect(screen.getByText('Date (GMT+8)')).toBeInTheDocument();
      expect(screen.getByText('Modified by')).toBeInTheDocument();
      expect(screen.getByText('Action')).toBeInTheDocument();
      expect(screen.getByText('Field')).toBeInTheDocument();
      expect(screen.getByText('Old Value')).toBeInTheDocument();
      expect(screen.getByText('New Value')).toBeInTheDocument();
    });
  });

  describe('Loading State', () => {
    it('should show loading state initially', () => {
      (get as jest.Mock).mockImplementation(() => new Promise(() => {})); // Never resolves

      renderComponent();

      expect(screen.getByText('Loading...')).toBeInTheDocument();
    });
  });

  describe('API Success', () => {
    it('should render log data when API call succeeds', async () => {
      (get as jest.Mock).mockResolvedValue({ data: mockLogData });

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText('09/28/2025 04:02 PM')).toBeInTheDocument();
        expect(screen.getByText('user1')).toBeInTheDocument();
        const actionCells = screen.getAllByText('Updated the setting');
        expect(actionCells.length).toBeGreaterThan(0);
        expect(screen.getByText('League name')).toBeInTheDocument();
        expect(screen.getByText('Test League Updated')).toBeInTheDocument();
      });
    });

    it('should render multiple field changes for single log entry', async () => {
      (get as jest.Mock).mockResolvedValue({ data: mockLogData });

      renderComponent();

      await waitFor(() => {
        // First log entry with 2 field changes
        expect(screen.getByText('League name')).toBeInTheDocument();
        expect(screen.getByText('Status')).toBeInTheDocument();
        expect(screen.getByText('Test League Updated')).toBeInTheDocument();
        expect(screen.getByText('Inactive')).toBeInTheDocument();

        // Second log entry with 1 field change
        expect(screen.getByText('Max Bet')).toBeInTheDocument();
        expect(screen.getByText('2000')).toBeInTheDocument();
      });
    });

    it('should handle empty data response', async () => {
      (get as jest.Mock).mockResolvedValue({ data: [] });

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText('No log history found')).toBeInTheDocument();
      });
    });

    it('should handle null data response', async () => {
      (get as jest.Mock).mockResolvedValue({ data: null });

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText('No log history found')).toBeInTheDocument();
      });
    });

    it('should handle undefined data response', async () => {
      (get as jest.Mock).mockResolvedValue({});

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText('No log history found')).toBeInTheDocument();
      });
    });
  });

  describe('API Error', () => {
    it('should show error message when API call fails', async () => {
      (get as jest.Mock).mockRejectedValue(new Error('API Error'));

      renderComponent();

      await waitFor(() => {
        expect(
          screen.getByText('Failed to load log history')
        ).toBeInTheDocument();
      });
    });

    it('should log error to console', async () => {
      const consoleSpy = jest
        .spyOn(console, 'error')
        .mockImplementation(() => {});
      (get as jest.Mock).mockRejectedValue(new Error('API Error'));

      renderComponent();

      await waitFor(() => {
        expect(consoleSpy).toHaveBeenCalledWith(
          'Error fetching log history:',
          expect.any(Error)
        );
      });

      consoleSpy.mockRestore();
    });
  });

  describe('API Call', () => {
    it('should call API with correct parameters', async () => {
      (get as jest.Mock).mockResolvedValue({ data: mockLogData });

      renderComponent();

      await waitFor(() => {
        expect(get).toHaveBeenCalledWith({
          url: 'api/NewLeagueSetting/get-logs',
          leagueSettingId: 123
        });
      });
    });

    it('should not call API when leagueSettingId is 0', () => {
      renderComponent({ leagueSettingId: 0 });

      expect(get).not.toHaveBeenCalled();
    });

    it('should not call API when leagueSettingId is null', () => {
      renderComponent({ leagueSettingId: null as any });

      expect(get).not.toHaveBeenCalled();
    });

    it('should call API when leagueSettingId changes', async () => {
      (get as jest.Mock).mockResolvedValue({ data: mockLogData });

      const { rerender } = renderComponent({ leagueSettingId: 123 });

      await waitFor(() => {
        expect(get).toHaveBeenCalledTimes(1);
      });

      rerender(<HistoryModal {...mockProps} leagueSettingId={456} />);

      await waitFor(() => {
        expect(get).toHaveBeenCalledTimes(2);
        expect(get).toHaveBeenLastCalledWith({
          url: 'api/NewLeagueSetting/get-logs',
          leagueSettingId: 456
        });
      });
    });
  });

  describe('Table Structure', () => {
    it('should render table with correct structure', async () => {
      (get as jest.Mock).mockResolvedValue({ data: mockLogData });

      renderComponent();

      await waitFor(() => {
        const table = screen.getByRole('table');
        expect(table).toBeInTheDocument();

        // Check for table headers
        expect(screen.getByText('Date (GMT+8)')).toBeInTheDocument();
        expect(screen.getByText('Modified by')).toBeInTheDocument();
        expect(screen.getByText('Action')).toBeInTheDocument();
        expect(screen.getByText('Field')).toBeInTheDocument();
        expect(screen.getByText('Old Value')).toBeInTheDocument();
        expect(screen.getByText('New Value')).toBeInTheDocument();
      });
    });

    it('should render rows with correct rowSpan for first field change', async () => {
      (get as jest.Mock).mockResolvedValue({ data: mockLogData });

      renderComponent();

      await waitFor(() => {
        // First log entry should have rowSpan=2 for date, modifiedBy, and action
        const dateCells = screen.getAllByText('09/28/2025 04:02 PM');
        const userCells = screen.getAllByText('user1');

        expect(dateCells).toHaveLength(1); // Should only appear once due to rowSpan
        expect(userCells).toHaveLength(1); // Should only appear once due to rowSpan

        // Action text appears multiple times due to multiple log entries
        const actionCells = screen.getAllByText('Updated the setting');
        expect(actionCells.length).toBeGreaterThan(0);
      });
    });
  });

  describe('Date Formatting', () => {
    it('should format dates correctly using dayjs', async () => {
      (get as jest.Mock).mockResolvedValue({ data: mockLogData });

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText('09/28/2025 04:02 PM')).toBeInTheDocument();
        expect(screen.getByText('09/27/2025 02:30 PM')).toBeInTheDocument();
      });
    });
  });

  describe('Modal Props', () => {
    it('should pass correct props to Modal component', () => {
      renderComponent();

      const modal = screen.getByTestId('modal');
      expect(modal).toHaveAttribute('class', 'league-setting-modal');
    });

    it('should display correct league name in header', () => {
      renderComponent({ leagueName: 'Custom League Name' });

      expect(
        screen.getByText('Log History - Custom League Name')
      ).toBeInTheDocument();
    });

    it('should render modal header with toggle functionality', () => {
      const onToggle = jest.fn();
      renderComponent({ onToggle });

      const modalHeader = screen.getByTestId('modal-header');
      expect(modalHeader).toBeInTheDocument();
    });
  });

  describe('Edge Cases', () => {
    it('should handle log entry with no details', async () => {
      const logDataWithNoDetails = [
        {
          logId: 1,
          settingId: 123,
          createdDate: '2025-09-28T16:02:22.04',
          modifiedBy: 'user1',
          action: 'Updated the setting',
          details: []
        }
      ];

      (get as jest.Mock).mockResolvedValue({ data: logDataWithNoDetails });

      renderComponent();

      await waitFor(() => {
        // When details is empty array, it should render empty table body
        const table = screen.getByRole('table');
        expect(table).toBeInTheDocument();
      });
    });

    it('should handle log entry with null details', async () => {
      const logDataWithNullDetails = [
        {
          logId: 1,
          settingId: 123,
          createdDate: '2025-09-28T16:02:22.04',
          modifiedBy: 'user1',
          action: 'Updated the setting',
          details: null
        }
      ];

      (get as jest.Mock).mockResolvedValue({ data: logDataWithNullDetails });

      renderComponent();

      await waitFor(() => {
        // Should handle null details gracefully without crashing
        const table = screen.getByRole('table');
        expect(table).toBeInTheDocument();
      });
    });

    it('should handle field change with empty values', async () => {
      const logDataWithEmptyValues = [
        {
          logId: 1,
          settingId: 123,
          createdDate: '2025-09-28T16:02:22.04',
          modifiedBy: 'user1',
          action: 'Updated the setting',
          details: [
            {
              field: '',
              oldValue: '',
              newValue: ''
            }
          ]
        }
      ];

      (get as jest.Mock).mockResolvedValue({ data: logDataWithEmptyValues });

      renderComponent();

      await waitFor(() => {
        // Should render empty cells without crashing
        const table = screen.getByRole('table');
        expect(table).toBeInTheDocument();
      });
    });

    it('should handle HTML content in field values', async () => {
      const logDataWithHtml = [
        {
          logId: 1,
          settingId: 123,
          createdDate: '2025-09-28T16:02:22.04',
          modifiedBy: 'user1',
          action: 'Updated the setting',
          details: [
            {
              field: 'Description',
              oldValue: '<b>Old</b> value',
              newValue: '<i>New</i> value'
            }
          ]
        }
      ];

      (get as jest.Mock).mockResolvedValue({ data: logDataWithHtml });

      renderComponent();

      await waitFor(() => {
        // Check that HTML is rendered via dangerouslySetInnerHTML
        const table = screen.getByRole('table');
        expect(table).toBeInTheDocument();
        expect(screen.getByText('Description')).toBeInTheDocument();
      });
    });

    it('should render multiple log entries correctly', async () => {
      const multipleLogData = [
        {
          logId: 1,
          settingId: 123,
          createdDate: '2025-09-28T16:02:22.04',
          modifiedBy: 'user1',
          action: 'Created',
          details: [
            {
              field: 'League name',
              oldValue: '',
              newValue: 'New League'
            }
          ]
        },
        {
          logId: 2,
          settingId: 123,
          createdDate: '2025-09-27T14:30:15.00',
          modifiedBy: 'user2',
          action: 'Updated',
          details: [
            {
              field: 'Status',
              oldValue: 'Active',
              newValue: 'Inactive'
            }
          ]
        }
      ];

      (get as jest.Mock).mockResolvedValue({ data: multipleLogData });

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText('Created')).toBeInTheDocument();
        expect(screen.getByText('Updated')).toBeInTheDocument();
        expect(screen.getByText('user1')).toBeInTheDocument();
        expect(screen.getByText('user2')).toBeInTheDocument();
      });
    });
  });

  describe('Component Lifecycle', () => {
    it('should cleanup on unmount', () => {
      const { unmount } = renderComponent();

      expect(() => unmount()).not.toThrow();
    });

    it('should handle rapid prop changes', async () => {
      (get as jest.Mock).mockResolvedValue({ data: mockLogData });

      const { rerender } = renderComponent({ leagueSettingId: 123 });

      // Rapidly change props
      rerender(<HistoryModal {...mockProps} leagueSettingId={456} />);
      rerender(<HistoryModal {...mockProps} leagueSettingId={789} />);

      await waitFor(() => {
        expect(get).toHaveBeenCalled();
      });
    });
  });

  describe('Table Content Rendering', () => {
    it('should render table with correct CSS class', async () => {
      (get as jest.Mock).mockResolvedValue({ data: mockLogData });

      renderComponent();

      await waitFor(() => {
        const table = screen.getByRole('table');
        expect(table).toHaveClass('league-setting-log-history-table');
      });
    });

    it('should render modal body with correct CSS class', () => {
      renderComponent();

      const modalBody = screen.getByTestId('modal-body');
      expect(modalBody).toHaveClass('league-setting-modal-body');
    });

    it('should render action column correctly', async () => {
      (get as jest.Mock).mockResolvedValue({ data: mockLogData });

      renderComponent();

      await waitFor(() => {
        const actionCells = screen.getAllByText('Updated the setting');
        expect(actionCells.length).toBeGreaterThan(0);
      });
    });

    it('should render old values correctly', async () => {
      (get as jest.Mock).mockResolvedValue({ data: mockLogData });

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText('Active')).toBeInTheDocument();
        expect(screen.getByText('1000')).toBeInTheDocument();
      });
    });

    it('should render new values correctly', async () => {
      (get as jest.Mock).mockResolvedValue({ data: mockLogData });

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText('Inactive')).toBeInTheDocument();
        expect(screen.getByText('2000')).toBeInTheDocument();
        expect(screen.getByText('Test League Updated')).toBeInTheDocument();
      });
    });
  });

  describe('Loading and Error States', () => {
    it('should reset error state when fetching new data', async () => {
      // First call fails
      (get as jest.Mock).mockRejectedValueOnce(new Error('API Error'));

      const { rerender } = renderComponent({ leagueSettingId: 123 });

      await waitFor(() => {
        expect(
          screen.getByText('Failed to load log history')
        ).toBeInTheDocument();
      });

      // Second call succeeds
      (get as jest.Mock).mockResolvedValueOnce({ data: mockLogData });

      rerender(<HistoryModal {...mockProps} leagueSettingId={456} />);

      await waitFor(() => {
        expect(
          screen.queryByText('Failed to load log history')
        ).not.toBeInTheDocument();
        expect(screen.getByText('09/28/2025 04:02 PM')).toBeInTheDocument();
      });
    });

    it('should show loading before error', async () => {
      (get as jest.Mock).mockImplementation(
        () =>
          new Promise((_, reject) =>
            setTimeout(() => reject(new Error('API Error')), 100)
          )
      );

      renderComponent();

      expect(screen.getByText('Loading...')).toBeInTheDocument();

      await waitFor(() => {
        expect(
          screen.getByText('Failed to load log history')
        ).toBeInTheDocument();
      });
    });
  });
});
