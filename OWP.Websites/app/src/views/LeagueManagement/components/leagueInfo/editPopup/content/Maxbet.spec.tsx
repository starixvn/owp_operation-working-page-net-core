import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import Maxbet from './Maxbet';
import { useAppDispatch } from '../../app/hooks';
import { updateObject } from '../slice/editSettingSlice';

jest.mock('../../app/hooks', () => ({
  useAppDispatch: jest.fn()
}));

jest.mock('../slice/editSettingSlice', () => ({
  updateObject: jest.fn()
}));

jest.mock('../../component/InputItem', () => ({
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

const mockDispatch = jest.fn();
const mockUseAppDispatch = useAppDispatch as jest.Mock;
const mockUpdateObject = updateObject as jest.Mock;

describe('Maxbet Component', () => {
  const defaultProps = {
    maxbetWD_HDP: '1000',
    wD_NoOfHDP: '5',
    maxbetWK_HDP: '2000',
    wK_NoOfHDP: '10'
  };

  beforeEach(() => {
    jest.clearAllMocks();
    mockUseAppDispatch.mockReturnValue(mockDispatch);
    mockUpdateObject.mockReturnValue({ type: 'updateObject' });
  });

  describe('Rendering', () => {
    it('should render title correctly', () => {
      render(<Maxbet {...defaultProps} />);

      expect(screen.getByText('MB')).toBeInTheDocument();
    });

    it('should render all four input fields', () => {
      render(<Maxbet {...defaultProps} />);

      expect(screen.getByText('Weekday HDP (K)')).toBeInTheDocument();
      expect(screen.getByText('Weekday NO. HDP')).toBeInTheDocument();
      expect(screen.getByText('Weekend HDP (K)')).toBeInTheDocument();
      expect(screen.getByText('Weekend NO. HDP')).toBeInTheDocument();
    });

    it('should display all values correctly', () => {
      render(<Maxbet {...defaultProps} />);

      expect(screen.getByTestId('input-Weekday HDP (K)')).toHaveValue('1000');
      expect(screen.getByTestId('input-Weekday NO. HDP')).toHaveValue('5');
      expect(screen.getByTestId('input-Weekend HDP (K)')).toHaveValue('2000');
      expect(screen.getByTestId('input-Weekend NO. HDP')).toHaveValue('10');
    });
  });

  describe('Input Changes', () => {
    it('should dispatch updateObject for maxbetWD_HDP when changed', () => {
      render(<Maxbet {...defaultProps} />);

      const input = screen.getByTestId('input-Weekday HDP (K)');
      fireEvent.change(input, { target: { value: '1500' } });

      expect(mockDispatch).toHaveBeenCalled();
      expect(mockUpdateObject).toHaveBeenCalledWith({
        property: 'maxbetWD_HDP',
        value: '1500'
      });
    });

    it('should dispatch updateObject for wD_NoOfHDP when changed', () => {
      render(<Maxbet {...defaultProps} />);

      const input = screen.getByTestId('input-Weekday NO. HDP');
      fireEvent.change(input, { target: { value: '8' } });

      expect(mockDispatch).toHaveBeenCalled();
      expect(mockUpdateObject).toHaveBeenCalledWith({
        property: 'wD_NoOfHDP',
        value: '8'
      });
    });

    it('should dispatch updateObject for maxbetWK_HDP when changed', () => {
      render(<Maxbet {...defaultProps} />);

      const input = screen.getByTestId('input-Weekend HDP (K)');
      fireEvent.change(input, { target: { value: '2500' } });

      expect(mockDispatch).toHaveBeenCalled();
      expect(mockUpdateObject).toHaveBeenCalledWith({
        property: 'maxbetWK_HDP',
        value: '2500'
      });
    });

    it('should dispatch updateObject for wK_NoOfHDP when changed', () => {
      render(<Maxbet {...defaultProps} />);

      const input = screen.getByTestId('input-Weekend NO. HDP');
      fireEvent.change(input, { target: { value: '15' } });

      expect(mockDispatch).toHaveBeenCalled();
      expect(mockUpdateObject).toHaveBeenCalledWith({
        property: 'wK_NoOfHDP',
        value: '15'
      });
    });

    it('should NOT dispatch when value is unchanged', () => {
      render(<Maxbet {...defaultProps} />);

      const input = screen.getByTestId('input-Weekday HDP (K)');
      fireEvent.change(input, { target: { value: '1000' } });

      expect(mockDispatch).not.toHaveBeenCalled();
    });
  });

  describe('Props Variations', () => {
    it('should handle empty string values', () => {
      render(
        <Maxbet maxbetWD_HDP='' wD_NoOfHDP='' maxbetWK_HDP='' wK_NoOfHDP='' />
      );

      expect(screen.getByTestId('input-Weekday HDP (K)')).toHaveValue('');
      expect(screen.getByTestId('input-Weekday NO. HDP')).toHaveValue('');
      expect(screen.getByTestId('input-Weekend HDP (K)')).toHaveValue('');
      expect(screen.getByTestId('input-Weekend NO. HDP')).toHaveValue('');
    });

    it('should handle zero values', () => {
      render(
        <Maxbet
          maxbetWD_HDP='0'
          wD_NoOfHDP='0'
          maxbetWK_HDP='0'
          wK_NoOfHDP='0'
        />
      );

      expect(screen.getByTestId('input-Weekday HDP (K)')).toHaveValue('0');
    });
  });
});
