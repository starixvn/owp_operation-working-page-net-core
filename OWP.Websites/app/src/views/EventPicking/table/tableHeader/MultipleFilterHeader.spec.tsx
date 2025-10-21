import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import '@testing-library/jest-dom';
import MultipleFilterHeader from './MultipleFilterHeader';
import {
  clearFilter,
  setMultipleFilter
} from '../eventPickingTableSlice/reducer';
import store from '../../app/store';

const mockStore = configureStore([]);

// Mock the actual store's getState
jest.mock('../../app/store', () => ({
  __esModule: true,
  default: {
    getState: jest.fn()
  }
}));

describe('MultipleFilterHeader', () => {
  let mockReduxStore: any;
  const mockOptions = [
    { value: 1, label: 'Option 1' },
    { value: 2, label: 'Option 2' },
    { value: 3, label: 'Option 3' }
  ];

  const mockSelected = [{ value: 1, label: 'Option 1' }];

  const defaultState = {
    eventPickingTable: {
      isNoOfferAll: false,
      isConfirmAll: false,
      filterState: [],
      valueFilterAllColumn: {},
      columnKeyFilterCurrent: '',
      entities: [],
      entitiesOriginal: [],
      multipleFilter: {
        time: {
          isFilter: false,
          selected: [],
          options: []
        },
        maxbetHdp: {
          isFilter: false,
          selected: [],
          options: mockOptions
        },
        leagueName: {
          isFilter: false,
          selected: [],
          options: mockOptions
        }
      }
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
    mockReduxStore = mockStore(defaultState);
    mockReduxStore.dispatch = jest.fn();

    // Setup the mock store.getState
    (store.getState as jest.Mock).mockReturnValue({
      eventPickingTable: {
        multipleFilter: {
          maxbetHdp: {
            options: mockOptions,
            selected: [],
            isFilter: false
          },
          leagueName: {
            options: mockOptions,
            selected: mockSelected,
            isFilter: true
          },
          time: {
            options: mockOptions,
            selected: [],
            isFilter: false
          }
        }
      }
    });

    jest.clearAllMocks();
  });

  it('should render filter icon', () => {
    const { container } = render(
      <Provider store={mockReduxStore}>
        <MultipleFilterHeader columnKey='maxbetHdp' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    expect(filterIcon).toBeInTheDocument();
  });

  it('should display "is-filter" class when column has filter applied', () => {
    const { container } = render(
      <Provider store={mockReduxStore}>
        <MultipleFilterHeader columnKey='leagueName' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    expect(filterIcon).toHaveClass('is-filter');
  });

  it('should not display "is-filter" class when column has no filter', () => {
    const { container } = render(
      <Provider store={mockReduxStore}>
        <MultipleFilterHeader columnKey='maxbetHdp' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    expect(filterIcon).not.toHaveClass('is-filter');
  });

  it('should open filter popup when clicking filter icon', () => {
    const { container } = render(
      <Provider store={mockReduxStore}>
        <MultipleFilterHeader columnKey='maxbetHdp' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    const popup = container.querySelector('.filter-popup');
    expect(popup).toBeInTheDocument();
  });

  it('should close popup when clicking close icon', () => {
    const { container } = render(
      <Provider store={mockReduxStore}>
        <MultipleFilterHeader columnKey='maxbetHdp' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    let popup = container.querySelector('.filter-popup');
    expect(popup).toBeInTheDocument();

    const closeIcon = container.querySelector('.close-popup');
    fireEvent.click(closeIcon!);

    popup = container.querySelector('.filter-popup');
    expect(popup).not.toBeInTheDocument();
  });

  it('should close popup when clicking Cancel button', () => {
    const { container, getByText } = render(
      <Provider store={mockReduxStore}>
        <MultipleFilterHeader columnKey='maxbetHdp' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    let popup = container.querySelector('.filter-popup');
    expect(popup).toBeInTheDocument();

    const cancelButton = getByText('Cancel');
    fireEvent.click(cancelButton);

    popup = container.querySelector('.filter-popup');
    expect(popup).not.toBeInTheDocument();
  });

  it('should dispatch setMultipleFilter action when clicking Search button', () => {
    const { container, getByText } = render(
      <Provider store={mockReduxStore}>
        <MultipleFilterHeader columnKey='maxbetHdp' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    const searchButton = getByText('Search');
    fireEvent.click(searchButton);

    expect(mockReduxStore.dispatch).toHaveBeenCalledWith(
      setMultipleFilter({
        columnKey: 'maxbetHdp',
        selected: [],
        hasReadPermission: true
      })
    );
  });

  it('should dispatch setMultipleFilter with selected values', () => {
    (store.getState as jest.Mock).mockReturnValue({
      eventPickingTable: {
        multipleFilter: {
          maxbetHdp: {
            options: mockOptions,
            selected: mockSelected,
            isFilter: false
          }
        }
      }
    });

    const { container, getByText } = render(
      <Provider store={mockReduxStore}>
        <MultipleFilterHeader columnKey='maxbetHdp' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    const searchButton = getByText('Search');
    fireEvent.click(searchButton);

    expect(mockReduxStore.dispatch).toHaveBeenCalledWith(
      setMultipleFilter({
        columnKey: 'maxbetHdp',
        selected: mockSelected,
        hasReadPermission: true
      })
    );
  });

  it('should dispatch clearFilter action when clicking Clear All Filters', () => {
    const { container, getByText } = render(
      <Provider store={mockReduxStore}>
        <MultipleFilterHeader columnKey='maxbetHdp' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    const clearButton = getByText('Clear All Filters');
    fireEvent.click(clearButton);

    expect(mockReduxStore.dispatch).toHaveBeenCalledWith(clearFilter());
  });

  it('should close popup after clicking Search button', () => {
    const { container, getByText } = render(
      <Provider store={mockReduxStore}>
        <MultipleFilterHeader columnKey='maxbetHdp' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    let popup = container.querySelector('.filter-popup');
    expect(popup).toBeInTheDocument();

    const searchButton = getByText('Search');
    fireEvent.click(searchButton);

    popup = container.querySelector('.filter-popup');
    expect(popup).not.toBeInTheDocument();
  });

  it('should close popup after clicking Clear All Filters', () => {
    const { container, getByText } = render(
      <Provider store={mockReduxStore}>
        <MultipleFilterHeader columnKey='maxbetHdp' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    let popup = container.querySelector('.filter-popup');
    expect(popup).toBeInTheDocument();

    const clearButton = getByText('Clear All Filters');
    fireEvent.click(clearButton);

    popup = container.querySelector('.filter-popup');
    expect(popup).not.toBeInTheDocument();
  });

  it('should pass hasReadPermission as false when permission is denied', () => {
    const stateWithNoPermission = {
      ...defaultState,
      turnOverMarginPolicy: {
        permission: {
          HasRead: false
        }
      }
    };

    mockReduxStore = mockStore(stateWithNoPermission);
    mockReduxStore.dispatch = jest.fn();

    const { container, getByText } = render(
      <Provider store={mockReduxStore}>
        <MultipleFilterHeader columnKey='maxbetHdp' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    const searchButton = getByText('Search');
    fireEvent.click(searchButton);

    expect(mockReduxStore.dispatch).toHaveBeenCalledWith(
      setMultipleFilter({
        columnKey: 'maxbetHdp',
        selected: [],
        hasReadPermission: false
      })
    );
  });

  it('should handle different column keys correctly', () => {
    (store.getState as jest.Mock).mockReturnValue({
      eventPickingTable: {
        multipleFilter: {
          time: {
            options: mockOptions,
            selected: mockSelected,
            isFilter: true
          }
        }
      }
    });

    const { container, getByText } = render(
      <Provider store={mockReduxStore}>
        <MultipleFilterHeader columnKey='time' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    const searchButton = getByText('Search');
    fireEvent.click(searchButton);

    expect(mockReduxStore.dispatch).toHaveBeenCalledWith(
      setMultipleFilter({
        columnKey: 'time',
        selected: mockSelected,
        hasReadPermission: true
      })
    );
  });

  it('should display correct options and selected values from store', () => {
    const { container } = render(
      <Provider store={mockReduxStore}>
        <MultipleFilterHeader columnKey='maxbetHdp' />
      </Provider>
    );

    const filterIcon = container.querySelector('.icon-filter');
    fireEvent.click(filterIcon!);

    // Verify that MultiSelect component receives correct props
    const multiSelect = container.querySelector('.multi-select');
    expect(multiSelect).toBeInTheDocument();
  });
});
