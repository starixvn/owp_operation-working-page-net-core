import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import CompareAgentPopup from './CompareAgentPopup';
import { useAppSelector } from '../../../app/hookNative';
import { compareDifferent } from '../helper';

// Mock useAppSelector hook
jest.mock('../../../app/hookNative', () => ({
  useAppSelector: jest.fn()
}));

// Mock compareDifferent function
jest.mock('../helper', () => ({
  compareDifferent: jest.fn()
}));

describe('CompareAgentPopup', () => {
  const mockSetIsOpenPopup = jest.fn();

  beforeEach(() => {
    jest.clearAllMocks();
    (useAppSelector as jest.Mock).mockImplementation(callback =>
      callback({
        events: {
          agentConfirmOption: [
            { value: '1', label: 'Agent 1' },
            { value: '2', label: 'Agent 2' }
          ],
          dataSource: []
        }
      })
    );
  });

  test('renders correctly', () => {
    render(<CompareAgentPopup setIsOpenPopup={mockSetIsOpenPopup} />);

    expect(screen.getByText('Check the difference with Agent File')).toBeInTheDocument();
    expect(screen.getByPlaceholderText(/Copy all the KO time of an agent/i)).toBeInTheDocument();
  });

  test('updates agent state on select change', () => {
    render(<CompareAgentPopup setIsOpenPopup={mockSetIsOpenPopup} />);

    fireEvent.change(screen.getByRole('combobox'), { target: { value: '2' } });

    expect(screen.getByRole('combobox')).toHaveValue('2');
  });

  test('updates textarea value on input change', () => {
    render(<CompareAgentPopup setIsOpenPopup={mockSetIsOpenPopup} />);

    fireEvent.change(screen.getByPlaceholderText(/Copy all the KO time of an agent/i), { target: { value: '01:00 PM' } });

    expect(screen.getByPlaceholderText(/Copy all the KO time of an agent/i)).toHaveValue('01:00 PM');
  });

  test('calls handleCheck and updates results', async () => {
    (compareDifferent as jest.Mock).mockReturnValue([
      { koTime: '01:00 PM', dataCount: 10, agentFileCount: 5 }
    ]);

    render(<CompareAgentPopup setIsOpenPopup={mockSetIsOpenPopup} />);

    fireEvent.change(screen.getByPlaceholderText(/Copy all the KO time of an agent/i), { target: { value: '01:00 PM' } });
    fireEvent.click(screen.getByText('Check'));

    await waitFor(() => {
      expect(compareDifferent).toHaveBeenCalledWith('01:00 PM', []);
    });
  });

  test('calls setIsOpenPopup with false on Cancel button click', () => {
    render(<CompareAgentPopup setIsOpenPopup={mockSetIsOpenPopup} />);

    fireEvent.click(screen.getByText('Cancel'));

    expect(mockSetIsOpenPopup).toHaveBeenCalledWith(false);
  });
});
