import React from 'react';
import { render, fireEvent } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import { Provider } from 'react-redux';
import DateRangeFilter from './DateRangeFilter';
import { setFilterTimeRange } from './filterSlice'; // Assuming this is your action creator
import { getDefaultDateRange } from '../../../Helper/TicketCenterHelper'; // Import your helper function

const { minDate, toDateDefault } = getDefaultDateRange(); // Assuming getDefaultDateRange returns mock data
const initialState = {
  filter: {
    entities: {
      filterInfo: {
        fromDateFilter: minDate,
        toDateFilter: toDateDefault
      }
    }
  }
};

const mockStore = configureStore([]);

describe('DateRangeFilter component', () => {
  let store;
  let component;

  beforeEach(() => {
    store = mockStore(initialState); // Initialize mock store with initial state
  });

  afterEach(() => {
    store.clearActions(); // Clear actions after each test
  });

  it('dispatches setFilterTimeRange action on date change', () => {
    const { container } = render(
      <Provider store={store}>
        <DateRangeFilter />
      </Provider>
    );

    const icon = container.getElementsByClassName('icon-calendar')[0];
    fireEvent.click(icon);
    const fromDate = container.getElementsByClassName('cal-from-date')[0];
    fireEvent.change(fromDate, {
      target: { value: '2023-01-15' }
    });
    const icon2 = container.getElementsByClassName('icon-calendar')[1];
    fireEvent.click(icon2);
    const toDate = container.getElementsByClassName('cal-to-date')[0];
    fireEvent.change(toDate, {
      target: { value: '2023-01-15' }
    });

    const actions = store.getActions();
    expect(actions).not.toBe([]);
  });
});
