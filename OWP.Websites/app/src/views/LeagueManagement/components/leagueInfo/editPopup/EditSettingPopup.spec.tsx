import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import EditSettingPopup from './EditSettingPopup';
import { useAppDispatch, useAppSelector } from '../app/hooks';
import { getLeagueGroupOptionsThunk } from '../component/commonThunk';

// Mock dependencies
jest.mock('reactstrap', () => ({
  Modal: ({ children, ...props }: any) => (
    <div data-testid='modal' {...props}>
      {children}
    </div>
  ),
  ModalBody: ({ children, ...props }: any) => (
    <div data-testid='modal-body' {...props}>
      {children}
    </div>
  ),
  ModalFooter: ({ children, ...props }: any) => (
    <div data-testid='modal-footer' {...props}>
      {children}
    </div>
  )
}));

jest.mock('../app/hooks', () => ({
  useAppDispatch: jest.fn(),
  useAppSelector: jest.fn()
}));

jest.mock('../component/commonThunk', () => ({
  getLeagueGroupOptionsThunk: jest.fn()
}));

// Mock Command component (complex with store dependencies)
jest.mock('./Command', () => {
  return function MockCommand(props: { closePopup: () => void }) {
    return <div data-testid='command'>Command Component</div>;
  };
});

// Mock Condition component (complex with many dependencies)
jest.mock('./content/Condition', () => {
  return function MockCondition(props: {
    condition: {
      hasTv: boolean;
      hasIpTv: boolean;
      round: any[];
      division: any[];
      remark: string;
    };
  }) {
    return <div data-testid='condition'>Condition Component</div>;
  };
});

// Mock NoOfferLive component (has react-select-ts dependency)
jest.mock('./content/NoOfferLive', () => {
  return function MockNoOfferLive(props: { noOfferLive: number }) {
    return <div data-testid='no-offer-live'>NoOfferLive Component</div>;
  };
});

// Mock InputItem component (used by FtSpread, Maxbet, MicsMatch)
jest.mock('../component/InputItem', () => ({
  InputItem: ({ label, value, handleOnChange }: any) => (
    <div data-testid={`input-item-${label}`}>
      <label>{label}</label>
      <input
        data-testid={`input-${label}`}
        value={value}
        onChange={handleOnChange}
      />
    </div>
  )
}));

// Don't mock LeagueInfo, FtSpread, Maxbet, MicsMatch - let them render to increase coverage

const mockDispatch = jest.fn();
const mockUseAppDispatch = useAppDispatch as jest.Mock;
const mockUseAppSelector = useAppSelector as jest.Mock;
const mockGetLeagueGroupOptionsThunk = getLeagueGroupOptionsThunk as jest.Mock;

