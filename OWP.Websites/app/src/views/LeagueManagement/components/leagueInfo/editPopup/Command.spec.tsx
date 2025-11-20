import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import Command from './Command';
import { useAppDispatch } from '../app/hooks';
import store from '../app/storeNative';
import { updateSettingService } from '../service/service';
import { fetchLeagueSettingInfo } from '../table/leagueInfoTableSlice/leagueInfoThunk';
import {
  setFocusSettingId,
  setIsSaveAction,
  setFilterChange
} from '../table/leagueInfoTableSlice/reducer';
import { hasInvalidDivision } from '../helper/helper';

jest.mock('../app/hooks', () => ({
  useAppDispatch: jest.fn()
}));

jest.mock('../app/storeNative', () => ({
  __esModule: true,
  default: {
    getState: jest.fn()
  }
}));

jest.mock('../service/service', () => ({
  updateSettingService: jest.fn()
}));

jest.mock('../table/leagueInfoTableSlice/leagueInfoThunk', () => ({
  fetchLeagueSettingInfo: jest.fn()
}));

jest.mock('../table/leagueInfoTableSlice/reducer', () => ({
  setFocusSettingId: jest.fn(),
  setIsSaveAction: jest.fn(),
  setFilterChange: jest.fn()
}));

jest.mock('../helper/helper', () => ({
  hasInvalidDivision: jest.fn()
}));

jest.mock('../../../../components/ToastrWrapper', () => ({
  showError: jest.fn(),
  showInfo: jest.fn(),
  show: jest.fn(),
  showDuplicateLeagueSetting: jest.fn()
}));

jest.mock('../../../constants/Const', () => ({
  message: {
    changeLeagueGroup: 'Are you sure you want to change league group?'
  }
}));

import ToastrWrapper from '../../../../components/ToastrWrapper';

const mockDispatch = jest.fn();
const mockUseAppDispatch = useAppDispatch as jest.Mock;
const mockUpdateSettingService = updateSettingService as jest.Mock;
const mockFetchLeagueSettingInfo = fetchLeagueSettingInfo as jest.Mock;
const mockSetFocusSettingId = setFocusSettingId as jest.Mock;
const mockSetIsSaveAction = setIsSaveAction as jest.Mock;
const mockSetFilterChange = setFilterChange as jest.Mock;
const mockHasInvalidDivision = hasInvalidDivision as jest.Mock;
const mockStoreGetState = store.getState as jest.Mock;

