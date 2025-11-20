import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import MicsMatch from './MicsMatch';
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

describe('MicsMatch Component', () => {
  const defaultProps = {
    lstNote: 'Test Note',
    remark2: 'Test Remark'
  };

  beforeEach(() => {
    jest.clearAllMocks();
    mockUseAppDispatch.mockReturnValue(mockDispatch);
    mockUpdateObject.mockReturnValue({ type: 'updateObject' });
  });

  describe('Rendering', () => {
    it('should render title correctly', () => {
      render(<MicsMatch {...defaultProps} />);

      expect(
        screen.getByText('Display in Misc match file')
      ).toBeInTheDocument();
    });

    it('should render both input fields', () => {
      render(<MicsMatch {...defaultProps} />);

      expect(
        screen.getByText('LST Note (Show under League)')
      ).toBeInTheDocument();
      expect(
        screen.getByText('Remark 2 (Show under Match)')
      ).toBeInTheDocument();
    });

    it('should display current values correctly', () => {
      render(<MicsMatch {...defaultProps} />);

      expect(
        screen.getByTestId('input-LST Note (Show under League)')
      ).toHaveValue('Test Note');
      expect(
        screen.getByTestId('input-Remark 2 (Show under Match)')
      ).toHaveValue('Test Remark');
    });
  });

  describe('Input Changes', () => {
    it('should dispatch updateObject for lstNote when changed', () => {
      render(<MicsMatch {...defaultProps} />);

      const input = screen.getByTestId('input-LST Note (Show under League)');
      fireEvent.change(input, { target: { value: 'New Note' } });

      expect(mockDispatch).toHaveBeenCalled();
      expect(mockUpdateObject).toHaveBeenCalledWith({
        property: 'lstNote',
        value: 'New Note'
      });
    });

    it('should dispatch updateObject for remark2 when changed', () => {
      render(<MicsMatch {...defaultProps} />);

      const input = screen.getByTestId('input-Remark 2 (Show under Match)');
      fireEvent.change(input, { target: { value: 'New Remark' } });

      expect(mockDispatch).toHaveBeenCalled();
      expect(mockUpdateObject).toHaveBeenCalledWith({
        property: 'remark2',
        value: 'New Remark'
      });
    });

    it('should NOT dispatch when value is unchanged', () => {
      render(<MicsMatch {...defaultProps} />);

      const input = screen.getByTestId('input-LST Note (Show under League)');
      fireEvent.change(input, { target: { value: 'Test Note' } });

      expect(mockDispatch).not.toHaveBeenCalled();
    });
  });

  describe('Props Variations', () => {
    it('should handle empty string values', () => {
      render(<MicsMatch lstNote='' remark2='' />);

      expect(
        screen.getByTestId('input-LST Note (Show under League)')
      ).toHaveValue('');
      expect(
        screen.getByTestId('input-Remark 2 (Show under Match)')
      ).toHaveValue('');
    });

    it('should handle long text values', () => {
      const longText = 'A'.repeat(100);
      render(<MicsMatch lstNote={longText} remark2={longText} />);

      expect(
        screen.getByTestId('input-LST Note (Show under League)')
      ).toHaveValue(longText);
    });

    it('should handle special characters', () => {
      render(<MicsMatch lstNote='Note & Co.' remark2='Remark <test>' />);

      expect(
        screen.getByTestId('input-LST Note (Show under League)')
      ).toHaveValue('Note & Co.');
      expect(
        screen.getByTestId('input-Remark 2 (Show under Match)')
      ).toHaveValue('Remark <test>');
    });
  });
});
