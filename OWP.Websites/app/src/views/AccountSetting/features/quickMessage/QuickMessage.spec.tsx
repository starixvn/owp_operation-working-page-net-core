import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { queryQuickMessageThunk } from './slice/quickMessageThunk';
import { addQuickMessage } from './slice/quickMessageSlice';
import QuickMessage from './QuickMessage';
import Loading from '../../../components/Loading';
import { REQUEST_STATUS } from '../../../Shared/Constants';

// Mocking external dependencies
jest.mock('../../app/hooks', () => ({
  useAppDispatch: jest.fn(),
  useAppSelector: jest.fn(),
}));
jest.mock('./slice/quickMessageThunk', () => ({
  queryQuickMessageThunk: jest.fn(),
}));
jest.mock('./slice/quickMessageSlice', () => ({
  addQuickMessage: jest.fn(),
}));
jest.mock('../../../components/Loading', () => () => <div>Loading...</div>);

const mockDispatch = jest.fn();
const mockUseAppDispatch = useAppDispatch as jest.Mock;
const mockUseAppSelector = useAppSelector as jest.Mock;

beforeEach(() => {
  jest.clearAllMocks();
  mockUseAppDispatch.mockReturnValue(mockDispatch);
});

describe('QuickMessage Component', () => {
  test('renders loading component when loading status is pending', () => {
    mockUseAppSelector.mockImplementation(callback =>
      callback({
        quickMessageSlice: {
          loading: REQUEST_STATUS.pending,
          editIndex: null,
          quickMessage: [],
        },
      })
    );

    render(<QuickMessage />);

    expect(screen.getByText('Loading...')).toBeInTheDocument();
  });

  test('renders quick messages when loading is complete', () => {
    mockUseAppSelector.mockImplementation(callback =>
      callback({
        quickMessageSlice: {
          loading: REQUEST_STATUS.success,
          editIndex: null,
          quickMessage: [
            { shortcut: '/a', message: 'Message A' },
            { shortcut: '/b', message: 'Message B' },
          ],
        },
      })
    );

    render(<QuickMessage />);

    expect(screen.getByText('Message A')).toBeInTheDocument();
    expect(screen.getByText('Message B')).toBeInTheDocument();
  });

  test('dispatches addQuickMessage when plus-icon is clicked', () => {
    mockUseAppSelector.mockImplementation(callback =>
      callback({
        quickMessageSlice: {
          loading: REQUEST_STATUS.success,
          editIndex: null,
          quickMessage: [],
        },
      })
    );

    const { container } = render(<QuickMessage />);

    fireEvent.click(container.querySelector('.plus-icon')!);

    expect(addQuickMessage).toHaveBeenCalled();
  });

  test('dispatches queryQuickMessageThunk on component mount', () => {
    render(<QuickMessage />);

    expect(queryQuickMessageThunk).toHaveBeenCalled();
  });
});