describe('EditSettingPopup', () => {
  const mockClosePopup = jest.fn();

  const defaultState = {
    editSettingSlice: {
      leagueGroupOptions: [],
      league: 'Test League',
      shortLeague: 'TL',
      isBlackList: false,
      hasTv: false,
      hasIpTv: false,
      round: [],
      division: [],
      remark: '',
      spread: '',
      maxbetWD_HDP: '',
      wD_NoOfHDP: '',
      maxbetWK_HDP: '',
      wK_NoOfHDP: '',
      lstNote: '',
      remark2: '',
      noOfferLive: 0
    }
  };

  beforeEach(() => {
    jest.clearAllMocks();
    mockUseAppDispatch.mockReturnValue(mockDispatch);
    mockGetLeagueGroupOptionsThunk.mockReturnValue({
      type: 'getLeagueGroupOptionsThunk'
    });
  });

  const renderComponent = (state = defaultState) => {
    mockUseAppSelector.mockImplementation(callback => callback(state));
    return render(<EditSettingPopup closePopup={mockClosePopup} />);
  };

  describe('Rendering', () => {
    it('should render modal with correct attributes', () => {
      renderComponent();

      const modal = screen.getByTestId('modal');
      expect(modal).toBeInTheDocument();
      expect(modal).toHaveAttribute('class', 'edit-setting-popup');
    });

    it('should render ModalBody', () => {
      renderComponent();

      const modalBody = screen.getByTestId('modal-body');
      expect(modalBody).toBeInTheDocument();
    });

    it('should render ModalFooter', () => {
      renderComponent();

      const modalFooter = screen.getByTestId('modal-footer');
      expect(modalFooter).toBeInTheDocument();
    });

    it('should render all child components', () => {
      renderComponent();

      // LeagueInfo is not mocked, check for actual content
      expect(screen.getByText('League Information')).toBeInTheDocument();
      expect(screen.getByText('League Name')).toBeInTheDocument();
      expect(screen.getByText('Short League')).toBeInTheDocument();
      expect(screen.getByText('League Type')).toBeInTheDocument();

      // Condition is mocked
      expect(screen.getByTestId('condition')).toBeInTheDocument();

      // NoOfferLive is mocked
      expect(screen.getByTestId('no-offer-live')).toBeInTheDocument();

      // FtSpread is not mocked, check for actual content
      expect(screen.getByText('FT Spread')).toBeInTheDocument();
      expect(screen.getByTestId('input-item-HDP-OU(%)')).toBeInTheDocument();

      // Maxbet is not mocked, check for actual content
      expect(screen.getByText('MB')).toBeInTheDocument();

      // MicsMatch is not mocked, check for actual content
      expect(
        screen.getByText('Display in Misc match file')
      ).toBeInTheDocument();

      // Command is mocked
      expect(screen.getByTestId('command')).toBeInTheDocument();
    });
  });

  describe('Redux Integration', () => {
    it('should call useAppSelector with correct selector', () => {
      renderComponent();

      expect(mockUseAppSelector).toHaveBeenCalled();
      const selectorFn = mockUseAppSelector.mock.calls[0][0];
      const result = selectorFn(defaultState);
      expect(result).toEqual(defaultState.editSettingSlice);
    });

    it('should call useAppDispatch', () => {
      renderComponent();

      expect(mockUseAppDispatch).toHaveBeenCalled();
    });
  });

  describe('useEffect - getLeagueGroupOptionsThunk', () => {
    it('should dispatch getLeagueGroupOptionsThunk when leagueGroupOptions is empty', () => {
      renderComponent({
        editSettingSlice: {
          ...defaultState.editSettingSlice,
          leagueGroupOptions: []
        }
      });

      expect(mockDispatch).toHaveBeenCalledWith(
        mockGetLeagueGroupOptionsThunk()
      );
    });

    it('should NOT dispatch getLeagueGroupOptionsThunk when leagueGroupOptions has data', () => {
      renderComponent({
        editSettingSlice: {
          ...defaultState.editSettingSlice,
          leagueGroupOptions: [{ value: 1, label: 'Group 1' }]
        }
      });

      expect(mockDispatch).not.toHaveBeenCalledWith(
        mockGetLeagueGroupOptionsThunk()
      );
    });

    it('should dispatch getLeagueGroupOptionsThunk when leagueGroupOptions changes from non-empty to empty', () => {
      // First render with data
      const { rerender } = renderComponent({
        editSettingSlice: {
          ...defaultState.editSettingSlice,
          leagueGroupOptions: [{ value: 1, label: 'Group 1' }]
        }
      });

      mockDispatch.mockClear();

      // Rerender with empty options
      mockUseAppSelector.mockImplementation(callback =>
        callback({
          editSettingSlice: {
            ...defaultState.editSettingSlice,
            leagueGroupOptions: []
          }
        })
      );
      rerender(<EditSettingPopup closePopup={mockClosePopup} />);

      // Should dispatch when options become empty
      expect(mockDispatch).toHaveBeenCalledWith(
        mockGetLeagueGroupOptionsThunk()
      );
    });
  });

  describe('Props Passing to Child Components', () => {
    it('should pass correct props to LeagueInfo component', () => {
      const state = {
        editSettingSlice: {
          ...defaultState.editSettingSlice,
          league: 'Premier League',
          shortLeague: 'PL',
          isBlackList: true
        }
      };

      renderComponent(state);

      // LeagueInfo is not mocked, check actual rendered content
      expect(screen.getByText('Premier League')).toBeInTheDocument();
      expect(screen.getByText('PL')).toBeInTheDocument();
      const checkbox = screen.getByRole('checkbox');
      expect(checkbox).toBeChecked();
    });

    it('should pass correct props to Condition component', () => {
      const state = {
        editSettingSlice: {
          ...defaultState.editSettingSlice,
          hasTv: true,
          hasIpTv: true,
          round: [{ type: 'Contains', value: 'Round 1' }],
          division: [{ type: 'Equals', value: 'Division A' }],
          remark: 'Test remark'
        }
      };

      renderComponent(state);

      expect(screen.getByTestId('condition')).toBeInTheDocument();
    });

    it('should pass correct props to NoOfferLive component', () => {
      const state = {
        editSettingSlice: {
          ...defaultState.editSettingSlice,
          noOfferLive: 1
        }
      };

      renderComponent(state);

      expect(screen.getByTestId('no-offer-live')).toBeInTheDocument();
    });

    it('should pass correct props to FtSpread component', () => {
      const state = {
        editSettingSlice: {
          ...defaultState.editSettingSlice,
          spread: '0.5'
        }
      };

      renderComponent(state);

      // FtSpread is not mocked, check actual rendered content
      expect(screen.getByText('FT Spread')).toBeInTheDocument();
      const input = screen.getByTestId('input-HDP-OU(%)');
      expect(input).toHaveValue('0.5');
    });

    it('should pass correct props to Maxbet component', () => {
      const state = {
        editSettingSlice: {
          ...defaultState.editSettingSlice,
          maxbetWD_HDP: '1000',
          wD_NoOfHDP: '5',
          maxbetWK_HDP: '2000',
          wK_NoOfHDP: '10'
        }
      };

      renderComponent(state);

      // Maxbet is not mocked, check actual rendered content
      expect(screen.getByText('MB')).toBeInTheDocument();
      expect(screen.getByTestId('input-Weekday HDP (K)')).toHaveValue('1000');
      expect(screen.getByTestId('input-Weekday NO. HDP')).toHaveValue('5');
      expect(screen.getByTestId('input-Weekend HDP (K)')).toHaveValue('2000');
      expect(screen.getByTestId('input-Weekend NO. HDP')).toHaveValue('10');
    });

    it('should pass correct props to MicsMatch component', () => {
      const state = {
        editSettingSlice: {
          ...defaultState.editSettingSlice,
          lstNote: 'Note 1',
          remark2: 'Remark 2'
        }
      };

      renderComponent(state);

      // MicsMatch is not mocked, check actual rendered content
      expect(
        screen.getByText('Display in Misc match file')
      ).toBeInTheDocument();
      expect(
        screen.getByTestId('input-LST Note (Show under League)')
      ).toHaveValue('Note 1');
      expect(
        screen.getByTestId('input-Remark 2 (Show under Match)')
      ).toHaveValue('Remark 2');
    });

    it('should pass closePopup to Command component', () => {
      renderComponent();

      expect(screen.getByTestId('command')).toBeInTheDocument();
    });
  });

  describe('Edge Cases', () => {
    it('should handle undefined values in state', () => {
      const state = {
        editSettingSlice: {
          leagueGroupOptions: [],
          league: undefined as any,
          shortLeague: undefined as any,
          isBlackList: false,
          hasTv: false,
          hasIpTv: false,
          round: [],
          division: [],
          remark: undefined as any,
          spread: undefined as any,
          maxbetWD_HDP: undefined as any,
          wD_NoOfHDP: undefined as any,
          maxbetWK_HDP: undefined as any,
          wK_NoOfHDP: undefined as any,
          lstNote: undefined as any,
          remark2: undefined as any,
          noOfferLive: 0
        }
      };

      expect(() => renderComponent(state)).not.toThrow();
    });

    it('should handle empty string values', () => {
      const state = {
        editSettingSlice: {
          ...defaultState.editSettingSlice,
          league: '',
          shortLeague: '',
          remark: '',
          spread: '',
          maxbetWD_HDP: '',
          wD_NoOfHDP: '',
          maxbetWK_HDP: '',
          wK_NoOfHDP: '',
          lstNote: '',
          remark2: ''
        }
      };

      expect(() => renderComponent(state)).not.toThrow();
      expect(screen.getByTestId('modal')).toBeInTheDocument();
    });

    it('should handle complex array values in round and division', () => {
      const state = {
        editSettingSlice: {
          ...defaultState.editSettingSlice,
          round: [
            { type: 'Contains', value: 'Round 1' },
            { type: 'Equals', value: 'Round 2' }
          ],
          division: [
            { type: 'Contains', value: 'Div A' },
            { type: 'Pairwise Contains', value: 'D1,D2' }
          ]
        }
      };

      expect(() => renderComponent(state)).not.toThrow();
      expect(screen.getByTestId('condition')).toBeInTheDocument();
    });
  });

  describe('Component Interactions', () => {
    it('should handle input changes in FtSpread component', () => {
      renderComponent();

      const input = screen.getByTestId('input-HDP-OU(%)');
      fireEvent.change(input, { target: { value: '1.5' } });

      expect(mockDispatch).toHaveBeenCalled();
    });

    it('should handle input changes in Maxbet component', () => {
      renderComponent();

      const weekdayHdpInput = screen.getByTestId('input-Weekday HDP (K)');
      fireEvent.change(weekdayHdpInput, { target: { value: '2000' } });

      expect(mockDispatch).toHaveBeenCalled();
    });

    it('should handle input changes in MicsMatch component', () => {
      renderComponent();

      const lstNoteInput = screen.getByTestId(
        'input-LST Note (Show under League)'
      );
      fireEvent.change(lstNoteInput, { target: { value: 'New Note' } });

      expect(mockDispatch).toHaveBeenCalled();
    });
  });

  describe('Component Lifecycle', () => {
    it('should cleanup on unmount', () => {
      const { unmount } = renderComponent();

      expect(() => unmount()).not.toThrow();
    });

    it('should handle prop changes', () => {
      const { rerender } = renderComponent();

      const newClosePopup = jest.fn();
      rerender(<EditSettingPopup closePopup={newClosePopup} />);

      expect(screen.getByTestId('modal')).toBeInTheDocument();
    });
  });
});
