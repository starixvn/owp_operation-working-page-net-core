import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import FtSpread from './FtSpread';
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

describe('FtSpread Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
    mockUseAppDispatch.mockReturnValue(mockDispatch);
    mockUpdateObject.mockReturnValue({ type: 'updateObject' });
  });

  describe('Rendering', () => {
    it('should render title correctly', () => {
      render(<FtSpread spread='0.5' />);

      expect(screen.getByText('FT Spread')).toBeInTheDocument();
    });

    it('should render InputItem with correct label', () => {
      render(<FtSpread spread='0.5' />);

      expect(screen.getByText('HDP-OU(%)')).toBeInTheDocument();
      expect(screen.getByTestId('input-item-HDP-OU(%)')).toBeInTheDocument();
    });

    it('should display current spread value', () => {
      render(<FtSpread spread='1.5' />);

      const input = screen.getByTestId('input-HDP-OU(%)');
      expect(input).toHaveValue('1.5');
    });
  });

  describe('Input Changes', () => {
    it('should dispatch updateObject when value changes', () => {
      render(<FtSpread spread='0.5' />);

      const input = screen.getByTestId('input-HDP-OU(%)');
      fireEvent.change(input, { target: { value: '1.5' } });

      expect(mockDispatch).toHaveBeenCalled();
      expect(mockUpdateObject).toHaveBeenCalledWith({
        property: 'spread',
        value: '1.5'
      });
    });

    it('should NOT dispatch when value is unchanged', () => {
      render(<FtSpread spread='0.5' />);

      const input = screen.getByTestId('input-HDP-OU(%)');
      fireEvent.change(input, { target: { value: '0.5' } });

      expect(mockDispatch).not.toHaveBeenCalled();
    });

    it('should handle empty string value', () => {
      render(<FtSpread spread='' />);

      const input = screen.getByTestId('input-HDP-OU(%)');
      fireEvent.change(input, { target: { value: '1.0' } });

      expect(mockDispatch).toHaveBeenCalled();
    });
  });

  describe('Props Variations', () => {
    it('should handle different spread values', () => {
      const { rerender } = render(<FtSpread spread='0.5' />);
      expect(screen.getByTestId('input-HDP-OU(%)')).toHaveValue('0.5');

      rerender(<FtSpread spread='2.0' />);
      expect(screen.getByTestId('input-HDP-OU(%)')).toHaveValue('2.0');
    });

    it('should handle decimal values', () => {
      render(<FtSpread spread='0.75' />);

      const input = screen.getByTestId('input-HDP-OU(%)');
      expect(input).toHaveValue('0.75');
    });
  });
});
