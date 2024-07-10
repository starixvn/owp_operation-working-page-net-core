import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { Provider } from 'react-redux';
import { configureStore } from '@reduxjs/toolkit';
import MultipleFilterHeader from './MultipleFilterHeader';
import reducer, { setMultipleFilter, handleClearFilter } from './tableSlice';
import { TColumnKey } from '../../type/table';
import { Option } from '../../../Shared/MultipleSelection/type';
import helper from './calculateFilterOption';
import { useAppDispatch } from '../../app/hookNative';
import React from 'react';

const mockDispatch = jest.fn();
jest.mock('../../app/hookNative');
jest.mock('./calculateFilterOption', () => ({
  MatchNo: jest.fn(() => [{ value: 'Match 1', label: 'Match 1' }]),
  MatchStatus: jest.fn(() => [{ value: 'Status 1', label: 'Status 1' }]),
}));

const mockStore = (preloadedState: any) => configureStore({
  reducer: { events: reducer },
  preloadedState
});



describe('MultipleFilterHeader', () => {
  it('should render and display filter options', () => {
    const store = mockStore({
      events: {
        dataSourceOriginal: [{ MatchNo: 'Match 1', MatchStatus: 'Status 1' }],
        multipleFilter: {
          MatchNo: { isFilter: false, selected: [] },
          MatchStatus: { isFilter: false, selected: [] },
        }
      }
    });

    const { container } = render(
      <Provider store={store}>
        <MultipleFilterHeader columnKey="MatchNo" />
      </Provider>
    );

    // Check if the filter options are rendered
    expect(container.querySelector('.filter-header')).toBeInTheDocument();
  });

  it('should dispatch setMultipleFilter on filter change', () => {
    const store = mockStore({
      events: {
        dataSourceOriginal: [{ MatchNo: 'Match 1', MatchStatus: 'Status 1' }],
        multipleFilter: {
          MatchNo: { isFilter: false, selected: [] },
          MatchStatus: { isFilter: false, selected: [] },
        }
      }
    });

    const { container } = render(
      <Provider store={store}>
        <MultipleFilterHeader columnKey="MatchNo" />
      </Provider>
    );

    (useAppDispatch as jest.Mock).mockReturnValue(mockDispatch);

    waitFor(() => {
      fireEvent.click(container.querySelector('.icon-filter')!);
      expect(mockDispatch).toHaveBeenCalled();
    });
  });

  it('should dispatch handleClearFilter on clear button click', () => {
    const store = mockStore({
      events: {
        dataSourceOriginal: [{ MatchNo: 'Match 1', MatchStatus: 'Status 1' }],
        multipleFilter: {
          MatchNo: { isFilter: true, selected: [{ value: 'Match 1', label: 'Match 1' }] },
          MatchStatus: { isFilter: false, selected: [] },
        }
      }
    });

    const { container } = render(
      <Provider store={store}>
        <MultipleFilterHeader columnKey="MatchNo" />
      </Provider>
    );

    // Mock dispatch
    (useAppDispatch as jest.Mock).mockReturnValue(mockDispatch);

    waitFor(() => {
      fireEvent.click(container.querySelector('.clear-all')!);
      expect(mockDispatch).toHaveBeenCalledWith(handleClearFilter());
    });
  });
});