describe('Command Component', () => {
  const mockClosePopup = jest.fn();

  const defaultStoreState = {
    editSettingSlice: {
      leagueSettingId: 123,
      league: 'Test League',
      owpLeagueId: 456,
      leagueGroupId: 789,
      oldLeagueGroupId: 789,
      leagueGroup: 'Test Group',
      spread: '1.5',
      maxbetWD_HDP: '1000',
      wD_NoOfHDP: '5',
      maxbetWK_HDP: '2000',
      wK_NoOfHDP: '10',
      lstNote: 'Note',
      remark2: 'Remark',
      hasTv: true,
      hasIpTv: false,
      remark: 'Test remark',
      round: [{ type: 'Contains', value: 'Round 1' }],
      division: [{ type: 'Equals', value: 'Div 1' }],
      isBlackList: false,
      noOfferLive: 0,
      comment: 'Comment',
      oldData: null
    },
    leagueInfoTableSlice: {
      leagueFilterHeader: {},
      multipleFilter: {},
      filterState: []
    }
  };

  beforeEach(() => {
    jest.clearAllMocks();
    mockUseAppDispatch.mockReturnValue(mockDispatch);
    mockStoreGetState.mockReturnValue(defaultStoreState);
    mockHasInvalidDivision.mockReturnValue(false);
    mockUpdateSettingService.mockResolvedValue({ status: 200 });
    (ToastrWrapper.show as jest.Mock).mockResolvedValue({ value: true });
    (ToastrWrapper.showInfo as jest.Mock).mockResolvedValue(undefined);
  });

  describe('Rendering', () => {
    it('should render Cancel and Save buttons', () => {
      render(<Command closePopup={mockClosePopup} />);

      expect(screen.getByText('Cancel')).toBeInTheDocument();
      expect(screen.getByText('Save')).toBeInTheDocument();
    });

    it('should have correct button classes', () => {
      const { container } = render(<Command closePopup={mockClosePopup} />);

      expect(container.querySelector('.btn-cancel')).toBeInTheDocument();
      expect(container.querySelector('.button-export')).toBeInTheDocument();
    });
  });

  describe('Cancel Button', () => {
    it('should call closePopup when Cancel button is clicked', () => {
      render(<Command closePopup={mockClosePopup} />);

      const cancelButton = screen.getByText('Cancel');
      fireEvent.click(cancelButton);

      expect(mockClosePopup).toHaveBeenCalledTimes(1);
    });
  });

  describe('Save Button - Validation', () => {
    it('should show error when division has invalid format', async () => {
      mockHasInvalidDivision.mockReturnValue(true);

      render(<Command closePopup={mockClosePopup} />);

      const saveButton = screen.getByText('Save');
      fireEvent.click(saveButton);

      await waitFor(() => {
        expect(ToastrWrapper.showError).toHaveBeenCalledWith(
          'Invalid format for Pairwise Contains. Enter values as pairs like D1,D2. Each item must be separated by commas.'
        );
        expect(mockUpdateSettingService).not.toHaveBeenCalled();
      });
    });
  });

  describe('Save Button - League Group Change', () => {
    it('should show confirmation when leagueGroupId changes', async () => {
      const stateWithChangedGroup = {
        ...defaultStoreState,
        editSettingSlice: {
          ...defaultStoreState.editSettingSlice,
          leagueGroupId: 999,
          oldLeagueGroupId: 789
        }
      };
      mockStoreGetState.mockReturnValue(stateWithChangedGroup);
      (ToastrWrapper.show as jest.Mock).mockResolvedValue({ value: true });

      render(<Command closePopup={mockClosePopup} />);

      const saveButton = screen.getByText('Save');
      fireEvent.click(saveButton);

      await waitFor(() => {
        expect(ToastrWrapper.show).toHaveBeenCalled();
      });
    });

    it('should not save when user cancels league group change', async () => {
      const stateWithChangedGroup = {
        ...defaultStoreState,
        editSettingSlice: {
          ...defaultStoreState.editSettingSlice,
          leagueGroupId: 999,
          oldLeagueGroupId: 789
        }
      };
      mockStoreGetState.mockReturnValue(stateWithChangedGroup);
      (ToastrWrapper.show as jest.Mock).mockResolvedValue({ value: false });

      render(<Command closePopup={mockClosePopup} />);

      const saveButton = screen.getByText('Save');
      fireEvent.click(saveButton);

      await waitFor(() => {
        expect(mockUpdateSettingService).not.toHaveBeenCalled();
      });
    });
  });

  describe('Save Button - Success', () => {
    it('should call updateSettingService with correct data', async () => {
      render(<Command closePopup={mockClosePopup} />);

      const saveButton = screen.getByText('Save');
      fireEvent.click(saveButton);

      await waitFor(() => {
        expect(mockUpdateSettingService).toHaveBeenCalledWith({
          leagueSettingId: 123,
          league: 'Test League',
          owpLeagueId: 456,
          leagueGroupId: 789,
          leagueGroup: 'Test Group',
          spread: '1.5',
          maxbetWD_HDP: '1000',
          wD_NoOfHDP: '5',
          maxbetWK_HDP: '2000',
          wK_NoOfHDP: '10',
          lstNote: 'Note',
          remark2: 'Remark',
          hasTv: true,
          hasIpTv: false,
          remark: 'Test remark',
          round: [{ type: 'Contains', value: 'Round 1' }],
          division: [{ type: 'Equals', value: 'Div 1' }],
          isBlackList: false,
          noOfferLive: 0,
          comment: 'Comment'
        });
      });
    });

    it('should show success message and dispatch actions on successful save', async () => {
      render(<Command closePopup={mockClosePopup} />);

      const saveButton = screen.getByText('Save');
      fireEvent.click(saveButton);

      await waitFor(() => {
        expect(ToastrWrapper.showInfo).toHaveBeenCalledWith(
          'The setting has been edited successfully!'
        );
        expect(mockDispatch).toHaveBeenCalledWith(mockSetIsSaveAction(true));
        expect(mockDispatch).toHaveBeenCalledWith(
          mockFetchLeagueSettingInfo({}, false)
        );
        expect(mockDispatch).toHaveBeenCalledWith(mockSetFocusSettingId(123));
        expect(mockClosePopup).toHaveBeenCalled();
      });
    });

    it('should filter empty values from round and division', async () => {
      const stateWithEmptyValues = {
        ...defaultStoreState,
        editSettingSlice: {
          ...defaultStoreState.editSettingSlice,
          round: [
            { type: 'Contains', value: 'Round 1' },
            { type: 'Contains', value: '' }
          ],
          division: [{ type: 'Equals', value: '' }]
        }
      };
      mockStoreGetState.mockReturnValue(stateWithEmptyValues);

      render(<Command closePopup={mockClosePopup} />);

      const saveButton = screen.getByText('Save');
      fireEvent.click(saveButton);

      await waitFor(() => {
        expect(mockUpdateSettingService).toHaveBeenCalledWith(
          expect.objectContaining({
            round: [{ type: 'Contains', value: 'Round 1' }],
            division: []
          })
        );
      });
    });
  });

  describe('Save Button - Error Handling', () => {
    it('should handle 400 error with duplicate league name', async () => {
      const errorResponse = {
        response: {
          status: 400,
          data: {
            message: 'Duplicate league name',
            duplicatedLeagueName: 'Test League'
          }
        }
      };
      mockUpdateSettingService.mockRejectedValue(errorResponse);

      render(<Command closePopup={mockClosePopup} />);

      const saveButton = screen.getByText('Save');
      fireEvent.click(saveButton);

      await waitFor(() => {
        expect(ToastrWrapper.showDuplicateLeagueSetting).toHaveBeenCalledWith(
          'Duplicate league name',
          'Test League'
        );
        // Note: closePopup is not called on error in the component
      });
    });

    it('should handle other errors gracefully', async () => {
      mockUpdateSettingService.mockRejectedValue(new Error('Network error'));

      render(<Command closePopup={mockClosePopup} />);

      const saveButton = screen.getByText('Save');
      fireEvent.click(saveButton);

      await waitFor(() => {
        // Note: closePopup is not called on error in the component
        expect(mockUpdateSettingService).toHaveBeenCalled();
      });
    });
  });

  describe('Filter Reset After Data Change', () => {
    it('should dispatch setFilterChange when data changes and filters are active', async () => {
      const oldData = {
        leagueGroup: 'Old Group',
        condition: {
          hasTv: false,
          hasIpTv: false,
          round: [],
          division: [],
          remark: ''
        },
        spreadShow: '1.0',
        maxbetWK_HDP_Show: '1000',
        wK_NoOfHDP_Show: '5',
        maxbetWD_HDP_Show: '500',
        wD_NoOfHDP_Show: '3',
        comment: 'Old comment',
        remark2: 'Old remark',
        lstNote: 'Old note'
      };

      const stateWithOldData = {
        ...defaultStoreState,
        editSettingSlice: {
          ...defaultStoreState.editSettingSlice,
          oldData: oldData as any,
          leagueGroup: 'New Group',
          spread: '1.5'
        },
        leagueInfoTableSlice: {
          ...defaultStoreState.leagueInfoTableSlice,
          filterState: [
            { columnKey: 'League_Group' },
            { columnKey: 'Spread_HDP_OU' }
          ]
        }
      };
      mockStoreGetState.mockReturnValue(stateWithOldData);

      render(<Command closePopup={mockClosePopup} />);

      const saveButton = screen.getByText('Save');
      fireEvent.click(saveButton);

      await waitFor(() => {
        expect(mockDispatch).toHaveBeenCalledWith(
          mockSetFilterChange([
            { columnKey: 'League_Group', keyword: '' },
            { columnKey: 'Spread_HDP_OU', keyword: '' }
          ])
        );
      });
    });
  });
});
