import '@testing-library/jest-dom';
import { fireEvent, render, screen } from '@testing-library/react';
import React from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hookNative';
import FilterDate from './FilterDate';
import { updateFilter } from './FilterSlice';

jest.mock('../../app/hookNative', () => ({
  useAppDispatch: jest.fn(),
  useAppSelector: jest.fn(),
}));
jest.mock('./FilterSlice', () => ({
  updateFilter: jest.fn(),
}));
jest.mock('../../../Shared/DateTime/dateTimeHelper', () => ({
  buildDefaultFilterDateProviderVerification: jest.fn().mockReturnValue(new Date('2024-07-10')),
}));

const mockDispatch = jest.fn();
const mockUseAppSelector = useAppSelector as jest.Mock;
const mockUseAppDispatch = useAppDispatch as jest.Mock;

describe('FilterDate Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
    mockUseAppDispatch.mockReturnValue(mockDispatch);
    mockUseAppSelector.mockImplementation((selector) =>
      selector({
        filterState: {
          filter: {
            eventDate: new Date('2024-07-09'),
          },
        },
      })
    );
  });

  test('renders the DatePickerComponent', () => {
    render(<FilterDate />);

    expect(screen.getByRole('textbox')).toBeInTheDocument();
  });

  test('dispatches updateFilter action on date change', () => {
    render(<FilterDate />);

    const datePicker = screen.getByRole('textbox');
    fireEvent.change(datePicker, { target: { value: '2024-07-10' } });

    expect(mockDispatch).toHaveBeenCalledWith(updateFilter({
      key: 'eventDate',
      value: new Date('2024-07-10')
    }));
  });
});
