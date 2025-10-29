import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import { Provider } from 'react-redux';
import { configureStore } from '@reduxjs/toolkit';
import ActionCell from './ActionCell';
import ToastrWrapper from '../../../components/ToastrWrapper';
import Spinner from '../../../components/Spinner';
import { removeSpecialProduct } from '../../service/Service';
import { getEventPickings } from '../eventPickingTableSlice/eventSliceThunkNative';

// Mock modules
jest.mock('../../../components/ToastrWrapper');
jest.mock('../../../components/Spinner');
jest.mock('../../service/Service');
jest.mock('../eventPickingTableSlice/eventSliceThunkNative');
jest.mock('../../helper/specialProductHelper', () => ({
  buildExistingMinorBettypeIdsMapping: jest.fn(() => '{"456":"101,102"}')
}));
jest.mock('../../modal/HistoryModal.native', () => {
  return function HistoryModal({ header, onToggle }) {
    return (
      <div data-testid="history-modal">
        <div data-testid="modal-header">{header}</div>
        <button onClick={onToggle}>Close Modal</button>
      </div>
    );
  };
});

describe('ActionCell', () => {
  let mockStore;
  const originalError = console.error;

  const mockMainMatchData = [
    {
      infoSiteMatchId: 123,
      leagueName: 'Test League',
      homeName: 'Home Team',
      awayName: 'Away Team',
      eventDateTime: '2024-01-15T10:00:00',
      minorBettypeId: 0,
      isChildMatch: false
    }
  ];

  const mockChildMatchData = [
    {
      infoSiteMatchId: 456,
      leagueName: 'Test League',
      homeName: 'Home Team',
      awayName: 'Away Team',
      eventDateTime: '2024-01-15T10:00:00',
      minorBettypeId: 101,
      isChildMatch: true
    }
  ];

  beforeEach(() => {
    // Suppress console errors for known warnings
    console.error = jest.fn();

    // Create mock store
    mockStore = configureStore({
      reducer: {
        pagePolicy: () => ({
          permission: { HasWrite: true }
        }),
        eventPickingTable: () => ({
          entities: {
            123: { infoSiteMatchId: 123, minorBettypeId: 0 },
            456: { infoSiteMatchId: 123, minorBettypeId: 101 }
          }
        }),
        filter: () => ({
          param: { eventDate: '2024-01-15' }
        })
      }
    });

    // Reset mocks
    ToastrWrapper.confirmYesNo = jest.fn();
    ToastrWrapper.showInfo = jest.fn();
    Spinner.show = jest.fn();
    Spinner.hide = jest.fn();
    removeSpecialProduct.mockReset();
    getEventPickings.mockReturnValue({ type: 'mock-action' });
  });

  afterEach(() => {
    console.error = originalError;
    jest.clearAllMocks();
  });

  describe('Main Match (non-child)', () => {
    it('should render log history icon for main match', () => {
      const { container } = render(
        <Provider store={mockStore}>
          <ActionCell dataSource={mockMainMatchData} rowIndex={0} />
        </Provider>
      );

      const historyIcon = container.querySelector('.icon-history-event');
      expect(historyIcon).toBeInTheDocument();
    });

    it('should open history modal when clicking log history icon', async () => {
      render(
        <Provider store={mockStore}>
          <ActionCell dataSource={mockMainMatchData} rowIndex={0} />
        </Provider>
      );

      const historyIcon = document.querySelector('.icon-history-event');
      fireEvent.click(historyIcon);

      await waitFor(() => {
        expect(screen.getByTestId('history-modal')).toBeInTheDocument();
      });
    });

    it('should display correct header in history modal', async () => {
      render(
        <Provider store={mockStore}>
          <ActionCell dataSource={mockMainMatchData} rowIndex={0} />
        </Provider>
      );

      const historyIcon = document.querySelector('.icon-history-event');
      fireEvent.click(historyIcon);

      await waitFor(() => {
        const modalHeader = screen.getByTestId('modal-header');
        expect(modalHeader).toHaveTextContent('Test League');
        expect(modalHeader).toHaveTextContent('Home Team vs Away Team');
      });
    });

    it('should close history modal when toggle is called', async () => {
      render(
        <Provider store={mockStore}>
          <ActionCell dataSource={mockMainMatchData} rowIndex={0} />
        </Provider>
      );

      const historyIcon = document.querySelector('.icon-history-event');
      fireEvent.click(historyIcon);

      await waitFor(() => {
        expect(screen.getByTestId('history-modal')).toBeInTheDocument();
      });

      const closeButton = screen.getByText('Close Modal');
      fireEvent.click(closeButton);

      await waitFor(() => {
        expect(screen.queryByTestId('history-modal')).not.toBeInTheDocument();
      });
    });
  });

  describe('Child Match (special product)', () => {
    it('should render delete icon for child match when HasWrite is true', () => {
      const { container } = render(
        <Provider store={mockStore}>
          <ActionCell dataSource={mockChildMatchData} rowIndex={0} />
        </Provider>
      );

      const deleteIcon = container.querySelector('.icon-delete');
      expect(deleteIcon).toBeInTheDocument();
    });

    it('should not render delete icon when HasWrite is false', () => {
      const storeWithoutWrite = configureStore({
        reducer: {
          pagePolicy: () => ({
            permission: { HasWrite: false }
          }),
          eventPickingTable: () => ({
            entities: {}
          }),
          filter: () => ({
            param: {}
          })
        }
      });

      const { container } = render(
        <Provider store={storeWithoutWrite}>
          <ActionCell dataSource={mockChildMatchData} rowIndex={0} />
        </Provider>
      );

      const deleteIcon = container.querySelector('.icon-delete');
      expect(deleteIcon).not.toBeInTheDocument();
    });

    it('should show confirmation dialog when clicking delete icon', async () => {
      ToastrWrapper.confirmYesNo.mockResolvedValue({ value: false });

      render(
        <Provider store={mockStore}>
          <ActionCell dataSource={mockChildMatchData} rowIndex={0} />
        </Provider>
      );

      const deleteIcon = document.querySelector('.icon-delete');
      fireEvent.click(deleteIcon);

      await waitFor(() => {
        expect(ToastrWrapper.confirmYesNo).toHaveBeenCalledWith(
          'Are you sure that you want to delete this event ?'
        );
      });
    });

    it('should not call API when user cancels deletion', async () => {
      ToastrWrapper.confirmYesNo.mockResolvedValue({ value: false });

      render(
        <Provider store={mockStore}>
          <ActionCell dataSource={mockChildMatchData} rowIndex={0} />
        </Provider>
      );

      const deleteIcon = document.querySelector('.icon-delete');
      fireEvent.click(deleteIcon);

      await waitFor(() => {
        expect(ToastrWrapper.confirmYesNo).toHaveBeenCalled();
      });

      expect(removeSpecialProduct).not.toHaveBeenCalled();
      expect(Spinner.show).not.toHaveBeenCalled();
    });

    it('should successfully delete special product when user confirms', async () => {
      ToastrWrapper.confirmYesNo.mockResolvedValue({ value: true });
      removeSpecialProduct.mockResolvedValue({ status: 200 });

      render(
        <Provider store={mockStore}>
          <ActionCell dataSource={mockChildMatchData} rowIndex={0} />
        </Provider>
      );

      const deleteIcon = document.querySelector('.icon-delete');
      fireEvent.click(deleteIcon);

      await waitFor(() => {
        expect(Spinner.show).toHaveBeenCalled();
      });

      await waitFor(() => {
        expect(removeSpecialProduct).toHaveBeenCalledWith({
          ParentId: '456',
          MinorBettypeId: '101',
          ExistingMinorBettypeIds: expect.any(String)
        });
      });

      await waitFor(() => {
        expect(ToastrWrapper.showInfo).toHaveBeenCalledWith(
          'Remove event successfully!'
        );
        expect(Spinner.hide).toHaveBeenCalled();
      });
    });

    it('should dispatch getEventPickings after successful deletion', async () => {
      ToastrWrapper.confirmYesNo.mockResolvedValue({ value: true });
      removeSpecialProduct.mockResolvedValue({ status: 200 });

      const dispatchSpy = jest.fn();
      mockStore.dispatch = dispatchSpy;

      render(
        <Provider store={mockStore}>
          <ActionCell dataSource={mockChildMatchData} rowIndex={0} />
        </Provider>
      );

      const deleteIcon = document.querySelector('.icon-delete');
      fireEvent.click(deleteIcon);

      await waitFor(() => {
        expect(removeSpecialProduct).toHaveBeenCalled();
      });

      await waitFor(() => {
        expect(dispatchSpy).toHaveBeenCalled();
      });
    });

    it('should not show success message when API returns non-200 status', async () => {
      ToastrWrapper.confirmYesNo.mockResolvedValue({ value: true });
      removeSpecialProduct.mockResolvedValue({ status: 500 });

      render(
        <Provider store={mockStore}>
          <ActionCell dataSource={mockChildMatchData} rowIndex={0} />
        </Provider>
      );

      const deleteIcon = document.querySelector('.icon-delete');
      fireEvent.click(deleteIcon);

      await waitFor(() => {
        expect(removeSpecialProduct).toHaveBeenCalled();
      });

      await waitFor(() => {
        expect(Spinner.hide).toHaveBeenCalled();
      });

      expect(ToastrWrapper.showInfo).not.toHaveBeenCalled();
    });
  });

  describe('PropTypes validation', () => {
    it('should render with required props', () => {
      expect(() => {
        render(
          <Provider store={mockStore}>
            <ActionCell dataSource={mockMainMatchData} rowIndex={0} />
          </Provider>
        );
      }).not.toThrow();
    });
  });
});

