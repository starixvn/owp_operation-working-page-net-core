import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import '@testing-library/jest-dom';
import FilterHeader from './FilterHeader';
import {
  clearFilter,
  filterBySearchKey,
  setColumnKeyValue,
  setValueFilter,
  clearSingleColumnFilter
} from '../eventPickingTableSlice/reducer';

const mockStore = configureStore([]);

describe('FilterHeader', () => {
  let store: any;
  const defaultState = {
    eventPickingTable: {
      isNoOfferAll: false,
      isConfirmAll: false,
      filterState: [],
      valueFilterAllColumn: {},
      columnKeyFilterCurrent: '',
      entities: [],
      entitiesOriginal: [],
      multipleFilter: {}
    },
    turnOverMarginPolicy: {
      permission: {
        HasRead: true
      }
    },
    pagePolicy: {
      permission: {
        HasWrite: true,
        HasRead: true
      }
    }
  };

  beforeEach(() => {
    store = mockStore(defaultState);
    store.dispatch = jest.fn();
    jest.clearAllMocks();
  });

  it('should render filter icon', () => {
    const { container } = render(
      <Provider store={store}>
        <FilterHeader columnKey='League' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    expect(filterIcon).toBeInTheDocument();
  });

  it('should open filter popup when clicking filter icon', () => {
    const { container } = render(
      <Provider store={store}>
        <FilterHeader columnKey='League' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    expect(store.dispatch).toHaveBeenCalledWith(setColumnKeyValue('League'));

    const popup = container.querySelector('.filter-popup');
    expect(popup).toBeInTheDocument();
  });

  it('should display filtered state when column is filtered', () => {
    const stateWithFilter = {
      ...defaultState,
      eventPickingTable: {
        ...defaultState.eventPickingTable,
        filterState: [{ columnKey: 'League', keyword: 'Premier' }]
      }
    };
    store = mockStore(stateWithFilter);
    store.dispatch = jest.fn();

    const { container } = render(
      <Provider store={store}>
        <FilterHeader columnKey='League' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter.is-filter');
    expect(filterIcon).toBeInTheDocument();
  });

  it('should call filterBySearchKey when clicking Search button', () => {
    const stateWithValue = {
      ...defaultState,
      eventPickingTable: {
        ...defaultState.eventPickingTable,
        valueFilterAllColumn: { League: 'Premier' },
        columnKeyFilterCurrent: 'League'
      }
    };
    store = mockStore(stateWithValue);
    store.dispatch = jest.fn();

    const { container, getByText } = render(
      <Provider store={store}>
        <FilterHeader columnKey='League' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    const searchButton = getByText('Search');
    fireEvent.click(searchButton);

    expect(store.dispatch).toHaveBeenCalledWith(
      filterBySearchKey({
        filterState: { columnKey: 'League', keyword: 'Premier' },
        hasReadPermission: true
      })
    );
  });

  it('should call clearFilter when clicking Clear All Filters', () => {
    const stateWithValue = {
      ...defaultState,
      eventPickingTable: {
        ...defaultState.eventPickingTable,
        valueFilterAllColumn: { League: 'Premier' },
        columnKeyFilterCurrent: 'League'
      }
    };
    store = mockStore(stateWithValue);
    store.dispatch = jest.fn();

    const { container, getByText } = render(
      <Provider store={store}>
        <FilterHeader columnKey='League' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    const clearButton = getByText('Clear All Filters');
    fireEvent.click(clearButton);

    expect(store.dispatch).toHaveBeenCalledWith(clearFilter());
  });

  it('should call setValueFilter when typing in input field', () => {
    const stateWithValue = {
      ...defaultState,
      eventPickingTable: {
        ...defaultState.eventPickingTable,
        columnKeyFilterCurrent: 'League'
      }
    };
    store = mockStore(stateWithValue);
    store.dispatch = jest.fn();

    const { container } = render(
      <Provider store={store}>
        <FilterHeader columnKey='League' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    const input = container.querySelector('.input-field') as HTMLInputElement;
    fireEvent.change(input, { target: { value: 'Premier' } });

    expect(store.dispatch).toHaveBeenCalledWith(
      setValueFilter({ columnKey: 'League', keyword: 'Premier' })
    );
  });

  it('should call filterBySearchKey when pressing Enter in input field', () => {
    const stateWithValue = {
      ...defaultState,
      eventPickingTable: {
        ...defaultState.eventPickingTable,
        valueFilterAllColumn: { League: 'Premier' },
        columnKeyFilterCurrent: 'League'
      }
    };
    store = mockStore(stateWithValue);
    store.dispatch = jest.fn();

    const { container } = render(
      <Provider store={store}>
        <FilterHeader columnKey='League' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    const input = container.querySelector('.input-field') as HTMLInputElement;
    fireEvent.keyDown(input, {
      key: 'Enter',
      currentTarget: { value: 'Premier' }
    });

    expect(store.dispatch).toHaveBeenCalledWith(
      filterBySearchKey({
        filterState: { columnKey: 'League', keyword: 'Premier' },
        hasReadPermission: true
      })
    );
  });

  it('should show popup with correct structure', () => {
    const { container } = render(
      <Provider store={store}>
        <FilterHeader columnKey='League' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    const popup = container.querySelector('.filter-popup');
    expect(popup).toBeInTheDocument();

    const inputField = container.querySelector('.input-field');
    expect(inputField).toBeInTheDocument();

    const searchButton = container.querySelector('.btn-search');
    expect(searchButton).toBeInTheDocument();

    const clearAllButton = container.querySelector('.clear-all');
    expect(clearAllButton).toBeInTheDocument();
  });

  it('should disable Search button when no filter value', () => {
    const stateWithValue = {
      ...defaultState,
      eventPickingTable: {
        ...defaultState.eventPickingTable,
        valueFilterAllColumn: {},
        columnKeyFilterCurrent: 'League'
      }
    };
    store = mockStore(stateWithValue);
    store.dispatch = jest.fn();

    const { container, getByText } = render(
      <Provider store={store}>
        <FilterHeader columnKey='League' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    const searchButton = getByText('Search');
    expect(searchButton).toBeDisabled();
  });

  it('should not call filterBySearchKey when pressing Enter with empty value', () => {
    const stateWithValue = {
      ...defaultState,
      eventPickingTable: {
        ...defaultState.eventPickingTable,
        valueFilterAllColumn: {},
        columnKeyFilterCurrent: 'League'
      }
    };
    store = mockStore(stateWithValue);
    const dispatchSpy = jest.fn();
    store.dispatch = dispatchSpy;

    const { container } = render(
      <Provider store={store}>
        <FilterHeader columnKey='League' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    // Clear previous dispatch calls
    dispatchSpy.mockClear();

    const input = container.querySelector('.input-field') as HTMLInputElement;
    fireEvent.keyDown(input, { key: 'Enter', currentTarget: { value: '' } });

    // Should not have called filterBySearchKey (only setColumnKeyValue was called on icon click)
    const filterBySearchKeyCalls = dispatchSpy.mock.calls.filter(
      call => call[0].type === filterBySearchKey.type
    );
    expect(filterBySearchKeyCalls).toHaveLength(0);
  });

  it('should use hasReadPermission from turnOverMarginPolicy', () => {
    const stateWithoutPermission = {
      ...defaultState,
      eventPickingTable: {
        ...defaultState.eventPickingTable,
        valueFilterAllColumn: { League: 'Premier' },
        columnKeyFilterCurrent: 'League'
      },
      turnOverMarginPolicy: {
        permission: {
          HasRead: false
        }
      }
    };
    store = mockStore(stateWithoutPermission);
    store.dispatch = jest.fn();

    const { container, getByText } = render(
      <Provider store={store}>
        <FilterHeader columnKey='League' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    const searchButton = getByText('Search');
    fireEvent.click(searchButton);

    expect(store.dispatch).toHaveBeenCalledWith(
      filterBySearchKey({
        filterState: { columnKey: 'League', keyword: 'Premier' },
        hasReadPermission: false
      })
    );
  });

  it('should call clearSingleColumnFilter when clicking Clear Column Filter', () => {
    const stateWithFilter = {
      ...defaultState,
      eventPickingTable: {
        ...defaultState.eventPickingTable,
        filterState: [{ columnKey: 'League', keyword: 'Premier' }],
        valueFilterAllColumn: { League: 'Premier' },
        columnKeyFilterCurrent: 'League'
      }
    };
    store = mockStore(stateWithFilter);
    store.dispatch = jest.fn();

    const { container, getByText } = render(
      <Provider store={store}>
        <FilterHeader columnKey='League' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    const clearColumnButton = getByText('Clear Column Filter');
    fireEvent.click(clearColumnButton);

    expect(store.dispatch).toHaveBeenCalledWith(
      clearSingleColumnFilter({
        columnKey: 'League',
        hasReadPermission: true
      })
    );
  });

  it('should disable Clear Column Filter when column is not filtered', () => {
    const stateWithoutFilter = {
      ...defaultState,
      eventPickingTable: {
        ...defaultState.eventPickingTable,
        filterState: [],
        columnKeyFilterCurrent: 'League'
      }
    };
    store = mockStore(stateWithoutFilter);
    store.dispatch = jest.fn();

    const { container } = render(
      <Provider store={store}>
        <FilterHeader columnKey='League' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    const clearColumnButton = container.querySelector('.clear-all');
    expect(clearColumnButton).toHaveClass('disabled');
  });
});
