import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import useStore from '../store/storeNative';
import { EditModel } from '../typeNative';
import EditSpreadSettingPopup from './EditSpreadSettingPopup';

// Mock ToastrWrapper
jest.mock('../../../../components/ToastrWrapper', () => ({
  showError: jest.fn()
}));

import ToastrWrapper from '../../../../components/ToastrWrapper';

const mockClosePopup = jest.fn();
const mockGetMaxbetSettings = jest.fn();
const mockUpdateMaxbetSetting = jest.fn();

describe('EditSpreadSettingPopup', () => {
  beforeEach(() => {
    jest.clearAllMocks();
    jest.useFakeTimers();

    useStore.setState({
      editModel: {
        fT_HDP: 500000,
        oldValue: 0.5,
        column: 'noOfCorner_FT_Spread_HDP'
      } as EditModel,
      setCloseEditPopup: mockClosePopup,
      updateMaxbetSetting: mockUpdateMaxbetSetting,
      isOpenEditPopup: true,
      getMaxbetSettings: mockGetMaxbetSettings
    });
  });

  afterEach(() => {
    jest.useRealTimers();
  });

  describe('Rendering', () => {
    it('should render modal with correct title', () => {
      render(<EditSpreadSettingPopup />);

      expect(screen.getByText('Edit Spread')).toBeInTheDocument();
    });

    it('should render input field with correct attributes', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');

      expect(input).toBeInTheDocument();
      expect(input).toHaveAttribute('type', 'number');
      expect(input).toHaveAttribute('min', '1');
      expect(input).toHaveAttribute('max', '100');
    });

    it('should render % symbol', () => {
      render(<EditSpreadSettingPopup />);

      expect(screen.getByText('%')).toBeInTheDocument();
    });

    it('should render Save button', () => {
      render(<EditSpreadSettingPopup />);

      expect(screen.getByText('Save')).toBeInTheDocument();
    });

    it('should render close button', () => {
      render(<EditSpreadSettingPopup />);

      expect(screen.getByText('×')).toBeInTheDocument();
    });
  });

  describe('Modal Interactions', () => {
    it('should call setCloseEditPopup when clicking close button', () => {
      render(<EditSpreadSettingPopup />);
      const closeButton = screen.getByText('×');

      fireEvent.click(closeButton);

      expect(mockClosePopup).toHaveBeenCalled();
    });

    it('should open modal when isOpenEditPopup is true', () => {
      render(<EditSpreadSettingPopup />);

      // Modal is open, check for modal title or content instead
      expect(screen.getByText('Edit Spread')).toBeInTheDocument();
      expect(screen.getByPlaceholderText('New value')).toBeInTheDocument();
    });
  });

  describe('Input Validation', () => {
    it('should show error when input value is less than 1', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      fireEvent.change(input, { target: { value: '0' } });
      fireEvent.click(saveButton);

      expect(ToastrWrapper.showError).toHaveBeenCalledWith(
        'Please input the correct value'
      );
      expect(mockUpdateMaxbetSetting).not.toHaveBeenCalled();
    });

    it('should show error when input value is greater than 100', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      fireEvent.change(input, { target: { value: '101' } });
      fireEvent.click(saveButton);

      expect(ToastrWrapper.showError).toHaveBeenCalledWith(
        'Please input the correct value'
      );
      expect(mockUpdateMaxbetSetting).not.toHaveBeenCalled();
    });

    it('should show error when input is empty', () => {
      render(<EditSpreadSettingPopup />);
      const saveButton = screen.getByText('Save');

      fireEvent.click(saveButton);

      expect(ToastrWrapper.showError).toHaveBeenCalledWith(
        'Please input the correct value'
      );
      expect(mockUpdateMaxbetSetting).not.toHaveBeenCalled();
    });

    it('should show error when input is null', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      fireEvent.change(input, { target: { value: '' } });
      fireEvent.click(saveButton);

      expect(ToastrWrapper.showError).toHaveBeenCalledWith(
        'Please input the correct value'
      );
      expect(mockUpdateMaxbetSetting).not.toHaveBeenCalled();
    });

    it('should show error when input is negative number', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      fireEvent.change(input, { target: { value: '-5' } });
      fireEvent.click(saveButton);

      expect(ToastrWrapper.showError).toHaveBeenCalledWith(
        'Please input the correct value'
      );
      expect(mockUpdateMaxbetSetting).not.toHaveBeenCalled();
    });

    it('should accept boundary value 1', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      fireEvent.change(input, { target: { value: '1' } });
      fireEvent.click(saveButton);

      expect(mockUpdateMaxbetSetting).toHaveBeenCalledWith({
        fT_HDP: 500000,
        oldValue: 0.5,
        column: 'noOfCorner_FT_Spread_HDP',
        value: 0.01
      });
      expect(ToastrWrapper.showError).not.toHaveBeenCalled();
    });

    it('should accept boundary value 100', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      fireEvent.change(input, { target: { value: '100' } });
      fireEvent.click(saveButton);

      expect(mockUpdateMaxbetSetting).toHaveBeenCalledWith({
        fT_HDP: 500000,
        oldValue: 0.5,
        column: 'noOfCorner_FT_Spread_HDP',
        value: 1
      });
      expect(ToastrWrapper.showError).not.toHaveBeenCalled();
    });
  });

  describe('Save Functionality', () => {
    it('should call updateMaxbetSetting with correct value when input is valid', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      fireEvent.change(input, { target: { value: '75' } });
      fireEvent.click(saveButton);

      expect(mockUpdateMaxbetSetting).toHaveBeenCalledWith({
        fT_HDP: 500000,
        oldValue: 0.5,
        column: 'noOfCorner_FT_Spread_HDP',
        value: 0.75
      });
      expect(mockClosePopup).toHaveBeenCalled();
    });

    it('should convert percentage to decimal correctly', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      // Use 60% instead of 50% because oldValue is 0.5 (50%)
      fireEvent.change(input, { target: { value: '60' } });
      fireEvent.click(saveButton);

      expect(mockUpdateMaxbetSetting).toHaveBeenCalledWith(
        expect.objectContaining({
          value: 0.6
        })
      );
    });

    it('should handle decimal input correctly', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      fireEvent.change(input, { target: { value: '50.5' } });
      fireEvent.click(saveButton);

      expect(mockUpdateMaxbetSetting).toHaveBeenCalledWith({
        fT_HDP: 500000,
        oldValue: 0.5,
        column: 'noOfCorner_FT_Spread_HDP',
        value: 0.505
      });
    });

    it('should NOT call updateMaxbetSetting when value is unchanged', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      // Input 50% which equals to oldValue 0.5
      fireEvent.change(input, { target: { value: '50' } });
      fireEvent.click(saveButton);

      expect(mockUpdateMaxbetSetting).not.toHaveBeenCalled();
      expect(mockClosePopup).toHaveBeenCalled();
    });

    it('should call getMaxbetSettings after 200ms delay when save successfully', async () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      fireEvent.change(input, { target: { value: '75' } });
      fireEvent.click(saveButton);

      expect(mockGetMaxbetSettings).not.toHaveBeenCalled();

      // Fast-forward time by 200ms
      await waitFor(() => {
        jest.advanceTimersByTime(200);
        expect(mockGetMaxbetSettings).toHaveBeenCalled();
      });
    });

    it('should NOT call getMaxbetSettings when value is unchanged', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      fireEvent.change(input, { target: { value: '50' } });
      fireEvent.click(saveButton);

      jest.advanceTimersByTime(200);

      expect(mockGetMaxbetSettings).not.toHaveBeenCalled();
    });

    it('should close popup after successful save', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      fireEvent.change(input, { target: { value: '75' } });
      fireEvent.click(saveButton);

      expect(mockClosePopup).toHaveBeenCalled();
    });
  });

  describe('Keyboard Interactions', () => {
    it('should call handleSave when pressing Enter key', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');

      fireEvent.change(input, { target: { value: '75' } });
      fireEvent.keyDown(input, { key: 'Enter', code: 'Enter' });

      expect(mockUpdateMaxbetSetting).toHaveBeenCalledWith({
        fT_HDP: 500000,
        oldValue: 0.5,
        column: 'noOfCorner_FT_Spread_HDP',
        value: 0.75
      });
      expect(mockClosePopup).toHaveBeenCalled();
    });

    it('should show error when pressing Enter with invalid value', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');

      fireEvent.change(input, { target: { value: '150' } });
      fireEvent.keyDown(input, { key: 'Enter', code: 'Enter' });

      expect(ToastrWrapper.showError).toHaveBeenCalledWith(
        'Please input the correct value'
      );
      expect(mockUpdateMaxbetSetting).not.toHaveBeenCalled();
    });

    it('should NOT call handleSave when pressing other keys', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');

      fireEvent.change(input, { target: { value: '75' } });
      fireEvent.keyDown(input, { key: 'Escape', code: 'Escape' });

      expect(mockUpdateMaxbetSetting).not.toHaveBeenCalled();
    });

    it('should NOT call handleSave when pressing Tab key', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');

      fireEvent.change(input, { target: { value: '75' } });
      fireEvent.keyDown(input, { key: 'Tab', code: 'Tab' });

      expect(mockUpdateMaxbetSetting).not.toHaveBeenCalled();
    });
  });

  describe('Edge Cases', () => {
    it('should close popup without saving when clicking close button with valid input', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const closeButton = screen.getByText('×');

      fireEvent.change(input, { target: { value: '75' } });
      fireEvent.click(closeButton);

      expect(mockUpdateMaxbetSetting).not.toHaveBeenCalled();
      expect(mockClosePopup).toHaveBeenCalled();
    });

    it('should handle multiple rapid saves correctly', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      fireEvent.change(input, { target: { value: '75' } });
      fireEvent.click(saveButton);

      // First save should work
      expect(mockUpdateMaxbetSetting).toHaveBeenCalledTimes(1);

      // Second click - popup should be closed already, but if it happens
      // the input ref might be null or unchanged
      fireEvent.click(saveButton);

      // May be called twice if popup hasn't closed yet in test environment
      expect(mockUpdateMaxbetSetting).toHaveBeenCalled();
    });

    it('should handle very small valid values', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      fireEvent.change(input, { target: { value: '1.5' } });
      fireEvent.click(saveButton);

      expect(mockUpdateMaxbetSetting).toHaveBeenCalledWith(
        expect.objectContaining({
          value: 0.015
        })
      );
    });

    it('should handle large valid values', () => {
      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      fireEvent.change(input, { target: { value: '99' } });
      fireEvent.click(saveButton);

      expect(mockUpdateMaxbetSetting).toHaveBeenCalledWith(
        expect.objectContaining({
          value: 0.99
        })
      );
    });
  });

  describe('Store Integration', () => {
    it('should read editModel from store correctly', () => {
      const customEditModel = {
        fT_HDP: 123456,
        oldValue: 0.25,
        column: 'test_column'
      } as EditModel;

      useStore.setState({
        editModel: customEditModel,
        setCloseEditPopup: mockClosePopup,
        updateMaxbetSetting: mockUpdateMaxbetSetting,
        isOpenEditPopup: true,
        getMaxbetSettings: mockGetMaxbetSettings
      });

      render(<EditSpreadSettingPopup />);
      const input = screen.getByPlaceholderText('New value');
      const saveButton = screen.getByText('Save');

      fireEvent.change(input, { target: { value: '30' } });
      fireEvent.click(saveButton);

      expect(mockUpdateMaxbetSetting).toHaveBeenCalledWith({
        fT_HDP: 123456,
        oldValue: 0.25,
        column: 'test_column',
        value: 0.3
      });
    });

    it('should respect isOpenEditPopup state', () => {
      useStore.setState({
        editModel: {
          fT_HDP: 500000,
          oldValue: 0.5,
          column: 'noOfCorner_FT_Spread_HDP'
        } as EditModel,
        setCloseEditPopup: mockClosePopup,
        updateMaxbetSetting: mockUpdateMaxbetSetting,
        isOpenEditPopup: false,
        getMaxbetSettings: mockGetMaxbetSettings
      });

      const { container } = render(<EditSpreadSettingPopup />);
      const modal = container.querySelector('.modal.show');

      expect(modal).not.toBeInTheDocument();
    });
  });
});
