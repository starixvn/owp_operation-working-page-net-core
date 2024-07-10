import { fireEvent, render, screen } from '@testing-library/react';
import React from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hookNative';
import { getEvents } from '../table/tableThunk';
import Filter from './Filter';

jest.mock('../../app/hookNative', () => ({
  useAppDispatch: jest.fn(),
  useAppSelector: jest.fn(),
}));
jest.mock('../table/tableThunk', () => ({
  getEvents: jest.fn(),
}));
jest.mock('./FilterDate', () => jest.fn(() => <div>FilterDate Component</div>));
jest.mock('../summarizeProvider/sum/SummarizeProviderIcon', () =>
  jest.fn(() => <div>SummarizeProviderIcon Component</div>)
);
jest.mock('../summarizeProvider/compare/CompareAgentIcon', () =>
  jest.fn(() => <div>CompareAgent Component</div>)
);

const mockDispatch = jest.fn();
const mockUseAppSelector = useAppSelector as jest.Mock;
const mockUseAppDispatch = useAppDispatch as jest.Mock;

describe('Filter Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
    mockUseAppDispatch.mockReturnValue(mockDispatch);
    mockUseAppSelector.mockImplementation(callback =>
      callback({
        filterState: {
          filter: {
            eventDate: '2024-07-08',
          },
        },
      })
    );
  });

  test('renders the Filter component correctly', () => {
    render(<Filter />);

    expect(screen.getByText('FilterDate Component')).toBeInTheDocument();
    expect(screen.getByText('SummarizeProviderIcon Component')).toBeInTheDocument();
    expect(screen.getByText('CompareAgent Component')).toBeInTheDocument();
    expect(screen.getByText('Search')).toBeInTheDocument();
  });

  test('dispatches getEvents action on component mount and button click', () => {
    render(<Filter />);

    expect(mockDispatch).toHaveBeenCalledWith(getEvents('2024-07-08'));

    fireEvent.click(screen.getByText('Search'));

    expect(mockDispatch).toHaveBeenCalledTimes(2);
    expect(mockDispatch).toHaveBeenCalledWith(getEvents('2024-07-08'));
  });
});
