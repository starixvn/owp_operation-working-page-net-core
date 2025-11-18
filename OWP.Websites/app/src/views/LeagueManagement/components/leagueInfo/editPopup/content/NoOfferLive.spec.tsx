import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import NoOfferLive from './NoOfferLive';
import { useAppDispatch } from '../../app/hooks';
import { updateObject } from '../slice/editSettingSlice';

jest.mock('../../app/hooks', () => ({
  useAppDispatch: jest.fn()
}));

jest.mock('../slice/editSettingSlice', () => ({
  updateObject: jest.fn()
}));

jest.mock('react-select-ts', () => {
  return function MockSelect({ options, value, onChange }: any) {
    return (
      <select
        data-testid='no-offer-live-select'
        value={value?.value?.toString() || ''}
        onChange={e => {
          const selected = options.find(
            (opt: any) => opt.value.toString() === e.target.value
          );
          onChange(selected || null);
        }}>
        <option value=''>Select...</option>
        {options.map((option: any) => (
          <option key={option.value} value={option.value.toString()}>
            {option.label}
          </option>
        ))}
      </select>
    );
  };
});

jest.mock('../../../../constants/MaxbetConst', () => ({
  NoOfferLive: [
    { value: 0, label: 'No' },
    { value: 1, label: 'Yes' }
  ]
}));

const mockDispatch = jest.fn();
const mockUseAppDispatch = useAppDispatch as jest.Mock;
const mockUpdateObject = updateObject as jest.Mock;

describe('NoOfferLive Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
    mockUseAppDispatch.mockReturnValue(mockDispatch);
    mockUpdateObject.mockReturnValue({ type: 'updateObject' });
  });

  describe('Rendering', () => {
    it('should render title correctly', () => {
      render(<NoOfferLive noOfferLive={0} />);

      expect(screen.getByText('No Offer Live')).toBeInTheDocument();
    });

    it('should render Select component', () => {
      render(<NoOfferLive noOfferLive={0} />);

      expect(screen.getByTestId('no-offer-live-select')).toBeInTheDocument();
    });

    it('should display selected value correctly', () => {
      render(<NoOfferLive noOfferLive={1} />);

      const select = screen.getByTestId('no-offer-live-select');
      expect(select).toHaveValue('1');
    });
  });

  describe('Selection Changes', () => {
    it('should dispatch updateObject when value changes', () => {
      render(<NoOfferLive noOfferLive={0} />);

      const select = screen.getByTestId('no-offer-live-select');
      fireEvent.change(select, { target: { value: '1' } });

      expect(mockDispatch).toHaveBeenCalled();
      expect(mockUpdateObject).toHaveBeenCalledWith({
        property: 'noOfferLive',
        value: 1
      });
    });

    it('should dispatch with value 0 when null is selected', () => {
      render(<NoOfferLive noOfferLive={1} />);

      const select = screen.getByTestId('no-offer-live-select');
      fireEvent.change(select, { target: { value: '' } });

      expect(mockDispatch).toHaveBeenCalled();
      expect(mockUpdateObject).toHaveBeenCalledWith({
        property: 'noOfferLive',
        value: 0
      });
    });
  });

  describe('Props Variations', () => {
    it('should handle value 0', () => {
      render(<NoOfferLive noOfferLive={0} />);

      const select = screen.getByTestId('no-offer-live-select');
      expect(select).toHaveValue('0');
    });

    it('should handle value 0 as No option', () => {
      render(<NoOfferLive noOfferLive={0} />);

      // When noOfferLive=0, it should find the option with value=0 (No)
      // The select should show "0" as the value
      const select = screen.getByTestId('no-offer-live-select');
      expect(select).toHaveValue('0');
    });

    it('should handle value 1', () => {
      render(<NoOfferLive noOfferLive={1} />);

      const select = screen.getByTestId('no-offer-live-select');
      expect(select).toHaveValue('1');
    });

    it('should handle value not in options', () => {
      render(<NoOfferLive noOfferLive={99} />);

      const select = screen.getByTestId('no-offer-live-select');
      expect(select).toHaveValue('');
    });
  });
});
