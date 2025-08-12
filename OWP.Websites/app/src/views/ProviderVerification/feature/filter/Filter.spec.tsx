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
jest.mock('../../app/storeNative', () => ({
  default: {
    getState: () => ({
      common: {
        confirmProvider: {
          confirmProviderOption: []
        },
        LSTBookProvider: {
          LSTBookProviderOption: []
        }
      }
    })
  }
}));
jest.mock('./FilterDate', () => jest.fn(() => <div>FilterDate Component</div>));
jest.mock('../summarizeProvider/sum/SummarizeProviderIcon', () =>
  jest.fn(() => <div>SummarizeProviderIcon Component</div>)
);
jest.mock('../summarizeProvider/compare/CompareAgentIcon', () =>
  jest.fn(() => <div>CompareAgent Component</div>)
);
jest.mock('./ConfirmProvider', () => jest.fn(() => <div>ConfirmProvider Component</div>));
jest.mock('./LSTBookProvider', () => jest.fn(() => <div>LSTBookProvider Component</div>));
jest.mock('./MismatchEvent', () => jest.fn(() => <div>MismatchEvent Component</div>));

const mockDispatch = jest.fn();
const mockUseAppSelector = useAppSelector as jest.Mock;
const mockUseAppDispatch = useAppDispatch as jest.Mock;

describe('Filter Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
    mockUseAppDispatch.mockReturnValue(mockDispatch);
    mockUseAppSelector.mockImplementation(callback =>
      callback({
        events: {
          dataSource: []
        },
        filterState: {
          filter: {
            eventDate: new Date('2024-07-08'),
            onlyShowIsMismatchEvent: false,
            confirmProvider: [],
            lstBookProvider: []
          },
        },
        common: {
          confirmProvider: {
            confirmProviderOption: []
          },
          LSTBookProvider: {
            LSTBookProviderOption: []
          }
        }
      })
    );
  });

  test('renders the Filter component correctly', () => {
    render(<Filter />);

    expect(screen.getByText('FilterDate Component')).toBeInTheDocument();
    expect(screen.getByText('LSTBookProvider Component')).toBeInTheDocument();
    expect(screen.getByText('ConfirmProvider Component')).toBeInTheDocument();
    expect(screen.getByText('MismatchEvent Component')).toBeInTheDocument();
    expect(screen.getByText('SummarizeProviderIcon Component')).toBeInTheDocument();
    expect(screen.getByText('CompareAgent Component')).toBeInTheDocument();
    expect(screen.getByText('Search')).toBeInTheDocument();
  });

  test('dispatches getEvents action on component mount and button click', () => {
    render(<Filter />);

    expect(mockDispatch).toHaveBeenCalledWith(getEvents({
      eventDate: new Date('2024-07-08'),
      onlyShowIsMismatchEvent: false,
      confirmProvider: null,
      lstBookProvider: null
    }));

    fireEvent.click(screen.getByText('Search'));

    expect(mockDispatch).toHaveBeenCalledTimes(2);
    expect(mockDispatch).toHaveBeenCalledWith(getEvents({
      eventDate: new Date('2024-07-08'),
      onlyShowIsMismatchEvent: false,
      confirmProvider: null,
      lstBookProvider: null
    }));
  });
});


